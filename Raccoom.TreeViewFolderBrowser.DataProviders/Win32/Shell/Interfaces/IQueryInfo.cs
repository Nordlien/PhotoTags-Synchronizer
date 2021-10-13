using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Raccoom.Win32
{
    [ComImport()]
    [Guid("00021500-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IQueryInfo
    {
        [PreserveSig]
        Int32 GetInfoTip(
            ShellAPI.QITIPF dwFlags,
            [MarshalAs(UnmanagedType.LPWStr)]
            out string ppwszTip);

        [PreserveSig]
        Int32 GetInfoFlags(
            out IntPtr pdwFlags);
    }
}
