using System;

namespace DeMol2018.BitcoinGame.ReactApp.ResultObjects
{
    public class StartNewRoundResult
    {
        public bool callSuccessful { get; set; }
        public int? newRoundNumber { get; set; }
        public string newRoundEndTime { get; set; }
    }
}