using System;

namespace DeMol2018.BitcoinGame.GameServer.ResultObjects
{
    public class LoginResult
    {
        public bool LoginSuccessful { get; set; }
        public bool IsAdmin { get; set; }
        public Guid PlayerGuid { get; set; }
        public UpdatedStateResult UpdatedState { get; set; }
    }
}