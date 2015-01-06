﻿using EPiServer.Business.Commerce.Payment.PayEx.Contracts;
using EPiServer.Business.Commerce.Payment.PayEx.Contracts.Commerce;
using EPiServer.Business.Commerce.Payment.PayEx.Dectorators.PaymentCapturers;
using EPiServer.Business.Commerce.Payment.PayEx.Dectorators.PaymentCompleters;
using EPiServer.Business.Commerce.Payment.PayEx.Dectorators.PaymentCreditors;
using EPiServer.Business.Commerce.Payment.PayEx.Dectorators.PaymentInitializers;

namespace EPiServer.Business.Commerce.Payment.PayEx.Models.PaymentMethods
{
    internal class CreditCard : PaymentMethod
    {
        private readonly IPaymentManager _paymentManager;
        private readonly IParameterReader _parameterReader;
        private readonly ILogger _logger;
        private readonly ICartActions _cartActions;
        private readonly IVerificationManager _verificationManager;
        public CreditCard() { } // Needed for unit testing

        public CreditCard(Mediachase.Commerce.Orders.Payment payment, IPaymentManager paymentManager, 
            IParameterReader parameterReader, ILogger logger, ICartActions cartActions, IVerificationManager verificationManager)
            : base(payment)
        {
            _paymentManager = paymentManager;
            _parameterReader = parameterReader;
            _logger = logger;
            _cartActions = cartActions;
            _verificationManager = verificationManager;
        }

        public override string PaymentMethodCode
        {
            get { return "VISA"; }
        }

        public override string DefaultView
        {
            get { return "CREDITCARD"; }
        }

        public override PurchaseOperation PurchaseOperation
        {
            get { return PurchaseOperation.AUTHORIZATION; }
        }

        public override PaymentInitializeResult Initialize()
        {
            IPaymentInitializer initializer = new GenerateOrderNumber(
                    new InitializePayment(
                    new RedirectUser(), _paymentManager, _parameterReader, _cartActions));
            return initializer.Initialize(this, null, null, null);
            //IPaymentInitializer initializer = new GenerateOrderNumber(
            //   new InitializePayment(
            //       new GetConsumerLegalAddress(null, _verificationManager), _paymentManager, _parameterReader, _cartActions));
            //return initializer.Initialize(this, null, null, null);
        }

        public override PaymentCompleteResult Complete(string orderRef)
        {
            IPaymentCompleter completer = new CompletePayment(null, _paymentManager, _logger);
            return completer.Complete(this, orderRef);
        }

        public override bool Capture()
        {
            IPaymentCapturer capturer = new CapturePayment(null, _logger, _paymentManager, _parameterReader);
            return capturer.Capture(this);
        }

        public override bool Credit()
        {
            IPaymentCreditor creditor = new CreditPayment(null, _logger, _paymentManager, _parameterReader);
            return creditor.Credit(this);
        }
    }
}
