using System;
using System.Collections.Generic;

namespace MetadataLibrary
{

    [Serializable]
    public struct KeywordTag
    {
        

        public string Keyword { get; set; }

        private float confidence;

        public float Confidence 
        {
            get { return confidence; }
            set { confidence = (float)Math.Round((float)value, SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimals); }
        }

        public KeywordTag(KeywordTag keyword)
        {
            Keyword = keyword.Keyword;
            confidence = (float)Math.Round(keyword.Confidence, SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimals); 
        }

        public KeywordTag(string keyword)
        {
            this.Keyword = keyword;
            this.confidence = 1.0F;
        }

        public KeywordTag(string keyword, float confidence)
        {
            this.Keyword = keyword;
            this.confidence = (float)Math.Round(confidence, SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimals);
        }

        public override bool Equals(object obj)
        {
            return obj is KeywordTag tag && Keyword == tag.Keyword;
        }

        public override int GetHashCode()
        {
            var hashCode = -2013257350;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Keyword);
            return hashCode;
        }

        public static bool operator ==(KeywordTag left, KeywordTag right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(KeywordTag left, KeywordTag right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return this.Keyword;
        }
    }
}
