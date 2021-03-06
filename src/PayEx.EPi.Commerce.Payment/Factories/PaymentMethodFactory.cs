﻿using log4net;
using PayEx.EPi.Commerce.Payment.Contracts;
using PayEx.EPi.Commerce.Payment.Contracts.Commerce;
using PayEx.EPi.Commerce.Payment.Models.PaymentMethods;

namespace PayEx.EPi.Commerce.Payment.Factories
{
    internal class PaymentMethodFactory : IPaymentMethodFactory
    {
        private readonly IPaymentManager _paymentManager;
        private readonly IParameterReader _parameterReader;
        private readonly ICartActions _cartActions;
        private readonly IVerificationManager _verificationManager;
        private readonly IOrderNumberGenerator _orderNumberGenerator;
        private readonly IAdditionalValuesFormatter _additionalValuesFormatter;
        private readonly IPaymentActions _paymentActions;
        private readonly IFinancialInvoicingOrderLineFormatter _financialInvoicingOrderLineFormatter;
        private readonly IMasterPassShoppingCartFormatter _masterPassShoppingCartXmlFormatter;

        protected readonly ILog Log = LogManager.GetLogger(Constants.Logging.DefaultLoggerName);
        private readonly IUpdateAddressHandler _updateAddressHandler;

        public PaymentMethodFactory(IPaymentManager paymentManager, IParameterReader parameterReader,
            ICartActions cartActions, IVerificationManager verificationManager,
            IOrderNumberGenerator orderNumberGenerator, IAdditionalValuesFormatter additionalValuesFormatter,
            IPaymentActions paymentActions, IFinancialInvoicingOrderLineFormatter financialInvoicingOrderLineFormatter,
            IUpdateAddressHandler updateAddressHandler, IMasterPassShoppingCartFormatter masterPassShoppingCartXmlFormatter)
        {
            _paymentManager = paymentManager;
            _parameterReader = parameterReader;
            _cartActions = cartActions;
            _verificationManager = verificationManager;
            _orderNumberGenerator = orderNumberGenerator;
            _additionalValuesFormatter = additionalValuesFormatter;
            _paymentActions = paymentActions;
            _financialInvoicingOrderLineFormatter = financialInvoicingOrderLineFormatter;
            _updateAddressHandler = updateAddressHandler;
            _masterPassShoppingCartXmlFormatter = masterPassShoppingCartXmlFormatter;
        }

        public PaymentMethod Create(Mediachase.Commerce.Orders.Payment payment)
        {
            if (payment == null)
                return null;

            Log.Info($"Attempting to resolve the PaymentMethod for payment with ID:{payment.Id}. PaymentMethodId:{payment.PaymentMethodId}");
            if (!(payment is PayExPayment))
            {
                Log.Error($"Payment with ID:{payment.Id} is not a PayExPayment and therefore it cannot be processed by the PayEx Payment Provider!");
                return null;
            }

            var paymentMethodDto =
                Mediachase.Commerce.Orders.Managers.PaymentManager.GetPaymentMethod(payment.PaymentMethodId);
            var systemKeyword =
                paymentMethodDto.PaymentMethod.FindByPaymentMethodId(payment.PaymentMethodId).SystemKeyword;
            Log.Info($"Resolving the PaymentMethod for payment with ID:{payment.Id}. The systemKeyword for this payment method is {systemKeyword}");

            switch (systemKeyword)
            {
                case Constants.Payment.DirectDebit.SystemKeyword:
                    return new DirectBankDebit(payment, _paymentManager, _parameterReader, _cartActions,
                        _orderNumberGenerator, _additionalValuesFormatter, _paymentActions);
                case Constants.Payment.Giftcard.SystemKeyword:
                    return new GiftCard(payment, _paymentManager, _parameterReader, _cartActions, _orderNumberGenerator,
                        _additionalValuesFormatter, _paymentActions);
                case Constants.Payment.Invoice.SystemKeyword:
                    return new Invoice(payment, _verificationManager, _paymentManager, _parameterReader, _cartActions,
                        _orderNumberGenerator, _additionalValuesFormatter, _paymentActions);
                case Constants.Payment.InvoiceLedger.SystemKeyword:
                    return new InvoiceLedger(payment, _paymentManager, _parameterReader, _cartActions,
                        _orderNumberGenerator, _additionalValuesFormatter, _paymentActions);
                case Constants.Payment.PartPayment.SystemKeyword:
                    return new PartPayment(payment, _paymentManager, _parameterReader, _cartActions,
                        _orderNumberGenerator, _additionalValuesFormatter, _paymentActions);
                case Constants.Payment.PayPal.SystemKeyword:
                    return new PayPal(payment, _paymentManager, _parameterReader, _cartActions, _orderNumberGenerator,
                        _additionalValuesFormatter, _paymentActions);
                case Constants.Payment.CreditCard.SystemKeyword:
                    return new CreditCard(payment, _paymentManager, _parameterReader, _cartActions,
                        _orderNumberGenerator, _additionalValuesFormatter, _paymentActions);
                case Constants.Payment.MasterPass.SystemKeyword:
                    return new MasterPass(payment, _paymentManager, _parameterReader, _cartActions,
                        _orderNumberGenerator, _additionalValuesFormatter, _paymentActions, _masterPassShoppingCartXmlFormatter);
                case Constants.Payment.FinancingInvoiceNorway.SystemKeyword:
                    return new FinancingInvoice(payment, _paymentManager, _parameterReader, _cartActions,
                        _orderNumberGenerator, _additionalValuesFormatter, _financialInvoicingOrderLineFormatter,
                        _paymentActions, Constants.Payment.FinancingInvoiceNorway.PaymentMethodCode, _updateAddressHandler);
                case Constants.Payment.FinancingInvoiceSweden.SystemKeyword:
                    return new FinancingInvoice(payment, _paymentManager, _parameterReader, _cartActions,
                        _orderNumberGenerator, _additionalValuesFormatter, _financialInvoicingOrderLineFormatter,
                        _paymentActions, Constants.Payment.FinancingInvoiceSweden.PaymentMethodCode, _updateAddressHandler);
            }

            Log.Error($"Could not resolve the PaymentMethod for payment with ID:{payment.Id}. The systemKeyword for this payment method is {systemKeyword}");
            return null;
        }
    }
}
