// ImageListView - A listview control for image files
// Copyright (C) 2009 Ozgur Ozcitak
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Ozgur Ozcitak (ozcitak@yahoo.com)

namespace Manina.Windows.Forms
{
    //JTN Added - Item File status
    public class ItemFileStatus //: IComparer
    {
        #region Exists
        public bool FileExists { get; set; } = true;
        public bool FailedToAccessInfo { get; set; } = false;
        public bool IsDirty { get; set; } = true;
        #endregion

        #region Access        
        public bool IsFileLockedReadAndWrite { get; set; } = false;
        public bool IsFileLockedForRead { get; set; } = false;
        public bool IsReadOnly { get; set; } = false;
        #endregion

        #region Located
        public bool IsInCloud { get; set; } = false;
        public bool IsVirtual { get; set; } = false;
        public bool IsOffline { get; set; } = false;

        public bool IsInCloudOrVirtualOrOffline
        {
            get
            {
                if (IsInCloud != IsVirtual != IsOffline)
                {
                    //DEBUG
                }
                return IsInCloud || IsVirtual || IsOffline;
            }
        }
        #endregion

        #region Processes
        public bool IsDownloadingFromCloud { get; set; } = false;        
        public bool IsExiftoolRunning { get; set; } = false;
        #endregion

        #region SortValue
        private int SortValue
        {
            get 
            {
                return
                    #region Exists
                    (FileExists ? 128 : 0) +
                    (FailedToAccessInfo ? 64 : 0) +
                    #endregion

                    #region Access
                    (IsFileLockedReadAndWrite ? 32 : 0) +
                    (IsFileLockedForRead ? 16 : 0) +
                    (IsReadOnly ? 8 : 0) +
                    #endregion

                    #region Located
                    (IsInCloud ? 4 : 0) +
                    (IsVirtual ? 4 : 0) +
                    (IsOffline ? 4 : 0) +
                    #endregion

                    #region Processes
                    (IsDownloadingFromCloud ? 2 : 0) +
                    (IsExiftoolRunning ? 1 : 0);
                    #endregion
            }
        }
        #endregion

        #region Compare
        public static int Compare(ItemFileStatus x, ItemFileStatus y)
        {
            return x.SortValue.CompareTo(y.SortValue);
        }
        #endregion
    }
}
