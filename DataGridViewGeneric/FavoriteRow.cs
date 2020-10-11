using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataGridViewGeneric
{
    public class FavoriteRow
    {
        public FavoriteRow(string headerName, string rowName, bool isHeader)
        {
            HeaderName = headerName;
            RowName = rowName;
            IsHeader = isHeader;
        }

        [JsonProperty("HeaderName")]
        public string HeaderName { get; set; }
        [JsonProperty("RowName")]
        public string RowName { get; set; }
        [JsonProperty("IsHeader")]
        public bool IsHeader { get; set; }

        public override bool Equals(object obj)
        {
            return obj is FavoriteRow row &&
                   HeaderName == row.HeaderName &&
                   RowName == row.RowName &&
                   IsHeader == row.IsHeader;
        }

        public override int GetHashCode()
        {
            int hashCode = 1181777091;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(HeaderName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RowName);
            hashCode = hashCode * -1521134295 + IsHeader.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(FavoriteRow left, FavoriteRow right)
        {
            return EqualityComparer<FavoriteRow>.Default.Equals(left, right);
        }

        public static bool operator !=(FavoriteRow left, FavoriteRow right)
        {
            return !(left == right);
        }
    }
}
