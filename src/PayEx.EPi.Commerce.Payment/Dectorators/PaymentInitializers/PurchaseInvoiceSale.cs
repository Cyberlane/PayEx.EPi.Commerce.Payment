﻿using System;
using log4net;
using PayEx.EPi.Commerce.Payment.Contracts;
using PayEx.EPi.Commerce.Payment.Contracts.Commerce;
using PayEx.EPi.Commerce.Payment.Models;
using PayEx.EPi.Commerce.Payment.Models.PaymentMethods;

namespace PayEx.EPi.Commerce.Payment.Dectorators.PaymentInitializers
{
    internal class PurchaseInvoiceSale : IPaymentInitializer
    {
        private readonly IPaymentManager _paymentManager;
        private readonly IPaymentActions _paymentActions;
        protected readonly ILog Log = LogManager.GetLogger(Constants.Logging.DefaultLoggerName);

        public PurchaseInvoiceSale(IPaymentManager paymentManager, IPaymentActions paymentActions)
        {
            _paymentManager = paymentManager;
            _paymentActions = paymentActions;
        }

        public PaymentInitializeResult Initialize(PaymentMethod currentPayment, string orderNumber, string returnUrl, string orderRef)
        {
            Log.Info($"Calling PurchaseInvoiceSale for payment with ID:{currentPayment.Payment.Id} belonging to order with ID: {currentPayment.OrderGroupId}");
            var customerDetails = CreateModel(currentPayment);
            if (customerDetails == null)
                throw new Exception("Payment class must be ExtendedPayExPayment when using this payment method");

            var result = _paymentManager.PurchaseInvoiceSale(orderRef, customerDetails);
            if (!result.Status.Success)
                return new PaymentInitializeResult { ErrorMessage = result.Status.Description };

            _paymentActions.UpdatePaymentInformation(currentPayment, result.TransactionNumber, result.PaymentMethod);

            Log.Info($"Successfully called PurchaseInvoiceSale for payment with ID:{currentPayment.Payment.Id} belonging to order with ID: {currentPayment.OrderGroupId}");
            return new PaymentInitializeResult { Success = true };
        }

        private CustomerDetails CreateModel(PaymentMethod currentPayment)
        {
            if (!(currentPayment.Payment is ExtendedPayExPayment))
                return null;

            var payment = currentPayment.Payment as ExtendedPayExPayment;

            return new CustomerDetails
            {
                SocialSecurityNumber = payment.SocialSecurityNumber,
                FirstName = payment.FirstName,
                LastName = payment.LastName,
                StreetAddress = payment.StreetAddress,
                City = payment.City,
                CoAddress = payment.CoAddress,
                CountryCode = payment.CountryCode,
                Email = payment.Email,
                IpAddress = currentPayment.Payment.ClientIpAddress,
                MobilePhone = payment.MobilePhone,
                PostNumber = payment.PostNumber,
            };
        }
    }
}
