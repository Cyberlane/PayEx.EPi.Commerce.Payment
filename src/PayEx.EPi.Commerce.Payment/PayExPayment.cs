﻿using System;
using System.Runtime.Serialization;
using Mediachase.Commerce.Orders;
using Mediachase.MetaDataPlus.Configurator;
using PayEx.EPi.Commerce.Payment.Contracts;

namespace PayEx.EPi.Commerce.Payment
{
    [Serializable]
    public class PayExPayment : Mediachase.Commerce.Orders.Payment, IPayExPayment
    {
        private static MetaClass _metaClass;

        public static MetaClass PayExPaymentMetaClass => _metaClass ?? (_metaClass = MetaClass.Load(OrderContext.MetaDataContext, "PayExPayment"));

        public string OrderNumber
        {
            get
            {
                return GetString("OrderNumber");
            }
            set
            {
                this["OrderNumber"] = value;
            }
        }

        public string PayExOrderRef
        {
            get
            {
                return GetString("PayExOrderRef");
            }
            set
            {
                this["PayExOrderRef"] = value;
            }
        }

        public string Description
        {
            get
            {
                return GetString("Description");
            }
            set
            {
                this["Description"] = value;
            }
        }

        public string ProductNumber
        {
            get
            {
                return GetString("ProductNumber");
            }
            set
            {
                this["ProductNumber"] = value;
            }
        }

        public string ClientIpAddress
        {
            get
            {
                return GetString("ClientIpAddress");
            }
            set
            {
                this["ClientIpAddress"] = value;
            }
        }

        public string CancelUrl
        {
            get
            {
                return GetString("CancelUrl");
            }
            set
            {
                this["CancelUrl"] = value;
            }
        }

        public string ReturnUrl
        {
            get
            {
                return GetString("ReturnUrl");
            }
            set
            {
                this["ReturnUrl"] = value;
            }
        }

        public string CustomerId
        {
            get
            {
                return GetString("CustomerId");
            }
            set
            {
                this["CustomerId"] = value;
            }
        }

        public int Vat
        {
            get
            {
                var vatString = GetString("Vat");
                int vat;
                return int.TryParse(vatString, out vat) ? vat : 0;
            }
            set
            {
                this["Vat"] = value;
            }
        }

        public PayExPayment()
            : base(PayExPaymentMetaClass)
        {
            PaymentType = PaymentType.Other;
            ImplementationClass = GetType().AssemblyQualifiedName;
        }

        public PayExPayment(MetaClass metaClass)
            : base(metaClass)
        {
            PaymentType = PaymentType.Other;
            ImplementationClass = GetType().AssemblyQualifiedName;
        }

        /// <summary>
        /// Create a new PayExPayment
        /// </summary>
        /// <param name="clientIpAddress">Clients IP address</param>
        /// <param name="productNumber">Merchant product number/reference for this specific product. We recommend that only the characters A-Z and 0-9 are used in this parameter.</param>
        /// <param name="cancelUrl">A string identifying the full URL for the page the user will be redirected to when the Cancel Purchase button is pressed by the user. We do not add data to the end of this string. Set to blank if you don’t want this functionality. (Note: This is the PayEx cancel button, and must not be associated with cancel buttons in the customers bank.)</param>
        /// <param name="returnUrl">A string identifying the full URL for the page the user will be redirected to after a successful purchase. We will add orderRef to the existing query, and if no query is supplied to the URL, then the query will be added.</param>
        /// <param name="description">Merchant’s description of the product. Can be empty.</param>
        public PayExPayment(string clientIpAddress, string productNumber, string cancelUrl, string returnUrl, string description)
            : base(PayExPaymentMetaClass)
        {
            if (string.IsNullOrWhiteSpace(clientIpAddress))
                throw new ArgumentException("clientIpAddress cannot be null or empty", nameof(clientIpAddress));
            if (string.IsNullOrWhiteSpace(productNumber))
                throw new ArgumentException("productNumber cannot be null or empty", nameof(productNumber));
            if (string.IsNullOrWhiteSpace(cancelUrl))
                throw new ArgumentException("cancelUrl cannot be null or empty", nameof(cancelUrl));
            if (string.IsNullOrWhiteSpace(returnUrl))
                throw new ArgumentException("returnUrl cannot be null or empty", nameof(returnUrl));
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("description cannot be null or empty", nameof(description));

            ClientIpAddress = clientIpAddress;
            Description = description;
            ProductNumber = productNumber;
            CancelUrl = cancelUrl;
            ReturnUrl = returnUrl;

            PaymentType = PaymentType.Other;
            ImplementationClass = GetType().AssemblyQualifiedName;
        }

        protected PayExPayment(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            PaymentType = PaymentType.Other;
            ImplementationClass = GetType().AssemblyQualifiedName;
        }
    }
}
