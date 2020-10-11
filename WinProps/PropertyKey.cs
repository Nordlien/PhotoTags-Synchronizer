using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static WinProps.PInvoke;

namespace WinProps {
	/// <summary>
	/// Identifies a Windows Property item. This class encapsulates the native <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb773381(v=vs.85).aspx">PROPERTYKEY</a> structure.
	/// </summary>
	[Serializable]
	public class PropertyKey : IDisposable, IComparable<PropertyKey> {
		internal PROPERTYKEY _propKey;
		[NonSerialized]
		private IntPtr _ppk = IntPtr.Zero;
		[NonSerialized]
		private bool _dirty = false;

		internal PropertyKey(PROPERTYKEY propKey) {
			_propKey = propKey;
		}
		internal PropertyKey(IntPtr ppk) {
			_ppk = ppk;
			_propKey = Marshal.PtrToStructure<PROPERTYKEY>(_ppk);
		}
		internal PropertyKey() { _propKey = new PROPERTYKEY(); }

		/// <summary>
		/// Creates a <see cref="PropertyKey"/>  from either a canonical name or a formatted PropertyKey.
		/// </summary>
		/// <param name="formatString">This string may either be the canonical name of a PropertyKey, or a formatted PropertyKey string.
		/// The former of these formats contain two or more user friendly names, delimited by a period(.) e.g. "System.Title", "System.Image.Dimensions". The other format is a string consisting of the
		/// PropertyKey's <see cref="FmtId"/>, which is a <see cref="Guid"/>, and the <see cref="Pid"/>, which is an integral value. The string will be formatted as "{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx} nn", 
		/// where the {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx} is the formatted Guid, and the nn is the Pid.
		/// </param>
		public PropertyKey(string formatString) {
			HRESULT hr;
			_ppk = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPERTYKEY)));
			try {
				// Is it a formatted propertykey or a canonical name?
				if (formatString.StartsWith("{")) {
					hr = PSPropertyKeyFromString(formatString, _ppk);
					if (hr.Failed) throw hr.GetException();
				}
				else {
					hr = PSGetPropertyKeyFromName(formatString, _ppk);
					if (hr.Failed) throw hr.GetException();
				}
			}
			catch {
				Marshal.FreeCoTaskMem(_ppk);
				_ppk = IntPtr.Zero;
				throw;
			}
			_propKey = Marshal.PtrToStructure<PROPERTYKEY>(_ppk);
		}

		/// <summary>
		/// Creates a <see cref="PropertyKey"/> with the specified <see cref="FmtId"/> and <see cref="Pid"/>
		/// </summary>
		/// <param name="fmtid">The <see cref="Guid"/> that identifies this PropertyKey</param>
		/// <param name="pid">The numerical pid that identifies this PropertyKey</param>
		public PropertyKey(Guid fmtid, uint pid) {
			_propKey = new PROPERTYKEY() { fmtid = fmtid, pid = pid };
		}

		/// <summary>
		/// Gets or sets the <see cref="Guid"/>  that identifies this <see cref="PropertyKey"/> 
		/// </summary>
		public Guid FmtId {
			get { return _propKey.fmtid; }
			set {
				if (_propKey.fmtid != value) {
					_propKey.fmtid = value;
					_dirty = true;
				}
			}
		}
		/// <summary>
		/// Gets or sets the numeric property id that identifies this <see cref="PropertyKey"/> 
		/// </summary>
		public uint Pid {
			get { return _propKey.pid; }
			set {
				if (_propKey.pid != value) {
					_propKey.pid = value;
					_dirty = true;
				}
			}
		}

		/// <summary>
		/// Gets the canonical name that identifies this <see cref="PropertyKey"/> 
		/// </summary>
		public string CanonicalName {
			get {
				string name;
				HRESULT hr = PSGetNameFromPropertyKey(MarshalledPointer, out name);
				if (hr.Failed) throw hr.GetException();
				return name;
			}
		}

		internal IntPtr MarshalledPointer {
			get {
				if (_ppk == IntPtr.Zero) {
					_ppk = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPERTYKEY)));
					Marshal.StructureToPtr(_propKey, _ppk, false);
				}
				else if (_dirty) {
					Marshal.StructureToPtr(_propKey, _ppk, true);
				}
				_dirty = false;
				return _ppk;
			}
		}

		internal PROPERTYKEY PROPERTKEY {
			get { return _propKey; }
			set {
				if (_propKey.fmtid != value.fmtid || _propKey.pid != value.pid) {
					_propKey = value;
					_dirty = true;
				}
			}
		}

		/// <summary>
		/// Overrides <see cref="IComparable{PropertyKey}.CompareTo(PropertyKey)"/>. Compares two <see cref="PropertyKey"/>s by <see cref="FmtId"/> and <see cref="Pid"/> 
		/// </summary>
		/// <param name="other">The PropertyKey to compare with</param>
		/// <returns>An integer value indicating the result of the comparison</returns>
		public int CompareTo(PropertyKey other) {
			int r = FmtId.CompareTo(other.FmtId);
			if (r == 0)
				r = Pid.CompareTo(other.Pid);
			return r;
		}

		/// <summary>
		/// Overrides <see cref="object.Equals(object)"/>. Tests two <see cref="PropertyKey"/>s for equality.
		/// </summary>
		/// <param name="obj">The object to test against</param>
		/// <returns>true if the PropertyKeys are the same, else false</returns>
		public override bool Equals(object obj) {
			if (obj is PropertyKey)
				return _propKey.fmtid == ((PropertyKey)obj)._propKey.fmtid && _propKey.pid == ((PropertyKey)obj)._propKey.pid;
			else if (obj is PROPERTYKEY)
				return _propKey.fmtid == ((PROPERTYKEY)obj).fmtid && _propKey.pid == ((PROPERTYKEY)obj).pid;
			return false;
		}

		/// <summary>
		/// Overrides <see cref="object.GetHashCode"/>.
		/// </summary>
		/// <returns>A hash-code for the PropertyKey</returns>
		public override int GetHashCode() {
			return unchecked((int)(((uint)_propKey.fmtid.GetHashCode() << 7) ^ _propKey.pid));
		}

		/// <summary>
		/// The format used for converting this <see cref="PropertyKey"/> <see cref="ToString(PKeyFormat)"/> 
		/// </summary>
		public enum PKeyFormat {
			/// <summary>
			/// Formats the PropertyKey as "{<see cref="FmtId"/>} <see cref="Pid"/>"
			/// </summary>
			FormattedKey,
			/// <summary>
			/// Formats the PropertyKey into its Canonical name
			/// </summary>
			CanonicalName
		}

		/// <summary>
		/// Overrides <see cref="object.ToString"/>. Formats the <see cref="PropertyKey"/> in its <see cref="PKeyFormat.FormattedKey"/> format.
		/// </summary>
		/// <returns>A string representation of this PropertyKey</returns>
		public override string ToString() {
			return ToString(PKeyFormat.FormattedKey);
		}

		/// <summary>
		/// Formats this <see cref="PropertyKey"/> according to the required <see cref="PKeyFormat"/> 
		/// </summary>
		/// <param name="format">The required format for the PropertyKey</param>
		/// <returns>A string representing this PropertyKey is the specified format</returns>
		public string ToString(PKeyFormat format) {
			if (format == PKeyFormat.CanonicalName)
				return CanonicalName;
			return string.Format("{0} {1}", _propKey.fmtid.ToString("B"), _propKey.pid);
		}

		/// <summary>
		/// Tests two <see cref="PropertyKey"/>s for equality
		/// </summary>
		/// <param name="key1">The PropertyKey on the LHS of the equation</param>
		/// <param name="key2">The PropertyKey on the RHS of the equation</param>
		/// <returns>True if the PropertyKeys are the same, else false</returns>
		public static bool operator ==(PropertyKey key1, PropertyKey key2) {
			if (((object)key1) == null)
				return ((object)key2) == null;
			return key1.Equals(key2);
		}

		/// <summary>
		/// Tests two <see cref="PropertyKey"/>s for inequality
		/// </summary>
		/// <param name="key1">The PropertyKey on the LHS of the equation</param>
		/// <param name="key2">The PropertyKey on the RHS of the equation</param>
		/// <returns>True if the PropertyKeys are different, else false</returns>
		public static bool operator !=(PropertyKey key1, PropertyKey key2) {
			if (((object)key1) == null)
				return ((object)key2) != null;
			return !key1.Equals(key2);
		}

		static Dictionary<string, PropertyKey> _keys;

		/// <summary>
		/// Gets a collection of all of the <see cref="PropertyKey"/>s known to the system, indexed by their canonical name
		/// </summary>
		public static IReadOnlyDictionary<string, PropertyKey> Keys {
			get {
				if (_keys == null) {
					_keys = new Dictionary<string, PropertyKey>();
					IntPtr puList = IntPtr.Zero;
					PSEnumeratePropertyDescriptions(0, IID.IProperyDescriptionList, out puList);
					try {
						using (PropertyDescriptionList pList = new PropertyDescriptionList(puList)) {
							foreach (PropertyDescription pDesc in pList) {
								_keys.Add(pDesc.CanonicalName, pDesc.PropertyKey);
							}
						}
					}
					finally {
						if (puList != IntPtr.Zero)
							Marshal.Release(puList);
					}
				}
				return _keys;
			}
		}


		#region IDisposable Support
		private bool disposedValue = false;

		/// <summary>
		/// Ensures that any unmanaged memory is released when the object is finalised
		/// </summary>
		~PropertyKey() {
			Dispose();
		}

		/// <summary>
		/// Frees unmanaged memory allocated for this object
		/// </summary>
		public void Dispose() {
			if (!disposedValue) {
				if (_ppk != IntPtr.Zero) {
					Marshal.DestroyStructure<PROPERTYKEY>(_ppk);
					Marshal.FreeCoTaskMem(_ppk);
					_ppk = IntPtr.Zero;
				}
				disposedValue = true;
			}
			GC.SuppressFinalize(this);
		}
		#endregion
	}

}
