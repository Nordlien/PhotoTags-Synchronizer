using System;
using System.Runtime.InteropServices;

namespace WinProps {
	[Serializable]
	class GUID : IDisposable {
		internal Guid _guid;
		[NonSerialized]
		IntPtr _pGUID = IntPtr.Zero;
		[NonSerialized]
		bool _dirty = false;

		public GUID(Guid guid) { _guid = guid; }
		public GUID(string guid) { _guid = new Guid(guid); }
		public GUID(int a, short b, short c, byte[] d) { _guid = new Guid(a, b, c, d); }
		public GUID(byte[] b) { _guid = new Guid(b); }
		public GUID(int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k) { _guid = new Guid(a, b, c, d, e, f, g, h, i, j, k); }
		public GUID(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k) { _guid = new Guid(a, b, c, d, e, f, g, h, i, j, k); }
		public GUID() { _guid = new Guid(); }

		public IntPtr MarshalledPointer {
			get {
				if (_pGUID == IntPtr.Zero) {
					_pGUID = Marshal.AllocCoTaskMem(16);
					_dirty = true;
				}
				if (_dirty) {
					if (_guid == Guid.Empty) {
						if (_pGUID != IntPtr.Zero)
							Marshal.FreeCoTaskMem(_pGUID);
						_pGUID = IntPtr.Zero;
					}
					else
						Marshal.Copy(_guid.ToByteArray(), 0, _pGUID, 16);
					_dirty = false;
				}
				return _pGUID;
			}
		}
		internal void MarshalPointerToValue() {
			byte[] data = new byte[16];
			Marshal.Copy(_pGUID, data, 0, 16);
			_guid = new Guid(data);
			_dirty = false;
		}

		public override bool Equals(object obj) {
			if (obj == null)
				return false;
			if (obj is GUID)
				return _guid.Equals(((GUID)obj)._guid);
			if (obj is Guid)
				return _guid.Equals((Guid)obj);
			if (obj is byte[])
				return _guid.ToByteArray().Equals((byte[])obj);
			return base.Equals(obj);
		}

		public override int GetHashCode() {
			return _guid.GetHashCode();
		}

		public override string ToString() {
			return _guid.ToString();
		}

		public string ToString(string format, IFormatProvider provider) {
			return _guid.ToString(format, provider);
		}

		public string ToString(string format) {
			return _guid.ToString(format);
		}

		public Guid Guid {
			get { return _guid; }
			set {
				_guid = value;
				_dirty = true;
			}
		}


		public static implicit operator GUID(Guid guid) { return new GUID(guid); }
		public static implicit operator Guid(GUID guid) { return guid.Guid; }
		public static implicit operator IntPtr(GUID guid) { return guid.MarshalledPointer; }

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing) {
			if (!disposedValue) {
				if (_pGUID != IntPtr.Zero) {
					Marshal.FreeCoTaskMem(_pGUID);
					_pGUID = IntPtr.Zero;
				}
				disposedValue = true;
			}
		}

		~GUID() {
			Dispose(false);
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
