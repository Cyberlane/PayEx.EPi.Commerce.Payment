﻿using EPiServer.Business.Commerce.Payment.PayEx.Contracts;
using EPiServer.Business.Commerce.Payment.PayEx.Models;
using EPiServer.Business.Commerce.Payment.PayEx.Models.PaymentMethods;

namespace EPiServer.Business.Commerce.Payment.PayEx.Dectorators.PaymentInitializers
{
    internal class GetConsumerLegalAddress : IPaymentInitializer
    {
        private readonly IVerificationManager _verificationManager;

        public GetConsumerLegalAddress(IPaymentInitializer paymentInitializer, IVerificationManager verificationManager)
        {
            _verificationManager = verificationManager;
        }

        public PaymentInitializeResult Initialize(PaymentMethod currentPayment, string orderNumber, string returnUrl, string orderRef)
        {
            var a = _verificationManager.GetConsumerLegalAddress("197311012525", "SE");
            return new PaymentInitializeResult { Success = true };
        }
    }
}
