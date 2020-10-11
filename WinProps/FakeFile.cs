using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace WinProps {

	[ComImport, Guid("01E18D10-4D8B-11d2-855D-006008059367"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IFileSystemBindData {
		void SetFileData([In, MarshalAs(UnmanagedType.Struct)] ref WIN32_FIND_DATA pfd);
		void GetFileData([Out, MarshalAs(UnmanagedType.Struct)] out WIN32_FIND_DATA pdf);
	}


	class FakeFile : IFileSystemBindData, IDisposable {

		const string STR_FILE_SYS_BIND_DATA = "File System Bind Data";
		const int STGM_CREATE = 0x00001000;

		internal WIN32_FIND_DATA _fd;
		internal IntPtr _ppidl = IntPtr.Zero;

		public FakeFile(ref WIN32_FIND_DATA pfd, string path) {
			_fd = pfd;
			CreateSimplePidl(path);
		}

		public void GetFileData([MarshalAs(UnmanagedType.Struct), Out] out WIN32_FIND_DATA pdf) {
			pdf = _fd;
		}

		public void SetFileData([In, MarshalAs(UnmanagedType.Struct)] ref WIN32_FIND_DATA pfd) {
			_fd = pfd;
		}

		void CreateBindCtx(out IBindCtx pbc) {
			System.Runtime.InteropServices.ComTypes.BIND_OPTS bo = new System.Runtime.InteropServices.ComTypes.BIND_OPTS() {
				cbStruct = Marshal.SizeOf(typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS)),
				dwTickCountDeadline = 0,
				grfFlags = 0,
				grfMode = STGM_CREATE
			};
			try {
				PInvoke.CreateBindCtx(0, out pbc);
				pbc.SetBindOptions(ref bo);
				pbc.RegisterObjectParam(STR_FILE_SYS_BIND_DATA, (IFileSystemBindData)this);
			}
			catch {
				pbc = null;
				throw;
			}
		}

		void CreateSimplePidl(string path) {
			IBindCtx pbc;
			uint sfgao;
			CreateBindCtx(out pbc);
			PInvoke.SHParseDisplayName(path, pbc, out _ppidl, 0, out sfgao);
		}

		public IShellItem GetShellItem() {
			object item;
			IntPtr ppv;
			PInvoke.SHCreateItemFromIDList(_ppidl, IID.IShellItem, out ppv);
			item = Marshal.GetUniqueObjectForIUnknown(ppv);
			Marshal.Release(ppv);
			return (IShellItem)item;
		}

		bool _disposed = false;
		public void Dispose() {
			if (!_disposed) {
				if (_ppidl != IntPtr.Zero) {
					try {
						Marshal.FreeCoTaskMem(_ppidl);
					}
					catch { }
					finally {
						_ppidl = IntPtr.Zero;
					}
				}
				GC.SuppressFinalize(this);
			}
			_disposed = true;
		}
		~FakeFile() { Dispose(); }

	}
}
