using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using static WinProps.PInvoke;

namespace WinProps {
	/// <summary>
	/// Defines a collection of related <see cref="PropertyDescription"/> items. This class encapsulates the COM 
	/// <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb761511(v=vs.85).aspx">IPropertyDescriptionList</a> interface.
	/// </summary>
	public class PropertyDescriptionList : IReadOnlyList<PropertyDescription>, IDisposable {

		IPropertyDescriptionList _propList = null;

		internal PropertyDescriptionList(IntPtr pUnk) {
			_propList = (IPropertyDescriptionList)Marshal.GetUniqueObjectForIUnknown(pUnk);
		}

		/// <summary>
		/// Creates a <see cref="PropertyDescriptionList"/> containing the <see cref="PropertyDescription"/>s defined for <paramref name="strPropList"/>
		/// </summary>
		/// <param name="strPropList">A string declaring which properties to include in this PropertyDescriptionList. The format of this string must be "prop:prop1;prop2;...;propN"
		/// where "prop1 - propN" are the Canonical names of the required <see cref="PropertyKey"/>s</param>
		public PropertyDescriptionList(string strPropList) {
			IntPtr pUnk;
			HRESULT hr = PSGetPropertyDescriptionListFromString(strPropList, IID.IProperyDescriptionList, out pUnk);
			if (hr.Failed) throw hr.GetException();
			try {
				_propList = (IPropertyDescriptionList)Marshal.GetUniqueObjectForIUnknown(pUnk);
			}
			finally {
				Marshal.Release(pUnk);
			}
		}

		/// <summary>
		/// Creates a <see cref="PropertyDescriptionList"/> containing <see cref="PropertyDescription"/>s for each property in <paramref name="properties"/>
		/// </summary>
		/// <param name="properties">An <see cref="IEnumerable{T}"/> of strings containing the canonical names for the required <see cref="PropertyKey"/>s</param>
		public PropertyDescriptionList(IEnumerable<string> properties) {
			string strPropList = "prop:" + string.Join(";", properties);
			IntPtr pUnk;
			HRESULT hr = PSGetPropertyDescriptionListFromString(strPropList, IID.IProperyDescriptionList, out pUnk);
			if (hr.Failed) throw hr.GetException();
			try {
				_propList = (IPropertyDescriptionList)Marshal.GetUniqueObjectForIUnknown(pUnk);
			}
			finally {
				Marshal.Release(pUnk);
			}
		}

		/// <summary>
		/// Creates a <see cref="PropertyDescriptionList"/> based on properties defined for a particular file or file type.
		/// </summary>
		/// <param name="path">The path or extension of the file type you want the PropertyDescriptionList for</param>
		/// <param name="propListKey">This <see cref="PropertyKey"/> defines the usage of the properties defined in the PropertyDescriptionList. It will be one of the
		/// <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/ff521713(v=vs.85).aspx">PropList</a> values</param>
		public PropertyDescriptionList(string path, PropertyKey propListKey) {
			IShellItem2 item = null;
			FakeFile file = null;
			if (File.Exists(path)) {
				IntPtr itemUnk;
				IntPtr pidl = ILCreateFromPath(Path.GetFullPath(path));
				SHCreateItemFromIDList(pidl, IID.IShellItem2, out itemUnk);
				item = (IShellItem2)Marshal.GetUniqueObjectForIUnknown(itemUnk);
				Marshal.Release(itemUnk);
				Marshal.FreeCoTaskMem(pidl);
			}
			else {
				string name;
				path = path.Substring(path.LastIndexOf('\\') + 1);
				if (path[0] == '.' || !(path.Contains('.')))
					name = Path.ChangeExtension("_fakeFile", path);
				else
					name = Path.GetFileName(path);
				WIN32_FIND_DATA fd = new WIN32_FIND_DATA() { dwFileAttributes = FileAttributes.Normal, nFileSizeLow = 42 };
				file = new FakeFile(ref fd, name);
				item = (IShellItem2)file.GetShellItem();
			}
			try {
				IntPtr ppUnk = IntPtr.Zero; ;
				item.GetPropertyDescriptionList(propListKey.MarshalledPointer, IID.IProperyDescriptionList, out ppUnk);
				_propList = (IPropertyDescriptionList)Marshal.GetUniqueObjectForIUnknown(ppUnk);
				Marshal.Release(ppUnk);
			}
			finally {
				if (item != null)
					Marshal.FinalReleaseComObject(item);
				if (file != null)
					file.Dispose();
			}
		}

		/// <summary>
		/// Creates a <see cref="PropertyDescriptionList"/> for the <see cref="PropertyKey"/>s required
		/// </summary>
		/// <param name="properties">An <see cref="IEnumerable{T}"/> of PropertyKeys required in the PropertyDescriptionList</param>
		public PropertyDescriptionList(IEnumerable<PropertyKey> properties) {
			string strPropList = "prop:" + string.Join(";", properties.Select(p=>p.CanonicalName));
			IntPtr pUnk;
			HRESULT hr = PSGetPropertyDescriptionListFromString(strPropList, IID.IProperyDescriptionList, out pUnk);
			if (hr.Failed) throw hr.GetException();
			try {
				_propList = (IPropertyDescriptionList)Marshal.GetUniqueObjectForIUnknown(pUnk);
			}
			finally {
				Marshal.Release(pUnk);
			}
		}

		/// <summary>
		/// Gets the <see cref="PropertyDescription"/> at the specified index
		/// </summary>
		/// <param name="index">The Index of the required PropertyDescription</param>
		/// <returns>The requested PropertyDescription</returns>
		public PropertyDescription this[int index] {
			get {
				IntPtr ppv;
				_propList.GetAt((uint)index, IID.IPropertyDescription, out ppv);
				try {
					return new PropertyDescription(ppv);
				}
				finally {
					Marshal.Release(ppv);
				}
			}
		}

		/// <summary>
		/// Gets the <see cref="PropertyDescription"/> with the specified <see cref="PropertyKey"/> 
		/// </summary>
		/// <param name="key">The PropertyKey of the required PropertyDescription</param>
		/// <returns>The requested PropertyDescription</returns>
		public PropertyDescription this[PropertyKey key] {
			get {
				return this.First(desc => desc.PropertyKey == key);
			}
		}

		/// <summary>
		/// Gets the number of <see cref="PropertyDescription"/> items in this <see cref="PropertyDescriptionList"/>
		/// </summary>
		public int Count {
			get {
				uint count;
				_propList.GetCount(out count);
				return (int)count;
			}
		}

		/// <summary>
		/// Returns an <see cref="IEnumerator"/> of <see cref="PropertyDescription"/>s for this <see cref="PropertyDescriptionList"/> 
		/// </summary>
		/// <returns>An enumerator that can iterate the PropertyDescriptions in this list</returns>
		public IEnumerator<PropertyDescription> GetEnumerator() {
			int count = Count;
			for (int i = 0; i < count; ++i)
				yield return this[i];
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		/// <summary>
		/// Releases the internal COM IPropertyDescriptionList reference
		/// </summary>
		public void Dispose() {
			if (_propList != null) {
				Marshal.FinalReleaseComObject(_propList);
				_propList = null;
			}
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Ensures that the unmanaged COM reference is released.
		/// </summary>
		~PropertyDescriptionList() {
			Dispose();
		}
	}
}
