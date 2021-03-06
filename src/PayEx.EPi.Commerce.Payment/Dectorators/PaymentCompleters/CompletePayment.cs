﻿using log4net;
using PayEx.EPi.Commerce.Payment.Contracts;
using PayEx.EPi.Commerce.Payment.Contracts.Commerce;
using PayEx.EPi.Commerce.Payment.Models;
using PayEx.EPi.Commerce.Payment.Models.PaymentMethods;
using PayEx.EPi.Commerce.Payment.Models.Result;

namespace PayEx.EPi.Commerce.Payment.Dectorators.PaymentCompleters
{
    internal class CompletePayment : IPaymentCompleter
    {
        private IPaymentCompleter _paymentCompleter;
        private readonly IPaymentManager _paymentManager;
        private readonly IPaymentActions _paymentActions;
        protected readonly ILog Log = LogManager.GetLogger(Constants.Logging.DefaultLoggerName);

        public CompletePayment(IPaymentCompleter paymentCompleter, IPaymentManager paymentManager, IPaymentActions paymentActions)
        {
            _paymentCompleter = paymentCompleter;
            _paymentManager = paymentManager;
            _paymentActions = paymentActions;
        }

        public PaymentCompleteResult Complete(PaymentMethod currentPayment, string orderRef)
        {
            Log.Info($"Completing payment with ID:{currentPayment.Payment.Id} belonging to order with ID: {currentPayment.OrderGroupId}");
            var completeResult = _paymentManager.Complete(orderRef);
            if (!completeResult.Success || string.IsNullOrWhiteSpace(completeResult.TransactionNumber))
                return new PaymentCompleteResult { TransactionErrorCode = completeResult.ErrorDetails != null ? completeResult.ErrorDetails.TransactionErrorCode : string.Empty };

            if (completeResult.GetTransactionDetails)
            {
                Log.Info($"Retrieving transaction details for payment with ID:{currentPayment.Payment.Id} belonging to order with ID: {currentPayment.OrderGroupId}");
                if (_paymentCompleter == null)
                    _paymentCompleter = new UpdateTransactionDetails(null, _paymentManager);
                _paymentCompleter = new UpdateTransactionDetails(_paymentCompleter, _paymentManager);
            }

            _paymentActions.UpdatePaymentInformation(currentPayment, completeResult.TransactionNumber, completeResult.PaymentMethod);

            var result = new PaymentCompleteResult { Success = true };
            if (_paymentCompleter != null)
                result = _paymentCompleter.Complete(currentPayment, orderRef);

            Log.Info($"Successfully completed payment with ID:{currentPayment.Payment.Id} belonging to order with ID: {currentPayment.OrderGroupId}");
            return result;
        }
    }
}
