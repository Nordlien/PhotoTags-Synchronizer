using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using static WinProps.PInvoke;

namespace WinProps {

	/// <summary>
	/// Represents the store of properties associated with an item. This class encapsulates the <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb761474(v=vs.85).aspx">IPropertyStore</a>,
	/// the <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb761466(v=vs.85).aspx">IPropertyStoreCache</a>, and the 
	/// <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb761452(v=vs.85).aspx">IPropertyStroeCapabilities</a> interfaces.
	/// </summary>
	public class PropertyStore : IReadOnlyList<PropertyKey>, IDisposable {

		/// <summary>
		/// Indicates the source of the <see cref="PropertyStore"/> 
		/// </summary>
		[Flags]
		public enum GetFlags {
			/// <summary>
			/// Returns a read-only <see cref="PropertyStore"/> that contains all properties.
			/// </summary>
			Default = 0,
			/// <summary>
			/// Returns only properties controlled by the property handler. Invalid combined with <see cref="Temporary"/>, <see cref="FastOnly"/>, or <see cref="BestEffort"/>
			/// </summary>
			HandlerOnly = 0x1,
			/// <summary>
			/// Returns properties that may be written back to the item. This flag implies <see cref="HandlerOnly"/>, and is invalid combined with <see cref="Temporary"/>, <see cref="FastOnly"/>,
			/// <see cref="BestEffort"/> or <see cref="DelayCreation"/>
			/// </summary>
			ReadWrite = 0x2,
			/// <summary>
			/// Provides an empty, writeable <see cref="PropertyStore"/> that exists only for as long as the item that created it. Implies <see cref="ReadWrite"/>, but cannot be combined with any other flags.
			/// </summary>
			Temporary = 0x4,
			/// <summary>
			/// Provides a <see cref="PropertyStore"/> that does not require reading or writing to the file system or network. Invalid combined with <see cref="Temporary"/>, <see cref="ReadWrite"/>, 
			/// <see cref="HandlerOnly"/> and <see cref="DelayCreation"/> flags.
			/// </summary>
			FastOnly = 0x8,
			/// <summary>
			/// Open a slow item if necessary. Slow items are generally offline files. Invalid in combination with <see cref="Temporary"/> or <see cref="FastOnly"/>.
			/// </summary>
			SlowItem = 0x10,
			/// <summary>
			/// Delays access to memory intensive operations, such as opening a file, until a request is made that requires such access. Invalid in combination with <see cref="Temporary"/> or <see cref="ReadWrite"/> .
			/// </summary>
			DelayCreation = 0x20,
			/// <summary>
			/// Does not fail if certain properties are unavailable. Cannot be combined with <see cref="Temporary"/>, <see cref="ReadWrite"/>, or <see cref="HandlerOnly"/> 
			/// </summary>
			BestEffort = 0x40,
			/// <summary>
			/// Obsolete as of Windows Server 2008 and Vista
			/// Do not obtain an opportunistic lock on the file. Normally, the shell will acquire an oplock on the file before it binds to the property handler. This flag should only ever be used if 
			/// the caller already has an oplock on the file.
			/// </summary>
			[Obsolete]
			NoOplock = 0x80,
			/// <summary>
			/// Not applicable to this library. Used for obtaining properties on the indexer for WDS results
			/// </summary>
			PreferQueryProperties = 0x100,
			/// <summary>
			/// Acquire properties from a file's secondary stream
			/// </summary>
			Extrinsic = 0x200,
			/// <summary>
			/// Acquire properties from a file's secondary stream only
			/// </summary>
			ExtrinsicOnly = 0x400
		}

		/// <summary>
		/// Reflects the status of a property within the <see cref="PropertyStore"/> 
		/// </summary>
		public enum PropertyState {
			/// <summary>
			/// The property is unaltered
			/// </summary>
			Normal = 0,
			/// <summary>
			/// The property does not exist for the item
			/// </summary>
			NotInSource = 1,
			/// <summary>
			/// The property has been changed, and has not yet been committed to the store
			/// </summary>
			Dirty = 2,
			/// <summary>
			/// The property cannot be modified
			/// </summary>
			ReadOnly = 3
		}

		IPropertyStore _pStore = null;
		IShellItem2 _pSource = null;

		internal PropertyStore(IntPtr pUnk) {
			_pStore = (IPropertyStore)Marshal.GetUniqueObjectForIUnknown(pUnk);
		}

		/// <summary>
		/// Creates an in-memory <see cref="GetFlags.ReadWrite"/> <see cref="PropertyStore"/> 
		/// </summary>
		public PropertyStore() {
			IntPtr ppv = IntPtr.Zero;
			PSCreateMemoryPropertyStore(IID.IPropertyStore, out ppv);
			_pStore = (IPropertyStore)Marshal.GetUniqueObjectForIUnknown(ppv);
			Marshal.Release(ppv);
		}

		/// <summary>
		/// Loads a <see cref="PropertyStore"/> for the specified file or extension. Note that if creating a PropertyStore based on extension only, this store will be read only. The <see cref="GetFlags.BestEffort"/> 
		/// should be used for retrieving that store, as the file does not exist to enable the reading of properties that would be stored in it, and any other flags are likely to produce an exception.
		/// </summary>
		/// <param name="path">The path or extension of the file the store is required for</param>
		/// <param name="flags">The <see cref="GetFlags"/> indicating the type of store to load</param>
		public PropertyStore(string path, GetFlags flags) {
			if (File.Exists(path)) {
				IntPtr pidl = ILCreateFromPath(Path.GetFullPath(path));
				try {
					IntPtr shUnk = IntPtr.Zero;
					HRESULT hr = SHCreateItemFromIDList(pidl, IID.IShellItem2, out shUnk);
					_pSource = (IShellItem2)Marshal.GetUniqueObjectForIUnknown(shUnk);
					try {
						IntPtr pUnk = IntPtr.Zero;
						_pSource.GetPropertyStore((GETPROPERTYSTOREFLAGS)flags, IID.IPropertyStore, out pUnk);
						try {
							_pStore = (IPropertyStore)Marshal.GetUniqueObjectForIUnknown(pUnk);
						}
						finally { Marshal.Release(pUnk); }
					}
					finally {
						if (shUnk != IntPtr.Zero)
							Marshal.Release(shUnk);
					}
				}
				finally {
					if (pidl != IntPtr.Zero)
						Marshal.FreeCoTaskMem(pidl);
				}
			}
			else {
				string file;
				path = path.Substring(path.LastIndexOf('\\') + 1);
				if (path[0] == '.' || !path.Contains('.'))
					file = Path.ChangeExtension("_Fake", path);
				else
					file = Path.GetFileName(path);
				WIN32_FIND_DATA fd = new WIN32_FIND_DATA() { nFileSizeLow = 42 };
				FakeFile f = new FakeFile(ref fd, file);
				_pSource = (IShellItem2)f.GetShellItem();
				IntPtr pUnk = IntPtr.Zero;
				try {
					_pSource.GetPropertyStore((GETPROPERTYSTOREFLAGS)flags, IID.IPropertyStore, out pUnk);
					_pStore = (IPropertyStore)Marshal.GetUniqueObjectForIUnknown(pUnk);
				}
				finally {
					if (pUnk != IntPtr.Zero)
						Marshal.Release(pUnk);
				}
			}
		}

		/// <summary>
		/// Loads a <see cref="PropertyStore"/> for the specified file or extension, containing only the specified properties. Note that if creating a PropertyStore based on extension only, this store will be read only. The <see cref="GetFlags.BestEffort"/> 
		/// should be used for retrieving that store, as the file does not exist to enable the reading of properties that would be stored in it, and any other flags are likely to produce an exception.
		/// </summary>
		/// <param name="path">The path or extension of the file the store is required for</param>
		/// <param name="keys">The required <see cref="PropertyKey"/>s</param>
		/// <param name="flags">The <see cref="GetFlags"/> indicating the type of store to load</param>
		public PropertyStore(string path, IEnumerable<PropertyKey> keys, GetFlags flags) {
			int nKeys = keys.Count();
			IntPtr keyArray = Marshal.AllocCoTaskMem(nKeys * Marshal.SizeOf(typeof(PROPERTYKEY)));
			try {
				IntPtr keyPtr = keyArray;
				foreach (PropertyKey key in keys) {
					Marshal.StructureToPtr<PROPERTYKEY>(key.PROPERTKEY, keyPtr, false);
					keyPtr += Marshal.SizeOf(typeof(PROPERTYKEY));
				}
				if (File.Exists(path)) {
					IntPtr pidl = ILCreateFromPath(Path.GetFullPath(path));
					try {
						IntPtr shUnk = IntPtr.Zero;
						SHCreateItemFromIDList(pidl, IID.IShellItem2, out shUnk);
						_pSource = (IShellItem2)Marshal.GetUniqueObjectForIUnknown(shUnk);
						try {
							IntPtr pUnk = IntPtr.Zero;
							_pSource.GetPropertyStoreForKeys(keyArray, (uint)nKeys, (GETPROPERTYSTOREFLAGS)flags, IID.IPropertyStore, out pUnk);
							try {
								_pStore = (IPropertyStore)Marshal.GetUniqueObjectForIUnknown(pUnk);
							}
							finally { Marshal.Release(pUnk); }
						}
						finally {
							if (shUnk != IntPtr.Zero)
								Marshal.Release(shUnk);
						}
					}
					finally {
						if (pidl != IntPtr.Zero)
							Marshal.FreeCoTaskMem(pidl);
					}
				}
				else {
					string file;
					path = path.Substring(path.LastIndexOf('\\') + 1);
					if (path[0] == '.' || !path.Contains('.'))
						file = Path.ChangeExtension("_Fake", path);
					else
						file = Path.GetFileName(path);
					WIN32_FIND_DATA fd = new WIN32_FIND_DATA() { nFileSizeLow = 42 };
					FakeFile f = new FakeFile(ref fd, file);
					_pSource = (IShellItem2)f.GetShellItem();
					IntPtr pUnk = IntPtr.Zero;
					try {
						_pSource.GetPropertyStoreForKeys(keyArray, (uint)nKeys, (GETPROPERTYSTOREFLAGS)flags, IID.IPropertyStore, out pUnk);
						_pStore = (IPropertyStore)Marshal.GetUniqueObjectForIUnknown(pUnk);
					}
					finally {
						if (pUnk != IntPtr.Zero)
							Marshal.Release(pUnk);
					}
				}
			}
			finally {
				if (keyArray != IntPtr.Zero)
					Marshal.FreeCoTaskMem(keyArray);
			}
		}

		/// <summary>
		/// Releases the reference to the internal COM objects required for this object
		/// </summary>
		public void Dispose() {
			if (_pStore != null) {
				Marshal.FinalReleaseComObject(_pStore);
				_pStore = null;
			}
			if (_pSource != null) {
				Marshal.FinalReleaseComObject(_pSource);
				_pSource = null;
			}
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Ensures all COM objects are released when the object is finalised
		/// </summary>
		~PropertyStore() {
			Dispose();
		}

		/// <summary>
		/// Gets the number of properties in the <see cref="PropertyStore"/> 
		/// </summary>
		public int Count {
			get {
				uint count;
				_pStore.GetCount(out count);
				return (int)count;
			}
		}

		/// <summary>
		/// Gets the <see cref="PropertyStore"/> in this <see cref="PropertyStore"/> at the specified <paramref name="index"/>
		/// </summary>
		/// <param name="index">The zero-based index of the required property</param>
		/// <returns>The PropertyKey identifying the property at the specified <paramref name="index"/> </returns>
		public PropertyKey this[int index] {
			get {
				IntPtr ppk = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPERTYKEY)));
				_pStore.GetAt((uint)index, ppk);
				return new PropertyKey(ppk);
			}
		}

		/// <summary>
		/// Gets or sets the value of a property in this <see cref="PropertyStore"/> 
		/// </summary>
		/// <param name="key">The <see cref="PropertyKey"/> identifying the property value requested</param>
		/// <returns>A <see cref="PropVariant"/> containing the requested property value</returns>
		public PropVariant this[PropertyKey key] {
			get {
				return GetValue(key);
			}
			set {
				SetValue(key, value);
			}
		}

		/// <summary>
		/// Gets the value of the specified <see cref="PropertyKey"/> 
		/// </summary>
		/// <param name="propKey">The <see cref="PropertyKey"/> identifying the required property</param>
		/// <returns>A <see cref="PropVariant"/> containing the value of the requested property</returns>
		public PropVariant GetValue(PropertyKey propKey) {
			IntPtr pValue = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			_pStore.GetValue(propKey.MarshalledPointer, pValue);
			return new PropVariant(pValue);
		}

		/// <summary>
		/// Gets the value of the specified <see cref="PropertyKey"/> 
		/// </summary>
		/// <param name="propKey">The <see cref="PropertyKey"/> identifying the required property</param>
		/// <param name="value">A <see cref="PropVariant"/> containing the new value for the property</param>
		public void SetValue(PropertyKey propKey, PropVariant value) {
			_pStore.SetValue(propKey.MarshalledPointer, value.MarshalledPointer);
		}

		/// <summary>
		/// Saves the properties back to the file they were retrieved from
		/// </summary>
		public void Commit() {
			_pStore.Commit();
		}

		/// <summary>
		/// Gets an <see cref="IEnumerator{PropertyKey}"/> that can iterate the <see cref="PropertyKey"/>s in this <see cref="PropertyStore"/> 
		/// </summary>
		/// <returns>An enumerator that can iterate the PropertyKeys in this store</returns>
		public IEnumerator<PropertyKey> GetEnumerator() {
			int count = Count;
			for (int i = 0; i < count; ++i) {
				yield return this[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		/// <summary>
		/// Gets the <see cref="PropertyState"/> for the specified <see cref="PropertyKey"/>
		/// </summary>
		/// <param name="propKey">The PropertyKey identifying the property required</param>
		/// <returns>The state of the requested property</returns>
		public PropertyState GetState(PropertyKey propKey) {
			try {
				PSC_STATE state;
				IPropertyStoreCache cache = (IPropertyStoreCache)_pStore;
				cache.GetState(propKey.MarshalledPointer, out state);
				return (PropertyState)state;
			}
			catch (Exception e) {
				throw new NotImplementedException("IPropertyStoreCache is not implemented by this PropertyStore", e);
			}
		}

		/// <summary>
		/// Sets the <see cref="PropertyState"/> for the selected <see cref="PropertyKey"/> 
		/// </summary>
		/// <param name="propKey">The PropertyKey identifying the property for the state change</param>
		/// <param name="state">The new state of the selected property</param>
		public void SetState(PropertyKey propKey, PropertyState state) {
			try {
				IPropertyStoreCache cache = (IPropertyStoreCache)_pStore;
				cache.SetState(propKey.MarshalledPointer, (PSC_STATE)state);
			}
			catch (Exception e) {
				throw new NotImplementedException("IPropertyStoreCache is not implemented by this PropertyStore", e);
			}
		}

		/// <summary>
		/// Determines if a property in this <see cref="PropertyStore"/>  can be modified
		/// </summary>
		/// <param name="propKey">The <see cref="PropertyKey"/> identifying the property in question</param>
		/// <returns>True if the property is editable, else false</returns>
		public bool IsEditable(PropertyKey propKey) {
			try {
				IPropertyStoreCapabilities caps = (IPropertyStoreCapabilities)_pStore;
				HRESULT hr = caps.IsPropertyWritable(propKey.MarshalledPointer);
				if (hr == HRESULT.S_OK)
					return true;
			}
			catch (Exception e) {
				throw new NotImplementedException("IPropertyStoreCapabilities is not implemented by this PropertyStore", e);
			}
			return false;
		}

		/// <summary>
		/// This function adds default properties to a <see cref="GetFlags.ReadWrite"/> <see cref="PropertyStore"/>.
		/// </summary>
		/// <param name="extn">The file extension for which the default properties are required</param>
		public void AddDefaultsByExtension(string extn) {
			SHAddDefaultPropertiesByExt(extn, _pStore).ThrowIfFailed();
		}
	}
}
