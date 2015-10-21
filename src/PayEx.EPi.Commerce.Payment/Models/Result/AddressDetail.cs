﻿using System.Xml.Serialization;

namespace PayEx.EPi.Commerce.Payment.Models.Result
{
    public class AddressDetail
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("address1")]
        public string Address { get; set; }

        [XmlElement("postNumber")]
        public string PostNumber { get; set; }

        [XmlElement("city")]
        public string City { get; set; }

        [XmlElement("country")]
        public string Country { get; set; }

        public string CustomerName => string.Join(" ", FirstName, LastName);
    }
}
