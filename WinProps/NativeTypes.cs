using System;
using System.Runtime.InteropServices;

namespace WinProps {
	// Native type declarations

	#region Structures

	#region PROPERTYKEY
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	struct PROPERTYKEY {
		public Guid fmtid;
		public uint pid;
	}
	#endregion

	struct HRESULT {
		public uint hr;
		public HRESULT(uint hr) {
			this.hr = hr;
		}

		public static HRESULT S_OK = new HRESULT(0);
		public static HRESULT S_FALSE = new HRESULT(1);
		public bool Success { get { return (hr & 0x80000000) == 0; } }
		public bool Failed { get { return (hr & 0x80000000) != 0; } }

		public override bool Equals(object obj) {
			if (obj is HRESULT)
				return hr == ((HRESULT)obj).hr;
			try {
				if (Convert.ToUInt32(obj) == hr)
					return true;
			}
			catch { }
			try {
				if (Convert.ToInt32(obj) == unchecked((int)hr))
					return true;
			}
			catch { }
			return false;
		}
		public override int GetHashCode() {
			return hr.GetHashCode();
		}

		public Exception GetException() {
			if (Failed)
				return Marshal.GetExceptionForHR(unchecked((int)hr));
			return null;
		}
		public void ThrowIfFailed() {
			if (Failed)
				Marshal.ThrowExceptionForHR(unchecked((int)hr));
		}
		public override string ToString() {
			return GetException()?.Message ?? (hr == 0 ? "S_OK" : (hr == 1 ? "S_FALSE" : string.Format("HRESULT = 0x{0:08X}", hr)));
		}

		public static implicit operator HRESULT(uint hr) { return new HRESULT(hr); }
		public static implicit operator HRESULT(int hr) { return new HRESULT(unchecked((uint)hr)); }
		public static implicit operator uint(HRESULT hr) { return hr.hr; }
		public static implicit operator int(HRESULT hr) { return unchecked((int)hr.hr); }
		public static explicit operator Exception(HRESULT hr) { return hr.GetException(); }
	}

	#region FILETIME
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	struct FILETIME {
		public uint dwLowDateTime;
		public uint dwHighDateTime;

		public static implicit operator ulong(FILETIME ft) {
			return ((ulong)ft.dwHighDateTime << 32) | ft.dwLowDateTime;
		}
		public static implicit operator FILETIME(ulong value) {
			return new FILETIME() {
				dwHighDateTime = (uint)(value >> 32),
				dwLowDateTime = (uint)(value & 0xffffffff)
			};
		}
	}
	#endregion

	#region PROPARRAY

	[StructLayout(LayoutKind.Sequential, Pack = 0)]
	struct PROPARRAY {
		public uint cElems;
		public IntPtr pElems;
	}

	#endregion

	#region PROPVARIANT
	[StructLayout(LayoutKind.Explicit, Pack = 1)]
	struct PROPVARIANT {
		[FieldOffset(0)]
		public VARENUM VarType;
		[FieldOffset(2)]
		public ushort wReserved1;
		[FieldOffset(4)]
		public ushort wReserved2;
		[FieldOffset(6)]
		public ushort wReserved3;

		[FieldOffset(8)]
		public byte bVal;
		[FieldOffset(8)]
		public sbyte cVal;
		[FieldOffset(8)]
		public ushort uiVal;
		[FieldOffset(8)]
		public short iVal;
		[FieldOffset(8)]
		public UInt32 uintVal;
		[FieldOffset(8)]
		public Int32 intVal;
		[FieldOffset(8)]
		public UInt64 ulVal;
		[FieldOffset(8)]
		public Int64 lVal;
		[FieldOffset(8)]
		public float fltVal;
		[FieldOffset(8)]
		public double dblVal;
		[FieldOffset(8)]
		public short boolVal;
		[FieldOffset(8)]
		public IntPtr pclsidVal; //this is for GUID ID pointer 
		[FieldOffset(8)]
		public IntPtr pszVal; //this is for ansi string pointer 
		[FieldOffset(8)]
		public IntPtr pwszVal; //this is for Unicode string pointer
		[FieldOffset(8)]
		public IntPtr punkVal; //this is for punkVal (interface pointer) 
		[FieldOffset(8)]
		public PROPARRAY ca;
		[FieldOffset(8)]
		public FILETIME filetime;

	}
	#endregion

	#region WIN32_FIND_DATA

	[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
	struct WIN32_FIND_DATA {
		public System.IO.FileAttributes dwFileAttributes;
		public FILETIME ftCreationTime;
		public FILETIME ftLastAccessTime;
		public FILETIME ftLastWriteTime;
		public uint nFileSizeHigh;
		public uint nFileSizeLow;
		public uint dwReserved0;
		public uint dwReserved1;
		[MarshalAs(UnmanagedType.LPTStr, SizeConst = 260)]
		public string cFileName;
		[MarshalAs(UnmanagedType.LPTStr, SizeConst = 14)]
		public string cAlternateFileName;
	}

	#endregion

	#endregion

	#region Enums

	[Flags]
	enum VARENUM : ushort {
		VT_EMPTY = 0,
		VT_NULL = 1,
		VT_I2 = 2,
		VT_I4 = 3,
		VT_R4 = 4,
		VT_R8 = 5,
		VT_CY = 6,
		VT_DATE = 7,
		VT_BSTR = 8,
		VT_DISPATCH = 9,
		VT_ERROR = 10,
		VT_BOOL = 11,
		VT_VARIANT = 12,
		VT_UNKNOWN = 13,
		VT_DECIMAL = 14,
		VT_I1 = 16,
		VT_UI1 = 17,
		VT_UI2 = 18,
		VT_UI4 = 19,
		VT_I8 = 20,
		VT_UI8 = 21,
		VT_INT = 22,
		VT_UINT = 23,
		VT_VOID = 24,
		VT_HRESULT = 25,
		VT_PTR = 26,
		VT_SAFEARRAY = 27,
		VT_CARRAY = 28,
		VT_USERDEFINED = 29,
		VT_LPSTR = 30,
		VT_LPWSTR = 31,
		VT_RECORD = 36,
		VT_INT_PTR = 37,
		VT_UINT_PTR = 38,
		VT_FILETIME = 64,
		VT_BLOB = 65,
		VT_STREAM = 66,
		VT_STORAGE = 67,
		VT_STREAMED_OBJECT = 68,
		VT_STORED_OBJECT = 69,
		VT_BLOB_OBJECT = 70,
		VT_CF = 71,
		VT_CLSID = 72,
		VT_VERSIONED_STREAM = 73,
		VT_BSTR_BLOB = 0xfff,
		VT_VECTOR = 0x1000,
		VT_ARRAY = 0x2000,
		VT_BYREF = 0x4000,
		VT_RESERVED = 0x8000,
		VT_ILLEGAL = 0xffff,
		VT_ILLEGALMASKED = 0xfff,
		VT_TYPEMASK = 0xfff
	}

	enum PROPDESC_ENUMFILTER {
		PDEF_ALL = 0,
		PDEF_SYSTEM = 1,
		PDEF_NONSYSTEM = 2,
		PDEF_VIEWABLE = 3,
		[Obsolete]
		PDEF_QUERYABLE = 4,
		[Obsolete]
		PDEF_INFULLTEXTQUERY = 5,
		PDEF_COLUMN = 6
	}

	[Flags]
	enum PROPDESC_FORMAT_FLAGS {
		PDFF_DEFAULT = 0x00000000,
		PDFF_PREFIXNAME = 0x00000001,   // Prefix the value with the property name
		PDFF_FILENAME = 0x00000002,   // Treat as a file name
		PDFF_ALWAYSKB = 0x00000004,   // Always format byte sizes as KB
		PDFF_RESERVED_RIGHTTOLEFT = 0x00000008,   // Reserved for legacy use.
		PDFF_SHORTTIME = 0x00000010,   // Show time as "5:17 pm"
		PDFF_LONGTIME = 0x00000020,   // Show time as "5:17:14 pm"
		PDFF_HIDETIME = 0x00000040,   // Hide the time-portion of the datetime
		PDFF_SHORTDATE = 0x00000080,   // Show date as "3/21/04"
		PDFF_LONGDATE = 0x00000100,   // Show date as "Monday, March 21, 2004"
		PDFF_HIDEDATE = 0x00000200,   // Hide the date-portion of the datetime
		PDFF_RELATIVEDATE = 0x00000400,   // Use friendly date descriptions like "Yesterday"
		PDFF_USEEDITINVITATION = 0x00000800,   // Use edit invitation text if failed or empty
		PDFF_READONLY = 0x00001000,   // Use readonly format, fill with default text if empty and !PDFF_FAILIFEMPTYPROP
		PDFF_NOAUTOREADINGORDER = 0x00002000,   // Don't detect reading order automatically. Useful if you will be converting to Ansi and don't want Unicode reading order characters
	}

	[Flags]
	enum PROPDESC_TYPE_FLAGS : uint {
		PDTF_DEFAULT = 0x00000000,
		PDTF_MULTIPLEVALUES = 0x00000001,
		PDTF_ISINNATE = 0x00000002,
		PDTF_ISGROUP = 0x00000004,
		PDTF_CANGROUPBY = 0x00000008,
		PDTF_CANSTACKBY = 0x00000010,
		PDTF_ISTREEPROPERTY = 0x00000020,
		[Obsolete("deprecated", true)]
		PDTF_INCLUDEINFULLTEXTQUERY = 0x00000040,
		PDTF_ISVIEWABLE = 0x00000080,
		[Obsolete("deprecated", true)]
		PDTF_ISQUERYABLE = 0x00000100,
		PDTF_CANBEPURGED = 0x00000200,
		PDTF_SEARCHRAWVALUE = 0x00000400,
		PDTF_DONTCOERCEEMPTYSTRINGS = 0x00000800,
		PDTF_ISSYSTEMPROPERTY = 0x80000000,
		PDTF_MASK_ALL = 0x80000FFF
	}

	[Flags]
	enum PROPDESC_VIEW_FLAGS {
		PDVF_DEFAULT = 0x00000000,
		PDVF_CENTERALIGN = 0x00000001,   // This property should be centered
		PDVF_RIGHTALIGN = 0x00000002,   // This property should be right aligned
		PDVF_BEGINNEWGROUP = 0x00000004,   // Show this property as the beginning of the next collection of properties in the view
		PDVF_FILLAREA = 0x00000008,   // Fill the remainder of the view area with the content of this property
		PDVF_SORTDESCENDING = 0x00000010,   // If this flag is set, the default sort for this property is highest-to-lowest. If this flag is not set, the default sort is lowest-to-highest
		PDVF_SHOWONLYIFPRESENT = 0x00000020,   // Only show this property if it is present
		PDVF_SHOWBYDEFAULT = 0x00000040,   // the property should be shown by default in a view (where applicable)
		PDVF_SHOWINPRIMARYLIST = 0x00000080,   // the property should be shown by default in primary column selection UI
		PDVF_SHOWINSECONDARYLIST = 0x00000100,   // the property should be shown by default in secondary column selection UI
		PDVF_HIDELABEL = 0x00000200,   // Hide the label if the view is normally inclined to show the label
									   // obsolete                 = 0x00000400,
		PDVF_HIDDEN = 0x00000800,   // Don't display this property as a column in the UI
		PDVF_CANWRAP = 0x00001000,   // the property can be wrapped to the next row
		PDVF_MASK_ALL = 0x00001BFF
	}

	enum PROPDESC_DISPLAYTYPE {
		PDDT_STRING = 0,
		PDDT_NUMBER = 1,
		PDDT_BOOLEAN = 2,
		PDDT_DATETIME = 3,
		PDDT_ENUMERATED = 4,    // Use GetEnumTypeList
	}

	enum PROPDESC_GROUPING_RANGE {
		PDGR_DISCRETE = 0,    // Display individual values
		PDGR_ALPHANUMERIC = 1,    // Display static alphanumeric ranges for values
		PDGR_SIZE = 2,    // Display static size ranges for values
		PDGR_DYNAMIC = 3,    // Display dynamically created ranges for the values
		PDGR_DATE = 4,    // Display month/year groups
		PDGR_PERCENT = 5,    // Display percent buckets
		PDGR_ENUMERATED = 6,    // Display buckets from GetEnumTypeList
	}

	enum PROPDESC_SORTDESCRIPTION {
		PDSD_GENERAL = 0,
		PDSD_A_Z = 1,
		PDSD_LOWEST_HIGHEST = 2,
		PDSD_SMALLEST_BIGGEST = 3,
		PDSD_OLDEST_NEWEST = 4,
	}

	enum PROPDESC_RELATIVEDESCRIPTION_TYPE {
		PDRDT_GENERAL = 0,
		PDRDT_DATE = 1,
		PDRDT_SIZE = 2,
		PDRDT_COUNT = 3,
		PDRDT_REVISION = 4,
		PDRDT_LENGTH = 5,
		PDRDT_DURATION = 6,
		PDRDT_SPEED = 7,
		PDRDT_RATE = 8,
		PDRDT_RATING = 9,
		PDRDT_PRIORITY = 10,
	}

	enum PROPDESC_AGGREGATION_TYPE {
		PDAT_DEFAULT = 0,    // Display "multiple-values"
		PDAT_FIRST = 1,    // Display first property value in the selection.
		PDAT_SUM = 2,    // Display the numerical sum of the values. This is never returned for VT_LPWSTR, VT_BOOL, and VT_FILETIME types.
		PDAT_AVERAGE = 3,    // Display the numerical average of the values. This is never returned for VT_LPWSTR, VT_BOOL, and VT_FILETIME types.
		PDAT_DATERANGE = 4,    // Display the date range of the values. This is only returned for VT_FILETIME types.
		PDAT_UNION = 5,    // Display values as union of all values. The order is undefined.
		PDAT_MAX = 6,    // Displays the maximum of all the values.
		PDAT_MIN = 7,    // Displays the minimum of all the values.
	}

	enum PROPDESC_CONDITION_TYPE {
		PDCOT_NONE = 0,
		PDCOT_STRING = 1,
		PDCOT_SIZE = 2,
		PDCOT_DATETIME = 3,
		PDCOT_BOOLEAN = 4,
		PDCOT_NUMBER = 5,
	}

	enum CONDITION_OPERATION {
		COP_IMPLICIT = 0,
		COP_EQUAL = 1,
		COP_NOTEQUAL = 2,
		COP_LESSTHAN = 3,
		COP_GREATERTHAN = 4,
		COP_LESSTHANOREQUAL = 5,
		COP_GREATERTHANOREQUAL = 6,
		COP_VALUE_STARTSWITH = 7,
		COP_VALUE_ENDSWITH = 8,
		COP_VALUE_CONTAINS = 9,
		COP_VALUE_NOTCONTAINS = 10,
		COP_DOSWILDCARDS = 11,
		COP_WORD_EQUAL = 12,
		COP_WORD_STARTSWITH = 13,
		COP_APPLICATION_SPECIFIC = 14
	}

	enum PROPENUMTYPE {
		PET_DISCRETEVALUE = 0,     // Use GetValue & GetDisplayText
		PET_RANGEDVALUE = 1,     // Use GetRangeValues & GetDisplayText
		PET_DEFAULTVALUE = 2,     // Use GetDisplayText
		PET_ENDRANGE = 3,     // Use GetValue
	}

	[Flags]
	enum SHCOLSTATE : uint {
		SHCOLSTATE_DEFAULT = 0,
		SHCOLSTATE_TYPE_STR = 0x1,
		SHCOLSTATE_TYPE_INT = 0x2,
		SHCOLSTATE_TYPE_DATE = 0x3,
		SHCOLSTATE_TYPEMASK = 0xf,
		SHCOLSTATE_ONBYDEFAULT = 0x10,
		SHCOLSTATE_SLOW = 0x20,
		SHCOLSTATE_EXTENDED = 0x40,
		SHCOLSTATE_SECONDARYUI = 0x80,
		SHCOLSTATE_HIDDEN = 0x100,
		SHCOLSTATE_PREFER_VARCMP = 0x200,
		SHCOLSTATE_PREFER_FMTCMP = 0x400,
		SHCOLSTATE_NOSORTBYFOLDERNESS = 0x800,
		SHCOLSTATE_VIEWONLY = 0x10000,
		SHCOLSTATE_BATCHREAD = 0x20000,
		SHCOLSTATE_NO_GROUPBY = 0x40000,
		SHCOLSTATE_FIXED_WIDTH = 0x1000,
		SHCOLSTATE_NODPISCALE = 0x2000,
		SHCOLSTATE_FIXED_RATIO = 0x4000,
		SHCOLSTATE_DISPLAYMASK = 0xf000
	}

	enum PSC_STATE {
		PSC_NORMAL = 0,
		PSC_NOTINSOURCE = 1,
		PSC_DIRTY = 2,
		PSC_READONLY = 3
	}

	[Flags]
	enum GETPROPERTYSTOREFLAGS {
		GPS_DEFAULT = 0,
		GPS_HANDLERPROPERTIESONLY = 0x1,
		GPS_READWRITE = 0x2,
		GPS_TEMPORARY = 0x4,
		GPS_FASTPROPERTIESONLY = 0x8,
		GPS_OPENSLOWITEM = 0x10,
		GPS_DELAYCREATION = 0x20,
		GPS_BESTEFFORT = 0x40,
		GPS_NO_OPLOCK = 0x80,
		GPS_PREFERQUERYPROPERTIES = 0x100,
		GPS_MASK_VALID = 0x1ff,
		GPS_EXTRINSICPROPERTIES = 0x00000200,
		GPS_EXTRINSICPROPERTIESONLY = 0x00000400
	}

	enum SIGDN : uint {
		SIGDN_NORMALDISPLAY = 0,
		SIGDN_PARENTRELATIVEPARSING = 0x80018001,
		SIGDN_PARENTRELATIVEFORADDRESSBAR = 0x8001c001,
		SIGDN_DESKTOPABSOLUTEPARSING = 0x80028000,
		SIGDN_PARENTRELATIVEEDITING = 0x80031001,
		SIGDN_DESKTOPABSOLUTEEDITING = 0x8004c000,
		SIGDN_FILESYSPATH = 0x80058000,
		SIGDN_PARENTRELATIVE = 0x80080001,
		SIGDN_PARENTRELATIVEFORUI = 0x80094001,
		URL = 0x80068000
	}

	[Flags]
	enum SFGAO : uint {
		SFGAO_CANCOPY = 0x1,
		SFGAO_CANMOVE = 0x2,
		SFGAO_CANLINK = 0x4,
		SFGAO_STORAGE = 0x00000008,
		SFGAO_CANRENAME = 0x00000010,
		SFGAO_CANDELETE = 0x00000020,
		SFGAO_HASPROPSHEET = 0x00000040,
		SFGAO_DROPTARGET = 0x00000100,
		SFGAO_CAPABILITYMASK = 0x00000177,
		SFGAO_SYSTEM = 0x00001000,
		SFGAO_ENCRYPTED = 0x00002000,
		SFGAO_ISSLOW = 0x00004000,
		SFGAO_GHOSTED = 0x00008000,
		SFGAO_LINK = 0x00010000,
		SFGAO_SHARE = 0x00020000,
		SFGAO_READONLY = 0x00040000,
		SFGAO_HIDDEN = 0x00080000,
		SFGAO_DISPLAYATTRMASK = 0x000FC000,
		SFGAO_FILESYSANCESTOR = 0x10000000,
		SFGAO_FOLDER = 0x20000000,
		SFGAO_FILESYSTEM = 0x40000000,
		SFGAO_HASSUBFOLDER = 0x80000000,
		SFGAO_CONTENTSMASK = 0x80000000,
		SFGAO_VALIDATE = 0x01000000,
		SFGAO_REMOVABLE = 0x02000000,
		SFGAO_COMPRESSED = 0x04000000,
		SFGAO_BROWSABLE = 0x08000000,
		SFGAO_NONENUMERATED = 0x00100000,
		SFGAO_NEWCONTENT = 0x00200000,
		SFGAO_CANMONIKER = 0x00400000,
		SFGAO_HASSTORAGE = 0x00400000,
		SFGAO_STREAM = 0x00400000,
		SFGAO_STORAGEANCESTOR = 0x00800000,
		SFGAO_STORAGECAPMASK = 0x70C50008,
	}

	enum SICHINTF : uint {
		SICHINT_DISPLAY = 0x00000000,
		SICHINT_ALLFIELDS = 0x80000000,
		SICHINT_CANONICAL = 0x10000000,
		SICHINT_TEST_FILESYSPATH_IF_NOT_EQUAL = 0x20000000
	}

	#endregion

}