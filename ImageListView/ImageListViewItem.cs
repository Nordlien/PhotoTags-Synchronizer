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

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Represents an item in the image list view.
    /// </summary>
    public class ImageListViewItem
    {
        #region Member Variables
        
        // Property backing fields
        internal int mIndex;
        private Color mBackColor;
        private Color mForeColor;
        private Guid mGuid;
        internal ImageListView mImageListView;
        internal bool mSelected;
        private string mText;
        private int mZOrder;

        // File info
        private DateTime mmFileDateCreated;
        private DateTime mFileDateCreated
        {
            get { return mmFileDateCreated; }
            set { isDirtyFileDateCreated = false; mmFileDateCreated = value; }
        }
        private bool isDirtyFileDateCreated = true;

        private DateTime mmFileDateModified;
        private DateTime mFileDateModified
        {
            get { return mmFileDateModified; }
            set { isDirtyFileDateModified = false; mmFileDateModified = value; }
        }
        private bool isDirtyFileDateModified = true;
        
        private string mmFileType;
        private string mFileType
        {
            get { return mmFileType; }
            set { isDirtyFileType = false; mmFileType = value; }
        }
        private bool isDirtyFileType = true;

        private string mmFileName; 
        private string mFileName
        {
            get { return mmFileName; }
            set { isDirtyFileName = false; mmFileName = value; }
        }
        private bool isDirtyFileName = true;

        private string mmFileDirectory;
        private string mFileDirectory
        {
            get { return mmFileDirectory; }
            set { isDirtyFileDirectory = false; mmFileDirectory = value; }
        }
        private bool isDirtyFileDirectory = true;

        private long mmFileSize;
        private long mFileSize
        {
            get { return mmFileSize; }
            set { isDirtyFileSize = false; mmFileSize = value; }
        }
        private bool isDirtyFileSize = true;

        private Size mmMediaDimensions;
        private Size mMediaDimensions
        {
            get { return mmMediaDimensions; }
            set { isDirtyMediaDimensions = false; mmMediaDimensions = value; }
        }
        private bool isDirtyMediaDimensions = true;

        // Exif tags
        private string mmCameraMake;
        private string mCameraMake
        {
            get { return mmCameraMake; }
            set { isDirtyCameraMake = false; mmCameraMake = value; }
        }
        private bool isDirtyCameraMake = true;

        private string mmCameraModel;
        private string mCameraModel
        {
            get { return mmCameraModel; }
            set { isDirtyCameraModel = false; mmCameraModel = value; }
        }
        private bool isDirtyCameraModel = true;

        private DateTime mmMediaDateTaken;
        private DateTime mMediaDateTaken
        {
            get { return mmMediaDateTaken; }
            set { isDirtyMediaDateTaken = false; mmMediaDateTaken = value; }
        }
        private bool isDirtyMediaDateTaken = true;

        //JTN: Added more column types
        private string mmMediaAlbum;
        private string mMediaAlbum
        {
            get { return mmMediaAlbum; }
            set { isDirtyMediaAlbum = false; mmMediaAlbum = value; }
        }
        private bool isDirtyMediaAlbum = true;

        private string mmMediaTitle;
        private string mMediaTitle
        {
            get { return mmMediaTitle; }
            set { isDirtyMediaTitle = false; mmMediaTitle = value; }
        }
        private bool isDirtyMediaTitle = true;

        private string mmMediaDescription;
        private string mMediaDescription
        {
            get { return mmMediaDescription; }
            set { isDirtyMediaDescription = false; mmMediaDescription = value; }
        }
        private bool isDirtyMediaDescription = true;

        private string mmMediaComment;
        private string mMediaComment
        {
            get { return mmMediaComment; }
            set { isDirtyMediaComment = false; mmMediaComment = value; }
        }
        private bool isDirtyMediaComment = true;

        private string mmMediaAuthor;
        private string mMediaAuthor
        {
            get { return mmMediaAuthor; }
            set { isDirtyMediaAuthor = false; mmMediaAuthor = value; }
        }
        private bool isDirtyMediaAuthor = true;

        private byte mmMediaRating;
        private byte mMediaRating
        {
            get { return mmMediaRating; }
            set { isDirtyMediaRating = false; mmMediaRating = value; }
        }
        private bool isDirtyMediaRating = true;

        private string mmLocationName;
        private string mLocationName
        {
            get { return mmLocationName; }
            set { isDirtyLocationName = false; mmLocationName = value; }
        }
        private bool isDirtyLocationName = true;

        private string mmLocationRegionState;
        private string mLocationRegionState
        {
            get { return mmLocationRegionState; }
            set { isDirtyLocationRegionState = false; mmLocationRegionState = value; }
        }
        private bool isDirtyLocationRegionState = true;

        private string mmLocationCity;
        private string mLocationCity
        {
            get { return mmLocationCity; }
            set { isDirtyLocationCity = false; mmLocationCity = value; }
        }
        private bool isDirtyLocationCity = true;

        private string mmLocationCountry;
        private string mLocationCountry
        {
            get { return mmLocationCountry; }
            set { isDirtyLocationCountry = false; mmLocationCountry = value; }
        }
        private bool isDirtyLocationCountry = true;

        // Used for virtual items
        internal bool isVirtualItem;
        internal object mVirtualItemKey;

        internal ImageListView.ImageListViewItemCollection owner;
        internal bool mIsDirty;
        bool isDirty
        {
            get { 
                return isDirtyFileDateCreated ||
                isDirtyFileDateModified ||
                isDirtyFileType ||
                isDirtyFileName ||
                isDirtyFileDirectory ||
                isDirtyFileSize ||
                isDirtyMediaDimensions ||
                isDirtyCameraMake ||
                isDirtyCameraModel ||
                isDirtyMediaDateTaken ||
                isDirtyMediaAlbum ||
                isDirtyMediaTitle ||
                isDirtyMediaDescription ||
                isDirtyMediaComment ||
                isDirtyMediaAuthor ||
                isDirtyMediaRating ||
                isDirtyLocationName ||
                isDirtyLocationRegionState ||
                isDirtyLocationCity ||
                isDirtyLocationCountry ; 
            }
            set
            {
                mIsDirty = value;
                if (mIsDirty)
                {
                    isDirtyFileDateCreated = value;
                    isDirtyFileDateModified = value;
                    isDirtyFileType = value;
                    isDirtyFileName = value;
                    isDirtyFileDirectory = value;
                    isDirtyFileSize = value;
                    isDirtyMediaDimensions = value;
                    isDirtyCameraMake = value;
                    isDirtyCameraModel = value;
                    isDirtyMediaDateTaken = value;
                    isDirtyMediaAlbum = value;
                    isDirtyMediaTitle = value;
                    isDirtyMediaDescription = value;
                    isDirtyMediaComment = value;
                    isDirtyMediaAuthor = value;
                    isDirtyMediaRating = value;
                    isDirtyLocationName = value;
                    isDirtyLocationRegionState = value;
                    isDirtyLocationCity = value;
                    isDirtyLocationCountry = value;
                }
            }
        }
        internal bool isFileInfoDirty //Added by JTN
        {
            get
            {
                return isDirtyFileDateCreated ||
                isDirtyFileDateModified ||
                isDirtyFileType ||
                isDirtyFileName ||
                isDirtyFileDirectory ||
                isDirtyFileSize /*||
                isDirtyMediaDimensions ||
                isDirtyCameraMake ||
                isDirtyCameraModel ||
                isDirtyMediaDateTaken ||
                isDirtyMediaAlbum ||
                isDirtyMediaTitle ||
                isDirtyMediaDescription ||
                isDirtyMediaComment ||
                isDirtyMediaAuthor ||
                isDirtyMediaRating ||
                isDirtyLocationName ||
                isDirtyLocationRegionState ||
                isDirtyLocationCity ||
                isDirtyLocationCountry */;
            }
        }
        private bool editing;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the background color of the item.
        /// </summary>
        [Category("Appearance"), Browsable(true), Description("Gets or sets the background color of the item."), DefaultValue(typeof(Color), "Transparent")]
        public Color BackColor
        {
            get
            {
                return mBackColor;
            }
            set
            {
                if (value != mBackColor)
                {
                    mBackColor = value;
                    if (mImageListView != null)
                        mImageListView.Refresh();
                }
            }
        }
        /// <summary>
        /// Gets the cache state of the item thumbnail.
        /// </summary>
        [Category("Behavior"), Browsable(false), Description("Gets the cache state of the item thumbnail.")]
        public CacheState ThumbnailCacheState { get { return mImageListView.cacheManager.GetCacheState(mGuid); } }
        /// <summary>
        /// Gets a value determining if the item is focused.
        /// </summary>
        [Category("Appearance"), Browsable(false), Description("Gets a value determining if the item is focused.")]
        public bool Focused
        {
            get
            {
                if (owner == null || owner.FocusedItem == null) return false;
                return (this == owner.FocusedItem);
            }
            set
            {
                if (owner != null)
                    owner.FocusedItem = this;
            }
        }
        /// <summary>
        /// Gets or sets the foreground color of the item.
        /// </summary>
        [Category("Appearance"), Browsable(true), Description("Gets or sets the foreground color of the item."), DefaultValue(typeof(Color), "WindowText")]
        public Color ForeColor
        {
            get
            {
                return mForeColor;
            }
            set
            {
                if (value != mForeColor)
                {
                    mForeColor = value;
                    if (mImageListView != null)
                        mImageListView.Refresh();
                }
            }
        }
        /// <summary>
        /// Gets the unique identifier for this item.
        /// </summary>
        [Category("Behavior"), Browsable(false), Description("Gets the unique identifier for this item.")]
        internal Guid Guid { get { return mGuid; } private set { mGuid = value; } }
        /// <summary>
        /// Gets the virtual item key associated with this item.
        /// Returns null if the item is not a virtual item.
        /// </summary>
        [Category("Behavior"), Browsable(false), Description("Gets the virtual item key associated with this item.")]
        public object VirtualItemKey { get { return mVirtualItemKey; } }
        /// <summary>
        /// Gets the ImageListView owning this item.
        /// </summary>
        [Category("Behavior"), Browsable(false), Description("Gets the ImageListView owning this item.")]
        public ImageListView ImageListView { get { return mImageListView; } private set { mImageListView = value; } }
        /// <summary>
        /// Gets the index of the item.
        /// </summary>
        [Category("Behavior"), Browsable(false), Description("Gets the index of the item."), EditorBrowsable(EditorBrowsableState.Advanced)]
        public int Index { get { return mIndex; } }
        /// <summary>
        /// Gets or sets a value determining if the item is selected.
        /// </summary>
        [Category("Appearance"), Browsable(true), Description("Gets or sets a value determining if the item is selected."), DefaultValue(false)]
        public bool Selected
        {
            get
            {
                return mSelected;
            }
            set
            {
                if (value != mSelected)
                {
                    mSelected = value;
                    if (mImageListView != null)
                        mImageListView.OnSelectionChangedInternal();
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the user-defined data associated with the item.
        /// </summary>
        [Category("Data"), Browsable(true), Description("Gets or sets the user-defined data associated with the item.")]
        public object Tag { get; set; }

        /// <summary>
        /// Gets or sets the text associated with this item. If left blank, item Text 
        /// reverts to the name of the image file.
        /// </summary>
        [Category("Appearance"), Browsable(true), Description("Gets or sets the text associated with this item. If left blank, item Text reverts to the name of the image file.")]
        public string Text
        {
            get
            {
                return mText;
            }
            set
            {
                mText = value;
                if (mImageListView != null)
                    mImageListView.Refresh();
            }
        }

        /// <summary>
        /// Gets the thumbnail image. If the thumbnail image is not cached, it will be 
        /// added to the cache queue and DefaultImage of the owner image list view will
        /// be returned. If the thumbnail could not be cached ErrorImage of the owner
        /// image list view will be returned.
        /// </summary>
        [Category("Appearance"), Browsable(false), Description("Gets the thumbnail image.")]
        public Image ThumbnailImage
        {
            get
            {
                if (mImageListView == null)
                    throw new InvalidOperationException("Owner control is null.");

                CacheState state = ThumbnailCacheState;
                if (state == CacheState.Error)
                    return mImageListView.ErrorImage;

                Image img = mImageListView.cacheManager.GetImage(Guid);
                if (img != null) return img;

                if (isVirtualItem)
                    mImageListView.cacheManager.Add(Guid, mVirtualItemKey, mImageListView.ThumbnailSize, mImageListView.UseEmbeddedThumbnails);
                else
                    mImageListView.cacheManager.Add(Guid, FileFullPath, mImageListView.ThumbnailSize, mImageListView.UseEmbeddedThumbnails);
                return mImageListView.DefaultImage;
            }
            
        }

        /// <summary>
        /// Gets or sets the draw order of the item.
        /// </summary>
        [Category("Appearance"), Browsable(true), Description("Gets or sets the draw order of the item."), DefaultValue(0)]
        public int ZOrder { get { return mZOrder; } set { mZOrder = value; } }

        /// <summary>
        /// Gets the creation date of the image file represented by this item.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the creation date of the image file represented by this item.")]
        public DateTime DateCreated { get { UpdateFileInfo(isDirtyFileDateCreated); return mFileDateCreated; } }
        
        /// <summary>
        /// Gets the modification date of the image file represented by this item.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the modification date of the image file represented by this item.")]
        
        public DateTime DateModified { get { UpdateFileInfo(isDirtyFileDateModified); return mFileDateModified; } }
        /// <summary>
        /// Gets the shell type of the image file represented by this item.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the shell type of the image file represented by this item.")]
        public string FileType { get { UpdateFileInfo(isDirtyFileType); return mFileType; } }
        
        /// <summary>
        /// Gets or sets the name of the image fie represented by this item.
        /// </summary>        
        [Category("Data"), Browsable(false), Description("Gets or sets the name of the image fie represented by this item.")]
        public string FileFullPath
        {
            get
            {
                return mFileName;
            }
            set
            {
                if (mFileName != value)
                {
                    mFileName = value;
                    if (!isVirtualItem)
                    {
                        isDirty = true;
                        //isFileInfoDirty = true;
                        if (mImageListView != null)
                        {
                            mImageListView.cacheManager.Remove(Guid);
                            mImageListView.itemCacheManager.Add(this);
                            mImageListView.Refresh();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the path of the image fie represented by this item.
        /// </summary>        
        [Category("Data"), Browsable(false), Description("Gets the path of the image fie represented by this item.")]
        public string FileDirectory { get { UpdateFileInfo(isDirtyFileDirectory); return mFileDirectory; } }
        
        /// <summary>
        /// Gets file size in bytes.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets file size in bytes.")]
        public long FileSize { get { UpdateFileInfo(isDirtyFileSize); return mFileSize; } }
        
        /// <summary>
        /// Gets image dimensions.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets image dimensions.")]
        public Size Dimensions { get { UpdateFileInfo(isDirtyMediaDimensions); return mMediaDimensions; } }

        /// <summary>
        /// Gets the camera model.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the camera make.")]
        public string CameraMake { get { UpdateFileInfo(isDirtyCameraMake); return mCameraMake; } }

        /// <summary>
        /// Gets the camera model.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the camera model.")]
        public string CameraModel { get { UpdateFileInfo(isDirtyCameraModel); return mCameraModel; } }
        
        /// <summary>
        /// Gets the date and time the image was taken.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the date and time the image was taken.")]
        public DateTime DateTaken { get { UpdateFileInfo(isDirtyMediaDateTaken); return mMediaDateTaken; } }
        
        //JTN: Added more column types
        
        /// <summary>
        /// Gets media album.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets media album.")]
        public string MediaAlbum { get { UpdateFileInfo(isDirtyMediaAlbum); return mMediaAlbum; } }

        /// <summary>
        /// Gets media title.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets media title.")]
        public string MediaTitle { get { UpdateFileInfo(isDirtyMediaTitle); return mMediaTitle; } }
        
        /// <summary>
        /// Gets media description.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets media description.")]
        public string MediaDescription { get { UpdateFileInfo(isDirtyMediaDescription); return mMediaDescription; } }

        /// <summary>
        /// Gets user comments.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets user comments.")]
        public string MediaComment { get { UpdateFileInfo(isDirtyMediaComment); return mMediaComment; } }

        /// <summary>
        /// Gets the name of the artist.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the name of the media file author.")]
        public string MediaAuthor { get { UpdateFileInfo(isDirtyMediaAuthor); return mMediaAuthor; } }

        /// <summary>
        /// Gets the media rating.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the media rating.")]
        public byte MediaRating { get { UpdateFileInfo(isDirtyMediaRating); return mMediaRating; } }

        /// <summary>
        /// Gets the media location name.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the name of the media location name.")]
        public string LocationName { get { UpdateFileInfo(isDirtyLocationName); return mLocationName; } }

        /// <summary>
        /// Gets the media location region or state.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the name of the media Llocation region or state.")]
        public string LocationRegionState { get { UpdateFileInfo(isDirtyLocationRegionState); return mLocationRegionState; } }

        /// <summary>
        /// Gets the media location city.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the name of the media location city.")]
        public string LocationCity { get { UpdateFileInfo(isDirtyLocationCity); return mLocationCity; } }

        /// <summary>
        /// Gets the media location country.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the name of the media location country.")]
        public string LocationCountry { get { UpdateFileInfo(isDirtyLocationCountry); return mLocationCountry; } }
        

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ImageListViewItem class.
        /// </summary>
        public ImageListViewItem()
        {
            mIndex = -1;
            owner = null;

            mBackColor = Color.Transparent;
            mForeColor = SystemColors.WindowText;
            mZOrder = 0;

            Guid = Guid.NewGuid();
            ImageListView = null;
            Selected = false;

            isDirty = true;
            //isFileInfoDirty = true;
            editing = false;

            mVirtualItemKey = null;
            isVirtualItem = false;
        }
        /// <summary>
        /// Initializes a new instance of the ImageListViewItem class.
        /// </summary>
        /// <param name="filename">The image filename representing the item.</param>
        public ImageListViewItem(string filename)
            : this()
        {
            mFileName = filename;
            mText = Path.GetFileName(filename);
        }

        /// <summary>
        /// Initializes a new instance of the ImageListViewItem class.
        /// </summary>
        /// <param name="filename">The image filename representing the item.</param>
        //JTN: Add by JTN, no read data already known
        public ImageListViewItem(FileInfo fileInfo)
            : this()
        {
            mFileName = fileInfo.FullName;
            mText = Path.GetFileName(mFileName);

            mFileDateCreated = fileInfo.CreationTime;
            mFileDateModified = fileInfo.LastWriteTime;
            mFileSize = fileInfo.Length;
            mFileDirectory = fileInfo.DirectoryName;

            //isFileInfoDirty = false;
            // Exif tags
            
        }

        /// <summary>
        /// Initializes a new instance of a virtual ImageListViewItem class.
        /// </summary>
        /// <param name="key">The key identifying this item.</param>
        /// <param name="text">Text of this item.</param>
        /// <param name="dimensions">Pixel dimensions of the source image.</param>
        public ImageListViewItem(object key, string text, Size dimensions)
            : this()
        {
            isVirtualItem = true;
            mVirtualItemKey = key;
            mText = text;
            mMediaDimensions = dimensions;
        }
        
        /// <summary>
        /// Initializes a new instance of a virtual ImageListViewItem class.
        /// </summary>
        /// <param name="key">The key identifying this item.</param>
        /// <param name="text">Text of this item.</param>
        public ImageListViewItem(object key, string text)
            : this(key, text, Size.Empty)
        {
            ;
        }
        
        /// <summary>
        /// Initializes a new instance of a virtual ImageListViewItem class.
        /// </summary>
        /// <param name="key">The key identifying this item.</param>
        public ImageListViewItem(object key)
            : this(key, string.Empty, Size.Empty)
        {
            ;
        }
        #endregion


        #region Instance Methods
        /// <summary>
        /// Begins editing the item.
        /// This method must be used while editing the item
        /// to prevent collisions with the cache manager.
        /// </summary>
        public void BeginEdit()
        {
            if (editing == true)
                throw new InvalidOperationException("Already editing this item.");

            if (mImageListView == null)
                throw new InvalidOperationException("Owner control is null.");

            UpdateFileInfo(false);
            mImageListView.cacheManager.BeginItemEdit(mGuid, mFileName);
            mImageListView.itemCacheManager.BeginItemEdit(mGuid);

            editing = true;
        }

        /// <summary>
        /// Ends editing and updates the item.
        /// </summary>
        /// <param name="update">If set to true, the item will be immediately updated.</param>
        public void EndEdit(bool update)
        {
            if (editing == false)
                throw new InvalidOperationException("This item is not being edited.");

            if (mImageListView == null)
                throw new InvalidOperationException("Owner control is null.");

            mImageListView.cacheManager.EndItemEdit(mGuid);
            mImageListView.itemCacheManager.EndItemEdit(mGuid);

            editing = false;
            if (update) Update();
        }

        /// <summary>
        /// Ends editing and updates the item.
        /// </summary>
        public void EndEdit()
        {
            EndEdit(true);
        }

        /// <summary>
        /// Updates item thumbnail and item details.
        /// </summary>
        public void Update()
        {
            isDirty = true;
            //isFileInfoDirty = true;
            if (mImageListView != null)
            {
                mImageListView.cacheManager.Remove(mGuid, true);
                mImageListView.itemCacheManager.Add(this);
                mImageListView.Refresh(); 
            }
        }

        /// <summary>
        /// Returns the sub item item text corresponding to the specified column type.
        /// </summary>
        /// <param name="type">The type of information to return.</param>
        /// <returns>Formatted text for the given column type.</returns>
        public string GetSubItemText(ColumnType type)
        {
            //JTN: Added more column types
            switch (type)
            {
                case ColumnType.FileDateCreated:
                    if (DateCreated == DateTime.MinValue) return "";
                    else return DateCreated.ToString("g");
                //TimeZone.TimeZoneLibrary.ToStringW3CDTF_UTC(newDateTimeFileCreated)

                case ColumnType.FileDateModified:
                    if (DateModified == DateTime.MinValue) return "";
                    else return DateModified.ToString("g");
                
                case ColumnType.FileFullPath:
                    return FileFullPath;
                case ColumnType.FileName:
                    return Text;
                case ColumnType.FileDirectory:
                    return FileDirectory;
                case ColumnType.FileSize:
                    if (FileSize == 0) return "";
                    else return Utility.FormatSize(FileSize);
                case ColumnType.FileType:
                    return FileType;
                case ColumnType.MediaDimensions:
                    if (Dimensions == Size.Empty) return "";
                    else return string.Format("{0} x {1}", Dimensions.Width, Dimensions.Height);
                    
                case ColumnType.CameraMake:
                    return CameraMake;
                case ColumnType.CameraModel:
                    return CameraModel;
                case ColumnType.MediaDateTaken:
                    if (DateTaken == DateTime.MinValue) return "";
                    else return DateTaken.ToString("g");                
                
                case ColumnType.MediaTitle:
                    return MediaTitle;
                case ColumnType.MediaDescription:
                    return MediaDescription;
                case ColumnType.MediaComment:
                    return MediaComment;
                case ColumnType.MediaAuthor:
                    return MediaAuthor;
                case ColumnType.MediaRating:
                    if (MediaRating <= 0) return "";
                    else return MediaRating.ToString();
                
                case ColumnType.MediaAlbum:
                    return MediaAlbum;
                case ColumnType.LocationName:
                    return LocationName;
                case ColumnType.LocationRegionState:
                    return LocationRegionState;
                case ColumnType.LocationCity:
                    return LocationCity;
                case ColumnType.LocationCountry:
                    return LocationCountry;
                default:
                    throw new ArgumentException("Unknown column type", "type");
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Updates file info for the image file represented by this item.
        /// </summary>
        private void UpdateFileInfo(bool isValueDirty)
        {
            if (!isValueDirty) return;
            //if (needCheckOnlyFileInfo && !isFileInfoDirty) return;
            //if (!needCheckOnlyFileInfo && !isDirty) return;

            if (isVirtualItem)
            {
                throw new Exception("Not implemented");
                /*
                if (mImageListView != null)
                {
                    VirtualItemDetailsEventArgs e = new VirtualItemDetailsEventArgs(mVirtualItemKey);
                    mImageListView.RetrieveVirtualItemDetailsInternal(e);
                    UpdateDetailsInternal(e); //Virtual
                }*/
            }
            else
            {
                RetrieveItemMetadataDetailsEventArgs e = new RetrieveItemMetadataDetailsEventArgs(mFileName);
                mImageListView.RetrieveItemMetadataDetailsInternal(e);

                if (e.FileMetadata != null)
                {
                    UpdateDetailsInternal(e.FileMetadata);
                }
                else    //If not handled using event, try do it internal
                {
                    Utility.ShellImageFileInfo info = new Utility.ShellImageFileInfo(mFileName);
                    UpdateDetailsInternal(info);
                }
            }
            //isFileInfoDirty = false;
            //isDirty = false;
        }
        /// <summary>
        /// Invoked by the worker thread to update item details.
        /// </summary>
        internal void UpdateDetailsInternal(Utility.ShellImageFileInfo info)
        {
            if (!isDirty && !isFileInfoDirty) return;
            if (info != null)
            {
                #region Provided by FileInfo
                if (info.IsFileDateCreatedSet) mFileDateCreated = info.FileDateCreated;
                if (info.IsFileDateModifiedSet) mFileDateModified = info.FileDateModified;
                if (info.IsFileSizeSet) mFileSize = info.FileSize;
                if (info.IsFileMimeTypeSet) mFileType = info.FileMimeType;
                if (info.IsFileDirectorySet) mFileDirectory = info.FileDirectory;
                #endregion 

                #region Provided by ShellImageFileInfo, MagickImage                                
                if (info.IsCameraMakeSet) mCameraMake = info.CameraMake;
                if (info.IsCameraModelSet) mCameraModel = info.CameraModel;
                if (info.IsMediaDimensionsSet) mMediaDimensions = info.MediaDimensions;
                if (info.IsMediaDateTakenSet) mMediaDateTaken = info.MediaDateTaken;
                #endregion 

                #region Provided by MagickImage, Exiftool
                if (info.IsMediaTitleSet) mMediaTitle = info.MediaTitle;
                if (info.IsMediaDescriptionSet) mMediaDescription = info.MediaDescription;
                if (info.IsMediaCommentSet) mMediaComment = info.MediaComment;
                if (info.IsMediaAuthorSet) mMediaAuthor = info.MediaAuthor;
                if (info.IsMediaRatingSet) mMediaRating = info.MediaRating;
                #endregion 

                #region Provided by Exiftool
                if (info.IsMediaAlbumSet) mMediaAlbum = info.MediaAlbum;
                if (info.IsLocationNameSet) mLocationName = info.LocationName;
                if (info.IsLocationRegionStateSet) mLocationRegionState = info.LocationRegionState;
                if (info.IsLocationCitySet) mLocationCity = info.LocationCity;
                if (info.IsLocationCountrySet) mLocationCountry = info.LocationCountry;
                #endregion

                //isFileInfoDirty = false;
                //isDirty = false;
            }            
        }
        
        #endregion
    }
}
