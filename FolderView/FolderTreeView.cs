/*

	Windows Forms Folder Tree View control for .Net
	Version 1.1, posted 20-Oct-2002
	(c)Copyright 2002 Furty (furty74@yahoo.com). All rights reserved.
	Free for any use, so long as copyright is acknowledged.
	
	This is an all-new version of the FolderTreeView control I posted here at CP some weeks ago.
	The control now starts in the Desktop namespace, and a new DrillToFolder method has been added
	so the startup folder can be specified. Please note that this control is not intended to have 
	all of the functionality of the actual Windows Explorer TreeView - it is a light-weight control 
	designed for use in projects where you want to supply a treeview for folder navigation, without supporting
	windows shell extensions. If you are looking for a control that supports shell extensions
	you should be looking at the excellent �xplorerTreeControl submitted by Carlos H Perez at the CP website.
	
	The 3 classes that make up the control have been merged into the one file here for ease of
	integration into your own projects. The reason for separate classes is that this code has been
	extracted from a much larger project I'm working on, and the code that is not required for this
	control has been removed.	
	
	Acknowledgments:
	Substantial portions of the ShellOperations and ExtractIcons classes were borrowed from the 
	FTPCom article written by Jerome Lacaille, available on the www.codeproject.com website.
	
	If you improve this control, please email me the updated source, and if you have any 
	comments or suggestions, please post your thoughts in the feedback section on the 
	codeproject.com page for this control.
	
	Version 1.11 Changes:
	Updated the GetDesktopIcon method so that the small (16x16) desktop icon is returned instead of the large version
	Added code to give the Desktop root node a FolderItem object tag equal to the DesktopDirectory SpecialFolder,
	this ensures that the desktop node returns a file path.
 
 */
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Threading;

namespace Furty.Windows.Forms
{
	#region FolderTreeView Class

	public class FolderTreeView : System.Windows.Forms.TreeView
	{
		private System.Windows.Forms.ImageList folderTreeViewImageList;
		private System.Globalization.CultureInfo cultureInfo = System.Globalization.CultureInfo.CurrentCulture;

		#region Constructors

		public FolderTreeView()
		{
			this.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeViewBeforeExpand);
		}

		public void InitFolderTreeView()
		{
            InitImageList();
            ShellOperations.PopulateTree(this, base.ImageList);
            if (this.Nodes.Count > 0)
			{
				this.Nodes[0].Expand();
			}
		}

		private void InitImageList()
		{
            // setup the image list to hold the folder icons
            folderTreeViewImageList = new System.Windows.Forms.ImageList();
			folderTreeViewImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			folderTreeViewImageList.ImageSize = new System.Drawing.Size(16, 16);
			folderTreeViewImageList.TransparentColor = System.Drawing.Color.Transparent;

			// add the Desktop icon to the image list
			try
			{
				folderTreeViewImageList.Images.Add(ExtractIcons.GetDesktopIcon());
			}
			catch
			{
				// Create a blank icon if the desktop icon fails for some reason
				Bitmap bmp = new Bitmap(16,16);
				Image img = (Image)bmp;
				folderTreeViewImageList.Images.Add((Image)img.Clone());
				bmp.Dispose();
			}
			this.ImageList = folderTreeViewImageList;
		}

		#endregion

		#region Event Handlers

		private void TreeViewBeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			this.BeginUpdate();
			ShellOperations.ExpandBranch(e.Node, this.ImageList);
			this.EndUpdate();
		}

		#endregion

		#region Furty.Windows.Forms.FolderTreeView Properties & Methods

		public string GetSelectedNodePath()
		{
			return ShellOperations.GetFileDirectory(SelectedNode);
		}

		public bool DrillToFolder(string folderPath)
		{
			bool folderFound = false;
			if(Directory.Exists(folderPath)) // don't bother drilling unless the directory exists
			{
				this.BeginUpdate();
				// if there's a trailing \ on the folderPath, remove it unless it's a drive letter
				if(folderPath.Length > 3 && folderPath.LastIndexOf("\\") == folderPath.Length -1)
					folderPath = folderPath.Substring(0, folderPath.Length -1);
				//Start drilling the tree
				DrillTree(this.Nodes[0].Nodes, folderPath.ToUpper(cultureInfo), ref folderFound);
				this.EndUpdate();
			}
			if(!folderFound)
				this.SelectedNode = this.Nodes[0];
			return folderFound;
		}

		private void DrillTree(TreeNodeCollection tnc, string path, ref bool folderFound)
		{
			foreach (TreeNode tn in tnc)
			{
				if (!folderFound)
				{
					this.SelectedNode = tn;
					string tnPath = ShellOperations.GetFileDirectory(tn).ToUpper(cultureInfo);
					if (path == tnPath && !folderFound)
					{
						this.SelectedNode = tn;
						tn.EnsureVisible();
						folderFound = true;
						break;
					}
					else if (path.IndexOf(tnPath) > -1 && !folderFound)
					{
						tn.Expand();
						DrillTree(tn.Nodes, path, ref folderFound);
					}
				}
			}
		}

		public TreeNode FindFolder(string folderPath)
        {
			TreeNode folderNodeFound = null;
			if (Directory.Exists(folderPath)) // don't bother drilling unless the directory exists
			{
				// if there's a trailing \ on the folderPath, remove it unless it's a drive letter
				if (folderPath.Length > 3 && folderPath.LastIndexOf("\\") == folderPath.Length - 1)
					folderPath = folderPath.Substring(0, folderPath.Length - 1);
				//Start drilling the tree
				FindFolderRecursive(this.Nodes[0].Nodes, folderPath.ToUpper(cultureInfo), ref folderNodeFound);
			}
			return folderNodeFound;
		}

		private void FindFolderRecursive(TreeNodeCollection tnc, string path, ref TreeNode folderNodeFound)
		{
			foreach (TreeNode tn in tnc)
			{
				if (tn.Tag.ToString() == "DUMMYNODE") continue;
				
				if (folderNodeFound == null)
				{
					string tnPath = ShellOperations.GetFileDirectory(tn).ToUpper(cultureInfo);
					if (path == tnPath && folderNodeFound == null)
					{
						folderNodeFound = tn;
						break;
					}
					else if (path.IndexOf(tnPath) > -1 && folderNodeFound == null)
					{						
						FindFolderRecursive(tn.Nodes, path, ref folderNodeFound);
					}
				}
			}
		}

		


		#endregion
        
		#region System.Windows.Forms.TreeView Properties

        public override System.Drawing.Color BackColor
        {
            get
            { return base.BackColor; }
            set
            { base.BackColor = value; }
        }
        
        public override System.Drawing.Image BackgroundImage
        {
            get
            { return base.BackgroundImage; }
            set
            { base.BackgroundImage = value; }
        }
        
        public override System.Drawing.Color ForeColor
        {
            get
            { return base.ForeColor; }
            set
            { base.ForeColor = value; }
        }
        
        public override string Text
        {
            get
            { return base.Text; }
            set
            { base.Text = value; }
        }
        
        public override bool AllowDrop
        {
            get
            { return base.AllowDrop; }
            set
            { base.AllowDrop = value; }
        }
        
        public override System.Windows.Forms.AnchorStyles Anchor
        {
            get
            { return base.Anchor; }
            set
            { base.Anchor = value; }
        }
        
        public override System.Windows.Forms.BindingContext BindingContext
        {
            get
            { return base.BindingContext; }
            set
            { base.BindingContext = value; }
        }
        
        public override System.Windows.Forms.ContextMenu ContextMenu
        {
            get
            { return base.ContextMenu; }
            set
            { base.ContextMenu = value; }
        }
        
        public override System.Windows.Forms.Cursor Cursor
        {
            get
            {  return base.Cursor; }
            set
            {  base.Cursor = value; }
        }
        
        public override System.Drawing.Rectangle DisplayRectangle
        {
            get
            { return base.DisplayRectangle; }
        }
        
        public override System.Windows.Forms.DockStyle Dock
        {
            get
            { return base.Dock;  }
            set
            { base.Dock = value; }
        }
        
        public override bool Focused
        {
            get
            { return base.Focused; }
        }
        
        public override System.Drawing.Font Font
        {
            get
            { return base.Font; }
            set
            { base.Font = value; }
        }
        
        public override System.Windows.Forms.RightToLeft RightToLeft
        {
            get
            { return base.RightToLeft; }
            set
            { base.RightToLeft = value; }
        }
        
        public override System.ComponentModel.ISite Site
        {
            get
            { return base.Site; }
            set
            { base.Site = value; }
        }

		#endregion

		#region System.Windows.Forms.TreeView Overrides
 
        public override void ResetText()
        {
            base.ResetText();
        }
        
        public override void Refresh()
        {
            base.Refresh();
        }
        
        public override void ResetRightToLeft()
        {
            base.ResetRightToLeft();
        }
        
        public override void ResetForeColor()
        {
            base.ResetForeColor();
        }
        
        public override void ResetFont()
        {
            base.ResetFont();
        }
        
        public override void ResetCursor()
        {
            base.ResetCursor();
        }
        
        public override void ResetBackColor()
        {
            base.ResetBackColor();
        }
        
        public override bool PreProcessMessage(ref System.Windows.Forms.Message msg)
        {
            return base.PreProcessMessage(ref msg);
        }
        
        public override System.Runtime.Remoting.ObjRef CreateObjRef(System.Type requestedType)
        {
            return base.CreateObjRef(requestedType);
        }
        
        public override object InitializeLifetimeService()
        {
            return base.InitializeLifetimeService();
        }
        
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        
        public override string ToString()
        {
            return base.ToString();
        }

		#endregion

    }

	#endregion

	#region ShellOperations Class

	public class ShellOperations
	{

		#region ShellFolder Enums
		// Enums for standard Windows shell folders
		public enum ShellFolder
		{
			Desktop = Shell32.ShellSpecialFolderConstants.ssfDESKTOP, 
			DesktopDirectory = Shell32.ShellSpecialFolderConstants.ssfDESKTOPDIRECTORY,
			MyComputer = Shell32.ShellSpecialFolderConstants.ssfDRIVES,
			MyDocuments = Shell32.ShellSpecialFolderConstants.ssfPERSONAL,
			MyPictures = Shell32.ShellSpecialFolderConstants.ssfMYPICTURES,
			History = Shell32.ShellSpecialFolderConstants.ssfHISTORY,
			Favorites = Shell32.ShellSpecialFolderConstants.ssfFAVORITES,
			Fonts = Shell32.ShellSpecialFolderConstants.ssfFONTS,
			ControlPanel = Shell32.ShellSpecialFolderConstants.ssfCONTROLS,
			TemporaryInternetFiles = Shell32.ShellSpecialFolderConstants.ssfINTERNETCACHE,
			MyNetworkPlaces = Shell32.ShellSpecialFolderConstants.ssfNETHOOD,
			NetworkNeighborhood = Shell32.ShellSpecialFolderConstants.ssfNETWORK,
			ProgramFiles = Shell32.ShellSpecialFolderConstants.ssfPROGRAMFILES,
			RecentFiles = Shell32.ShellSpecialFolderConstants.ssfRECENT,
			StartMenu = Shell32.ShellSpecialFolderConstants.ssfSTARTMENU,
			Windows = Shell32.ShellSpecialFolderConstants.ssfWINDOWS,
			Printers = Shell32.ShellSpecialFolderConstants.ssfPRINTERS,
			RecycleBin = Shell32.ShellSpecialFolderConstants.ssfBITBUCKET,
			Cookies = Shell32.ShellSpecialFolderConstants.ssfCOOKIES,
			ApplicationData = Shell32.ShellSpecialFolderConstants.ssfAPPDATA,
			SendTo = Shell32.ShellSpecialFolderConstants.ssfSENDTO,
			StartUp = Shell32.ShellSpecialFolderConstants.ssfSTARTUP
		}
		#endregion
		
		#region FolderTreeView Methods

		#region GetFileDirectory
		public static string GetFileDirectory(TreeNode tn)
		{
			try
			{
				Shell32.FolderItem folderItem = (Shell32.FolderItem)tn.Tag;
				string folderPath = folderItem.Path;

				if(folderItem.IsFileSystem && Directory.Exists(folderPath)) //Avoid checking path as this ::{20D04FE0-3AEA-1069-A2D8-08002B30309D}
                    return folderPath;
				else
					return "";
			}
			catch
			{
				return "";
			}
		}
		#endregion

		#region Populate Tree
		public static void PopulateTree(TreeView tree, ImageList imageList)
		{
			int imageCount = imageList.Images.Count -1;
			tree.Nodes.Clear();
            
            AddRootNode(tree, ref imageCount, imageList, ShellFolder.Desktop, true);
          
            if (tree.Nodes.Count > 1)
			{
				tree.SelectedNode = tree.Nodes[1];
				ExpandBranch(tree.Nodes[1], imageList);
			}
        }
		#endregion

		#region Add Root Node
		private static void AddRootNode(TreeView tree, ref int imageCount, ImageList imageList, ShellFolder shellFolder, bool getIcons)
		{
            try
            {
                Shell32.Shell shell32 = new Shell32.Shell();
                Shell32.Folder shell32Folder = shell32.NameSpace(shellFolder);
                Shell32.FolderItems items = shell32Folder.Items();

                tree.Nodes.Clear();
                TreeNode desktop = new TreeNode("Desktop", 0, 0);

                // Added in version 1.11
                // add a FolderItem object to the root (Desktop) node tag that corresponds to the DesktopDirectory namespace
                // This ensures that the GetSelectedNodePath will return the actual Desktop folder path when queried.
                // There's possibly a better way to create a Shell32.FolderItem instance for this purpose, 
                // but I surely don't know it

                Shell32.Folder dfolder = shell32.NameSpace(ShellFolder.DesktopDirectory);
                foreach (Shell32.FolderItem fi in dfolder.ParentFolder.Items())
                {
                    if (fi.Name == dfolder.Title)
                    {
                        desktop.Tag = fi;
                        break;
                    }
                }

                // Add the Desktop root node to the tree
                tree.Nodes.Add(desktop);

                // iterate through the Desktop namespace and populate the first level nodes
                if (items != null)
                {
                    for (int index = 0; index < items.Count; index++)
                    //foreach (Shell32.FolderItem item in items) //
                    {
                        Shell32.FolderItem item = items.Item(index);

                        if (item.IsFolder) // this ensures that desktop shortcuts etc are not displayed
                        {
                            TreeNode tn = AddTreeNode(item, ref imageCount, imageList, getIcons);
                            desktop.Nodes.Add(tn);
                            CheckForSubDirs(tn, imageList);
                        }
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
		#endregion

		#region Fill Sub Dirs
		private static void FillSubDirectories(TreeNode tn, ref int imageCount, ImageList imageList, bool getIcons)
		{
			Shell32.FolderItem folderItem = (Shell32.FolderItem)tn.Tag;
			Shell32.Folder folder = (Shell32.Folder)folderItem.GetFolder;

			foreach(Shell32.FolderItem item in folder.Items())
			{
				if(item.IsFileSystem && item.IsFolder && !item.IsBrowsable)
				{
					TreeNode ntn = AddTreeNode(item, ref imageCount, imageList, getIcons);
					tn.Nodes.Add(ntn);
					CheckForSubDirs(ntn, imageList);
				}
			}
		}
		#endregion

		#region Create Dummy Node
		private static void CheckForSubDirs(TreeNode tn, ImageList imageList)
		{
			if(tn.Nodes.Count == 0)
			{
				try
				{
					// create dummy nodes for any subfolders that have further subfolders
					Shell32.FolderItem folderItem = (Shell32.FolderItem)tn.Tag;
					Shell32.Folder folder = (Shell32.Folder)folderItem.GetFolder;

					bool hasFolders = false;
					foreach(Shell32.FolderItem item in folder.Items())
					{
						if(item.IsFileSystem && item.IsFolder && !item.IsBrowsable)
						{
							hasFolders = true;
							break;
						}
					}
					if(hasFolders)
					{
						TreeNode ntn = new TreeNode();
						ntn.Tag = "DUMMYNODE";
						tn.Nodes.Add(ntn);
					}
				}
				catch {}
			}
		}
		#endregion

		#region Expand Branch
		public static void ExpandBranch(TreeNode tn, ImageList imageList)
		{
			// if there's a dummy node present, clear it and replace with actual contents
			if(tn.Nodes.Count == 1 && tn.Nodes[0].Tag.ToString() == "DUMMYNODE")
			{
				tn.Nodes.Clear();
				Shell32.FolderItem folderItem = (Shell32.FolderItem)tn.Tag;
				Shell32.Folder folder = (Shell32.Folder)folderItem.GetFolder;
				int imageCount = imageList.Images.Count - 1;
				foreach(Shell32.FolderItem item in folder.Items())
				{
					if(item.IsFileSystem && item.IsFolder && !item.IsBrowsable)
					{
						TreeNode ntn = AddTreeNode(item, ref imageCount, imageList, true);
						tn.Nodes.Add(ntn);
						CheckForSubDirs(ntn, imageList);
					}
				}
			}
		}
		#endregion

		#region Add Tree Node
		
		private static TreeNode AddTreeNode(Shell32.FolderItem item, ref int imageCount, ImageList imageList, bool getIcons)
		{
            TreeNode tn = new TreeNode();
			tn.Text = item.Name;
			tn.Tag = item;

            if (getIcons)
			{
				try
				{
                    Bitmap bitmapIcon = ExtractIcons.GetIcon(item, false);
                    Bitmap bitmapIconSelected = null;
                    if (bitmapIcon != null) bitmapIconSelected = ExtractIcons.GetIcon(item, true); //To save time

                    if (bitmapIcon != null && bitmapIconSelected != null)
                    {
                        imageCount++;
                        tn.ImageIndex = imageCount;
                        imageList.Images.Add(bitmapIcon);

                        imageCount++;
                        tn.SelectedImageIndex = imageCount;
                        imageList.Images.Add(bitmapIconSelected);
                    } else
                    {
                        tn.ImageIndex = 1;
                        tn.SelectedImageIndex = 2;
                    }
                }
                catch // use default 
				{
					tn.ImageIndex = 1;
					tn.SelectedImageIndex = 2;
				}
			}
			else // use default
			{
				tn.ImageIndex = 1;
				tn.SelectedImageIndex = 2;
			}
            return tn;
		}

		#endregion

		#endregion
	}

	#endregion

	#region ExtractIcons Class

	public class ExtractIcons
	{
		#region Structs & Enum

		[StructLayout(LayoutKind.Sequential)]
			private struct SHFILEINFO
		{
			public SHFILEINFO(bool b)
			{
				hIcon=IntPtr.Zero;iIcon=0;dwAttributes=0;szDisplayName="";szTypeName="";
			}
			public IntPtr hIcon;
			public int iIcon;
			public uint dwAttributes;
			[MarshalAs(UnmanagedType.LPStr, SizeConst=260)]
			public string szDisplayName;
			[MarshalAs(UnmanagedType.LPStr, SizeConst=80)]
			public string szTypeName;
		};

		private enum SHGFI
		{
			ICON =             0x000000100,     // get icon
			DISPLAYNAME =      0x000000200,     // get display name
			TYPENAME =         0x000000400,     // get type name
			ATTRIBUTES =       0x000000800,     // get attributes
			ICONLOCATION =     0x000001000,     // get icon location
			EXETYPE =          0x000002000,     // return exe type
			SYSICONINDEX =     0x000004000,     // get system icon index
			LINKOVERLAY =      0x000008000,     // put a link overlay on icon
			SELECTED =         0x000010000,     // show icon in selected state
			ATTR_SPECIFIED =   0x000020000,     // get only specified attributes
			LARGEICON =        0x000000000,     // get large icon
			SMALLICON =        0x000000001,     // get small icon
			OPENICON =         0x000000002,     // get open icon
			SHELLICONSIZE =    0x000000004,     // get shell size icon
			PIDL =             0x000000008,     // pszPath is a pidl
			USEFILEATTRIBUTES = 0x000000010     // use passed dwFileAttribute
		}

		#endregion


		#region Get Folder Icons

		
		[DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, 
			out SHFILEINFO psfi, uint cbfileInfo, SHGFI uFlags );


		//	DestroyIcon
		//	If the function succeeds, the return value is nonzero.
		//	If the function fails, the return value is zero.
		[DllImport("user32.dll", EntryPoint = "DestroyIcon", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
		private static extern int DestroyIcon(IntPtr hIcon);
		
		//File attribute constants 
		public const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
		public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

		//Removed , ImageList imageList from args, since not in use
		//Change to from string strPath to Shell32.FolderItem item, need folder information
		public static Bitmap GetIcon(Shell32.FolderItem item, bool selected)
		{
            
			SHFILEINFO info = new SHFILEINFO(true);
			int cbFileInfo = Marshal.SizeOf(info);

            SHGFI flags;
			if (!selected)
				flags = SHGFI.ICON | SHGFI.SMALLICON;
			else 
                flags = SHGFI.ICON | SHGFI.SMALLICON | SHGFI.OPENICON;

			//Set coorect flags for Files or Folder
			if (item.IsFolder && !item.IsFileSystem) flags |= SHGFI.USEFILEATTRIBUTES;

			
			//Added correct folder attribute or get error
			SHGetFileInfo(item.Path, FILE_ATTRIBUTE_NORMAL | FILE_ATTRIBUTE_DIRECTORY, out info, (uint)cbFileInfo, flags);
            
            //Return bitmap instead of Icon as workaround for black background
            Bitmap bitmap = null;

			//Avoid memory leak, release icon again after copied
			if (info.hIcon != IntPtr.Zero)
            {
                //bitmap = new Bitmap(Icon.FromHandle(info.hIcon).ToBitmap());
                bitmap = new Bitmap(Icon.FromHandle(info.hIcon).Width, Icon.FromHandle(info.hIcon).Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(bitmap);
                g.FillRectangle(new SolidBrush(Color.Transparent), 0, 0, bitmap.Width, bitmap.Width); //I don't know why, if I don't clear the rectagle, it fails
                g.DrawImage(Icon.FromHandle(info.hIcon).ToBitmap(), 0, 0);
                DestroyIcon(info.hIcon);
            } 
            return bitmap;
        }

		#endregion

		#region Get Desktop Icon

		// Retreive the desktop icon from Shell32.dll - it always appears at index 34 in all shell32 versions.
		// This is probably NOT the best way to retreive this icon, but it works - if you have a better way
		// by all means let me know..

		//		[DllImport("Shell32.dll", CharSet=CharSet.Auto)]
		//		public static extern IntPtr ExtractIcon(int hInst, string lpszExeFileName, int nIconIndex);
		//
		//		public static Icon GetDesktopIcon()
		//		{
		//			IntPtr i = ExtractIcon(0, Environment.SystemDirectory + "\\shell32.dll", 34);
		//			return Icon.FromHandle(i);
		//		}

		// Updated this method in v1.11 so that the icon returned is a small icon, not a large icon as
		// returned by the old method above

		[DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
		public static extern uint ExtractIconEx(
			string lpszFile, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons );

		public static Bitmap GetDesktopIcon()
		{
			IntPtr[] handlesIconLarge = new IntPtr[1];
			IntPtr[] handlesIconSmall = new IntPtr[1];
			uint i = ExtractIconEx(Environment.SystemDirectory + "\\shell32.dll", 34, 
				handlesIconLarge, handlesIconSmall, 1);

			//Retrn bitmap instead of Icon as workaround for black background
			Bitmap bitmap = Icon.FromHandle(handlesIconSmall[0]).ToBitmap();
            //Avoid memory leak, release icon again after copied
            //if (handlesIconSmall[0] != IntPtr.Zero) DestroyIcon(handlesIconSmall[0]);

            return bitmap;
		}
		
		#endregion
		
	}

	#endregion


}

