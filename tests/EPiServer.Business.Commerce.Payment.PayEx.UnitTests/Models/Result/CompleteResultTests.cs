﻿using EPiServer.Business.Commerce.Payment.PayEx.Models.Result;
using EPiServer.Business.Commerce.Payment.PayEx.Payment;
using NUnit.Framework;

namespace EPiServer.Business.Commerce.Payment.PayEx.UnitTests.Models.Result
{
    [TestFixture]
    internal class CompleteResultTests
    {
        [TestCase(TransactionStatus.Initialize, true, true)]
        [TestCase(TransactionStatus.Authorize, true, true)]
        [TestCase(TransactionStatus.Initialize, false, false)]
        [TestCase(TransactionStatus.Authorize, false, false)]
        [TestCase(TransactionStatus.Failure, true, false)]
        [TestCase(TransactionStatus.Failure, false, false)]
        public void Success_ReturnsCorrectResult(TransactionStatus transactionStatus, bool statusSuccess, bool expected)
        {
            Status status = Factory.CreateStatus(statusSuccess);

            CompleteResult completeResult = new CompleteResult { Status = status, TransactionStatus = transactionStatus };
            Assert.AreEqual(expected, completeResult.Success);
        }

        [TestCase(TransactionStatus.Initialize, true)]
        [TestCase(TransactionStatus.Authorize, false)]
        public void GetTransactionDetails_ReturnsCorrectResult(TransactionStatus transactionStatus, bool expected)
        {
            CompleteResult completeResult = new CompleteResult { TransactionStatus = transactionStatus };
            Assert.AreEqual(expected, completeResult.GetTransactionDetails);
        }
    }
}
