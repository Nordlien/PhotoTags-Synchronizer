using System;
using System.Collections.Generic;

namespace MetadataLibrary
{

    [Serializable]
    public struct KeywordTag
    {
        public string Keyword { get; set; }
        public double Confidence { get; set; }

        public KeywordTag(string keyword) : this()
        {
            Keyword = keyword;
            Confidence = 1F;
        }

        public KeywordTag(string keyword, double confidence) : this()
        {
            Keyword = keyword;
            Confidence = confidence;
        }

        public override bool Equals(object obj)
        {
            return obj is KeywordTag tag &&
                   Keyword == tag.Keyword;
                    //&& Confidence == tag.Confidence;
        }

        public override int GetHashCode()
        {
            var hashCode = -2013257350;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Keyword);
            //hashCode = hashCode * -1521134295 + Confidence.GetHashCode();
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
