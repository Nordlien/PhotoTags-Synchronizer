using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using static WinProps.PInvoke;

namespace WinProps {

	/// <summary>
	/// Exception thrown when an attempt is made to change a <see cref="PropVariant"/>'s type to an incompatible type.
	/// </summary>
	public class VariantTypeException : Exception {
		/// <summary>
		/// Creates a <see cref="VariantTypeException"/> with the specified error message
		/// </summary>
		/// <param name="message"></param>
		public VariantTypeException(string message) : base(message) { }
		/// <summary>
		/// Creates a <see cref="VariantTypeException"/> with the specified error message, and a reference to the exception that caused this object to be created.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public VariantTypeException(string message, Exception inner) : base(message, inner) { }
	}

	class PropVariantComparer : IComparer<PropVariant> {
		public enum CompareUnit {
			Default = 0,
			Second = 1,
			Minute = 2,
			Hour = 3,
			Day = 4,
			Month = 5,
			Year = 6
		}

		[Flags]
		public enum CompareFlags {
			Default = 0,
			TreatEmptyAsGreaterThan = 1,
			UseStrCmp = 2,
			UseStrCmpC = 4,
			UseStrCmpI = 8,
			UseStrCmpIC = 16
		}

		private CompareUnit _compareUnit = CompareUnit.Default;
		private CompareFlags _compareFlags = CompareFlags.Default;

		public PropVariantComparer() { }
		public PropVariantComparer(CompareUnit unit) { _compareUnit = unit; }
		public PropVariantComparer(CompareFlags flags) { _compareFlags = flags; }
		public PropVariantComparer(CompareUnit unit, CompareFlags flags) { _compareUnit = unit; _compareFlags = flags; }

		public int Compare(PropVariant x, PropVariant y) {
			return PropVariantCompareEx(x.MarshalledPointer, y.MarshalledPointer, _compareUnit, _compareFlags);
		}
	}

	/// <summary>
	/// A container for a property's value
	/// </summary>
	[Serializable]
	public class PropVariant : IDisposable, IComparable<PropVariant>, ISerializable {
		[NonSerialized]
		private PROPVARIANT _propVar;
		[NonSerialized]
		private IntPtr _ppv = IntPtr.Zero;
		[NonSerialized]
		private bool _dirty = false;

		static PropVariantComparer DefaultComparer { get; } = new PropVariantComparer();

		/// <summary>
		/// The format in which to store a <see cref="Guid"/> 
		/// </summary>
		public enum GuidFormat {
			/// <summary>
			/// Stored as a <see cref="Guid"/>  in the PropVariant
			/// </summary>
			Native,
			/// <summary>
			/// The <see cref="Guid"/> is stored as a string
			/// </summary>
			String,
			/// <summary>
			/// The <see cref="Guid"/> is stored as an array of 16 bytes
			/// </summary>
			Buffer
		}

		#region Constructors

		internal PropVariant(PROPVARIANT pv) {
			_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			IntPtr src = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			Marshal.StructureToPtr(pv, src, false);
			try {
				HRESULT hr;
				if ((hr = PropVariantCopy(_ppv, src)).Failed) throw (Exception)hr;
			}
			catch {
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = IntPtr.Zero;
				throw;
			}
			finally {
				Marshal.FreeCoTaskMem(src);
			}
			MarshalPointerToValue();
		}

		internal PropVariant(IntPtr ppv) {
			_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			try {
				HRESULT hr;
				if ((hr = PropVariantCopy(_ppv, ppv)).Failed) throw (Exception)hr;
			}
			catch {
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = IntPtr.Zero;
				throw;
			}
			MarshalPointerToValue();
		}

		/// <summary>
		/// Creates a new, empty <see cref="PropVariant"/> 
		/// </summary>
		public PropVariant() {
			_propVar = new PROPVARIANT();
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with the value from another.
		/// </summary>
		/// <param name="pv"></param>
		public PropVariant(PropVariant pv) {
			_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			try {
				HRESULT hr;
				if ((hr = PropVariantCopy(_ppv, pv.MarshalledPointer)).Failed) throw (Exception)hr;
			}
			catch {
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = IntPtr.Zero;
				throw;
			}
			MarshalPointerToValue();
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with a boolean value.
		/// </summary>
		/// <param name="boolVal">The initial value of the new PropVariant</param>
		public PropVariant(bool boolVal) {
			_propVar = new PROPVARIANT();
			_propVar.VarType = VARENUM.VT_BOOL;
			_propVar.boolVal = Convert.ToInt16(boolVal);
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with an array of boolean values.
		/// </summary>
		/// <param name="boolArray">The initial value of the new PropVariant</param>
		public PropVariant(bool[] boolArray) {
			HRESULT hr;
			_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			try {
				hr = InitPropVariantFromBooleanVector(boolArray, (uint)boolArray.Length, _ppv);
				if (hr.Failed) throw (Exception)hr;
			}
			catch {
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = IntPtr.Zero;
				throw;
			}
			MarshalPointerToValue();
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with an array of bytes.
		/// </summary>
		/// <param name="buffer">The initial value of the new PropVariant</param>
		public PropVariant(byte[] buffer) {
			HRESULT hr;
			_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			try {
				if ((hr = InitPropVariantFromBuffer(buffer, (uint)buffer.Length, _ppv)).Failed) throw (Exception)hr;
			}
			catch {
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = IntPtr.Zero;
				throw;
			}
			MarshalPointerToValue();
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with a <see cref="Guid"/> 
		/// </summary>
		/// <param name="guid">The initial value of the new PropVariant</param>
		/// <param name="format">The <see cref="GuidFormat"/> that defines how the PropVariant should save the Guid</param>
		public PropVariant(Guid guid, GuidFormat format = GuidFormat.Native) {

			IntPtr nativeGuid = IntPtr.Zero;

			try {
				HRESULT hr;
				_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
				switch (format) {
				case GuidFormat.Native:
					nativeGuid = Marshal.AllocCoTaskMem(16);
					Marshal.Copy(guid.ToByteArray(), 0, nativeGuid, 16);
					if ((hr = InitPropVariantFromCLSID(nativeGuid, _ppv)).Failed) throw (Exception)hr;
					break;
				case GuidFormat.String:
					nativeGuid = Marshal.AllocCoTaskMem(16);
					Marshal.Copy(guid.ToByteArray(), 0, nativeGuid, 16);
					if ((hr = InitPropVariantFromGUIDAsString(nativeGuid, _ppv)).Failed) throw (Exception)hr;
					break;
				case GuidFormat.Buffer:
					if ((hr = InitPropVariantFromBuffer(guid.ToByteArray(), 16, _ppv)).Failed) throw (Exception)hr;
					break;
				default:
					throw new ArgumentException();
				}
			}
			catch {
				if (_ppv != IntPtr.Zero) {
					Marshal.FreeCoTaskMem(_ppv);
					_ppv = IntPtr.Zero;
				}
				throw;
			}
			finally {
				if (nativeGuid != IntPtr.Zero)
					Marshal.FreeCoTaskMem(nativeGuid);
			}
			MarshalPointerToValue();
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with a double precision value.
		/// </summary>
		/// <param name="dblVal">The initial value of the new PropVariant</param>
		public PropVariant(double dblVal) {
			_propVar = new PROPVARIANT();
			_propVar.VarType = VARENUM.VT_R8;
			_propVar.dblVal = dblVal;
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with an array of double precision values.
		/// </summary>
		/// <param name="dblArray">The initial value of the new PropVariant</param>
		public PropVariant(double[] dblArray) {
			HRESULT hr;
			_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			try {
				if ((hr = InitPropVariantFromDoubleVector(dblArray, (uint)dblArray.Length, _ppv)).Failed) throw (Exception)hr;
			}
			catch {
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = IntPtr.Zero;
				throw;
			}
			MarshalPointerToValue();
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with a <see cref="DateTime"/> value.
		/// </summary>
		/// <param name="dtVal">The initial value of the new PropVariant</param>
		public PropVariant(DateTime dtVal) {
			HRESULT hr;
			_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			IntPtr pft = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(FILETIME)));
			Marshal.WriteInt64(pft, dtVal.ToFileTimeUtc());
			try {
				hr = InitPropVariantFromFileTime(pft, _ppv);
				if (hr.Failed)
					throw (Exception)hr;
			}
			catch {
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = IntPtr.Zero;
				throw;
			}
			finally {
				Marshal.FreeCoTaskMem(pft);
			}
			MarshalPointerToValue();
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with an array of <see cref="DateTime"/> values.
		/// </summary>
		/// <param name="dtArray">The initial value of the new PropVariant</param>
		public PropVariant(DateTime[] dtArray) {
			HRESULT hr;
			_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			int cElems = dtArray.Length;
			IntPtr pfv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(FILETIME)) * cElems);
			try {
				for (int i = 0; i < cElems; ++i)
					Marshal.WriteInt64(pfv, i * Marshal.SizeOf(typeof(FILETIME)), dtArray[i].ToFileTimeUtc());
				hr = InitPropVariantFromFileTimeVector(pfv, (uint)cElems, _ppv);
				if (hr.Failed) throw (Exception)hr;
			}
			catch {
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = IntPtr.Zero;
				throw;
			}
			finally {
				Marshal.FreeCoTaskMem(pfv);
			}
			MarshalPointerToValue();
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with a 16 bit integer value.
		/// </summary>
		/// <param name="iVal">The initial value of the new PropVariant</param>
		public PropVariant(short iVal) {
			_propVar = new PROPVARIANT();
			_propVar.VarType = VARENUM.VT_I2;
			_propVar.iVal = iVal;
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with an array of 16 bit integer values.
		/// </summary>
		/// <param name="array">The initial value of the new PropVariant</param>
		public PropVariant(short[] array) {
			HRESULT hr;
			_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			try {
				if ((hr = InitPropVariantFromInt16Vector(array, (uint)array.Length, _ppv)).Failed) throw (Exception)hr;
			}
			catch {
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = IntPtr.Zero;
				throw;
			}
			MarshalPointerToValue();
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with a 32 bit integer value.
		/// </summary>
		/// <param name="iVal">The initial value of the new PropVariant</param>
		public PropVariant(int iVal) {
			_propVar = new PROPVARIANT();
			_propVar.VarType = VARENUM.VT_I4;
			_propVar.intVal = iVal;
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with an array of 32 bit integer values.
		/// </summary>
		/// <param name="array">The initial value of the new PropVariant</param>
		public PropVariant(int[] array) {
			HRESULT hr;
			_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			try {
				if ((hr = InitPropVariantFromInt32Vector(array, (uint)array.Length, _ppv)).Failed) throw (Exception)hr;
			}
			catch {
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = IntPtr.Zero;
				throw;
			}
			MarshalPointerToValue();
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with a 64 bit integer value.
		/// </summary>
		/// <param name="lVal">The initial value of the new PropVariant</param>
		public PropVariant(long lVal) {
			_propVar = new PROPVARIANT();
			_propVar.VarType = VARENUM.VT_I8;
			_propVar.lVal = lVal;
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with an array of 64 bit integer values.
		/// </summary>
		/// <param name="array">The initial value of the new PropVariant</param>
		public PropVariant(long[] array) {
			HRESULT hr;
			_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			try {
				if ((hr = InitPropVariantFromInt64Vector(array, (uint)array.Length, _ppv)).Failed) throw (Exception)hr;
			}
			catch {
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = IntPtr.Zero;
				throw;
			}
			MarshalPointerToValue();
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with a string value.
		/// </summary>
		/// <param name="sValue">The initial value of the new PropVariant</param>
		public PropVariant(string sValue) {
			_propVar = new PROPVARIANT();
			_propVar.VarType = VARENUM.VT_LPWSTR;
			_propVar.pwszVal = Marshal.StringToCoTaskMemUni(sValue);
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with an array of string values.
		/// </summary>
		/// <param name="sValues">The initial value of the new PropVariant</param>
		public PropVariant(string[] sValues) {
			HRESULT hr;
			_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			try {
				if ((hr = InitPropVariantFromStringVector(sValues, (uint)sValues.Length, _ppv)).Failed) throw (Exception)hr;
			}
			catch {
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = IntPtr.Zero;
				throw;
			}
			MarshalPointerToValue();
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with a variant value.
		/// </summary>
		/// <param name="obj">The initial value of the new PropVariant</param>
		public PropVariant(object obj) {
			_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			try {
				HRESULT hr = VariantToPropVariant(ref obj, _ppv);
				if (hr.Failed)
					throw (Exception)hr;
			}
			catch {
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = IntPtr.Zero;
				throw;
			}
			MarshalPointerToValue();
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with a variant value, changing the type of variant in the process.
		/// </summary>
		/// <param name="obj">The initial value of the new PropVariant</param>
		/// <param name="vt">The type of the new PropVariant</param>
		public PropVariant(object obj, VarEnum vt) : this(obj) {
			if (_propVar.VarType != (VARENUM)vt) {
				IntPtr newVar = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
				try {
					PropVariantChangeType(newVar, MarshalledPointer, 0, (VARENUM)vt).ThrowIfFailed();
				}
				catch {
					Marshal.FreeCoTaskMem(newVar);
					throw;
				}
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = newVar;
				MarshalPointerToValue();
			}
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with an unsigned 16 bit integer value.
		/// </summary>
		/// <param name="uiVal">The initial value of the new PropVariant</param>
		public PropVariant(ushort uiVal) {
			_propVar = new PROPVARIANT();
			_propVar.VarType = VARENUM.VT_UI2;
			_propVar.uiVal = uiVal;
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with an array of unsigned 16 bit integer values.
		/// </summary>
		/// <param name="array">The initial value of the new PropVariant</param>
		public PropVariant(ushort[] array) {
			HRESULT hr;
			_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			try {
				if ((hr = InitPropVariantFromUInt16Vector(array, (uint)array.Length, _ppv)).Failed) throw (Exception)hr;
			}
			catch {
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = IntPtr.Zero;
				throw;
			}
			MarshalPointerToValue();
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with an unsigned 32 bit integer value.
		/// </summary>
		/// <param name="uiVal">The initial value of the new PropVariant</param>
		public PropVariant(uint uiVal) {
			_propVar = new PROPVARIANT();
			_propVar.VarType = VARENUM.VT_UI4;
			_propVar.uintVal = uiVal;
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with an array of unsigned 32 bit integer values.
		/// </summary>
		/// <param name="array">The initial value of the new PropVariant</param>
		public PropVariant(uint[] array) {
			HRESULT hr;
			_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			try {
				if ((hr = InitPropVariantFromUInt32Vector(array, (uint)array.Length, _ppv)).Failed) throw (Exception)hr;
			}
			catch {
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = IntPtr.Zero;
				throw;
			}
			MarshalPointerToValue();
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with an unsigned 64 bit integer value.
		/// </summary>
		/// <param name="ulVal">The initial value of the new PropVariant</param>
		public PropVariant(ulong ulVal) {
			_propVar = new PROPVARIANT();
			_propVar.VarType = VARENUM.VT_UI8;
			_propVar.ulVal = ulVal;
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with an array of unsigned 64 bit integer values.
		/// </summary>
		/// <param name="array">The initial value of the new PropVariant</param>
		public PropVariant(ulong[] array) {
			HRESULT hr;
			_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			try {
				if ((hr = InitPropVariantFromUInt64Vector(array, (uint)array.Length, _ppv)).Failed) throw (Exception)hr;
			}
			catch {
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = IntPtr.Zero;
				throw;
			}
			MarshalPointerToValue();
		}


		#endregion

		#region Specialised Construction

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> and initialises it with an element from another PropVariant array.
		/// </summary>
		/// <param name="pvIn">The original PropVariant array</param>
		/// <param name="iElem">The zero based element of the PropVariant array to initialise the new PropVariant with</param>
		/// <returns>The new PropVariant</returns>
		public static PropVariant FromVectorElement(PropVariant pvIn, int iElem) {
			PropVariant pv = new PropVariant();
			try {
				HRESULT hr = InitPropVariantFromPropVariantVectorElem(pvIn.MarshalledPointer, (uint)iElem, pv.MarshalledPointer);
				if (hr.Failed) throw (Exception)hr;
			}
			catch {
				pv.Dispose();
				throw;
			}
			pv.MarshalPointerToValue();
			return pv;
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> string array and initialises it with values from a semicolon delimited string
		/// </summary>
		/// <param name="psz">The initial semicolon delimited string</param>
		/// <returns>The new PropVariant</returns>
		public static PropVariant FromStringAsVector(string psz) {
			PropVariant pv = new PropVariant();
			try {
				HRESULT hr = InitPropVariantFromStringAsVector(psz, pv.MarshalledPointer);
				if (hr.Failed) throw (Exception)hr;
			}
			catch {
				pv.Dispose();
				throw;
			}
			pv.MarshalPointerToValue();
			return pv;
		}

		/// <summary>
		/// Creates a <see cref="PropVariant"/>  vector with a single element containing the value of the PropVariant parameter
		/// </summary>
		/// <param name="pvIn">The value to initialise the element of the array with</param>
		/// <returns>The new PropVariant array</returns>
		public static PropVariant AsVector(PropVariant pvIn) {
			PropVariant pvOut = new PropVariant();
			try {
				HRESULT hr = InitPropVariantVectorFromPropVariant(pvIn.MarshalledPointer, pvOut.MarshalledPointer);
				if (hr.Failed) throw (Exception)hr;
			}
			catch {
				pvOut.Dispose();
				throw;
			}
			pvOut.MarshalPointerToValue();
			return pvOut;
		}

		/// <summary>
		/// Creates a new <see cref="PropVariant"/> , and initialises it with a value passed in a second PropVariant, changing the type of the new value in the process
		/// </summary>
		/// <param name="pvIn">The original PropVariant value</param>
		/// <param name="newType">The type of the new PropVariant</param>
		/// <returns>The new PropVariant</returns>
		public static PropVariant AsType(PropVariant pvIn, VarEnum newType) {
			PropVariant pvDest = new PropVariant();
			try {
				HRESULT hr = PropVariantChangeType(pvDest.MarshalledPointer, pvIn.MarshalledPointer, 0, (VARENUM)newType);
				if (hr.Failed) throw (Exception)hr;
			}
			catch {
				pvDest.Dispose();
				throw;
			}
			pvDest.MarshalPointerToValue();
			return pvDest;
		}

		/// <summary>
		/// Duplicates a <see cref="PropVariant"/>. Note that this is a deep copy, duplicating any data contained in the PropVariant rather than simply copying the pointer.
		/// </summary>
		/// <param name="pvIn">The original PropVariant</param>
		/// <returns>The duplicated PropVariant</returns>
		public static PropVariant Copy(PropVariant pvIn) { return new PropVariant(pvIn); }

		#endregion

		#region Values

		/// <summary>
		/// Gets or sets the value of the <see cref="PropVariant"/> as a variant object.
		/// </summary>
		public object Value {
			get {
				object val;
				HRESULT hr = PropVariantToVariant(MarshalledPointer, out val);
				if (hr.Failed) throw (Exception)hr;
				return val;
			}
			set {
				Clear();
				HRESULT hr = VariantToPropVariant(ref value, _ppv);
				if (hr.Failed)
					throw (Exception)hr;
				MarshalPointerToValue();
			}
		}

		/// <summary>
		/// Sets the value of a <see cref="PropVariant"/> to that of a variant object, and then changes its type to that indicated by <paramref name="vt"/>.
		/// </summary>
		/// <param name="vt">The type required for the PropVariant</param>
		/// <param name="value">The new value of the PropVariant</param>
		public void SetValueAs(VarEnum vt, object value) {
			Clear();
			HRESULT hr = VariantToPropVariant(ref value, _ppv);
			if (hr.Failed) throw (Exception)hr;
			IntPtr newVar = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
			hr = PropVariantChangeType(newVar, _ppv, 0, (VARENUM)vt);
			if (hr.Failed) {
				Marshal.FreeCoTaskMem(newVar);
				throw (Exception)hr;
			}
			Marshal.FreeCoTaskMem(_ppv);
			_ppv = newVar;
			MarshalPointerToValue();
		}

		/// <summary>
		/// Coerces a <see cref="PropVariant"/> value to the specified type. and returns that value as a native instance of that type
		/// </summary>
		/// <typeparam name="T">The native type required</typeparam>
		/// <returns>The value of the PropVariant</returns>
		public T ToType<T>() {
			using (PropVariant propVar = new PropVariant()) {
				HRESULT hr;
				VARENUM vt = VARENUM.VT_EMPTY;
				switch (Type.GetTypeCode(typeof(T))) {
				case TypeCode.Boolean:
					vt = VARENUM.VT_BOOL;
					break;
				case TypeCode.Byte:
					vt = VARENUM.VT_UI1;
					break;
				case TypeCode.Char:
					vt = VARENUM.VT_I1;
					break;
				case TypeCode.Single:
					vt = VARENUM.VT_R4;
					break;
				case TypeCode.Double:
					vt = VARENUM.VT_R8;
					break;
				case TypeCode.DateTime:
					vt = VARENUM.VT_FILETIME;
					break;
				case TypeCode.Decimal:
					vt = VARENUM.VT_DECIMAL;
					break;
				case TypeCode.Int16:
					vt = VARENUM.VT_I2;
					break;
				case TypeCode.Int32:
					vt = VARENUM.VT_I4;
					break;
				case TypeCode.Int64:
					vt = VARENUM.VT_I8;
					break;
				case TypeCode.UInt16:
					vt = VARENUM.VT_UI2;
					break;
				case TypeCode.UInt32:
					vt = VARENUM.VT_UI4;
					break;
				case TypeCode.UInt64:
					vt = VARENUM.VT_UI8;
					break;
				case TypeCode.String: {
						IntPtr pStr = IntPtr.Zero;
						PropVariantToStringAlloc(MarshalledPointer, out pStr).ThrowIfFailed();
						object sResult = Marshal.PtrToStringAuto(pStr);
						Marshal.FreeCoTaskMem(pStr);
						return (T)sResult;
					}
				case TypeCode.Object:
					if (typeof(T) == typeof(Guid)) {
						try {
							if (IsVector && _propVar.VarType.HasFlag(VARENUM.VT_UI1)) {
								byte[] data = (byte[])Value;
								object result = new Guid(data);
								return (T)result;
							}
							if (IsString) {
								object result = new Guid((string)Value);
								return (T)result;
							}
							if (_propVar.VarType == VARENUM.VT_CLSID || _propVar.VarType == VARENUM.VT_RECORD)
								return (T)Value;
						}
						catch { }
					}
					throw new VariantTypeException($"Cannot convert PropVariant with {_propVar.VarType} To {typeof(T)}");
				default:
					throw new VariantTypeException($"Cannot convert PropVariant with {_propVar.VarType} To {typeof(T)}");
				}
				hr = PropVariantChangeType(propVar.MarshalledPointer, MarshalledPointer, 0, vt);
				if (hr.Failed) throw new VariantTypeException($"Cannot convert  PropVariant with {_propVar.VarType} To {typeof(T)}", hr.GetException());
				propVar.MarshalPointerToValue();
				return (T)propVar.Value;
			}
		}

		/// <summary>
		/// Reads an element of an array contained in a <see cref="PropVariant"/> and returns it in a new PropVariant.
		/// </summary>
		/// <param name="iElem">The required element of the current PropVariant</param>
		/// <returns>The item located at <paramref name="iElem"/></returns>
		public PropVariant this[int iElem] {
			get {
				int nElems = (int)PropVariantGetElementCount(MarshalledPointer);
				if (nElems == 0)
					throw new NullReferenceException("PropVariant is Empty");
				if (iElem >= nElems)
					throw new IndexOutOfRangeException();
				return FromVectorElement(this, iElem);
			}
		}

		/// <summary>
		/// The length of the array contained in the current <see cref="PropVariant"/> 
		/// </summary>
		public int Length {
			get {
				return IsVector ? (int)PropVariantGetElementCount(MarshalledPointer) : 0;
			}
		}

		/// <summary>
		/// Returns true if this PropVariant contains a Date and Time structure, else false
		/// </summary>
		public bool IsDateTime {
			get {
				return _propVar.VarType == VARENUM.VT_FILETIME;
			}
		}

		/// <summary>
		/// Returns true if this <see cref="PropVariant"/>  contains a floating point number, else false
		/// </summary>
		public bool IsFloat {
			get {
				return _propVar.VarType == VARENUM.VT_R4 || _propVar.VarType == VARENUM.VT_R8;
			}
		}

		/// <summary>
		/// Returns true if this <see cref="PropVariant"/>  contains an integer of any size, else false
		/// </summary>
		public bool IsInteger {
			get {
				return IsSignedInteger || IsUnsignedInteger;
			}
		}

		/// <summary>
		/// Returns true if this <see cref="PropVariant"/>  contains a numerical value, else false
		/// </summary>
		public bool IsNumber {
			get {
				return IsInteger || IsFloat;
			}
		}

		/// <summary>
		/// Returns true if this <see cref="PropVariant"/>  contains a signed integer of any size, else false
		/// </summary>
		public bool IsSignedInteger {
			get {
				return _propVar.VarType == VARENUM.VT_I1 || _propVar.VarType == VARENUM.VT_I2 || _propVar.VarType == VARENUM.VT_I4 || _propVar.VarType == VARENUM.VT_I8;
			}
		}

		/// <summary>
		/// Returns true if this <see cref="PropVariant"/> can be represented as a string, else false
		/// </summary>
		public bool IsString {
			get {
				IntPtr pStr = PropVariantToStringWithDefault(MarshalledPointer, null);
				return pStr != IntPtr.Zero;
			}
		}

		/// <summary>
		/// True if this <see cref="PropVariant"/> contains an unsigned integer of any size, else false.
		/// </summary>
		public bool IsUnsignedInteger {
			get {
				return _propVar.VarType == VARENUM.VT_UI1 || _propVar.VarType == VARENUM.VT_UI2 || _propVar.VarType == VARENUM.VT_UI4 || _propVar.VarType == VARENUM.VT_UI8;
			}
		}

		/// <summary>
		/// True if this <see cref="PropVariant"/> contains an array, else false
		/// </summary>
		public bool IsVector {
			get {
				return (_propVar.VarType & (VARENUM.VT_ARRAY | VARENUM.VT_VECTOR | VARENUM.VT_SAFEARRAY)) != 0;
			}
		}

		#endregion

		#region Helpers

		/// <summary>
		/// Clears all the data from this <see cref="PropVariant"/>, releasing any allocated memory if required.
		/// </summary>
		public void Clear() {
			HRESULT hr = PropVariantClear(MarshalledPointer);
			if (hr.Failed)
				throw (Exception)hr;
			MarshalPointerToValue();
		}

		internal IntPtr MarshalledPointer {
			get {
				if (_ppv == IntPtr.Zero) {
					_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
					Marshal.StructureToPtr(_propVar, _ppv, false);
				}
				else if (_dirty) {
					Marshal.StructureToPtr(_propVar, _ppv, true);
				}
				_dirty = false;
				return _ppv;
			}
		}

		internal void MarshalPointerToValue() {
			_propVar = Marshal.PtrToStructure<PROPVARIANT>(_ppv);
		}

		/// <summary>
		/// Overrides <see cref="object.Equals(object)"/>. Returns true if this <see cref="PropVariant"/>  is equal to the supplied object. This comparison is performed on the <see cref="ToString"/> representation of the objects.
		/// </summary>
		/// <param name="obj">The object to compare to the current PropVariant</param>
		/// <returns>True if the PropVariants are the same, else false</returns>
		public override bool Equals(object obj) {
			return ToString().Equals(obj?.ToString());
		}

		/// <summary>
		/// Overrides <see cref="object.GetHashCode"/>. Returns a HashCode calculated on the result of the <see cref="ToString"/> representation of this object
		/// </summary>
		/// <returns>The generated HashCode</returns>
		public override int GetHashCode() {
			return ToString().GetHashCode();
		}

		/// <summary>
		/// Returns a string representation of this <see cref="PropVariant"/>. The string is formatted as "VarEnum Type : (Native Type)Value"
		/// </summary>
		/// <returns>The string representation of this PropVariant</returns>
		public override string ToString() {
			object obj = Value;
			return (_propVar.VarType).ToString() + " : " + obj == null ? "null" : ("(" + obj.GetType().ToString() + ")"  + obj.ToString());
		}

		#endregion

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		/// <summary>
		/// Ensures unmanaged memory is freed when the PropVariant is finalised
		/// </summary>
		~PropVariant() {
			Dispose();
		}

		/// <summary>
		/// Frees unmanaged memory used by this <see cref="PropVariant"/> 
		/// </summary>
		public void Dispose() {
			if (!disposedValue) {
				PropVariantClear(MarshalledPointer);
				if (_ppv != IntPtr.Zero) {
					Marshal.FreeCoTaskMem(_ppv);
					_ppv = IntPtr.Zero;
				}
				disposedValue = true;
			}
			GC.SuppressFinalize(this);
		}

		#endregion

		#region IComparable<PropVariant>

		/// <summary>
		/// Overrides <see cref="IComparable{PropVariant}"/>. This comparer uses the <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb776517(v=vs.85).aspx">PropVariantCompareEx</a> function to
		/// compare the values of two <see cref="PropVariant"/> objects.
		/// </summary>
		/// <param name="other">The PropVariant to compare with this PropVariant</param>
		/// <returns>An integer indicating the relationship between the two PropVariants</returns>
		public int CompareTo(PropVariant other) {
			return DefaultComparer.Compare(this, other);
		}

		#endregion

		#region ISerializable

		/// <summary>
		/// Constructor used to deserialise a <see cref="PropVariant"/> that has previously been serialised. The serialisation process uses the 
		/// <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb776579(v=vs.85).aspx">StgSerializePropVariant</a> and 
		/// <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb776578(v=vs.85).aspx">StgDeserializePropVariant</a> functions to perform
		/// the serialisation and deserialisation of the value of the PropVariant data, but also includes a DataSize element to provide the length of the serialised data.
		/// </summary>
		/// <param name="info">The serialisation data</param>
		/// <param name="context">Defines the source of the serialisation data</param>
		public PropVariant(SerializationInfo info, StreamingContext context) {
			_ppv = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));

			uint size = info.GetUInt32("DataSize");
			byte[] serialized = (byte[])info.GetValue("Data", typeof(byte[]));
			IntPtr data = Marshal.AllocCoTaskMem((int)size);
			Marshal.Copy(serialized, 0, data, (int)size);
			try {
				HRESULT hr = StgDeserializePropVariant(data, size, _ppv);
				if (hr.Failed) throw hr.GetException();
			}
			catch {
				Marshal.FreeCoTaskMem(data);
				Marshal.FreeCoTaskMem(_ppv);
				_ppv = IntPtr.Zero;
				throw;
			}
			Marshal.FreeCoTaskMem(data);
			MarshalPointerToValue();
		}

		/// <summary>
		/// Serialises a <see cref="PropVariant"/>. The serialisation process uses the 
		/// <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb776579(v=vs.85).aspx">StgSerializePropVariant</a> and 
		/// <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb776578(v=vs.85).aspx">StgDeserializePropVariant</a> functions to perform
		/// the serialisation and deserialisation of the value of the PropVariant data, but also includes a DataSize element to provide the length of the serialised data.
		/// </summary>
		/// <param name="info">The serialisation data</param>
		/// <param name="context">Defines the destination of the serialisation data</param>
		public void GetObjectData(SerializationInfo info, StreamingContext context) {
			IntPtr data;
			uint size;
			byte[] serialized;

			HRESULT hr = StgSerializePropVariant(MarshalledPointer, out data, out size);
			if (hr.Failed) throw hr.GetException();
			info.AddValue("DataSize", size);
			serialized = new byte[size];
			Marshal.Copy(data, serialized, 0, (int)size);
			Marshal.FreeCoTaskMem(data);
			info.AddValue("Data", serialized);
		}

		#endregion
	}
}
