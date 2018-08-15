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
            var transactions = CreateListOfTransactionsWithTransactionRingOfSize(ringSize, Guid.NewGuid());

            var calculatedResult = MoneySwap.GetMoneySwapResultOfTransactions(transactions);

            Assert.AreEqual(result, calculatedResult, $"MoneySwap result is incorrect. Ring of size {ringSize} should yield {result}");
        }

        [TestMethod]
        public void Test_Stuivertje_Wisselen_With_Sender_In_Multiple_Rings_Works()
        {
            var initiatorGuid = Guid.NewGuid();

            var firstRingTransactions = CreateListOfTransactionsWithTransactionRingOfSize(2, initiatorGuid);
            var secondRingTransactions = CreateListOfTransactionsWithTransactionRingOfSize(2, initiatorGuid);

            var allTransactions = firstRingTransactions.Concat(secondRingTransactions).ToList();

            var calculatedResult = MoneySwap.GetMoneySwapResultOfTransactions(allTransactions);

            Assert.AreEqual(400, calculatedResult, $"MoneySwap result is incorrect with multiple rings");
        }

        [TestMethod]
        public void Test_Stuivertje_Wisselen_Takes_Largest_Ring_And_Does_Not_Use_Transactions_Twice()
        {
            var largestRingSize = 4;
            var initiatorGuid = Guid.NewGuid();

            var firstRingTransactions = CreateListOfTransactionsWithTransactionRingOfSize(largestRingSize, initiatorGuid);

            var extraTransaction = new OutgoingTransaction {
                Id = Guid.NewGuid(),
                Amount = 500,
                SenderWalletId = firstRingTransactions[largestRingSize - 2].SenderWalletId,
                ReceiverWalletId = initiatorGuid,
                RoundNumber = 1,
                Timestamp = DateTime.UtcNow
            };

            var allTransactions = firstRingTransactions.Append(extraTransaction).ToList();

            var calculatedResult = MoneySwap.GetMoneySwapResultOfTransactions(allTransactions);

            Assert.AreEqual(600, calculatedResult, $"MoneySwap result does not correctly take the largest ring");
        }

        [TestMethod]
        public void Test_Stuivertje_Wisselen_Does_Not_Include_A_Sender_Twice()
        {
            var initiatorGuid = Guid.NewGuid();

            var firstRingTransactions = CreateListOfTransactionsWithTransactionRingOfSize(2, initiatorGuid);
            var secondRingTransactions = CreateListOfTransactionsWithTransactionRingOfSize(2, initiatorGuid);
            var thirdRingTransactions = CreateListOfTransactionsWithTransactionRingOfSize(2, initiatorGuid);

            var allTransactions = firstRingTransactions
                .Concat(secondRingTransactions)
                .Concat(thirdRingTransactions)
                .ToList();

            var calculatedResult = MoneySwap.GetMoneySwapResultOfTransactions(allTransactions);

            Assert.AreEqual(600, calculatedResult, $"MoneySwap result is including a sender twice");
        }

        [TestMethod]
        public void Test_Stuivertje_Wisselen_Does_Not_Have_Daans_Bug_Anymore()
        {
            var firstGuid = Guid.NewGuid();
            var secondGuid = Guid.NewGuid();
            var thirdGuid = Guid.NewGuid();
            var fourthGuid = Guid.NewGuid();

            var firstRingTransactions = CreateListOfTwoTransactionsBetweenTwoWalletGuids(firstGuid, secondGuid);
            var secondRingTransactions = CreateListOfTwoTransactionsBetweenTwoWalletGuids(secondGuid, thirdGuid);
            var thirdRingTransactions = CreateListOfTwoTransactionsBetweenTwoWalletGuids(thirdGuid, fourthGuid);

            var allTransactions = firstRingTransactions
                .Concat(secondRingTransactions)
                .Concat(thirdRingTransactions)
                .ToList();

            var calculatedResult = MoneySwap.GetMoneySwapResultOfTransactions(allTransactions);

            Assert.AreEqual(600, calculatedResult, $"MoneySwap result is incorrect: Daan's bug");
        }

        private List<OutgoingTransaction> CreateListOfTwoTransactionsBetweenTwoWalletGuids(Guid firstGuid, Guid secondGuid)
        {
            return new List<OutgoingTransaction> {
                new OutgoingTransaction {
                    Id = Guid.NewGuid(),
                    Amount = 500,
                    RoundNumber = 1,
                    Timestamp = DateTime.UtcNow,
                    SenderWalletId = firstGuid,
                    ReceiverWalletId = secondGuid
                },
                new OutgoingTransaction {
                    Id = Guid.NewGuid(),
                    Amount = 500,
                    RoundNumber = 1,
                    Timestamp = DateTime.UtcNow,
                    SenderWalletId = secondGuid,
                    ReceiverWalletId = firstGuid
                }
            };
        }

        private List<OutgoingTransaction> CreateListOfTransactionsWithTransactionRingOfSize(int ringSize, Guid initiatorWalletId)
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

            output.Last().ReceiverWalletId = initiatorWalletId;
            output.First().SenderWalletId = initiatorWalletId;

            return output;
        }
    }
}