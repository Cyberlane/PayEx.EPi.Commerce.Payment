﻿using Mediachase.Commerce.Orders;

namespace EPiServer.Business.Commerce.Payment.PayEx.Contracts.Commerce
{
    public interface IOrderNumberGenerator
    {
        /// <summary>
        /// Returns a generated ordernumber for the given cart
        /// </summary>
        string Generate(Cart cart);
    }
}
