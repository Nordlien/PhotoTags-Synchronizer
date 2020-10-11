namespace WinProps {
	class IID {
		public static GUID IPropertyDescription { get; } = typeof(IPropertyDescription).GUID;
		public static GUID IProperyDescriptionList { get; } = typeof(IPropertyDescriptionList).GUID;
		public static GUID IPropertyEnumType { get; } = typeof(IPropertyEnumType).GUID;
		public static GUID IPropertyEnumTypeList { get; } = typeof(IPropertyEnumTypeList).GUID;
		public static GUID IPropertyStore { get; } = typeof(IPropertyStore).GUID;
		public static GUID IShellItem { get; } = typeof(IShellItem).GUID;
		public static GUID IShellItem2 { get; } = typeof(IShellItem2).GUID;
	}
}
