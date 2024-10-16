namespace DeMol2018.BitcoinGame.GameServer.ResultObjects
{
    public class FetchNewGameStateResult
    {
        public bool CallSuccessful { get; set; }
        public UpdatedStateResult UpdatedState { get; set; }
    }
}