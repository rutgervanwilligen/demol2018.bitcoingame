using System;
using System.Collections.Generic;

namespace DeMol2018.BitcoinGame.Application.Services.StuivertjeWisselen
{
    public class SearchResult : IEquatable<SearchResult>
    {
        public int Depth => TransactionGuids.Count;
        public HashSet<Guid> TransactionGuids { get; set; }

        public override int GetHashCode()
        {
            return Depth;
        }

        public bool Equals(SearchResult other)
        {
            return other != null && TransactionGuids.SetEquals(other.TransactionGuids);
        }
    }
}