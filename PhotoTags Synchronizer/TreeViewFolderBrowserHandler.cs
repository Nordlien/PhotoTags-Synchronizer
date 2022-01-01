using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Raccoom.Windows.Forms;

namespace PhotoTagsSynchronizer
{
    public static class TreeViewFolderBrowserHandler
    {
        #region TreeViewFolderBrowser - Remove TreeNode
        public static void RemoveTreeNode(TreeViewFolderBrowser folderTreeViewFolder, TreeNode treeNode)
        {
            if (treeNode != null)
            {
                TreeNodePath node = (TreeNodePath)treeNode;
                Raccoom.Win32.ShellItem folderItem = ((Raccoom.Win32.ShellItem)node.Tag);
                folderItem.ClearFolders();
                node.Remove();
            }
        }
        #endregion

        #region TreeViewFolderBrowser - Refresh TreeNode
        public static void RefreshTreeNode(TreeViewFolderBrowser folderTreeViewFolder, TreeNode treeNode)
        {
            if (treeNode != null)
            {

                TreeNodePath node = (TreeNodePath)treeNode;
                Raccoom.Win32.ShellItem folderItem = ((Raccoom.Win32.ShellItem)node.Tag);
                folderItem.ClearFolders();
                node.Refresh();
                folderTreeViewFolder.UseWaitCursor = true;
                folderTreeViewFolder.BeginUpdate();
                treeNode.Collapse();
                treeNode.Expand();
                folderTreeViewFolder.EndUpdate();
                folderTreeViewFolder.UseWaitCursor = false;
            }
        }
        #endregion

        #region TreeViewFolderBrowser - RefreshFolderWithName
        public static void RefreshFolderWithName(TreeViewFolderBrowser folderTreeViewFolder, string folder, bool refreshParentFolder)
        {
            string refreshThisFolder;
            if (refreshParentFolder)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(folder);
                refreshThisFolder = directoryInfo.Parent.FullName; //Parent folder
            }
            else refreshThisFolder = folder;

            List<TreeNode> treeNodes = new List<TreeNode>();
            FindAllNodesRecursive(folderTreeViewFolder.Nodes, refreshThisFolder, ref treeNodes);
            foreach (TreeNode treeNode in treeNodes) RefreshTreeNode(folderTreeViewFolder, treeNode);
        }
        #endregion

        #region TreeViewFolderBrowser - RemoveFolderWithName
        public static void RemoveFolderWithName(TreeViewFolderBrowser folderTreeViewFolder, string folder)
        {
            List<TreeNode> targetNodes = TreeViewFolderBrowserHandler.FindAllNodes(folderTreeViewFolder.Nodes, folder);
            foreach (TreeNode targetNode in targetNodes)
            {
                TreeViewFolderBrowserHandler.RemoveTreeNode(folderTreeViewFolder, targetNode);
            }
        }
        #endregion

        #region TreeViewFolderBrowser - FindAllNodes
        public static List<TreeNode> FindAllNodes(TreeNodeCollection treeNodeCollection, string directory)
        {
            List<TreeNode> treeNodeFound = new List<TreeNode>();
            FindAllNodesRecursive(treeNodeCollection, directory, ref treeNodeFound);
            return treeNodeFound;
        }

        public static void FindAllNodesRecursive(TreeNodeCollection treeNodeCollection, string directory, ref List<TreeNode> treeNodeFound)
        {
            foreach (TreeNode treeNodeSearch in treeNodeCollection)
            {
                TreeNodePath treeNodePath = (TreeNodePath)treeNodeSearch;
                if (string.Compare(treeNodePath.Path, directory, true) == 0)
                {
                    treeNodeFound.Add(treeNodeSearch);
                }
                if (treeNodeSearch.Nodes != null) FindAllNodesRecursive(treeNodeSearch.Nodes, directory, ref treeNodeFound);
            }
        }
        #endregion

        #region TreeViewFolderBrowserEnabled
        public static void Enabled(TreeViewFolderBrowser treeViewFolderBrowser, bool enabled)
        {
            //treeViewFolderBrowser.Enabled = enabled;
        }
        #endregion
    }
}
