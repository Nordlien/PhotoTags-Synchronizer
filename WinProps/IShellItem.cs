using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace WinProps {
	[ComImport, Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IShellItem {
		void BindToHandler(
			[In, MarshalAs(UnmanagedType.Interface)]IBindCtx pbc,
			[In] IntPtr bhid,	// A Guid for the Bind Handler (BHID)
			[In] IntPtr riid,	// The iid of the required interface
			[Out] out IntPtr ppv);  // IUnknown
		void GetParent(
			[Out] out IntPtr ppsi);	// IShellItem
		void GetDisplayName(
			[In]SIGDN sigdnName,
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
		void GetAttributes(
			[In]SFGAO sfgaoMask,
			[Out] out SFGAO psfgaoAttribs);
		void Compare(
			[In, MarshalAs(UnmanagedType.Interface)] IShellItem psi,
			[In] SICHINTF hint,
			[Out] out int piOrder);
	}

	[ComImport, Guid("7e9fb0d3-919f-4307-ab2e-9b1860310c93"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IShellItem2 {
		#region inherited from IShellItem
		void BindToHandler(
			[In, MarshalAs(UnmanagedType.Interface)]IBindCtx pbc,
			[In] IntPtr bhid,   // A Guid for the Bind Handler (BHID)
			[In] IntPtr riid,   // The iid of the required interface
			[Out] out IntPtr ppv);  // IUnknown
		void GetParent(
			[Out] out IntPtr ppsi); // IShellItem
		void GetDisplayName(
			[In]SIGDN sigdnName,
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
		void GetAttributes(
			[In]SFGAO sfgaoMask,
			[Out] out SFGAO psfgaoAttribs);
		void Compare(
			[In, MarshalAs(UnmanagedType.Interface)] IShellItem psi,
			[In] SICHINTF hint,
			[Out] out int piOrder);
		#endregion
		void GetPropertyStore(
			[In] GETPROPERTYSTOREFLAGS flags,
			[In] IntPtr riid,	// IID_IPropertyStore
			[Out] out IntPtr ppv);	// IPropertyStore
		void GetPropertyStoreWithCreateObject(
			[In] GETPROPERTYSTOREFLAGS flags,
			[In] IntPtr punkCreateObject,   // factory for low-rights creation of type ICreateObject
			[In] IntPtr riid,   // IID_IPropertyStore
			[Out] out IntPtr ppv);  // IPropertyStore
		void GetPropertyStoreForKeys(
			[In] IntPtr rgKeys,	// A Pointer to an array of PROPERTYKEY structures
			[In] uint cKeys,
			[In] GETPROPERTYSTOREFLAGS flags,
			[In] IntPtr riid,   // IID_IPropertyStore
			[Out] out IntPtr ppv);  // IPropertyStore
		void GetPropertyDescriptionList(
			[In] IntPtr keyType,	// A Propertykey defining the list to get eg: System.PropList.FillDetails
			[In] IntPtr riid,   // IID_IPropertyDescriptionList
			[Out] out IntPtr ppv);  // IPropertyDescriptionList
		void Update(
			[In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc);
		void GetProperty(
			[In] IntPtr propKey,
			[Out] out IntPtr ppropvar); // PROPVARIANT
		void GetCLSID(
			[In] IntPtr propKey,
			[Out] out IntPtr pclsid);
		void GetFileTime(
			[In] IntPtr propKey,
			[Out] out long pft);
		void GetInt32(
			[In] IntPtr propKey,
			[Out] out int pi);
		void GetString(
			[In] IntPtr propKey,
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string ppsz);
		void GetUInt32(
			[In] IntPtr propKey,
			[Out] out uint pui);
		void GetUInt64(
			[In] IntPtr propKey,
			[Out] out ulong pull);
		void GetBool(
			[In] IntPtr propKey,
			[Out, MarshalAs(UnmanagedType.Bool)] out bool pf);
	}
}
