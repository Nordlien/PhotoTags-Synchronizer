using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using MetadataLibrary;
using ImageAndMovieFileExtentions;
using Manina.Windows.Forms;
using System.Threading;
using Thumbnails;
using System.Diagnostics;
using Krypton.Toolkit;
using Raccoom.Windows.Forms;
using FileHandeling;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
        #region TreeViewFolderBrowser - BeforeSelect - Click
        private void treeViewFolderBrowser1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (GlobalData.IsApplicationClosing) e.Cancel = true;
            if (!DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect())
            {
                if (DoNotTrigger_TreeViewFilter_BeforeAndAfterCheck()) e.Cancel = true;
                if (IsPopulatingAnything("Select Items")) e.Cancel = true;
                if (SaveBeforeContinue(true) == DialogResult.Cancel) e.Cancel = true;
            }
        }
        #endregion

        #region TreeViewFolderBrowser - AfterSelect - Click
        private void treeViewFolderBrowser1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect()) return;
            if (IsDragAndDropActive()) return;
            
            try
            {
                GlobalData.SearchFolder = true;
                ImageListView_FetchListOfMediaFiles_FromFolder_and_Aggregate(false, true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TreeViewFolderBrowser - GetNodeFolderRealPath
        private string GetNodeFolderRealPath(TreeNodePath treeNodePath)
        {
            string folder = treeNodePath?.Path == null ? "" : treeNodePath?.Path; //"C:\\Users\\nordl\\OneDrive\\Skrivebord"
            if (!Directory.Exists(folder))
            {
                try
                {
                    if (treeNodePath.Tag is Raccoom.Win32.ShellItem shellItem) folder = Raccoom.Win32.ShellItem.GetRealPath(shellItem);
                    if (folder.StartsWith("::{")) folder = "";
                }
                catch { }
            }
            return Directory.Exists(folder) ? folder : "";
        }
        #endregion

        #region TreeViewFolderBrowser - GetSelectedNodeFullRealPath() 
        private string GetSelectedNodeFullRealPath()
        {
            return GetNodeFolderRealPath(treeViewFolderBrowser1.SelectedNode as TreeNodePath);
        }
        #endregion

        #region TreeViewFolderBrowser - GetNodeFolderFullLinkPath 
        private string GetNodeFolderFullLinkPath(TreeNodePath treeNodePath)
        {
            return treeNodePath?.FullPath == null ? "" : treeNodePath?.FullPath; //"Desktop"
            //Path     "C:\\Users\\nordl\\OneDrive\\Pictures JTNs OneDrive\\a-- PhotoTags Synchronizer --a"
            //FullPath "Desktop\\This PC\\Pictures\\a-- PhotoTags Synchronizer --a"
        }
        #endregion

        #region TreeViewFolderBrowser - GetSelectedNodeFullLinkPath 
        private string GetSelectedNodeFullLinkPath()
        {
            return GetNodeFolderFullLinkPath(treeViewFolderBrowser1.SelectedNode as TreeNodePath);
        }
        #endregion 


    }
}

