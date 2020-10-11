using System;
using System.Runtime.InteropServices;

namespace WinProps {
	[ComImport, Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IPropertyStore {

		void GetCount(
			[Out] out uint cProps);
		void GetAt(
			[In] uint iProp,
			[Out] IntPtr pkey);
		void GetValue(
			[In] IntPtr pkey,
			[Out] IntPtr ppropvar);
		void SetValue(
			[In] IntPtr pkey,
			[In] IntPtr ppropvar);
		void Commit();
	}

	[ComImport, Guid("3017056d-9a91-4e90-937d-746c72abbf4f"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IPropertyStoreCache {
		#region IPropertyStore
		void GetCount(
			[Out] out uint cProps);
		void GetAt(
			[In] uint iProp,
			[Out] IntPtr pkey);
		void GetValue(
			[In] IntPtr pkey,
			[Out] IntPtr ppropvar);
		void SetValue(
			[In] IntPtr pkey,
			[In] IntPtr ppropvar);
		void Commit();
		#endregion
		void GetState(
			[In] IntPtr pkey,
			[Out] out PSC_STATE pstate);
		void GetValueAndState(
			[In] IntPtr pkey,
			[Out] IntPtr ppropvar,
			[Out] out PSC_STATE pstate);
		void SetState(
			[In] IntPtr pkey,
			[In] PSC_STATE pstate);
		void SetValueAndState(
			[In] IntPtr pkey,
			[In] IntPtr ppropvar,
			[In] PSC_STATE pstate);
	}

	[ComImport, Guid("c8e2d566-186e-4d49-bf41-6909ead56acc"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IPropertyStoreCapabilities {
		[PreserveSig]
		int IsPropertyWritable(
			[In] IntPtr pkey);
	}
}