using DataGridViewGeneric;
using LocationNames;
using MetadataLibrary;
using MetadataPriorityLibrary;
using System;
using System.Collections.Generic;
using WinProps;

namespace DataGridViewGeneric
{
    public class RowIndenifier : IEquatable<RowIndenifier>
    {
        public RowIndenifier() : this (null, null)
        {
        }
        public RowIndenifier(string headerName) : this (headerName, "")
        {
        }

        public RowIndenifier(string headerName, string rowName)
        {
            HeaderName = headerName;
            RowName = rowName;
        }

        public string HeaderName { get; set; } = null;
        public string RowName { get; set; } = null;

        public override bool Equals(object obj)
        {
            return Equals(obj as RowIndenifier);
        }

        public bool Equals(RowIndenifier other)
        {
            return other != null &&
                   HeaderName == other.HeaderName &&
                   RowName == other.RowName;
        }

        public override int GetHashCode()
        {
            int hashCode = -480052933;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(HeaderName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RowName);
            return hashCode;
        }

        public static bool operator ==(RowIndenifier left, RowIndenifier right)
        {
            return EqualityComparer<RowIndenifier>.Default.Equals(left, right);
        }

        public static bool operator !=(RowIndenifier left, RowIndenifier right)
        {
            return !(left == right);
        }
    }
    public class DataGridViewGenericRow : IEquatable<DataGridViewGenericRow>
    {
        public RowIndenifier RowIndenifier { get; set; } = new RowIndenifier();
        public string HeaderName
        {
            get { return RowIndenifier.HeaderName; }
            set { RowIndenifier.HeaderName = value; }
        }
        public string RowName 
        { 
            get { return RowIndenifier.RowName; } 
            set { RowIndenifier.RowName = value; } 
        }

        public ReadWriteAccess ReadWriteAccess { get; set; }
        public bool IsHeader { get; set; }
        public bool IsMultiLine { get; set; }
        public bool IsFavourite { get; set; }
        public bool IsEqual { get; set; }
        public PropertyKey PropertyKey { get; set; }
        public Metadata Metadata { get; set; }
        public FileEntryAttribute FileEntryAttribute { get; set; }
        public LocationCoordinate LocationCoordinate { get; set; }
        public MetadataPriorityKey MetadataPriorityKey { get; set; }

        public FavoriteRow GetFavouriteRow()
        {
            return new FavoriteRow(HeaderName, RowName, IsHeader);
        }

        public DataGridViewGenericRow(string headerName) 
            : this(headerName, "", ReadWriteAccess.ForceCellToReadOnly, true, false, null, null, null, null, null) { }
        public DataGridViewGenericRow(string headerName, string rowName) 
            : this(headerName, rowName, ReadWriteAccess.AllowCellReadAndWrite, false, false, null, null, null, null, null) { }

        public DataGridViewGenericRow(string headerName, string rowName, ReadWriteAccess readWriteAccess) 
            : this(headerName, rowName, readWriteAccess, false, false, null, null, null, null, null) { }
        
        public DataGridViewGenericRow(string headerName, string rowName, bool isMultiLine, MetadataPriorityKey metadataPriorityKey) 
            : this(headerName, rowName, ReadWriteAccess.AllowCellReadAndWrite, false, isMultiLine, null, null, null, metadataPriorityKey, null) { }

        public DataGridViewGenericRow(string headerName, string rowName, ReadWriteAccess readWriteAccess, bool isMultiLine, PropertyKey propertyKey)
            : this(headerName, rowName, readWriteAccess, false, isMultiLine, propertyKey, null, null, null, null) { }

        public DataGridViewGenericRow(string headerName, string rowName, Metadata metadata, FileEntryAttribute fileEntryAttribute)
            : this(headerName, rowName, ReadWriteAccess.AllowCellReadAndWrite, false, false, null, metadata, fileEntryAttribute, null, null) { }

        public DataGridViewGenericRow(string headerName, string rowName, LocationCoordinate locationCoordinate)
            : this(headerName, rowName, ReadWriteAccess.AllowCellReadAndWrite, false, false, null, null, null, null, locationCoordinate) { }

        public DataGridViewGenericRow(string headerName, string rowName, MetadataPriorityKey metadataPriorityKey)
            : this(headerName, rowName, ReadWriteAccess.AllowCellReadAndWrite, false, false, null, null, null, metadataPriorityKey, null) { }
        

        private DataGridViewGenericRow(string headerName, string rowName, ReadWriteAccess readWriteAcess, bool isHeader, bool isMultiLine, PropertyKey propertyKey, 
            Metadata metadata, FileEntryAttribute fileEntryAttribute,
            MetadataPriorityKey metadataPriorityKey, LocationCoordinate locationCoordinate)
        {
            this.HeaderName = headerName ?? throw new ArgumentNullException(nameof(rowName));
            this.RowName = rowName == null ? "" : rowName;  
            this.ReadWriteAccess = readWriteAcess;
            this.IsHeader = isHeader;
            this.IsEqual = false;
            this.IsFavourite = false;
            this.IsMultiLine = isMultiLine;
            this.PropertyKey = propertyKey;
            this.Metadata = metadata;
            this.FileEntryAttribute = fileEntryAttribute;
            this.MetadataPriorityKey = metadataPriorityKey;
            this.LocationCoordinate = locationCoordinate;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DataGridViewGenericRow);
        }

        public bool Equals(DataGridViewGenericRow other)
        {
            return other != null &&
                HeaderName == other.HeaderName &&
                RowName == other.RowName &&                   
                IsHeader == other.IsHeader;
        }

        public override int GetHashCode()
        {
            int hashCode = -970052756;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(HeaderName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RowName);
            return hashCode;
        }

        public override string ToString()
        {
            return HeaderName + "/" + RowName; 
        }

        public static bool operator ==(DataGridViewGenericRow left, DataGridViewGenericRow right)
        {
            return EqualityComparer<DataGridViewGenericRow>.Default.Equals(left, right);
        }

        public static bool operator !=(DataGridViewGenericRow left, DataGridViewGenericRow right)
        {
            return !(left == right);
        }
    }
}
