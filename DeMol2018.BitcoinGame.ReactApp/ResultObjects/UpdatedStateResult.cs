using System;

namespace DeMol2018.BitcoinGame.ReactApp.ResultObjects
{
    public class UpdatedStateResult
    {
        public int UserWalletAddress { get; set; }
        public int UserCurrentBalance { get; set; }
        public int? CurrentRoundNumber { get; set; }
        public string CurrentRoundEndTime { get; set; }
    }
}