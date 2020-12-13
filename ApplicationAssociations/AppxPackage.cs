using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAssociations
{
    //------------------------------------------------------------------
    //https://stackoverflow.com/questions/32122679/getting-icon-of-modern-windows-app-from-a-desktop-application/36559301#36559301
    //------------------------------------------------------------------
    /*
    foreach (var p in Process.GetProcesses())
    {
        var package = AppxPackage.FromProcess(p);
        if (package != null)
        {
            AppxPackage.ShowConsole(0, package);

        }
    }

    var package2 = AppxPackage.FromAppId("Microsoft.Windows.Photos_2020.20110.11001.0_x64__8wekyb3d8bbwe");
    AppxPackage.ShowConsole(0, package2);

------------------------------------------------------------------------------------
FullName               : Microsoft.Windows.Photos_2020.20110.11001.0_x64__8wekyb3d8bbwe
FamilyName             : Microsoft.Windows.Photos_8wekyb3d8bbwe
IsFramework            : False
ApplicationUserModelId : 
Path                   : C:\Program Files\WindowsApps\Microsoft.Windows.Photos_2020.20110.11001.0_x64__8wekyb3d8bbwe
Publisher              : CN=Microsoft Corporation, O=Microsoft Corporation, L=Redmond, S=Washington, C=US
PublisherId            : 8wekyb3d8bbwe
Logo                   : Assets\PhotosStoreLogo.png
Best Logo Path         : C:\Program Files\WindowsApps\Microsoft.Windows.Photos_2020.20110.11001.0_x64__8wekyb3d8bbwe\Assets\PhotosStoreLogo.scale-200.png
ProcessorArchitecture  : x64
Version                : 2020.20110.11001.0
PublisherDisplayName   : Microsoft Corporation
   Localized           : 
DisplayName            : ms-resource:AppStoreName
   Localized           : Microsoft Photos
Description            : 
   Localized           : 
--:
    App [0] Description       : ms-resource:AppDescription
      Localized           : Photos app
    App [0] DisplayName       : ms-resource:AppFriendlyName
      Localized           : Photos
    App [0] ShortName         : 
      Localized           : 
    App [0] EntryPoint        : AppStubCS.Windows.App
    App [0] Executable        : Microsoft.Photos.exe
    App [0] Id                : App
    App [0] Logo              : Assets\PhotosMedTile.png
    App [0] SmallLogo         : Assets\PhotosAppList.png
    App [0] StartPage         : 
    App [0] Square150x150Logo : Assets\PhotosMedTile.png
    App [0] Square30x30Logo   : Assets\PhotosAppList.png
    App [0] BackgroundColor   : #0078D4
    App [0] ForegroundText    : light
    App [0] WideLogo          : Assets\PhotosWideTile.png
    App [0] Wide310x310Logo   : 
    App [0] Square310x310Logo : Assets\PhotosLargeTile.png
    App [0] Square70x70Logo   : Assets\PhotosSmallTile.png
    App [0] MinWidth          : 
    App [0] Square71x71Logo   : 
    App [1] Description       : ms-resource:SecondaryTileDescriptionA
      Localized           : Video Editor
    App [1] DisplayName       : ms-resource:SecondaryTileDescriptionA
      Localized           : Video Editor
    App [1] ShortName         : 
      Localized           : 
    App [1] EntryPoint        : Windows.FullTrustApplication
    App [1] Executable        : VideoProjectsLauncher.exe
    App [1] Id                : SecondaryEntry
    App [1] Logo              : Assets\VideoEditor\VideoEditorMedTile.png
    App [1] SmallLogo         : Assets\VideoEditor\VideoEditorAppList.png
    App [1] StartPage         : 
    App [1] Square150x150Logo : Assets\VideoEditor\VideoEditorMedTile.png
    App [1] Square30x30Logo   : Assets\VideoEditor\VideoEditorAppList.png
    App [1] BackgroundColor   : #0078D4
    App [1] ForegroundText    : light
    App [1] WideLogo          : Assets\VideoEditor\VideoEditorWideTile.png
    App [1] Wide310x310Logo   : 
    App [1] Square310x310Logo : Assets\VideoEditor\VideoEditorLargeTile.png
    App [1] Square70x70Logo   : Assets\VideoEditor\VideoEditorSmallTile.png
    App [1] MinWidth          : 
    App [1] Square71x71Logo   : 
Deps                   :
 ------------------------------------------------------------------------------------
 FullName               : Microsoft.UI.Xaml.2.4_2.42007.9001.0_x64__8wekyb3d8bbwe
 FamilyName             : Microsoft.UI.Xaml.2.4_8wekyb3d8bbwe
 IsFramework            : True
 ApplicationUserModelId : 
 Path                   : C:\Program Files\WindowsApps\Microsoft.UI.Xaml.2.4_2.42007.9001.0_x64__8wekyb3d8bbwe
 Publisher              : CN=Microsoft Corporation, O=Microsoft Corporation, L=Redmond, S=Washington, C=US
 PublisherId            : 8wekyb3d8bbwe
 Logo                   : logo.png
 Best Logo Path         : 
 ProcessorArchitecture  : x64
 Version                : 2.42007.9001.0
 PublisherDisplayName   : Microsoft Platform Extensions
    Localized           : 
 DisplayName            : Microsoft.UI.Xaml.2.4
    Localized           : 
 Description            : Microsoft.UI.Xaml
    Localized           : 
 --:
 Deps                   :
 ------------------------------------------------------------------------------------
 ------------------------------------------------------------------------------------
 FullName               : Microsoft.NET.Native.Framework.2.2_2.2.29512.0_x64__8wekyb3d8bbwe
 FamilyName             : Microsoft.NET.Native.Framework.2.2_8wekyb3d8bbwe
 IsFramework            : True
 ApplicationUserModelId : 
 Path                   : C:\Program Files\WindowsApps\Microsoft.NET.Native.Framework.2.2_2.2.29512.0_x64__8wekyb3d8bbwe
 Publisher              : CN=Microsoft Corporation, O=Microsoft Corporation, L=Redmond, S=Washington, C=US
 PublisherId            : 8wekyb3d8bbwe
 Logo                   : logo.png
 Best Logo Path         : 
 ProcessorArchitecture  : x64
 Version                : 2.2.29512.0
 PublisherDisplayName   : Microsoft Corporation
    Localized           : 
 DisplayName            : Microsoft .Net Native Framework Package 2.2
    Localized           : 
 Description            : Microsoft .Net Native Framework Package 2.2
    Localized           : 
 --:
 Deps                   :
 ------------------------------------------------------------------------------------
 ------------------------------------------------------------------------------------
 FullName               : Microsoft.NET.Native.Runtime.2.2_2.2.28604.0_x64__8wekyb3d8bbwe
 FamilyName             : Microsoft.NET.Native.Runtime.2.2_8wekyb3d8bbwe
 IsFramework            : True
 ApplicationUserModelId : 
 Path                   : C:\Program Files\WindowsApps\Microsoft.NET.Native.Runtime.2.2_2.2.28604.0_x64__8wekyb3d8bbwe
 Publisher              : CN=Microsoft Corporation, O=Microsoft Corporation, L=Redmond, S=Washington, C=US
 PublisherId            : 8wekyb3d8bbwe
 Logo                   : logo.png
 Best Logo Path         : 
 ProcessorArchitecture  : x64
 Version                : 2.2.28604.0
 PublisherDisplayName   : Microsoft Corporation
    Localized           : 
 DisplayName            : Microsoft .Net Native Runtime Package 2.2
    Localized           : 
 Description            : Microsoft .Net Native Runtime Package 2.2
    Localized           : 
 --:
 Deps                   :
 ------------------------------------------------------------------------------------
 ------------------------------------------------------------------------------------
 FullName               : Microsoft.VCLibs.140.00_14.0.29231.0_x64__8wekyb3d8bbwe
 FamilyName             : Microsoft.VCLibs.140.00_8wekyb3d8bbwe
 IsFramework            : True
 ApplicationUserModelId : 
 Path                   : C:\Program Files\WindowsApps\Microsoft.VCLibs.140.00_14.0.29231.0_x64__8wekyb3d8bbwe
 Publisher              : CN=Microsoft Corporation, O=Microsoft Corporation, L=Redmond, S=Washington, C=US
 PublisherId            : 8wekyb3d8bbwe
 Logo                   : logo.png
 Best Logo Path         : 
 ProcessorArchitecture  : x64
 Version                : 14.0.29231.0
 PublisherDisplayName   : Microsoft Platform Extensions
    Localized           : 
 DisplayName            : Microsoft Visual C++ 2015 UWP Runtime Package
    Localized           : 
 Description            : Microsoft Visual C++ 2015 UWP Runtime support for native applications
    Localized           : 
 --:
 Deps                   :
 ------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
    */
    public sealed class AppxPackage
    {
        private List<AppxApp> _apps = new List<AppxApp>();
        private IAppxManifestProperties _properties;

        #region ShowConsole for debug
        public static void ShowConsole(int indent, AppxPackage package)
        {
            string sindent = new string(' ', indent);
            Console.WriteLine(sindent + "------------------------------------------------------------------------------------");
            Console.WriteLine(sindent + "FullName               : " + package.FullName);
            Console.WriteLine(sindent + "FamilyName             : " + package.FamilyName);
            Console.WriteLine(sindent + "IsFramework            : " + package.IsFramework);
            Console.WriteLine(sindent + "ApplicationUserModelId : " + package.ApplicationUserModelId);
            Console.WriteLine(sindent + "Path                   : " + package.Path);
            Console.WriteLine(sindent + "Publisher              : " + package.Publisher);
            Console.WriteLine(sindent + "PublisherId            : " + package.PublisherId);
            Console.WriteLine(sindent + "Logo                   : " + package.Logo);
            Console.WriteLine(sindent + "Best Logo Path         : " + package.FindHighestScaleQualifiedImagePath(package.Logo));
            Console.WriteLine(sindent + "ProcessorArchitecture  : " + package.ProcessorArchitecture);
            Console.WriteLine(sindent + "Version                : " + package.Version);
            Console.WriteLine(sindent + "PublisherDisplayName   : " + package.PublisherDisplayName);
            Console.WriteLine(sindent + "   Localized           : " + package.LoadResourceString(package.PublisherDisplayName));
            Console.WriteLine(sindent + "DisplayName            : " + package.DisplayName);
            Console.WriteLine(sindent + "   Localized           : " + package.LoadResourceString(package.DisplayName));
            Console.WriteLine(sindent + "Description            : " + package.Description);
            Console.WriteLine(sindent + "   Localized           : " + package.LoadResourceString(package.Description));

            Console.WriteLine(sindent + "--:");
            int i = 0;
            foreach (var app in package.Apps)
            {
                Console.WriteLine(sindent + "    App [" + i + "] Description       : " + app.Description);
                Console.WriteLine(sindent + "      Localized           : " + package.LoadResourceString(app.Description));
                Console.WriteLine(sindent + "    App [" + i + "] DisplayName       : " + app.DisplayName);
                Console.WriteLine(sindent + "      Localized           : " + package.LoadResourceString(app.DisplayName));
                Console.WriteLine(sindent + "    App [" + i + "] ShortName         : " + app.ShortName);
                Console.WriteLine(sindent + "      Localized           : " + package.LoadResourceString(app.ShortName));
                Console.WriteLine(sindent + "    App [" + i + "] EntryPoint        : " + app.EntryPoint);
                Console.WriteLine(sindent + "    App [" + i + "] Executable        : " + app.Executable);
                Console.WriteLine(sindent + "    App [" + i + "] Id                : " + app.Id);
                Console.WriteLine(sindent + "    App [" + i + "] Logo              : " + app.Logo);
                Console.WriteLine(sindent + "    App [" + i + "] SmallLogo         : " + app.SmallLogo);
                Console.WriteLine(sindent + "    App [" + i + "] StartPage         : " + app.StartPage);
                Console.WriteLine(sindent + "    App [" + i + "] Square150x150Logo : " + app.Square150x150Logo);
                Console.WriteLine(sindent + "    App [" + i + "] Square30x30Logo   : " + app.Square30x30Logo);
                Console.WriteLine(sindent + "    App [" + i + "] BackgroundColor   : " + app.BackgroundColor);
                Console.WriteLine(sindent + "    App [" + i + "] ForegroundText    : " + app.ForegroundText);
                Console.WriteLine(sindent + "    App [" + i + "] WideLogo          : " + app.WideLogo);
                Console.WriteLine(sindent + "    App [" + i + "] Wide310x310Logo   : " + app.Wide310x310Logo);
                Console.WriteLine(sindent + "    App [" + i + "] Square310x310Logo : " + app.Square310x310Logo);
                Console.WriteLine(sindent + "    App [" + i + "] Square70x70Logo   : " + app.Square70x70Logo);
                Console.WriteLine(sindent + "    App [" + i + "] MinWidth          : " + app.MinWidth);
                Console.WriteLine(sindent + "    App [" + i + "] Square71x71Logo   : " + app.GetStringValue("Square71x71Logzo"));
                i++;
            }

            Console.WriteLine(sindent + "Deps                   :");
            foreach (var dep in package.DependencyGraph)
            {
                ShowConsole(indent + 1, dep);
            }
            Console.WriteLine(sindent + "------------------------------------------------------------------------------------");
        }
        #endregion 


        public static SortedList<string, ApplicationData> GetApps()
        {
            SortedList<string, ApplicationData> applicationDatas = new SortedList<string, ApplicationData>();

            foreach (var p in Process.GetProcesses())
            {
                var package = AppxPackage.FromProcess(p);
                if (package != null)
                {

                    //AppxPackage.ShowConsole(0, package);
                    
                    ApplicationData applicationData = new ApplicationData();
                    applicationData.ProgId = package.FamilyName;
                    applicationData.Command = "";
                    applicationData.FriendlyAppName = package.LoadResourceString(package.DisplayName) == null ? package.DisplayName: package.LoadResourceString(package.DisplayName);
                    applicationData.AppIconReference = package.FindHighestScaleQualifiedImagePath(package.Logo);

                    //AssocQuery assocQuery = new AssocQuery(progId);
                    //applicationData.ApplicationId = assocQuery.AppId;
                    //applicationData.Command = assocQuery.Command;
                    //applicationData.FriendlyAppName = assocQuery.FriendlyAppName;
                    //applicationData.AppIconReference = assocQuery.AppIconReference;

                    //AddApplicationData(extention, applicationData);
                    var verbs = ProtocolAssociationsRegistry.GetVerbsByProgId(applicationData.ProgId);

                    //if (verbs.Length > 0)
                    {
                        foreach (string verb in verbs)
                        {
                            string command = "";
                            if (!string.IsNullOrEmpty(applicationData.Command))
                            {
                                command = AssocQuery.GetCommandProgId(applicationData.ProgId, verb);
                            }
                            applicationData.AddVerb(verb, command);
                        }

                        if (!applicationDatas.Keys.Contains(applicationData.FriendlyAppName)) applicationDatas.Add(applicationData.FriendlyAppName, applicationData);
                    }
                }
            }
            return applicationDatas;

        }
        private AppxPackage()
        {
        }

        public string FullName { get; private set; }
        public string Path { get; private set; }
        public string Publisher { get; private set; }
        public string PublisherId { get; private set; }
        public string ResourceId { get; private set; }
        public string FamilyName { get; private set; }
        public string ApplicationUserModelId { get; private set; }
        public string Logo { get; private set; }
        public string PublisherDisplayName { get; private set; }
        public string Description { get; private set; }
        public string DisplayName { get; private set; }
        public bool IsFramework { get; private set; }
        public Version Version { get; private set; }
        public AppxPackageArchitecture ProcessorArchitecture { get; private set; }

        public IReadOnlyList<AppxApp> Apps { get { return _apps; } }

        public IEnumerable<AppxPackage> DependencyGraph { get { return QueryPackageInfo(FullName, PackageConstants.PACKAGE_FILTER_ALL_LOADED).Where(p => p.FullName != FullName); } }


        public string FindHighestScaleQualifiedImagePath(string resourceName)
        {
            if (resourceName == null)
                throw new ArgumentNullException("resourceName");

            const string scaleToken = ".scale-";
            var sizes = new List<int>();
            string name = System.IO.Path.GetFileNameWithoutExtension(resourceName);
            string ext = System.IO.Path.GetExtension(resourceName);
            foreach (var file in Directory.EnumerateFiles(System.IO.Path.Combine(Path, System.IO.Path.GetDirectoryName(resourceName)), name + scaleToken + "*" + ext))
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                int pos = fileName.IndexOf(scaleToken) + scaleToken.Length;
                string sizeText = fileName.Substring(pos);
                int size;
                if (int.TryParse(sizeText, out size))
                {
                    sizes.Add(size);
                }
            }
            if (sizes.Count == 0)
                return null;

            sizes.Sort();
            return System.IO.Path.Combine(Path, System.IO.Path.GetDirectoryName(resourceName), name + scaleToken + sizes.Last() + ext);
        }

        public override string ToString()
        {
            return FullName;
        }

        public static AppxPackage FromWindow(IntPtr handle)
        {
            int processId;
            GetWindowThreadProcessId(handle, out processId);
            if (processId == 0) return null;
            return FromProcess(processId);
        }

        public static AppxPackage FromProcess(Process process)
        {
            if (process == null) process = Process.GetCurrentProcess();
            try
            {
                return FromProcess(process.Handle);
            }
            catch
            {
                // probably access denied on .Handle
                return null;
            }
        }

        public static AppxPackage FromProcess(int processId)
        {
            const int QueryLimitedInformation = 0x1000;
            IntPtr hProcess = OpenProcess(QueryLimitedInformation, false, processId);
            try
            {
                return FromProcess(hProcess);
            }
            finally
            {
                if (hProcess != IntPtr.Zero)
                {
                    CloseHandle(hProcess);
                }
            }
        }

        public static AppxPackage FromProcess(IntPtr hProcess)
        {
            if (hProcess == IntPtr.Zero) return null;

            // hprocess must have been opened with QueryLimitedInformation
            int len = 0;
            GetPackageFullName(hProcess, ref len, null);
            if (len == 0) return null;

            var sb = new StringBuilder(len);
            string fullName = GetPackageFullName(hProcess, ref len, sb) == 0 ? sb.ToString() : null;
            if (string.IsNullOrEmpty(fullName)) return null; // not an AppX

            //-----fullName example : "Microsoft.ZuneVideo_10.20092.14511.0_x64__8wekyb3d8bbwe"
            var package = QueryPackageInfo(fullName, PackageConstants.PACKAGE_FILTER_HEAD).First();

            len = 0;
            GetApplicationUserModelId(hProcess, ref len, null);
            sb = new StringBuilder(len);
            package.ApplicationUserModelId = GetApplicationUserModelId(hProcess, ref len, sb) == 0 ? sb.ToString() : null;
            return package;
        }

        public static AppxPackage FromAppId(string appId)
        {
            var package = QueryPackageInfo(appId, PackageConstants.PACKAGE_FILTER_HEAD);

            //len = 0;
            //GetApplicationUserModelId(hProcess, ref len, null);
            //sb = new StringBuilder(len);
            //package.ApplicationUserModelId = appId;
            return package.First();
        }

        public string GetPropertyStringValue(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            return GetStringValue(_properties, name);
        }

        public bool GetPropertyBoolValue(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            return GetBoolValue(_properties, name);
        }

        public string LoadResourceString(string resource)
        {
            return LoadResourceString(FullName, resource);
        }

        private static IEnumerable<AppxPackage> QueryPackageInfo(string fullName, PackageConstants flags)
        {
            IntPtr infoRef;
            OpenPackageInfoByFullName(fullName, 0, out infoRef);
            if (infoRef != IntPtr.Zero)
            {
                IntPtr infoBuffer = IntPtr.Zero;
                try
                {
                    int len = 0;
                    int count;
                    GetPackageInfo(infoRef, flags, ref len, IntPtr.Zero, out count);
                    if (len > 0)
                    {
                        var factory = (IAppxFactory)new AppxFactory();
                        infoBuffer = Marshal.AllocHGlobal(len);
                        int res = GetPackageInfo(infoRef, flags, ref len, infoBuffer, out count);
                        for (int i = 0; i < count; i++)
                        {
                            var info = (PACKAGE_INFO)Marshal.PtrToStructure(infoBuffer + i * Marshal.SizeOf(typeof(PACKAGE_INFO)), typeof(PACKAGE_INFO));
                            var package = new AppxPackage();
                            package.FamilyName = Marshal.PtrToStringUni(info.packageFamilyName);
                            package.FullName = Marshal.PtrToStringUni(info.packageFullName);
                            package.Path = Marshal.PtrToStringUni(info.path);
                            package.Publisher = Marshal.PtrToStringUni(info.packageId.publisher);
                            package.PublisherId = Marshal.PtrToStringUni(info.packageId.publisherId);
                            package.ResourceId = Marshal.PtrToStringUni(info.packageId.resourceId);
                            package.ProcessorArchitecture = info.packageId.processorArchitecture;
                            package.Version = new Version(info.packageId.VersionMajor, info.packageId.VersionMinor, info.packageId.VersionBuild, info.packageId.VersionRevision);

                            // read manifest
                            string manifestPath = System.IO.Path.Combine(package.Path, "AppXManifest.xml");
                            const int STGM_SHARE_DENY_NONE = 0x40;
                            IStream strm;
                            SHCreateStreamOnFileEx(manifestPath, STGM_SHARE_DENY_NONE, 0, false, IntPtr.Zero, out strm);
                            if (strm != null)
                            {
                                var reader = factory.CreateManifestReader(strm);
                                package._properties = reader.GetProperties();
                                package.Description = package.GetPropertyStringValue("Description");
                                package.DisplayName = package.GetPropertyStringValue("DisplayName");
                                package.Logo = package.GetPropertyStringValue("Logo");
                                package.PublisherDisplayName = package.GetPropertyStringValue("PublisherDisplayName");
                                package.IsFramework = package.GetPropertyBoolValue("Framework");

                                var apps = reader.GetApplications();
                                while (apps.GetHasCurrent())
                                {
                                    var app = apps.GetCurrent();
                                    var appx = new AppxApp(app);
                                    appx.Description = GetStringValue(app, "Description");
                                    appx.DisplayName = GetStringValue(app, "DisplayName");
                                    appx.EntryPoint = GetStringValue(app, "EntryPoint");
                                    appx.Executable = GetStringValue(app, "Executable");
                                    appx.Id = GetStringValue(app, "Id");
                                    appx.Logo = GetStringValue(app, "Logo");
                                    appx.SmallLogo = GetStringValue(app, "SmallLogo");
                                    appx.StartPage = GetStringValue(app, "StartPage");
                                    appx.Square150x150Logo = GetStringValue(app, "Square150x150Logo");
                                    appx.Square30x30Logo = GetStringValue(app, "Square30x30Logo");
                                    appx.BackgroundColor = GetStringValue(app, "BackgroundColor");
                                    appx.ForegroundText = GetStringValue(app, "ForegroundText");
                                    appx.WideLogo = GetStringValue(app, "WideLogo");
                                    appx.Wide310x310Logo = GetStringValue(app, "Wide310x310Logo");
                                    appx.ShortName = GetStringValue(app, "ShortName");
                                    appx.Square310x310Logo = GetStringValue(app, "Square310x310Logo");
                                    appx.Square70x70Logo = GetStringValue(app, "Square70x70Logo");
                                    appx.MinWidth = GetStringValue(app, "MinWidth");
                                    package._apps.Add(appx);
                                    apps.MoveNext();
                                }
                                Marshal.ReleaseComObject(strm);
                            }
                            yield return package;
                        }
                        Marshal.ReleaseComObject(factory);
                    }
                }
                finally
                {
                    if (infoBuffer != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(infoBuffer);
                    }
                    ClosePackageInfo(infoRef);
                }
            }
        }

        public static string LoadResourceString(string packageFullName, string resource)
        {
            if (packageFullName == null) throw new ArgumentNullException("packageFullName");
            if (string.IsNullOrWhiteSpace(resource)) return null;

            const string resourceScheme = "ms-resource:";
            if (!resource.StartsWith(resourceScheme)) return null;

            string part = resource.Substring(resourceScheme.Length);
            string url;

            if (part.StartsWith("/")) url = resourceScheme + "//" + part;            
            else url = resourceScheme + "///resources/" + part;
            
            string source = string.Format("@{{{0}? {1}}}", packageFullName, url);
            var sb = new StringBuilder(1024);
            int i = SHLoadIndirectString(source, sb, sb.Capacity, IntPtr.Zero);
            if (i != 0) return null;

            return sb.ToString();
        }

        private static string GetStringValue(IAppxManifestProperties props, string name)
        {
            if (props == null) return null;

            string value;
            props.GetStringValue(name, out value);
            return value;
        }

        private static bool GetBoolValue(IAppxManifestProperties props, string name)
        {
            bool value;
            props.GetBoolValue(name, out value);
            return value;
        }

        internal static string GetStringValue(IAppxManifestApplication app, string name)
        {
            string value;
            app.GetStringValue(name, out value);
            return value;
        }

        [Guid("5842a140-ff9f-4166-8f5c-62f5b7b0c781"), ComImport]
        private class AppxFactory
        {
        }

        [Guid("BEB94909-E451-438B-B5A7-D79E767B75D8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IAppxFactory
        {
            void _VtblGap0_2(); // skip 2 methods
            IAppxManifestReader CreateManifestReader(IStream inputStream);
        }

        [Guid("4E1BD148-55A0-4480-A3D1-15544710637C"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IAppxManifestReader
        {
            void _VtblGap0_1(); // skip 1 method
            IAppxManifestProperties GetProperties();
            void _VtblGap1_5(); // skip 5 methods
            IAppxManifestApplicationsEnumerator GetApplications();
        }

        [Guid("9EB8A55A-F04B-4D0D-808D-686185D4847A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IAppxManifestApplicationsEnumerator
        {
            IAppxManifestApplication GetCurrent();
            bool GetHasCurrent();
            bool MoveNext();
        }

        [Guid("5DA89BF4-3773-46BE-B650-7E744863B7E8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IAppxManifestApplication
        {
            [PreserveSig]
            int GetStringValue([MarshalAs(UnmanagedType.LPWStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] out string vaue);
        }

        [Guid("03FAF64D-F26F-4B2C-AAF7-8FE7789B8BCA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IAppxManifestProperties
        {
            [PreserveSig]
            int GetBoolValue([MarshalAs(UnmanagedType.LPWStr)] string name, out bool value);
            [PreserveSig]
            int GetStringValue([MarshalAs(UnmanagedType.LPWStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] out string vaue);
        }

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern int SHLoadIndirectString(string pszSource, StringBuilder pszOutBuf, int cchOutBuf, IntPtr ppvReserved);

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern int SHCreateStreamOnFileEx(string fileName, int grfMode, int attributes, bool create, IntPtr reserved, out IStream stream);

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int OpenPackageInfoByFullName(string packageFullName, int reserved, out IntPtr packageInfoReference);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetPackageInfo(IntPtr packageInfoReference, PackageConstants flags, ref int bufferLength, IntPtr buffer, out int count);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int ClosePackageInfo(IntPtr packageInfoReference);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetPackageFullName(IntPtr hProcess, ref int packageFullNameLength, StringBuilder packageFullName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetApplicationUserModelId(IntPtr hProcess, ref int applicationUserModelIdLength, StringBuilder applicationUserModelId);

        #region enum PackageConstants
        [Flags]
        private enum PackageConstants
        {
            PACKAGE_FILTER_ALL_LOADED = 0x00000000,
            PACKAGE_PROPERTY_FRAMEWORK = 0x00000001,
            PACKAGE_PROPERTY_RESOURCE = 0x00000002,
            PACKAGE_PROPERTY_BUNDLE = 0x00000004,
            PACKAGE_FILTER_HEAD = 0x00000010,
            PACKAGE_FILTER_DIRECT = 0x00000020,
            PACKAGE_FILTER_RESOURCE = 0x00000040,
            PACKAGE_FILTER_BUNDLE = 0x00000080,
            PACKAGE_INFORMATION_BASIC = 0x00000000,
            PACKAGE_INFORMATION_FULL = 0x00000100,
            PACKAGE_PROPERTY_DEVELOPMENT_MODE = 0x00010000,
        }
        #endregion

        #region struct PACKAGE_INFO
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct PACKAGE_INFO
        {
            public int reserved;
            public int flags;
            public IntPtr path;
            public IntPtr packageFullName;
            public IntPtr packageFamilyName;
            public PACKAGE_ID packageId;
        }
        #endregion

        #region struct PACKAGE_ID
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct PACKAGE_ID
        {
            public int reserved;
            public AppxPackageArchitecture processorArchitecture;
            public ushort VersionRevision;
            public ushort VersionBuild;
            public ushort VersionMinor;
            public ushort VersionMajor;
            public IntPtr name;
            public IntPtr publisher;
            public IntPtr resourceId;
            public IntPtr publisherId;
        }
        #endregion 
    }

    public sealed class AppxApp
    {
        private AppxPackage.IAppxManifestApplication _app;

        internal AppxApp(AppxPackage.IAppxManifestApplication app)
        {
            _app = app;
        }

        public string GetStringValue(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            return AppxPackage.GetStringValue(_app, name);
        }

        // we code well-known but there are others (like Square71x71Logo, Square44x44Logo, whatever ...)
        // https://msdn.microsoft.com/en-us/library/windows/desktop/hh446703.aspx
        public string Description { get; internal set; }
        public string DisplayName { get; internal set; }
        public string EntryPoint { get; internal set; }
        public string Executable { get; internal set; }
        public string Id { get; internal set; }
        public string Logo { get; internal set; }
        public string SmallLogo { get; internal set; }
        public string StartPage { get; internal set; }
        public string Square150x150Logo { get; internal set; }
        public string Square30x30Logo { get; internal set; }
        public string BackgroundColor { get; internal set; }
        public string ForegroundText { get; internal set; }
        public string WideLogo { get; internal set; }
        public string Wide310x310Logo { get; internal set; }
        public string ShortName { get; internal set; }
        public string Square310x310Logo { get; internal set; }
        public string Square70x70Logo { get; internal set; }
        public string MinWidth { get; internal set; }
    }

    public enum AppxPackageArchitecture
    {
        x86 = 0,
        Arm = 5,
        x64 = 9,
        Neutral = 11,
        Arm64 = 12
    }
}
