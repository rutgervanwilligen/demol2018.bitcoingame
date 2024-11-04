namespace DeMol2018.BitcoinGame.GameServer.ResultObjects;

public record MakeTransactionResult
{
    public bool TransactionSuccessful { get; set; }
    public int? UserCurrentBalance { get; set; }
}