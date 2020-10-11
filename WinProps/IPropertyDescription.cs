using System;
using System.Runtime.InteropServices;

namespace WinProps {
	[ComImport, Guid("6f79d558-3e96-4549-a1d1-7d75d2288814"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IPropertyDescription {
		void GetPropertyKey(
			[Out] IntPtr pkey); // out PROPERTYKEY
		void GetCanonicalName(
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
		void GetPropertyType(
			[Out, MarshalAs(UnmanagedType.U2)] out VARENUM pvartype);
		void GetDisplayName(
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
		void GetEditInvitation(
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszInvite);
		void GetTypeFlags(
			[In] PROPDESC_TYPE_FLAGS mask,
			[Out] out PROPDESC_TYPE_FLAGS ppdtFlags);
		void GetViewFlags(
			[Out] out PROPDESC_VIEW_FLAGS ppdvFlags);
		void GetDefaultColumnWidth(
			[Out] out uint pcxChars);
		void GetDisplayType(
			[Out] out PROPDESC_DISPLAYTYPE pdisplaytype);
		void GetColumnState(
			[Out] out SHCOLSTATE pcsFlags);
		void GetGroupingRange(
			[Out] out PROPDESC_GROUPING_RANGE pgr);
		void GetRelativeDescriptionType(
			[Out] out PROPDESC_RELATIVEDESCRIPTION_TYPE prdt);
		void GetRelativeDescription(
			[In] IntPtr propvar1,   // ref PROPVARIANT 
			[In] IntPtr propvar2, // ref PROPVARIANT
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc1,
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszDesc2);
		void GetSortDescription(
			[Out] out PROPDESC_SORTDESCRIPTION psd);
		void GetSortDescriptionLabel(
			[In, MarshalAs(UnmanagedType.Bool)] bool fDescending,
			[Out, MarshalAs(UnmanagedType.LPWStr)]out string ppszDescription);
		void GetAggregationType(
			[Out] out PROPDESC_AGGREGATION_TYPE paggtype);
		void GetConditionType(
			[Out] out PROPDESC_CONDITION_TYPE pcontype,
			[Out] out CONDITION_OPERATION popDefault);
		void GetEnumTypeList(
			[In] IntPtr riid,   // IID_IPropertyEnumTypeList
			[Out] out IntPtr ppv);
		void CoerceToCanonicalValue(
			[In, Out] IntPtr ppropvar); // ref PROPVARIANT
		void FormatForDisplay(
			[In] IntPtr propvar, // ref PROPVARIANT
			[In] PROPDESC_FORMAT_FLAGS pdfFlags,
			[Out, MarshalAs(UnmanagedType.LPWStr)]out string ppszDisplay);
		[PreserveSig]
		int IsValueCanonical(
			[In] IntPtr propvar); // ref PROPVARIANT
	}

}
