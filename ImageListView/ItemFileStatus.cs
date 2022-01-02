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
        public bool IsInCloud { get; set; } = false;
        public bool IsDownloadingFromCloud { get; set; } = false;
        public bool IsReadOnly { get; set; } = false;
        public bool IsExiftoolRunning { get; set; } = false;

        private int SortValue
        {
            get 
            {
                return
                    (IsInCloud ? 1 : 0) +
                    (IsDownloadingFromCloud ? 2 : 0) +
                    (IsExiftoolRunning ? 4 : 0) +
                    (IsReadOnly ? 8 : 0);
            }
        }

        public static int Compare(ItemFileStatus x, ItemFileStatus y)
        {
            return x.SortValue.CompareTo(y.SortValue);
        }

    }
}
