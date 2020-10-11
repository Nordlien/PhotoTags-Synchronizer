using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace WinProps {
	/// <summary>
	/// Defines <see cref="PropertyEnumeration.Value"/>s for a property. PropertyEnumeration Values are more diverse than normal enumerations, with each value able to represent a range of possible
	/// property values. Values also do not need to be numeric. This class encapsulates the <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb761483(v=vs.85).aspx">IPropertyEnumTypeList</a> COM interface.
	/// </summary>
	public class PropertyEnumeration : IReadOnlyList<PropertyEnumeration.Value>, IDisposable {
		IPropertyEnumTypeList _pList = null;

		internal PropertyEnumeration(IntPtr pUnk) {
			_pList = (IPropertyEnumTypeList)Marshal.GetUniqueObjectForIUnknown(pUnk);
		}

		/// <summary>
		/// Creates a <see cref="PropertyEnumeration"/> list of possible values for a property
		/// </summary>
		/// <param name="propDesc">A <see cref="PropertyDescription"/> describing the property the enumeration list is required for</param>
		public PropertyEnumeration(PropertyDescription propDesc) {
			IntPtr pUnk = propDesc.GetEnumTypeListPointer();
			_pList = (IPropertyEnumTypeList)Marshal.GetUniqueObjectForIUnknown(pUnk);
			Marshal.Release(pUnk);
		}

		/// <summary>
		/// Gets the number of possible enumeration <see cref="Value"/>s available for this property
		/// </summary>
		public int Count {
			get {
				uint count;
				_pList.GetCount(out count);
				return (int)count;
			}
		}

		/// <summary>
		/// Gets the enumeration <see cref="Value"/> at the specified index.
		/// </summary>
		/// <param name="index">The index of the required enumeration Value</param>
		/// <returns>The enumeration Value at the specified index</returns>
		public Value this[int index] {
			get {
				IntPtr pUnk;
				_pList.GetAt((uint)index, IID.IPropertyEnumType, out pUnk);
				Value value = new Value(pUnk);
				Marshal.Release(pUnk);
				return value;
			}
		}

		/// <summary>
		/// Gets an <see cref="IEnumerator{T}"/> that iterates the possible <see cref="Value"/>s in this enumeration.
		/// </summary>
		/// <returns>An <see cref="IEnumerator{T}"/> that iterates the possible <see cref="Value"/>s in this enumeration.</returns>
		public IEnumerator<Value> GetEnumerator() {
			int count = Count;
			for (int i = 0; i < count; ++i) {
				yield return this[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		/// <summary>
		/// Releases the internal IPropertyEnumTypeList COM pointer
		/// </summary>
		public void Dispose() {
			if (_pList != null)
				Marshal.ReleaseComObject(_pList);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Ensures the release of the IPropertyEnumTypeList COM pointer upon finalisation.
		/// </summary>
		~PropertyEnumeration() {
			Dispose();
		}

		/// <summary>
		/// The possible type of enumeration <see cref="Value"/>s
		/// </summary>
		public enum EnumType {
			/// <summary>
			/// The enumeration <see cref="Value"/>s are unique for each property value.
			/// </summary>
			DiscreteValue = 0,
			/// <summary>
			/// The enumeration <see cref="Value"/> is part of a range of values.
			/// </summary>
			RangedValue = 1,
			/// <summary>
			/// This value can be used as the default if none of the enumeration values match
			/// </summary>
			DefaultValue = 2,
			/// <summary>
			/// This value is the last value in the range
			/// </summary>
			EndRange = 3
		}

		/// <summary>
		/// Represents an Enumeration value. This class encapsulates the <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb761495(v=vs.85).aspx">IPropertyEnumType</a> COM interface. 
		/// Note that this class does not have any public constructors. You must get a particular value from a <see cref="PropertyEnumeration"/> object.
		/// </summary>
		public class Value : IDisposable {
			IPropertyEnumType _propEnumType = null;

			internal Value(IntPtr pUnk) {
				_propEnumType = (IPropertyEnumType)Marshal.GetUniqueObjectForIUnknown(pUnk);
			}

			/// <summary>
			/// Gets the <see cref="PropertyEnumeration.EnumType"/> of enumeration <see cref="Value"/> 
			/// </summary>
			public EnumType EnumType {
				get {
					PROPENUMTYPE type;
					_propEnumType.GetEnumType(out type);
					return (EnumType)type;
				}
			}

			/// <summary>
			/// Gets the discrete <see cref="Value"/> of this enumeration. The return is a <see cref="Tuple"/> containing a <see cref="PropVariant"/> with the value of the enumeration, and a string with the
			/// enumeration's display.
			/// </summary>
			public Tuple<PropVariant, string> DiscreteValue {
				get {
					PropVariant value = new PropVariant();
					string displayText;
					_propEnumType.GetValue(value.MarshalledPointer); value.MarshalPointerToValue();
					_propEnumType.GetDisplayText(out displayText);
					return new Tuple<PropVariant, string>(value, displayText);
				}
			}

			/// <summary>
			/// Gets the minimum and maximum property values of an enumeration value. This property returns a <see cref="Tuple"/> with two <see cref="PropVariant"/> objects defining the minimum and
			/// maximum property values, and a string representing the enumeration value.
			/// </summary>
			public Tuple<PropVariant, PropVariant, string> Range {
				get {
					PropVariant min = new PropVariant();
					PropVariant set = new PropVariant();
					string displayText;
					_propEnumType.GetRangeMinValue(min.MarshalledPointer); min.MarshalPointerToValue();
					_propEnumType.GetRangeSetValue(set.MarshalledPointer); set.MarshalPointerToValue();
					_propEnumType.GetDisplayText(out displayText);
					return new Tuple<PropVariant, PropVariant, string>(min, set, displayText);
				}
			}

			/// <summary>
			/// Gets the default text for this enumeration <see cref="Value"/> 
			/// </summary>
			public string DefaultValue {
				get {
					string displayText;
					_propEnumType.GetDisplayText(out displayText);
					return displayText;
				}
			}

			/// <summary>
			/// Gets the value of an <see cref="EnumType.EndRange"/> enumeration type.
			/// </summary>
			public PropVariant EndRange {
				get {
					PropVariant value = new PropVariant();
					_propEnumType.GetValue(value.MarshalledPointer); value.MarshalPointerToValue();
					return value;
				}
			}

			/// <summary>
			/// Releases the internal IPropertyEnumType COM pointer
			/// </summary>
			public void Dispose() {
				if (_propEnumType != null) {
					Marshal.FinalReleaseComObject(_propEnumType);
				}
				GC.SuppressFinalize(this);
			}

			/// <summary>
			/// Ensures the COM pointer is released when the object is finalised.
			/// </summary>
			~Value() {
				Dispose();
			}
		}
	}
}
