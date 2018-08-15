using System;
using System.Collections.Generic;
using System.Linq;
using DeMol2018.BitcoinGame.Application.Services.StuivertjeWisselen;
using DeMol2018.BitcoinGame.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeMol2018.BitcoinGame.Application.Tests
{
    [TestClass]
    public class MoneySwapTests
    {
        [DataTestMethod]
        [DataRow(2, 200)]
        [DataRow(3, 400)]
        [DataRow(4, 600)]
        [DataRow(5, 800)]
        [DataRow(6, 1000)]
        [DataRow(7, 1200)]
        [DataRow(8, 1400)]
        [DataRow(9, 1600)]
        [DataRow(10, 1800)]
        public void Test_Stuivertje_Wisselen_With_Single_Ring_Works(int ringSize, int result)
        {
            var transactions = CreateListOfTransactionsWithTransactionRingOfSize(ringSize);

            var calculatedResult = MoneySwap.GetMoneySwapResultOfTransactions(transactions);

            Assert.AreEqual(result, calculatedResult, $"MoneySwap result is incorrect. Ring of size {ringSize} should yield {result}");
        }

        private List<OutgoingTransaction> CreateListOfTransactionsWithTransactionRingOfSize(int ringSize)
        {
            var inBetweenSenderGuid = Guid.NewGuid();
            var inBetweenReceiverGuid = Guid.NewGuid();

            var output = new List<OutgoingTransaction>();

            for (var i = 0; i < ringSize; i++)
            {
                output.Add(new OutgoingTransaction {
                    Id = Guid.NewGuid(),
                    Amount = 500,
                    RoundNumber = 1,
                    Timestamp = DateTime.UtcNow,
                    SenderWalletId = inBetweenSenderGuid,
                    ReceiverWalletId = inBetweenReceiverGuid
                });

                inBetweenSenderGuid = inBetweenReceiverGuid;
                inBetweenReceiverGuid = Guid.NewGuid();
            }

            output.Last().ReceiverWalletId = output.First().SenderWalletId;

            return output;
        }
    }
}