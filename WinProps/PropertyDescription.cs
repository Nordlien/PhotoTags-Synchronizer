using System;
using System.Runtime.InteropServices;
using static WinProps.PInvoke;

namespace WinProps {

	/// <summary>
	/// Encapsulates the <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb761561(v=vs.85).aspx">IPropertyDescription</a> interface.
	/// Provides details about the type and display of a Windows Property.
	/// </summary>
	public class PropertyDescription : IDisposable {
		#region Enums required
		/// <summary>
		/// Defines how property should aligned when it is displayed.
		/// </summary>
		public enum Alignment {
			/// <summary>
			/// Left-align the property value
			/// </summary>
			Left,
			/// <summary>
			/// Centre-align the property value
			/// </summary>
			Centre,
			/// <summary>
			/// Right-align the property value
			/// </summary>
			Right
		}
		/// <summary>
		/// Defines the type of property to display
		/// </summary>
		public enum Display {
			/// <summary>
			/// Display the property value as a string
			/// </summary>
			String = 0,
			/// <summary>
			/// Display the property value as a number
			/// </summary>
			Numeric = 1,
			/// <summary>
			/// Display the property value as a boolean
			/// </summary>
			Boolean = 2,
			/// <summary>
			/// Display the property value as a date and time
			/// </summary>
			DateTime = 3,
			/// <summary>
			/// Display the property as an enumerated value
			/// </summary>
			Enumerated = 4
		}
		/// <summary>
		/// Defines the type of the ListView column when shown in explorer
		/// </summary>
		public enum ColumnType {
			/// <summary>
			/// The column displays string types
			/// </summary>
			String = 0,
			/// <summary>
			/// The column displays integer types
			/// </summary>
			Integer = 1,
			/// <summary>
			/// The column displays date and time types
			/// </summary>
			DateTime = 2
		}
		/// <summary>
		/// Defines how the properties should be sorted
		/// </summary>
		[Flags]
		public enum ColumnSortFlags {
			/// <summary>
			/// Compares each value of the properties
			/// </summary>
			ValueCompare = 0x200,
			/// <summary>
			/// Compares the strings when the property has been formatted for display
			/// </summary>
			FormattedCompare = 0x400,
			/// <summary>
			/// Tells the sorting procedure not to sort folders separately
			/// </summary>
			NoFolderness = 0x800
		}
		/// <summary>
		/// Defines how a column's width is handled
		/// </summary>
		[Flags]
		public enum ColumnWidthFlags {
			/// <summary>
			/// The column is a fixed width, and cannot be resized
			/// </summary>
			FixedWidth = 0x1000,
			/// <summary>
			/// The column is the same width, regardless of the device resolution
			/// </summary>
			NoDPIScale = 0x2000,
			/// <summary>
			/// The column maintains a fixed ratio of width to height
			/// </summary>
			FixedRatio = 0x4000
		}
		/// <summary>
		/// Defines a user-friendly terminology to be used when two properties are compared with each other.
		/// Each element defines three terms: one for less-then, one for equal to, and one for greater than.
		/// </summary>
		public enum RelativeDescriptions {
			/// <summary>
			/// Different/Same/Different
			/// </summary>
			General = 0,
			/// <summary>
			/// Earlier/Same/Later
			/// </summary>
			Date = 1,
			/// <summary>
			/// Smaller/Same/Larger
			/// </summary>
			Size = 2,
			/// <summary>
			/// Smaller/Same/Larger
			/// </summary>
			Count = 3,
			/// <summary>
			/// Earlier/Same/Later
			/// </summary>
			Revision = 4,
			/// <summary>
			/// Shorter/Same/Longer
			/// </summary>
			Length = 5,
			/// <summary>
			/// Shorter/Same/Longer
			/// </summary>
			Duration = 6,
			/// <summary>
			/// Slower/Same/Faster
			/// </summary>
			Speed = 7,
			/// <summary>
			/// Slower/Same/Faster
			/// </summary>
			Rate = 8,
			/// <summary>
			/// Lower/Same/Higher
			/// </summary>
			Rating = 9,
			/// <summary>
			/// Lower/Same/Higher
			/// </summary>
			Priority = 10,
		}
		/// <summary>
		/// Provides a user-friendly description on how sorting should be performed. Each item provides a description for ascending or descending order
		/// </summary>
		public enum SortMethod {
			/// <summary>
			/// "Sort going up"/"Sort going down"
			/// </summary>
			General = 0,
			/// <summary>
			/// "A on top"/"Z on top"
			/// </summary>
			Alphbetical = 1,
			/// <summary>
			/// "Lowest on top"/"Highest on top"
			/// </summary>
			LowestToHighest = 2,
			/// <summary>
			/// "Smallest on top"/"Largest on top"
			/// </summary>
			SmallestToBiggest = 3,
			/// <summary>
			/// "Oldest on top"/"Newest on top"
			/// </summary>
			OldestToNewest = 4
		}

		/// <summary>
		/// Specifies how aggregated properties should be displayed when multiple items are selected.
		/// </summary>
		public enum AggregateMethod {
			/// <summary>
			/// Displays "Multiple Values"
			/// </summary>
			Default = 0,
			/// <summary>
			/// Displays the first property in the collection
			/// </summary>
			First = 1,
			/// <summary>
			/// Displays the sum of all the properties in the collection
			/// </summary>
			Sum = 2,
			/// <summary>
			/// Displays the average pf all the properties in the collection
			/// </summary>
			Average = 3,
			/// <summary>
			/// Displays a date range representing the properties in the collection
			/// </summary>
			DateRange = 4,
			/// <summary>
			/// Displays a union of all the properties in the collection
			/// </summary>
			Union = 5,
			/// <summary>
			/// Displays the maximum value of the properties in the collection
			/// </summary>
			Max = 6,
			/// <summary>
			/// Displays the minimum value of the properties in the collection
			/// </summary>
			Min = 7
		}

		/// <summary>
		/// A set of flags that tell the system how to format a property for display
		/// </summary>
		[Flags]
		public enum FormatFlags {
			/// <summary>
			/// Uses the formatting defined by the <see cref="PropertyDescription"/>
			/// </summary>
			Default = 0x00000000,
			/// <summary>
			/// Prefix the value with the property name
			/// </summary>
			PrefixName = 0x00000001,
			/// <summary>
			/// Treat the property value as a file name
			/// </summary>
			FileName = 0x00000002,
			/// <summary>
			/// Always format byte sizes as KB
			/// </summary>
			AlwaysKB = 0x00000004,
			//PDFF_RESERVED_RIGHTTOLEFT = 0x00000008,   // Reserved for legacy use.
			/// <summary>
			/// Format time as "h:mm tt"
			/// </summary>
			ShortTime = 0x00000010,
			/// <summary>
			/// Format time as "h:mm:ss tt"
			/// </summary>
			LongTime = 0x00000020,
			/// <summary>
			/// Hide the time portion of a DateTime
			/// </summary>
			HideTime = 0x00000040,
			/// <summary>
			/// Format the date as "M/dd/yy"
			/// </summary>
			ShortDate = 0x00000080,
			/// <summary>
			/// Formats the date as "dddd, MMMM d, yyyy"
			/// </summary>
			LongDate = 0x00000100,
			/// <summary>
			/// Hides the date portion of a DateTime
			/// </summary>
			HideDate = 0x00000200,
			/// <summary>
			/// Uses a friendly desction of a relative date e.g. "Yesterday"
			/// </summary>
			RelativeDate = 0x00000400,
			/// <summary>
			/// Use the <see cref="EditInvitation"/> text if the property is empty or failed to be retrieved
			/// </summary>
			UseEditInvitation = 0x00000800,
			/// <summary>
			/// Use a read-only format, and fill with default text if empty
			/// </summary>
			ReadOnly = 0x00001000,
			/// <summary>
			/// Don't detect the reading order automatically. 
			/// </summary>
			NoAutoReadingOrder = 0x00002000, 
		}
		#endregion

		IPropertyDescription _propDescription = null;

		/// <summary>
		/// Gets a <see cref="PropertyDescription"/> for the property represented by a canonical name.
		/// </summary>
		/// <param name="CanonicalName">The canonical name of the property for which the <see cref="PropertyDescription"/> is requested</param>
		/// <example>new PropertyDescription("System.Title")</example>
		public PropertyDescription(string CanonicalName) {
			IntPtr ppv;
			HRESULT hr = PSGetPropertyDescriptionByName(CanonicalName, IID.IPropertyDescription, out ppv);
			if (hr.Failed) throw hr.GetException();
			try {
				_propDescription = (IPropertyDescription)Marshal.GetUniqueObjectForIUnknown(ppv);
			}
			finally {
				Marshal.Release(ppv);
			}
		}


		/// <summary>
		/// Gets a <see cref="PropertyDescription"/> for the specified <see cref="PropertyKey"/> 
		/// </summary>
		/// <param name="key">The <see cref="PropertyKey"/> for which the <see cref="PropertyDescription"/> is required
		/// </param>
		public PropertyDescription(PropertyKey key) {
			IntPtr ppv;
			HRESULT hr = PSGetPropertyDescription(key.MarshalledPointer, IID.IPropertyDescription, out ppv);
//if (hr.Failed) throw hr.GetException();
			if (!hr.Failed)
			{
				try
				{
					_propDescription = (IPropertyDescription)Marshal.GetUniqueObjectForIUnknown(ppv);
				}
				finally
				{
					Marshal.Release(ppv);
				}
			} else 
			{

            }
		}

		internal PropertyDescription(IntPtr pUnk) {
			_propDescription = (IPropertyDescription)Marshal.GetUniqueObjectForIUnknown(pUnk);
		}

		/// <summary>
		/// Gets the <see cref="PropertyKey"/> that this <see cref="PropertyDescription"/>  defines.
		/// </summary>
		public PropertyKey PropertyKey {
			get {
				IntPtr ppk = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPERTYKEY)));
				_propDescription.GetPropertyKey(ppk);
				return new PropertyKey(ppk);
			}
		}

		/// <summary>
		/// Gets the <see cref="AggregateMethod"/> to perform if the property is aggregated.
		/// </summary>
		public AggregateMethod AggregateType {
			get {
				PROPDESC_AGGREGATION_TYPE method;
				_propDescription.GetAggregationType(out method);
				return (AggregateMethod)method;
			}
		}

		/// <summary>
		/// Gets the canonical name of the property described by this <see cref="PropertyDescription"/> 
		/// </summary>
		public string CanonicalName {
			get {
				string name;
				_propDescription.GetCanonicalName(out name);
				return name;
			}
		}

		/// <summary>
		/// Returns the <see cref="VarEnum"/> type of the <see cref="PropVariant"/> used to represent the value of the property.
		/// </summary>
		public VarEnum PropertyType {
			get {
				VARENUM vt;
				_propDescription.GetPropertyType(out vt);
				return (VarEnum)vt;
			}
		}

		/// <summary>
		/// Returns a user-friendly name for a prompt for the property.
		/// </summary>
		public string DisplayName {
			get {
				string name;
				_propDescription.GetDisplayName(out name);
				return name;
			}
		}

		/// <summary>
		/// This text can be used as a descriptive prompt for the entry of a property
		/// </summary>
		public string EditInvitation {
			get {
				string value;
				_propDescription.GetEditInvitation(out value);
				return value;
			}
		}

		/// <summary>
		/// Describes a set of values that describe the type of data contained in a property
		/// </summary>
		public class _TypeFlags {
			PROPDESC_TYPE_FLAGS _flags;

			internal _TypeFlags(PROPDESC_TYPE_FLAGS flags) { _flags = flags; }

			/// <summary>
			/// Describes a property that may have multiple values
			/// </summary>
			public bool IsMultiValued { get { return _flags.HasFlag(PROPDESC_TYPE_FLAGS.PDTF_MULTIPLEVALUES); } }
			/// <summary>
			/// Describes a property where the value is generally defined by the data in the file itself, or by the application that creates the file
			/// </summary>
			public bool IsInnate { get { return _flags.HasFlag(PROPDESC_TYPE_FLAGS.PDTF_ISINNATE); } }
			/// <summary>
			/// This property defines a group heading. It does not have a value in itself per se.
			/// </summary>
			public bool IsGroup { get { return _flags.HasFlag(PROPDESC_TYPE_FLAGS.PDTF_ISGROUP); } }
			/// <summary>
			/// Indicates that items can be grouped by the value in this property
			/// </summary>
			public bool CanGroupBy { get { return _flags.HasFlag(PROPDESC_TYPE_FLAGS.PDTF_CANGROUPBY); } }
			/// <summary>
			/// Indicates that the user can stack by this property.
			/// </summary>
			public bool CanStackBy { get { return _flags.HasFlag(PROPDESC_TYPE_FLAGS.PDTF_CANSTACKBY); } }
			/// <summary>
			/// Indicates that this property contains a hierarchy.
			/// </summary>
			public bool IsTreeProperty { get { return _flags.HasFlag(PROPDESC_TYPE_FLAGS.PDTF_ISTREEPROPERTY); } }
			/// <summary>
			/// Indicates if a property is intended to be viewable. This flag is used in places such as the selection of columns in Explorer.
			/// </summary>
			public bool IsViewable { get { return _flags.HasFlag(PROPDESC_TYPE_FLAGS.PDTF_ISVIEWABLE); } }
			/// <summary>
			/// Allows an innate property to be purged. Only valid in Vista SP1 and later.
			/// </summary>
			public bool CanBePurged { get { return _flags.HasFlag(PROPDESC_TYPE_FLAGS.PDTF_CANBEPURGED); } }
			/// <summary>
			/// Uses the unformatted value of the property for searching
			/// </summary>
			public bool SearchRawValue { get { return _flags.HasFlag(PROPDESC_TYPE_FLAGS.PDTF_SEARCHRAWVALUE); } }
			/// <summary>
			/// If set, empty strings are not coerced to a VT_NULL or VT_EMPTY type.
			/// </summary>
			public bool DontCoerceEmptyStrings { get { return _flags.HasFlag(PROPDESC_TYPE_FLAGS.PDTF_DONTCOERCEEMPTYSTRINGS); } }
			/// <summary>
			/// This property is owned by the system
			/// </summary>
			public bool IsSystemProperty { get { return _flags.HasFlag(PROPDESC_TYPE_FLAGS.PDTF_ISSYSTEMPROPERTY); } }
		}

		_TypeFlags _typeFlags = null;
		/// <summary>
		/// Represents a collection of values that define the type of data maintained by the property
		/// </summary>
		public _TypeFlags TypeFlags {
			get {
				if (_typeFlags == null) {
					PROPDESC_TYPE_FLAGS flags;
					_propDescription.GetTypeFlags(PROPDESC_TYPE_FLAGS.PDTF_MASK_ALL, out flags);
					_typeFlags = new _TypeFlags(flags);
				}
				return _typeFlags;
			}
		}

		/// <summary>
		/// Describes a set of values that describe how a property should be displayed
		/// </summary>
		public class _ViewFlags {
			PROPDESC_VIEW_FLAGS _flags;
			internal _ViewFlags(PROPDESC_VIEW_FLAGS flags) { _flags = flags; }

			/// <summary>
			/// Defines how a property should be aligned within a column. <seealso cref="PropertyDescription.Alignment"/>
			/// </summary>
			public Alignment Alignment {
				get {
					switch (_flags & (PROPDESC_VIEW_FLAGS.PDVF_CENTERALIGN | PROPDESC_VIEW_FLAGS.PDVF_RIGHTALIGN)) {
					case PROPDESC_VIEW_FLAGS.PDVF_RIGHTALIGN:
						return Alignment.Right;
					case PROPDESC_VIEW_FLAGS.PDVF_CENTERALIGN:
						return Alignment.Centre;
					default:
						return Alignment.Left;
					}
				}
			}

			/// <summary>
			/// This property is shown at the beginning of the next set of properties.
			/// </summary>
			public bool BeginNewGroup { get { return _flags.HasFlag(PROPDESC_VIEW_FLAGS.PDVF_BEGINNEWGROUP); } }
			/// <summary>
			/// Fills the remainder of the view area with the contents of this property.
			/// </summary>
			public bool FillArea { get { return _flags.HasFlag(PROPDESC_VIEW_FLAGS.PDVF_FILLAREA); } }
			/// <summary>
			/// Sort this property is reverse order
			/// </summary>
			public bool SortDescending { get { return _flags.HasFlag(PROPDESC_VIEW_FLAGS.PDVF_SORTDESCENDING); } }
			/// <summary>
			/// Only display this property if it is present
			/// </summary>
			public bool ShowOnlyIfPresent { get { return _flags.HasFlag(PROPDESC_VIEW_FLAGS.PDVF_SHOWONLYIFPRESENT); } }
			/// <summary>
			/// Show this property by default
			/// </summary>
			public bool ShowByDefault { get { return _flags.HasFlag(PROPDESC_VIEW_FLAGS.PDVF_SHOWBYDEFAULT); } }
			/// <summary>
			/// This property should be shown in the primary column UI by default
			/// </summary>
			public bool ShowInPrimaryList { get { return _flags.HasFlag(PROPDESC_VIEW_FLAGS.PDVF_SHOWINPRIMARYLIST); } }
			/// <summary>
			/// This property should be shown in the secondary column UI by default
			/// </summary>
			public bool ShowInSecondaryList { get { return _flags.HasFlag(PROPDESC_VIEW_FLAGS.PDVF_SHOWINSECONDARYLIST); } }
			/// <summary>
			/// Hide the label if the view would normally display one.
			/// </summary>
			public bool HideLabel { get { return _flags.HasFlag(PROPDESC_VIEW_FLAGS.PDVF_HIDELABEL); } }
			/// <summary>
			/// Do not display this property in the column UI
			/// </summary>
			public bool Hidden { get { return _flags.HasFlag(PROPDESC_VIEW_FLAGS.PDVF_HIDDEN); } }
			/// <summary>
			/// This property can be wrapped across more than one row
			/// </summary>
			public bool CanWrap { get { return _flags.HasFlag(PROPDESC_VIEW_FLAGS.PDVF_CANWRAP); } }
		}

		_ViewFlags _vFlags = null;
		/// <summary>
		/// Represents a set of values that describe how a property should be displayed
		/// </summary>
		public _ViewFlags ViewFlags {
			get {
				if (_vFlags == null) {
					PROPDESC_VIEW_FLAGS f;
					_propDescription.GetViewFlags(out f);
					_vFlags = new _ViewFlags(f);
				}
				return _vFlags;
			}
		}

		/// <summary>
		/// Gets the default width of the column, in characters.
		/// </summary>
		public int DefaultColumnWidth {
			get {
				uint w;
				_propDescription.GetDefaultColumnWidth(out w);
				return (int)w;
			}
		}

		/// <summary>
		/// Gets the <see cref="Display"/> type of this property
		/// </summary>
		public Display DisplayType {
			get {
				PROPDESC_DISPLAYTYPE display;
				_propDescription.GetDisplayType(out display);
				return (Display)display;
			}
		}

		/// <summary>
		/// True if this property type contains an enumerated value, else false. <seealso cref="PropertyEnumeration"/>
		/// </summary>
		public bool IsEnumerationType {
			get {
				return DisplayType.HasFlag(Display.Enumerated);
			}
		}

		/// <summary>
		/// Describes a set of values that determine how a property is managed in columns
		/// </summary>
		public class _ColumnFlags {
			SHCOLSTATE _flags;
			internal _ColumnFlags(SHCOLSTATE flags) { _flags = flags; }

			/// <summary>
			/// Returns the <see cref="ColumnType"/> that describes how the column will present the property
			/// </summary>
			public ColumnType Type { get { return (ColumnType)(_flags & SHCOLSTATE.SHCOLSTATE_TYPEMASK); } }
			/// <summary>
			/// The column should be available by default in the details view
			/// </summary>
			public bool OnByDefault { get { return _flags.HasFlag(SHCOLSTATE.SHCOLSTATE_ONBYDEFAULT); } }
			/// <summary>
			/// This property may be slow to populate. It should be performed in a background thread.
			/// </summary>
			public bool Slow { get { return _flags.HasFlag(SHCOLSTATE.SHCOLSTATE_SLOW); } }
			/// <summary>
			/// This property is provided by the property handler, and not by the folder.
			/// </summary>
			public bool Extended { get { return _flags.HasFlag(SHCOLSTATE.SHCOLSTATE_EXTENDED); } }
			/// <summary>
			/// This column's option will appear in the "More" option, rather than in the context menu
			/// </summary>
			public bool SecondaryUI { get { return _flags.HasFlag(SHCOLSTATE.SHCOLSTATE_SECONDARYUI); } }
			/// <summary>
			/// The column will not be displayed in the UI
			/// </summary>
			public bool Hidden { get { return _flags.HasFlag(SHCOLSTATE.SHCOLSTATE_HIDDEN); } }
			/// <summary>
			/// Provides instructions on how the column should be sorted. <seealso cref="ColumnSortFlags"/>
			/// </summary>
			public ColumnSortFlags SortFlags { get { return (ColumnSortFlags)(_flags & (SHCOLSTATE.SHCOLSTATE_PREFER_VARCMP | SHCOLSTATE.SHCOLSTATE_PREFER_FMTCMP | SHCOLSTATE.SHCOLSTATE_NOSORTBYFOLDERNESS)); } }
			/// <summary>
			/// This column is only displayed in the UI
			/// </summary>
			public bool ViewOnly { get { return _flags.HasFlag(SHCOLSTATE.SHCOLSTATE_VIEWONLY); } }
			/// <summary>
			/// Marks columns with values that should be read in a batch
			/// </summary>
			public bool BatchRead { get { return _flags.HasFlag(SHCOLSTATE.SHCOLSTATE_BATCHREAD); } }
			/// <summary>
			/// Disallow the view to be grouped by this property
			/// </summary>
			public bool NoGroupBy { get { return _flags.HasFlag(SHCOLSTATE.SHCOLSTATE_NO_GROUPBY); } }
			/// <summary>
			/// Describes how the width of the column is managed. See <see cref="ColumnWidthFlags"/>
			/// </summary>
			public ColumnWidthFlags DisplayFlags { get { return (ColumnWidthFlags)(_flags & SHCOLSTATE.SHCOLSTATE_DISPLAYMASK); } }
		}
		_ColumnFlags _columnFlags = null;
		/// <summary>
		/// Represents a set of values that determine how a property is managed in columns
		/// </summary>
		public _ColumnFlags ColumnState {
			get {
				if (_columnFlags == null) {
					SHCOLSTATE flags;
					_propDescription.GetColumnState(out flags);
					_columnFlags = new _ColumnFlags(flags);
				}
				return _columnFlags;
			}
		}

		/// <summary>
		/// Determines how this property will display <see cref="RelativeDescriptions"/>.
		/// </summary>
		public RelativeDescriptions RelativeDescriptionType {
			get {
				PROPDESC_RELATIVEDESCRIPTION_TYPE type;
				_propDescription.GetRelativeDescriptionType(out type);
				return (RelativeDescriptions)type;
			}
		}

		/// <summary>
		/// Compares two properties, and returns a descriptive result of the comparison.
		/// </summary>
		/// <param name="value1">The first property value to compare</param>
		/// <param name="value2">The second property value in the comparison</param>
		/// <returns>A tuple consisting of two strings. The first string contains the description of <paramref name="value1"/> relationship to <paramref name="value2"/>, and the second is the reverse,
		/// how <paramref name="value2"/> relates to <paramref name="value1"/>
		/// </returns>
		Tuple<string, string> GetRelativeDescription(PropVariant value1, PropVariant value2) {
			string d1, d2;
			_propDescription.GetRelativeDescription(value1.MarshalledPointer, value1.MarshalledPointer, out d1, out d2);
			return new Tuple<string, string>(d1, d2);
		}

		/// <summary>
		/// Gets the <see cref="SortMethod"/> describing how the property is sorted.
		/// </summary>
		public SortMethod SortDescription {
			get {
				PROPDESC_SORTDESCRIPTION sort;
				_propDescription.GetSortDescription(out sort);
				return (SortMethod)sort;
			}
		}

		/// <summary>
		/// Provides a prompt for how the property should be sorted
		/// </summary>
		public string SortDescriptionLabel {
			get {
				string desc;
				_propDescription.GetSortDescriptionLabel(false, out desc);
				return desc;
			}
		}


		/// <summary>
		/// Provides a prompt for how the property should be sorted
		/// </summary>
		public string SortDescriptionLabelDescending {
			get {
				string desc;
				_propDescription.GetSortDescriptionLabel(true, out desc);
				return desc;
			}
		}

		internal IntPtr GetEnumTypeListPointer() {
			IntPtr pUnk;

			_propDescription.GetEnumTypeList(IID.IPropertyEnumTypeList, out pUnk);
			return pUnk;
		}

		/// <summary>
		/// Gets a <see cref="PropertyEnumeration"/> that describes how this property's values are enumerated
		/// </summary>
		public PropertyEnumeration EnumType {
			get {
				IntPtr pUnk;
				PropertyEnumeration pe;
				
				_propDescription.GetEnumTypeList(IID.IPropertyEnumTypeList, out pUnk);
				pe = new PropertyEnumeration(pUnk);
				Marshal.Release(pUnk);
				return pe;
			}
		}

		/// <summary>
		/// Coerces a <see cref="PropVariant"/> value into a canonical format to suit this property.
		/// </summary>
		/// <param name="value">The <see cref="PropVariant"/> value to coerce into a canonical format</param>
		public void CoerceToCanonicalValue(PropVariant value) {
			_propDescription.CoerceToCanonicalValue(value.MarshalledPointer);
			value.MarshalPointerToValue();
		}

		/// <summary>
		/// Checks to see if a <see cref="PropVariant"/>'s value is canonical according to the definition of this property 
		/// </summary>
		/// <param name="value">The <see cref="PropVariant"/> value to check against this property's description</param>
		/// <returns>true if the value is a canonical representation of the property's value, else false</returns>
		public bool IsCanonical(PropVariant value) {
			HRESULT hr = _propDescription.IsValueCanonical(value.MarshalledPointer);
			hr.ThrowIfFailed();
			return hr == HRESULT.S_OK;
		}

		/// <summary>
		/// Formats a property value for display purposes
		/// </summary>
		/// <param name="value">The property value to format</param>
		/// <param name="flags">Additional <see cref="FormatFlags"/> with formatting instructions.</param>
		/// <returns>A string containing the property's value in a suitably formatted structure</returns>
		public string FormatForDisplay(PropVariant value, FormatFlags flags) {
			string sFormatted;
			_propDescription.FormatForDisplay(value.MarshalledPointer, (PROPDESC_FORMAT_FLAGS)flags, out sFormatted);
			return sFormatted;
		}

		/// <summary>
		/// Releases the COM pointer maintained for and by this instance
		/// </summary>
		public void Dispose() {
			if (_propDescription != null) {
				Marshal.FinalReleaseComObject(_propDescription);
				_propDescription = null;
			}
		}
	}
}
