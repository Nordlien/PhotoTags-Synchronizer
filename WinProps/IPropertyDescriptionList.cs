using System;
using System.Runtime.InteropServices;

namespace WinProps {

	[ComImport, Guid("1f9fc1d0-c39b-4b26-817f-011967d3440e"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IPropertyDescriptionList {
		void GetCount(
			[Out] out uint pcElem);
		void GetAt(
			[In] uint iElem,
			[In] IntPtr riid,	// IID_IPropertyDescription
			[Out] out IntPtr ppv);	// IPropertyDescription
	}

}
