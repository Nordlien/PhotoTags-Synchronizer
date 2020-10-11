using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace WinProps {
	/// <summary>
	/// Represents an Item (File, Folder, or Namespace Extension)
	/// </summary>
	public class ShellItem : IDisposable {
		/// <summary>
		/// Attributes that can be retrieved on an item
		/// </summary>
		[Flags]
		public enum Attributes : uint {
			/// <summary>
			/// The specified item can be copied
			/// </summary>
			CanCopy = 0x1,
			/// <summary>
			/// The specified item can be moved
			/// </summary>
			CanMove = 0x2,
			/// <summary>
			/// A .lnk file can be created for the specified item
			/// </summary>
			CanLink = 0x4,
			/// <summary>
			/// Specifies that an item can be bound to an IStorage through IShellFolder.BindToObject
			/// </summary>
			Storage = 0x00000008,
			/// <summary>
			/// Specifies that an item can be renamed
			/// </summary>
			CanRename = 0x00000010,
			/// <summary>
			/// Specifies that an item can be deleted
			/// </summary>
			CanDelete = 0x00000020,
			/// <summary>
			/// The specified item has a property sheet
			/// </summary>
			HasPropertySheet = 0x00000040,
			/// <summary>
			/// The specified item is a drop target
			/// </summary>
			DropTarget = 0x00000100,
			/// <summary>
			/// The specified item is a system item
			/// </summary>
			System = 0x00001000,
			/// <summary>
			/// The item is encrypted
			/// </summary>
			Encrypted = 0x00002000,
			/// <summary>
			/// Accessing the item is expected to be a slow operation. Note that querying this flag is also considered to be a slow operation, and should probably be done on a background thread.
			/// </summary>
			Slow = 0x00004000,
			/// <summary>
			/// The item is unavailable to the user
			/// </summary>
			Ghosted = 0x00008000,
			/// <summary>
			/// The item is a shortcut
			/// </summary>
			Link = 0x00010000,
			/// <summary>
			/// The specified item is shared
			/// </summary>
			Shared = 0x00020000,
			/// <summary>
			/// The specified item is read-only
			/// </summary>
			ReadOnly = 0x00040000,
			/// <summary>
			/// The specified item is hidden
			/// </summary>
			Hidden = 0x00080000,
			/// <summary>
			/// Indicates that this item, or one of its descendants, is a system item
			/// </summary>
			FileSystemAncestor = 0x10000000,
			/// <summary>
			/// The item is a folder
			/// </summary>
			Folder = 0x20000000,
			/// <summary>
			/// The item is part of the file system
			/// </summary>
			FileSystem = 0x40000000,
			/// <summary>
			/// Indicates that the item may have sub-folders
			/// </summary>
			HasSubFolder = 0x80000000,
			/// <summary>
			/// Input only. Specifies that the call should validate that the item exists
			/// </summary>
			Validate = 0x01000000,
			/// <summary>
			/// Indicates that the item is, or resides on, removable media.
			/// </summary>
			Removable = 0x02000000,
			/// <summary>
			/// The item has been compressed
			/// </summary>
			Compressed = 0x04000000,
			/// <summary>
			/// The item can be hosted in a browser
			/// </summary>
			Browsable = 0x08000000,
			/// <summary>
			/// The items have not come from an enumeration (such as IShellFolder.EnumObjects)
			/// </summary>
			NonEnumerated = 0x00100000,
			/// <summary>
			/// The item contains new content, as defined by the application
			/// </summary>
			NewContent = 0x00200000,
			/// <summary>
			/// The item has an associated stream
			/// </summary>
			Stream = 0x00400000,
			/// <summary>
			/// Children of this item can be accessed through IStream or IStorage interfaces.
			/// </summary>
			StorageAncestor = 0x00800000,
		}

		IShellItem2 _pItem;

		/// <summary>
		/// Creates a shell item from the specified path.
		/// </summary>
		/// <param name="path">The path of the item to create</param>
		/// <param name="allowFake">Create the item, even if the specified file does not exist. Note that if a directory is supplied as part of the path, it <em>must</em> already exist</param>
		public ShellItem(string path, bool allowFake = false) {
			if (File.Exists(path) || Directory.Exists(path)) {
				IBindCtx ctx = null;
				IntPtr ppv;
				HRESULT hr = PInvoke.SHCreateItemFromParsingName(path, ctx, IID.IShellItem, out ppv);
				hr.ThrowIfFailed();
				_pItem = (IShellItem2)Marshal.GetUniqueObjectForIUnknown(ppv);
				Marshal.Release(ppv);
			}
			else if (allowFake) {
				if (path.Contains('\\')) {
					string parent = Path.GetDirectoryName(path);
					if (!Directory.Exists(path))
						throw new DirectoryNotFoundException(parent);
					string fname = Path.GetFileName(path);
					WIN32_FIND_DATA wd = new WIN32_FIND_DATA() { cFileName = fname };
					using (FakeFile fake = new FakeFile(ref wd, fname)) {
						IntPtr pparent = PInvoke.ILCreateFromPath(parent);
						try {
							IntPtr pidl = PInvoke.ILFindLastID(fake._ppidl);
							IntPtr pUnk = IntPtr.Zero;
							PInvoke.SHCreateItemWithParent(pparent, IntPtr.Zero, pidl, IID.IShellItem, out pUnk).ThrowIfFailed();
							_pItem = (IShellItem2)Marshal.GetUniqueObjectForIUnknown(pUnk);
							Marshal.Release(pUnk);
						}
						finally {
							if (pparent != IntPtr.Zero)
								Marshal.FreeCoTaskMem(pparent);
						}
					}
				}
				else {
					WIN32_FIND_DATA wd = new WIN32_FIND_DATA() { cFileName = path };
					using (FakeFile fake = new FakeFile(ref wd, path)) {
						_pItem = (IShellItem2)fake.GetShellItem();
					}
				}
			}
			else
				throw new FileNotFoundException();
		}

		private ShellItem() { _pItem = null; }
		internal ShellItem(IntPtr pUnk) {
			_pItem = (IShellItem2)Marshal.GetUniqueObjectForIUnknown(pUnk);
		}

		/// <summary>
		/// Releases the internal COM reference to the IShellItem
		/// </summary>
		public void Dispose() {
			if (_pItem != null) {
				Marshal.FinalReleaseComObject(_pItem);
				_pItem = null;
			}
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Ensures that the COM reference is released on finalisation
		/// </summary>
		~ShellItem() { Dispose(); }


		/// <summary>
		/// Returns the full path to this item
		/// </summary>
		public string FullName {
			get {
				string name;
				_pItem.GetDisplayName(SIGDN.SIGDN_DESKTOPABSOLUTEEDITING, out name);
				return name;
			}
		}

		/// <summary>
		/// Gets the display name of this item relative to its parent
		/// </summary>
		public string DisplayName {
			get {
				string name;
				_pItem.GetDisplayName(SIGDN.SIGDN_NORMALDISPLAY, out name);
				return name;
			}
		}

		/// <summary>
		/// Gets the name of this item relative to its parent
		/// </summary>
		public string Name {
			get {
				string name;
				_pItem.GetDisplayName(SIGDN.SIGDN_PARENTRELATIVE, out name);
				return name;
			}
		}

		/// <summary>
		/// Gets the Parent ShellItem of this Item
		/// </summary>
		public ShellItem Parent {
			get {
				ShellItem pParent;
				IntPtr ppsi;
				_pItem.GetParent(out ppsi);
				pParent = new ShellItem(ppsi);
				Marshal.Release(ppsi);
				return pParent;
			}
		}

		/// <summary>
		/// Gets the selected <see cref="Attributes"/> for this item
		/// </summary>
		/// <param name="mask">The attributes required</param>
		/// <returns>The results of the item's attributes and the <paramref name="mask"/> attributes</returns>
		public Attributes GetAttributes(Attributes mask) {
			SFGAO attr;
			_pItem.GetAttributes((SFGAO)mask, out attr);
			return (Attributes)attr;
		}

		/// <summary>
		/// Gets the boolean value of the specified <see cref="PropertyKey"/> 
		/// </summary>
		/// <param name="key">The property being requested</param>
		/// <returns>The boolean result of the property</returns>
		public bool GetBool(PropertyKey key) {
			bool bVal;
			_pItem.GetBool(key.MarshalledPointer, out bVal);
			return bVal;
		}

		/// <summary>
		/// Gets the CLSID value of the specified <see cref="PropertyKey"/> 
		/// </summary>
		/// <param name="key">The property being requested</param>
		/// <returns>The requested CLSID</returns>
		public Guid GetCLSID(PropertyKey key) {
			IntPtr ppg;
			byte[] data = new byte[16];
			_pItem.GetCLSID(key.MarshalledPointer, out ppg);
			Marshal.Copy(ppg, data, 0, 16);
			Marshal.FreeCoTaskMem(ppg);
			return new Guid(data);
		}

		/// <summary>
		/// Gets the file time from the specified <see cref="PropertyKey"/> 
		/// </summary>
		/// <param name="key">The property being requested</param>
		/// <returns>The FILETIME duly converted to a DateTime structure</returns>
		public DateTime GetFileTime(PropertyKey key) {
			long ft;
			_pItem.GetFileTime(key.MarshalledPointer, out ft);
			return DateTime.FromFileTime(ft);
		}

		/// <summary>
		/// Gets the int value from the specified <see cref="PropertyKey"/> 
		/// </summary>
		/// <param name="key">The property being requested</param>
		/// <returns>The integer value of the specified property</returns>
		public int GetInt32(PropertyKey key) {
			int iVal;
			_pItem.GetInt32(key.MarshalledPointer, out iVal);
			return iVal;
		}

		/// <summary>
		/// Gets the value from the specified <see cref="PropertyKey"/> 
		/// </summary>
		/// <param name="key">The property being requested</param>
		/// <returns>The value of the specified property</returns>
		public PropVariant GetProperty(PropertyKey key) {
			IntPtr ppv;
			_pItem.GetProperty(key.MarshalledPointer, out ppv);
			PropVariant v = new PropVariant(ppv);
			Marshal.FreeCoTaskMem(ppv);
			return v;
		}

		/// <summary>
		/// Gets a <see cref="PropertyDescriptionList"/> from the specified <see cref="PropertyKey"/> 
		/// </summary>
		/// <param name="key">The property list being requested</param>
		/// <returns>The list of property descriptions specified by the property key</returns>
		public PropertyDescriptionList GetPropertyDescriptionList(PropertyKey key) {
			IntPtr pUnk;
			_pItem.GetPropertyDescriptionList(key.MarshalledPointer, IID.IProperyDescriptionList, out pUnk);
			PropertyDescriptionList list = new PropertyDescriptionList(pUnk);
			Marshal.Release(pUnk);
			return list;
		}

		/// <summary>
		/// Gets a <see cref="PropertyStore"/> using the specified <see cref="PropertyStore.GetFlags"/> 
		/// </summary>
		/// <param name="flags">The flags to specify the PropertyStore retrieval method</param>
		/// <returns>The PropertyStore associated with this item</returns>
		public PropertyStore GetPropertyStore(PropertyStore.GetFlags flags) {
			IntPtr pUnk;
			_pItem.GetPropertyStore((GETPROPERTYSTOREFLAGS)flags, IID.IPropertyStore, out pUnk);
			PropertyStore store = new PropertyStore(pUnk);
			Marshal.Release(pUnk);
			return store;
		}

		/// <summary>
		/// Gets a <see cref="PropertyStore"/> containing the nominated properties using the specified <see cref="PropertyStore.GetFlags"/> 
		/// </summary>
		/// <param name="flags">The flags to specify the PropertyStore retrieval method</param>
		/// <param name="keys">The <see cref="PropertyKey"/>s required in the store </param>
		/// <returns>The PropertyStore associated with this item containing the nominated properties</returns>
		public PropertyStore GetPropertyStoreForKeys(PropertyStore.GetFlags flags, params PropertyKey[] keys) {
			IntPtr arry = Marshal.AllocCoTaskMem(keys.Length + Marshal.SizeOf(typeof(PROPERTYKEY)));
			try {
				IntPtr ptr = arry;
				foreach (PropertyKey k in keys) {
					PInvoke.MoveMemory(ptr, k.MarshalledPointer, Marshal.SizeOf(typeof(PROPERTYKEY)));
					ptr += Marshal.SizeOf(typeof(PROPERTYKEY));
				}
				IntPtr pUnk;
				_pItem.GetPropertyStoreForKeys(arry, (uint)keys.Length, (GETPROPERTYSTOREFLAGS)flags, IID.IPropertyStore, out pUnk);
				PropertyStore store = new PropertyStore(pUnk);
				Marshal.Release(pUnk);
				return store;
			}
			finally {
				Marshal.FreeCoTaskMem(arry);
			}
		}

		/// <summary>
		/// Gets the string value from the specified <see cref="PropertyKey"/> 
		/// </summary>
		/// <param name="key">The property being requested</param>
		/// <returns>The string value of the specified property</returns>
		public string GetString(PropertyKey key) {
			string sVal;
			_pItem.GetString(key.MarshalledPointer, out sVal);
			return sVal;
		}

		/// <summary>
		/// Gets the unsigned int value from the specified <see cref="PropertyKey"/> 
		/// </summary>
		/// <param name="key">The property being requested</param>
		/// <returns>The unsigned integer value of the specified property</returns>
		public uint GetUInt32(PropertyKey key) {
			uint iVal;
			_pItem.GetUInt32(key.MarshalledPointer, out iVal);
			return iVal;
		}

		/// <summary>
		/// Gets the unsigned long value from the specified <see cref="PropertyKey"/> 
		/// </summary>
		/// <param name="key">The property being requested</param>
		/// <returns>The unsigned long value of the specified property</returns>
		public ulong GetUInt64(PropertyKey key) {
			ulong iVal;
			_pItem.GetUInt64(key.MarshalledPointer, out iVal);
			return iVal;
		}

		/// <summary>
		/// Ensures any cached data on this item is saved
		/// </summary>
		/// <param name="pbc">An <see cref="IBindCtx"/> interface for a Bind Context </param>
		public void Update(IBindCtx pbc) {
			_pItem.Update(pbc);
		}
	}
}
