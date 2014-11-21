﻿
using Epinova.PayExProvider.Commerce;
using Epinova.PayExProvider.Contracts;
using Epinova.PayExProvider.Dectorators.PaymentCapturers;
using Epinova.PayExProvider.Dectorators.PaymentCompleters;
using Epinova.PayExProvider.Dectorators.PaymentInitializers;
using Epinova.PayExProvider.Price;

namespace Epinova.PayExProvider.Models.PaymentMethods
{
    public class CreditCard : PaymentMethod
    {
        public CreditCard()
        {
        }

        public CreditCard(Mediachase.Commerce.Orders.Payment payment) : base(payment)
        {
        }

        public override PaymentInitializeResult Initialize()
        {
            IPaymentInitializer initializer = new GenerateOrderNumber(
                    new InitializePayment(
                    new RedirectUser(), new PaymentManager(), new ParameterReader(), new CartActions(new Logger())));
            return initializer.Initialize(this, null, null);
        }

        public override PaymentCompleteResult Complete(string orderRef)
        {
            IPaymentCompleter completer = new CompletePayment(null, new PaymentManager(), new Logger());
            return completer.Complete(this, orderRef);
        }

        public override bool Capture()
        {
            IPaymentCapturer capturer = new CapturePayment(null, new Logger(), new PaymentManager(), new ParameterReader());
            return capturer.Capture(this);
        }
    }
}
