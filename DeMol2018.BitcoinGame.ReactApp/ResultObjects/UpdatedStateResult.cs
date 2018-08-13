using System;

namespace DeMol2018.BitcoinGame.ReactApp.ResultObjects
{
    public class UpdatedStateResult
    {
        public int? UserWalletAddress { get; set; }
        public int? UserCurrentBalance { get; set; }
        public Guid? CurrentGameId { get; set; }
        public int? LastRoundNumber { get; set; }
        public int? CurrentRoundNumber { get; set; }
        public string CurrentRoundEndTime { get; set; }
        public NonPlayerWalletResult[] NonPlayerWallets { get; set; }
        public int MoneyWonSoFar { get; set; }
        public bool? GameHasFinished { get; set; }
        public int? NumberOfJokersWon { get; set; }
    }
}