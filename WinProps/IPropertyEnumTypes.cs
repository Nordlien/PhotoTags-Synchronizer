using System;
using System.Runtime.InteropServices;

namespace WinProps {
	[ComImport, Guid("a99400f4-3d84-4557-94ba-1242fb2cc9a6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IPropertyEnumTypeList {
		void GetCount(
			[Out] out uint pctypes);
		void GetAt(
			[In] uint itype,
			[In] IntPtr riid,	// IPropertyEnumType
			[Out] out IntPtr ppv);
		[Obsolete]
		void GetConditionAt(
			[In] uint nIndex,
			[In] int riid,
			[Out] out IntPtr ppv);
		void FindMatchingIndex(
			[In] IntPtr propvarCmp,	// PROPVARIANT
			[Out] out uint pnIndex);
	}

	[ComImport, Guid("11e1fbf9-2d56-4a6b-8db3-7cd193a471f2"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IPropertyEnumType {
		void GetEnumType(
			[Out] out PROPENUMTYPE penumtype);
		void GetValue(
			[Out] IntPtr ppropvar);
		void GetRangeMinValue(
			[Out] IntPtr ppropvarMin);
		void GetRangeSetValue(
			[Out] IntPtr ppropvarSet);
		void GetDisplayText(
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);
	}
}
