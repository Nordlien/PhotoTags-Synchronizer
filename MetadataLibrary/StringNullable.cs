using System;
using System.Collections.Generic;


namespace MetadataLibrary
{
    public class StringNullable : IComparable<StringNullable>, IEquatable<StringNullable>
    {
        public StringNullable(string fullFilename)
        {
            StringValue = fullFilename;
        }

        public string StringValue { get; set; }

        public override bool Equals(object other)
        {
            return this.Equals(other as FileEntry);
        }

        public bool Equals(StringNullable other)
        {
            if (other is null) return false; // If parameter is null, return false.
            if (Object.ReferenceEquals(this, other)) return true; // Optimization for a common success case.
            if (this.GetType() != other.GetType()) return false; // If run-time types are not exactly the same, return false. Due to compare of FileEntryImage, FileEntryBroker
            
            return StringValue == other.StringValue; 
        }

        public override int GetHashCode()
        {
            int hashCode = -975839579;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StringValue);
            return hashCode;
        }

        public int CompareTo(StringNullable other)
        {
            int compare = 0;

            if (StringValue == null) 
            {
                if (other.StringValue != null) compare = -1;           //this.Fullname ==  null and other.Fullname NOT null
            } 
            else if (other.StringValue == null) compare = 1;         //this.Fullname NOT null and other.Fullname ==  null                
            else compare = StringValue.CompareTo(other.StringValue);  //both != NULL, then do normale string compare

            return compare;
        }

        public static bool operator ==(StringNullable left, StringNullable right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(StringNullable left, StringNullable right)
        {
            return !(left == right);
        }
    }


}
