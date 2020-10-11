using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace WinProps {
	static class PInvoke {

		#region PropertyKey Functions

		[DllImport("Propsys.dll")]
		public static extern HRESULT PSGetPropertyKeyFromName(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszName,
			[Out] IntPtr pKey
			);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PSPropertyKeyFromString(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszString,
			[Out] IntPtr pKey
			);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PSGetNameFromPropertyKey(
			[In] IntPtr refPropKey,
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string name);

		#endregion

		#region PropVariant Functions

		#region Manipulation

		[DllImport("Ole32.dll")]
		public static extern HRESULT PropVariantClear(
					[In, Out] IntPtr ppv);

		[DllImport("Ole32.dll")]
		public static extern HRESULT PropVariantCopy(
			[Out] IntPtr ppvDest,
			[In] IntPtr ppvSrc);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToVariant(
			[In] IntPtr pPropVar,
			[Out] out object pVariant);

		[DllImport("Propsys.dll")]
		public static extern HRESULT VariantToPropVariant(
			[In] ref object pVariant,
			[Out] IntPtr pPropVar);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantChangeType(
			[Out] IntPtr ppvDest,
			[In] IntPtr ppvIn,
			[In] int flags, // Reserved, must be zero
			[In] VARENUM vt);

		[DllImport("Propsys.dll")]
		public static extern int PropVariantCompareEx(
			[In] IntPtr ppv1,
			[In] IntPtr ppv2,
			[In] PropVariantComparer.CompareUnit compareUnit,
			[In] PropVariantComparer.CompareFlags compareFlags);

		#endregion

		#region Init

		[DllImport("Propsys.dll")]
		public static extern HRESULT InitPropVariantFromBooleanVector(
			[In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Bool)] bool[] fVal,
			[In] uint cElems,
			[Out] IntPtr ppv);

		[DllImport("Propsys.dll")]
		public static extern HRESULT InitPropVariantFromBuffer(
			[In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)] byte[] pv,
			[In] uint cb,
			[Out] IntPtr ppropvar);

		[DllImport("Propsys.dll")]
		public static extern HRESULT InitPropVariantFromCLSID(
			[In] IntPtr pGuid,
			[Out] IntPtr ppropvar);

		[DllImport("Propsys.dll")]
		public static extern HRESULT InitPropVariantFromGUIDAsString(
			[In] IntPtr pGuid,
			[Out] IntPtr ppropvar);

		[DllImport("Propsys.dll")]
		public static extern HRESULT InitPropVariantFromDoubleVector(
			[In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] pv,
			[In] uint cb,
			[Out] IntPtr ppropvar);

		[DllImport("Propsys.dll")]
		public static extern HRESULT InitPropVariantFromFileTime(
			[In] IntPtr pft,
			[Out] IntPtr ppropvar);

		[DllImport("Propsys.dll")]
		public static extern HRESULT InitPropVariantFromFileTimeVector(
			[In] IntPtr pftv,
			[In] uint cb,
			[Out] IntPtr ppropvar);

		[DllImport("Propsys.dll")]
		public static extern HRESULT InitPropVariantFromInt16Vector(
			[In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I2)] short[] pv,
			[In] uint cb,
			[Out] IntPtr ppropvar);

		[DllImport("Propsys.dll")]
		public static extern HRESULT InitPropVariantFromUInt16Vector(
			[In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2)] ushort[] pv,
			[In] uint cb,
			[Out] IntPtr ppropvar);

		[DllImport("Propsys.dll")]
		public static extern HRESULT InitPropVariantFromInt32Vector(
			[In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] pv,
			[In] uint cb,
			[Out] IntPtr ppropvar);

		[DllImport("Propsys.dll")]
		public static extern HRESULT InitPropVariantFromUInt32Vector(
			[In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)] uint[] pv,
			[In] uint cb,
			[Out] IntPtr ppropvar);

		[DllImport("Propsys.dll")]
		public static extern HRESULT InitPropVariantFromInt64Vector(
			[In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I8)] long[] pv,
			[In] uint cb,
			[Out] IntPtr ppropvar);

		[DllImport("Propsys.dll")]
		public static extern HRESULT InitPropVariantFromUInt64Vector(
			[In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U8)] ulong[] pv,
			[In] uint cb,
			[Out] IntPtr ppropvar);

		[DllImport("Propsys.dll")]
		public static extern HRESULT InitPropVariantFromStringAsVector(
			[In, MarshalAs(UnmanagedType.LPWStr)] string psz,
			[Out] IntPtr ppropvar);

		[DllImport("Propsys.dll")]
		public static extern HRESULT InitPropVariantFromPropVariantVectorElem(
			[In] IntPtr ppvIn,
			[In] uint iElem,
			[Out] IntPtr ppropvar);

		[DllImport("Propsys.dll")]
		public static extern HRESULT InitPropVariantFromStringVector(
			[In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[] psz,
			[In] uint cElems,
			[Out] IntPtr ppropvar);

		[DllImport("Propsys.dll")]
		public static extern HRESULT InitPropVariantVectorFromPropVariant(
			[In] IntPtr ppvIn,
			[Out] IntPtr ppropvar);

		#endregion

		#region Values

		[DllImport("Propsys.dll")]
		public static extern uint PropVariantGetElementCount(
			[In] IntPtr ppv);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToWinRTPropertyValue(
			[In] IntPtr pPropVar,
			[In] IntPtr riid,
			[Out, MarshalAs(UnmanagedType.Interface)] out object ppv);

		#region Strings

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToStringAlloc(
			[In] IntPtr ppv,
			[Out] out IntPtr ppStr);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToStringVectorAlloc(
			[In] IntPtr ppv,
			[Out] out IntPtr pplpwstr,
			[Out] out uint cElems);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantGetStringElem(
			[In] IntPtr ppv,
			[In] uint iElem,
			[Out] out IntPtr psz);

		[DllImport("Propsys.dll")]
		public static extern IntPtr PropVariantToStringWithDefault(
			[In] IntPtr ppv,
			[In, MarshalAs(UnmanagedType.LPWStr)] string pStr);

		#endregion

		#region Boolean

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToBoolean(
			[In] IntPtr ppv,
			[Out, MarshalAs(UnmanagedType.Bool)] out bool pbVal);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToBooleanVectorAlloc(
			[In] IntPtr ppv,
			[Out] out IntPtr pBools,
			[Out] out uint cElems);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantGetBooleanElem(
			[In] IntPtr ppv,
			[In] uint iElem,
			[Out, MarshalAs(UnmanagedType.Bool)] out bool pfVal);

		[DllImport("Propsys.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PropVariantToBooleanWithDefault(
			[In] IntPtr ppv,
			[In, MarshalAs(UnmanagedType.Bool)] bool bVal);

		#endregion

		#region Buffer

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToBuffer(
			[In] IntPtr ppv,
			[Out] IntPtr pBuff,
			[In] uint cElems);

		#endregion

		#region Double

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToDouble(
			[In] IntPtr ppv,
			[Out, MarshalAs(UnmanagedType.R8)] out double pbVal);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToDoubleVectorAlloc(
			[In] IntPtr ppv,
			[Out] out IntPtr pdVals,
			[Out] out uint cElems);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantGetDoubleElem(
			[In] IntPtr ppv,
			[In] uint iElem,
			[Out, MarshalAs(UnmanagedType.R8)] out double pfVal);

		[DllImport("Propsys.dll")]
		[return: MarshalAs(UnmanagedType.R8)]
		public static extern double PropVariantToDoubleWithDefault(
			[In] IntPtr ppv,
			[In, MarshalAs(UnmanagedType.R8)] double dVal);

		#endregion

		#region FILETIME

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToFileTime(
			[In] IntPtr ppv,
			[In] uint flags,
			[Out] IntPtr pft);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToFileTimeVectorAlloc(
			[In] IntPtr ppv,
			[Out] out IntPtr pftVals,
			[Out] out uint cElems);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantGetFileTimeElem(
			[In] IntPtr ppv,
			[In] uint iElem,
			[Out] IntPtr pft);

		#endregion

		#region GUID

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToGUID(
			[In] IntPtr ppv,
			[Out] IntPtr pGuid);

		#endregion

		#region Int16

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToInt16(
			[In] IntPtr ppv,
			[Out, MarshalAs(UnmanagedType.I2)] out short piVal);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantGetInt16Elem(
			[In] IntPtr ppv,
			[In] uint iElem,
			[Out, MarshalAs(UnmanagedType.I2)] out short piVal);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToInt16VectorAlloc(
			[In] IntPtr ppv,
			[Out] out IntPtr piVals,
			[Out] out uint cElems);

		[DllImport("Propsys.dll")]
		[return: MarshalAs(UnmanagedType.I2)]
		public static extern short PropVariantToInt16WithDefault(
			[In] IntPtr ppv,
			[In, MarshalAs(UnmanagedType.I2)] short iVal);

		#endregion

		#region Int32

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToInt32(
			[In] IntPtr ppv,
			[Out, MarshalAs(UnmanagedType.I4)] out int piVal);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToInt32VectorAlloc(
			[In] IntPtr ppv,
			[Out] out IntPtr piVals,
			[Out] out uint cElems);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantGetInt32Elem(
			[In] IntPtr ppv,
			[In] uint iElem,
			[Out, MarshalAs(UnmanagedType.I4)] out int piVal);

		[DllImport("Propsys.dll")]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern int PropVariantToInt32WithDefault(
			[In] IntPtr ppv,
			[In, MarshalAs(UnmanagedType.I4)] int iVal);

		#endregion

		#region Int64

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToInt64(
			[In] IntPtr ppv,
			[Out, MarshalAs(UnmanagedType.I8)] out long piVal);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToInt64VectorAlloc(
			[In] IntPtr ppv,
			[Out] out IntPtr piVals,
			[Out] out uint cElems);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantGetInt64Elem(
			[In] IntPtr ppv,
			[In] uint iElem,
			[Out, MarshalAs(UnmanagedType.I8)] out long piVal);

		[DllImport("Propsys.dll")]
		[return: MarshalAs(UnmanagedType.I8)]
		public static extern long PropVariantToInt64WithDefault(
			[In] IntPtr ppv,
			[In, MarshalAs(UnmanagedType.I8)] long iVal);

		#endregion

		#region UInt16

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToUInt16(
			[In] IntPtr ppv,
			[Out, MarshalAs(UnmanagedType.U2)] out ushort piVal);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToUInt16VectorAlloc(
			[In] IntPtr ppv,
			[Out] out IntPtr piVals,
			[Out] out uint cElems);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantGetUInt16Elem(
			[In] IntPtr ppv,
			[In] uint iElem,
			[Out, MarshalAs(UnmanagedType.U2)] out ushort piVal);

		[DllImport("Propsys.dll")]
		[return: MarshalAs(UnmanagedType.U2)]
		public static extern ushort PropVariantToUInt16WithDefault(
			[In] IntPtr ppv,
			[In, MarshalAs(UnmanagedType.U2)] ushort iVal);

		#endregion

		#region UInt32

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToUInt32(
			[In] IntPtr ppv,
			[Out, MarshalAs(UnmanagedType.U4)] out uint piVal);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToUInt32VectorAlloc(
			[In] IntPtr ppv,
			[Out] out IntPtr piVals,
			[Out] out uint cElems);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantGetUInt32Elem(
			[In] IntPtr ppv,
			[In] uint iElem,
			[Out, MarshalAs(UnmanagedType.U4)] out uint piVal);

		[DllImport("Propsys.dll")]
		[return: MarshalAs(UnmanagedType.U4)]
		public static extern uint PropVariantToUInt32WithDefault(
			[In] IntPtr ppv,
			[In, MarshalAs(UnmanagedType.U4)] uint iVal);

		#endregion

		#region UInt64

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToUInt64(
			[In] IntPtr ppv,
			[Out, MarshalAs(UnmanagedType.U8)] out ulong piVal);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantToUInt64VectorAlloc(
			[In] IntPtr ppv,
			[Out] out IntPtr piVals,
			[Out] out uint cElems);

		[DllImport("Propsys.dll")]
		public static extern HRESULT PropVariantGetUInt64Elem(
			[In] IntPtr ppv,
			[In] uint iElem,
			[Out, MarshalAs(UnmanagedType.U8)] out ulong piVal);

		[DllImport("Propsys.dll")]
		[return: MarshalAs(UnmanagedType.U8)]
		public static extern ulong PropVariantToUInt64WithDefault(
			[In] IntPtr ppv,
			[In, MarshalAs(UnmanagedType.U8)] ulong iVal);

		#endregion

		#endregion

		#region Serialization

		[DllImport("Propsys.dll")]
		public static extern HRESULT StgSerializePropVariant(
			[In] IntPtr pPropVar,
			[Out] out IntPtr ppPropData,
			[Out] out uint pcb);

		[DllImport("Propsys.dll")]
		public static extern HRESULT StgDeserializePropVariant(
			[In] IntPtr pPropData,
			[In] uint cbMax,
			[Out] IntPtr ppPropVar);

		#endregion

		#endregion

		#region PropertyDescriptionList Functions

		[DllImport("Propsys.dll", CharSet = CharSet.Unicode)]
		public static extern HRESULT PSEnumeratePropertyDescriptions(
			[In] int Filter,
			[In] IntPtr riid,   // IID_IPropertyDescriptionList
			[Out] out IntPtr ppv);  // IPropertyDescrptionList **

		[DllImport("Propsys.dll", CharSet = CharSet.Unicode)]
		public static extern HRESULT PSGetPropertyDescriptionListFromString(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszPropList,
			[In] IntPtr riid,   // IID_IPropertyDescriptionList
			[Out] out IntPtr ppv);  // IPropertyDescrptionList **

		#endregion

		#region PropertyDescription Functions

		[DllImport("Propsys.dll", CharSet = CharSet.Unicode)]
		public static extern HRESULT PSGetPropertyDescription(
			[In] IntPtr ppk,    // PROPERTYKEY *
			[In] IntPtr riid,   // IID_IPropertyDescription
			[Out] out IntPtr ppv);  // IPropertyDescription **

		[DllImport("Propsys.dll", CharSet = CharSet.Unicode)]
		public static extern HRESULT PSGetPropertyDescriptionByName(
			[In, MarshalAs(UnmanagedType.LPWStr)] string lpName,
			[In] IntPtr riid,   // IID_IPropertyDescription
			[Out] out IntPtr ppv);  // IPropertyDescription **

		#endregion

		#region PropertyStore
		[DllImport("Propsys.dll", CharSet = CharSet.Unicode)]
		public static extern HRESULT PSCreateMemoryPropertyStore(
			[In] IntPtr riid,	// IID_IPropertyStore
			[Out] out IntPtr ppv);  // IPropertyStore**
		#endregion

		#region Shell Functions

		[DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Error)]
		public static extern int SHCreateItemFromParsingName(
			 [In, MarshalAs(UnmanagedType.LPWStr)] string pszPath,
			 [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc,
			 [In] IntPtr riid,	// The Shell Item's IID
			 [Out] out IntPtr ppv); // IUnknown pointer to IShellItem* or IShellItem2*

		[DllImport("Ole32.dll")]
		[return: MarshalAs(UnmanagedType.Error)]
		public static extern int CreateBindCtx([In]uint reserved, [Out, MarshalAs(UnmanagedType.Interface)] out IBindCtx pbc);

		[DllImport("Shell32.dll")]
		[return: MarshalAs(UnmanagedType.Error)]
		public static extern int SHParseDisplayName(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszName,
			[In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc,
			[Out] out IntPtr ppidl,
			[In] uint sfgaoMask,
			[Out] out uint psfgao);

		[DllImport("Shell32.dll")]
		[return: MarshalAs(UnmanagedType.Error)]
		public static extern int SHCreateItemFromIDList(
			[In] IntPtr pidl,
			[In] IntPtr riid,	// IID_IShellItem
			[Out] out IntPtr ppv);

		[DllImport("Shell32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr ILCreateFromPath(
			[In, MarshalAs(UnmanagedType.LPTStr)] string pszPath);

		[DllImport("Shell32.dll", CharSet = CharSet.Auto)]
		public static extern HRESULT SHAddDefaultPropertiesByExt(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszExt,
			[In, MarshalAs(UnmanagedType.Interface)] IPropertyStore pPropStore);

		[DllImport("Shell32.dll", PreserveSig = true)]
		public static extern IntPtr ILFindLastID(
			[In] IntPtr pidl);

		[DllImport("Shell32.dll", PreserveSig = true)]
		public static extern WinProps.HRESULT SHCreateItemWithParent(
			[In] IntPtr pidlParent,
			[In] IntPtr psfParent,  // IShellFolder
			[In] IntPtr pidl,
			[In] IntPtr riid,
			[Out] out IntPtr ppvItem);

		#endregion

		#region Misc
		[DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
		public static extern void MoveMemory(
			[In] IntPtr dest, 
			[In] IntPtr src, 
			[In] int size);
		#endregion
	}

}