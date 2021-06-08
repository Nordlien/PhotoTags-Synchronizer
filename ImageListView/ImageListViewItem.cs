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
        private DateTime mDateAccessed;
        private DateTime mDateCreated;
        private DateTime mDateModified;
        private string mFileType;
        private string mFileName;
        private string mFileDirectory;
        private long mFileSize;
        private Size mDimensions;
        private SizeF mResolution;
        
        // Exif tags
        
        private string mEquipmentModel;
        private DateTime mDateTaken;
        private string mCopyright;
        private string mExposureTime;
        private float mFNumber;
        private ushort mISOSpeed;
        private string mShutterSpeed;
        private string mAperture;
        

        //JTN: Added more column types
        private string mMediaAlbum;
        private string mMediaTitle;
        private string mMediaDescription;
        private string mMediaComment;
        private string mMediaAuthor;
        private byte mMediaRating;        
        private string mLocationName;
        private string mLocationRegionState;
        private string mLocationCity;
        private string mLocationCountry;
        

        // Used for virtual items
        internal bool isVirtualItem;
        internal object mVirtualItemKey;

        internal ImageListView.ImageListViewItemCollection owner;
        internal bool isDirty;
        internal bool isFileInfoDirty; //Added by JTN
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
        /// Gets the last access date of the image file represented by this item.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the last access date of the image file represented by this item.")]
        public DateTime DateAccessed { get { UpdateFileInfo(true); return mDateAccessed; } }
        
        /// <summary>
        /// Gets the creation date of the image file represented by this item.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the creation date of the image file represented by this item.")]
        public DateTime DateCreated { get { UpdateFileInfo(true); return mDateCreated; } }
        
        /// <summary>
        /// Gets the modification date of the image file represented by this item.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the modification date of the image file represented by this item.")]
        
        public DateTime DateModified { get { UpdateFileInfo(true); return mDateModified; } }
        /// <summary>
        /// Gets the shell type of the image file represented by this item.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the shell type of the image file represented by this item.")]
        public string FileType { get { UpdateFileInfo(false); return mFileType; } }
        
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
                        isFileInfoDirty = true;
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

        /*
            mFileType = fileInfo.TypeName;
            mDimensions = fileInfo.Dimensions;
            mResolution = fileInfo.Resolution;            
            mImageDescription = fileInfo.ImageDescription;
            mEquipmentModel = fileInfo.EquipmentModel;
            mDateTaken = infileInfofo.DateTaken;
            mArtist = fileInfo.Artist;
            mCopyright = fileInfo.Copyright;
            mExposureTime = fileInfo.ExposureTime;
            mFNumber = fileInfo.FNumber;
            mISOSpeed = fileInfo.ISOSpeed;
            mShutterSpeed = fileInfo.ShutterSpeed;
            mAperture = fileInfo.ApertureValue;
            mUserComment = fileInfo.UserComment;
            */

        /// <summary>
        /// Gets the path of the image fie represented by this item.
        /// </summary>        
        [Category("Data"), Browsable(false), Description("Gets the path of the image fie represented by this item.")]
        public string FileDirectory { get { UpdateFileInfo(true); return mFileDirectory; } }
        
        /// <summary>
        /// Gets file size in bytes.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets file size in bytes.")]
        public long FileSize { get { UpdateFileInfo(true); return mFileSize; } }
        
        /// <summary>
        /// Gets image dimensions.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets image dimensions.")]
        public Size Dimensions { get { UpdateFileInfo(false); return mDimensions; } }
        
        /// <summary>
        /// Gets image resolution in pixels per inch.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets image resolution in pixels per inch.")]
        public SizeF Resolution { get { UpdateFileInfo(false); return mResolution; } }
        
        /// <summary>
        /// Gets the camera model.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the camera model.")]
        public string EquipmentModel { get { UpdateFileInfo(false); return mEquipmentModel; } }
        
        /// <summary>
        /// Gets the date and time the image was taken.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the date and time the image was taken.")]
        public DateTime DateTaken { get { UpdateFileInfo(false); return mDateTaken; } }
        
        
        
        /// <summary>
        /// Gets image copyright information.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets image copyright information.")]
        public string Copyright { get { UpdateFileInfo(false); return mCopyright; } }
        
        /// <summary>
        /// Gets the exposure time in seconds.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the exposure time in seconds.")]
        public string ExposureTime { get { UpdateFileInfo(false); return mExposureTime; } }
        
        /// <summary>
        /// Gets the F number.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the F number.")]
        public float FNumber { get { UpdateFileInfo(false); return mFNumber; } }
        
        /// <summary>
        /// Gets the ISO speed.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the ISO speed.")]
        public ushort ISOSpeed { get { UpdateFileInfo(false); return mISOSpeed; } }
        
        /// <summary>
        /// Gets the shutter speed.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the shutter speed.")]
        public string ShutterSpeed { get { UpdateFileInfo(false); return mShutterSpeed; } }
        
        /// <summary>
        /// Gets the lens aperture value.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the lens aperture value.")]
        public string Aperture { get { UpdateFileInfo(false); return mAperture; } }

        //JTN: Added more column types
        
        /// <summary>
        /// Gets media album.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets media album.")]
        public string MediaAlbum { get { UpdateFileInfo(false); return mMediaAlbum; } }

        /// <summary>
        /// Gets media title.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets media title.")]
        public string MediaTitle { get { UpdateFileInfo(false); return mMediaTitle; } }
        
        /// <summary>
        /// Gets media description.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets media description.")]
        public string MediaDescription { get { UpdateFileInfo(false); return mMediaDescription; } }

        /// <summary>
        /// Gets user comments.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets user comments.")]
        public string MediaComment { get { UpdateFileInfo(false); return mMediaComment; } }

        /// <summary>
        /// Gets the name of the artist.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the name of the media file author.")]
        public string MediaAuthor { get { UpdateFileInfo(false); return mMediaAuthor; } }

        /// <summary>
        /// Gets the media rating.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the media rating.")]
        public byte MediaRating { get { UpdateFileInfo(false); return mMediaRating; } }

        /// <summary>
        /// Gets the media location name.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the name of the media location name.")]
        public string LocationName { get { UpdateFileInfo(false); return mLocationName; } }

        /// <summary>
        /// Gets the media location region or state.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the name of the media Llocation region or state.")]
        public string LocationRegionState { get { UpdateFileInfo(false); return mLocationRegionState; } }

        /// <summary>
        /// Gets the media location city.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the name of the media location city.")]
        public string LocationCity { get { UpdateFileInfo(false); return mLocationCity; } }

        /// <summary>
        /// Gets the media location country.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the name of the media location country.")]
        public string LocationCountry { get { UpdateFileInfo(false); return mLocationCountry; } }
        

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
            isFileInfoDirty = true;
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

            mDateAccessed = fileInfo.LastAccessTime;
            mDateCreated = fileInfo.CreationTime;
            mDateModified = fileInfo.LastWriteTime;
            mFileSize = fileInfo.Length;
            mFileDirectory = fileInfo.DirectoryName;

            isFileInfoDirty = false;
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
            mDimensions = dimensions;
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
            isFileInfoDirty = true;
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
                case ColumnType.DateAccessed:
                    if (DateAccessed == DateTime.MinValue) return "";
                    else return DateAccessed.ToString("g");
                case ColumnType.DateCreated:
                    if (DateCreated == DateTime.MinValue) return "";
                    else return DateCreated.ToString("g");
                case ColumnType.DateModified:
                    if (DateModified == DateTime.MinValue) return "";
                    else return DateModified.ToString("g");
                case ColumnType.FileFullPath:
                    return FileFullPath;
                case ColumnType.Name:
                    return Text;
                case ColumnType.FileDirectory:
                    return FileDirectory;
                case ColumnType.FileSize:
                    if (FileSize == 0) return "";
                    else return Utility.FormatSize(FileSize);
                case ColumnType.FileType:
                    return FileType;
                case ColumnType.Dimensions:
                    if (Dimensions == Size.Empty) return "";
                    else return string.Format("{0} x {1}", Dimensions.Width, Dimensions.Height);
                case ColumnType.Resolution:
                    if (Resolution == SizeF.Empty) return "";
                    else return string.Format("{0} x {1}", Resolution.Width, Resolution.Height);
                
                case ColumnType.EquipmentModel:
                    return EquipmentModel;
                case ColumnType.DateTaken:
                    if (DateTaken == DateTime.MinValue) return "";
                    else return DateTaken.ToString("g");                
                case ColumnType.Copyright:
                    return Copyright;
                case ColumnType.ExposureTime:
                    return ExposureTime;
                case ColumnType.FNumber:
                    if (FNumber == 0.0f) return "";
                    else return FNumber.ToString("f2");
                case ColumnType.ISOSpeed:
                    if (ISOSpeed == 0) return "";
                    else return ISOSpeed.ToString();
                case ColumnType.ShutterSpeed:
                    return ShutterSpeed;
                case ColumnType.Aperture:
                    return Aperture;
                

                case ColumnType.MediaAlbum:
                    return MediaAlbum;
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
        private void UpdateFileInfo(bool needCheckOnlyFileInfo)
        {
            if (needCheckOnlyFileInfo && !isFileInfoDirty) return;
            if (!needCheckOnlyFileInfo && !isDirty) return;

            if (isVirtualItem)
            {
                if (mImageListView != null)
                {
                    VirtualItemDetailsEventArgs e = new VirtualItemDetailsEventArgs(mVirtualItemKey);
                    mImageListView.RetrieveVirtualItemDetailsInternal(e);
                    UpdateDetailsInternal(e); //Virtual
                }
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
            isFileInfoDirty = false;
            isDirty = false;
        }
        /// <summary>
        /// Invoked by the worker thread to update item details.
        /// </summary>
        internal void UpdateDetailsInternal(Utility.ShellImageFileInfo info)
        {
            if (!isDirty && !isFileInfoDirty) return;
            if (info != null)
            {
                mDateAccessed = info.LastAccessTime;
                mDateCreated = info.CreationTime;
                mDateModified = info.LastWriteTime;
                mFileSize = info.Size;
                mFileType = info.TypeName;
                mFileDirectory = info.DirectoryName;
                mDimensions = info.Dimensions;
                mResolution = info.Resolution;
                // Exif tags
                mEquipmentModel = info.EquipmentModel;
                mDateTaken = info.DateTaken;
                mCopyright = info.Copyright;
                mExposureTime = info.ExposureTime;
                mFNumber = info.FNumber;
                mISOSpeed = info.ISOSpeed;
                mShutterSpeed = info.ShutterSpeed;
                mAperture = info.ApertureValue;
                
                //Exif extras
                mMediaAlbum = info.MediaAlbum;
                mMediaTitle = info.MediaTitle;
                mMediaDescription = info.MediaDescription;

                mMediaComment = info.MediaComment;
                mMediaAuthor = info.MediaAuthor;
                mMediaRating = info.MediaRating;
                mLocationName = info.LocationName;
                mLocationRegionState = info.LocationRegionState;
                mLocationCity = info.LocationCity;
                mLocationCountry = info.LocationCountry;

                isFileInfoDirty = false;
                isDirty = false;
            }            
        }
        /// <summary>
        /// Invoked by the worker thread to update item details.
        /// </summary>
        internal void UpdateDetailsInternal(VirtualItemDetailsEventArgs info)
        {
            if (!isDirty && !isFileInfoDirty) return;

            mDateAccessed = info.DateAccessed;
            mDateCreated = info.DateCreated;
            mDateModified = info.DateModified;
            mFileName = info.FileName;
            mFileSize = info.FileSize;
            mFileType = info.FileType;
            mFileDirectory = info.FileDirectory;
            mDimensions = info.Dimensions;
            mResolution = info.Resolution;
            // Exif tags
            mEquipmentModel = info.EquipmentModel;
            mDateTaken = info.DateTaken;
            mCopyright = info.Copyright;
            mExposureTime = info.ExposureTime;
            mFNumber = info.FNumber;
            mISOSpeed = info.ISOSpeed;
            mShutterSpeed = info.ShutterSpeed;
            mAperture = info.Aperture;
            
            //Exif extras
            mMediaAlbum = info.MediaAlbum;
            mMediaTitle = info.MediaTitle;
            mMediaDescription = info.MediaDescription;
            mMediaComment = info.MediaComment;
            mMediaAuthor = info.MediaAuthor;
            mMediaRating = info.MediaRating;
            mLocationName = info.LocationName;
            mLocationRegionState = info.LocationRegionState;
            mLocationCity = info.LocationCity;
            mLocationCountry = info.LocationCountry;
            
            isFileInfoDirty = false;
            isDirty = false;
        }
        #endregion
    }
}
