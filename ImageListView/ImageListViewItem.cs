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
using System.Collections;
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
        //JTN: MediaFileAttributes
        #region DateTime FileDate
        private DateTime mFileDate
        {
            get
            {
                UpdateFileInfo(FileDatePropertyStatus);
                if (mFileDateCreated != DateTime.MinValue && mFileDateModified != DateTime.MinValue) return (mFileDateCreated < mFileDateModified ? mFileDateCreated : mFileDateModified);
                else if (mFileDateCreated == DateTime.MinValue && mFileDateModified != DateTime.MinValue) return mFileDateModified;
                else if (mFileDateCreated != DateTime.MinValue && mFileDateModified == DateTime.MinValue) return mFileDateCreated;
                else return DateTime.MinValue;
            }
        }

        private PropertyStatus FileDatePropertyStatus
        {
            get 
            { 
                if (FileDateCreatedPropertyStatus == PropertyStatus.IsDirty || FileDateModifiedPropertyStatus == PropertyStatus.IsDirty) return PropertyStatus.IsDirty;
                if (FileDateCreatedPropertyStatus == PropertyStatus.Requested || FileDateModifiedPropertyStatus == PropertyStatus.Requested) return PropertyStatus.Requested;
                return PropertyStatus.IsSet;
            }
        }
        #endregion

        #region FileSmartDate
        private DateTime mmFileSmartDate;
        private DateTime mFileSmartDate
        {
            get { return mmFileSmartDate; }
            set { FileSmartDatePropertyStatus = PropertyStatus.IsSet; mmFileSmartDate = value; }
        }
        private PropertyStatus FileSmartDatePropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region FileDateCreated
        private DateTime mmFileDateCreated;
        private DateTime mFileDateCreated
        {
            get { return mmFileDateCreated; }
            set { FileDateCreatedPropertyStatus = PropertyStatus.IsSet; mmFileDateCreated = value; }
        }
        private PropertyStatus FileDateCreatedPropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region FileDateModified
        private DateTime mmFileDateModified;
        private DateTime mFileDateModified
        {
            get { return mmFileDateModified; }
            set { FileDateModifiedPropertyStatus = PropertyStatus.IsSet; mmFileDateModified = value; }
        }
        private PropertyStatus FileDateModifiedPropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region FileType
        private string mmFileType;
        private string mFileType
        {
            get { return mmFileType; }
            set { FileTypePropertyStatus = PropertyStatus.IsSet; mmFileType = value; }
        }
        private PropertyStatus FileTypePropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region FileName
        private string mmFileName; 
        private string mFileName
        {
            get { return mmFileName; }
            set { FileNamePropertyStatus = PropertyStatus.IsSet; mmFileName = value; }
        }
        private PropertyStatus FileNamePropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region FileDirectory
        private string mmFileDirectory;
        private string mFileDirectory
        {
            get { return mmFileDirectory; }
            set { FileDirectoryPropertyStatus = PropertyStatus.IsSet; mmFileDirectory = value; }
        }
        private PropertyStatus FileDirectoryPropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region FileSize
        private long mmFileSize;
        private long mFileSize
        {
            get { return mmFileSize; }
            set { FileSizePropertyStatus = PropertyStatus.IsSet; mmFileSize = value; }
        }
        private PropertyStatus FileSizePropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region FileStatus
        private FileStatus mmFileStatus = new FileStatus();
        private FileStatus mFileStatus
        {
            get { return mmFileStatus; }
            set { FileStatusPropertyStatus = PropertyStatus.IsSet; mmFileStatus = value; }
        }
        private PropertyStatus FileStatusPropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region MediaDimensions
        private Size mmMediaDimensions;
        private Size mMediaDimensions
        {
            get { return mmMediaDimensions; }
            set { MediaPropertyStatusDimensions = PropertyStatus.IsSet; mmMediaDimensions = value; }
        }
        private PropertyStatus MediaPropertyStatusDimensions = PropertyStatus.IsDirty;
        #endregion

        // Exif tags
        #region CameraMake
        private string mmCameraMake;
        private string mCameraMake
        {
            get { return mmCameraMake; }
            set { CameraMakePropertyStatus = PropertyStatus.IsSet; mmCameraMake = value; }
        }
        private PropertyStatus CameraMakePropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region CameraModel
        private string mmCameraModel;
        private string mCameraModel
        {
            get { return mmCameraModel; }
            set { CameraModelPropertyStatus = PropertyStatus.IsSet; mmCameraModel = value; }
        }
        private PropertyStatus CameraModelPropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region MediaDateTaken
        private DateTime mmMediaDateTaken;
        private DateTime mMediaDateTaken
        {
            get { return mmMediaDateTaken; }
            set { MediaDateTakenPropertyStatus = PropertyStatus.IsSet; mmMediaDateTaken = value; }
        }
        private PropertyStatus MediaDateTakenPropertyStatus = PropertyStatus.IsDirty;
        #endregion

        //JTN: Added more column types
        #region MediaAlbum
        private string mmMediaAlbum;
        private string mMediaAlbum
        {
            get { return mmMediaAlbum; }
            set { MediaAlbumPropertyStatus = PropertyStatus.IsSet; mmMediaAlbum = value; }
        }
        private PropertyStatus MediaAlbumPropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region MediaTitle
        private string mmMediaTitle;
        private string mMediaTitle
        {
            get { return mmMediaTitle; }
            set { MediaTitlePropertyStatus = PropertyStatus.IsSet; mmMediaTitle = value; }
        }
        private PropertyStatus MediaTitlePropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region MediaDescription
        private string mmMediaDescription;
        private string mMediaDescription
        {
            get { return mmMediaDescription; }
            set { MediaDescriptionPropertyStatus = PropertyStatus.IsSet; mmMediaDescription = value; }
        }
        private PropertyStatus MediaDescriptionPropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region MediaComment
        private string mmMediaComment;
        private string mMediaComment
        {
            get { return mmMediaComment; }
            set { MediaCommentPropertyStatus = PropertyStatus.IsSet; mmMediaComment = value; }
        }
        private PropertyStatus MediaCommentPropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region MediaAuthor
        private string mmMediaAuthor;
        private string mMediaAuthor
        {
            get { return mmMediaAuthor; }
            set { MediaAuthorPropertyStatus = PropertyStatus.IsSet; mmMediaAuthor = value; }
        }
        private PropertyStatus MediaAuthorPropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region MediaRating
        private byte mmMediaRating;
        private byte mMediaRating
        {
            get { return mmMediaRating; }
            set { MediaRatingPropertyStatus = PropertyStatus.IsSet; mmMediaRating = value; }
        }
        private PropertyStatus MediaRatingPropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region LocationDateTime
        private DateTime mmLocationDateTime;
        private DateTime mLocationDateTime
        {
            get { return mmLocationDateTime; }
            set { LocationDateTimePropertyStatus = PropertyStatus.IsSet; mmLocationDateTime = value; }
        }
        private PropertyStatus LocationDateTimePropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region LocationTimeZone
        private string mmLocationTimeZone;
        private string mLocationTimeZone
        {
            get { return mmLocationTimeZone; }
            set { LocationTimeZonePropertyStatus = PropertyStatus.IsSet; mmLocationTimeZone = value; }
        }
        private PropertyStatus LocationTimeZonePropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region LocationName
        private string mmLocationName;
        private string mLocationName
        {
            get { return mmLocationName; }
            set { LocationNamePropertyStatus = PropertyStatus.IsSet; mmLocationName = value; }
        }
        private PropertyStatus LocationNamePropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region LocationRegionState
        private string mmLocationRegionState;
        private string mLocationRegionState
        {
            get { return mmLocationRegionState; }
            set { LocationRegionStatePropertyStatus = PropertyStatus.IsSet; mmLocationRegionState = value; }
        }
        private PropertyStatus LocationRegionStatePropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region LocationCity
        private string mmLocationCity;
        private string mLocationCity
        {
            get { return mmLocationCity; }
            set { LocationCityPropertyStatus = PropertyStatus.IsSet; mmLocationCity = value; }
        }
        private PropertyStatus LocationCityPropertyStatus = PropertyStatus.IsDirty;
        #endregion

        #region LocationCountry
        private string mmLocationCountry;
        private string mLocationCountry
        {
            get { return mmLocationCountry; }
            set { LocationCountryPropertyStatus = PropertyStatus.IsSet; mmLocationCountry = value; }
        }
        private PropertyStatus LocationCountryPropertyStatus = PropertyStatus.IsDirty;
        #endregion

        // Used for virtual items
        internal bool isVirtualItem;
        internal object mVirtualItemKey;

        internal ImageListView.ImageListViewItemCollection owner;

        //JTN: Added more column types
        //JTN: MediaFileAttributes
        #region HasAnyPropertyThisStatus
        private bool HasAnyPropertyThisStatus(PropertyStatus propertyStatus)
        {
            if (FileSmartDatePropertyStatus == propertyStatus) return true;
            if (FileDateCreatedPropertyStatus == propertyStatus) return true;
            if (FileDateModifiedPropertyStatus == propertyStatus) return true;
            if (FileTypePropertyStatus == propertyStatus) return true;
            if (FileNamePropertyStatus == propertyStatus) return true;
            if (FileDirectoryPropertyStatus == propertyStatus) return true;
            if (FileSizePropertyStatus == propertyStatus) return true;
            if (FileStatusPropertyStatus == propertyStatus) return true;
            if (MediaPropertyStatusDimensions == propertyStatus) return true;
            if (CameraMakePropertyStatus == propertyStatus) return true;
            if (CameraModelPropertyStatus == propertyStatus) return true;
            if (MediaDateTakenPropertyStatus == propertyStatus) return true;
            if (MediaAlbumPropertyStatus == propertyStatus) return true;
            if (MediaTitlePropertyStatus == propertyStatus) return true;
            if (MediaDescriptionPropertyStatus == propertyStatus) return true;
            if (MediaCommentPropertyStatus == propertyStatus) return true;
            if (MediaAuthorPropertyStatus == propertyStatus) return true;
            if (MediaRatingPropertyStatus == propertyStatus) return true;
            if (LocationDateTimePropertyStatus == propertyStatus) return true; ;
            if (LocationTimeZonePropertyStatus == propertyStatus) return true;
            if (LocationNamePropertyStatus == propertyStatus) return true;
            if (LocationRegionStatePropertyStatus == propertyStatus) return true;
            if (LocationCityPropertyStatus == propertyStatus) return true;
            if (LocationCountryPropertyStatus == propertyStatus) return true;
            return false;
        }

        public bool IsItemDirty()
        {
            if (HasAnyPropertyThisStatus(PropertyStatus.IsDirty)) return true;
            return false;
        }
        #endregion

        //JTN: Added more column types
        //JTN: MediaFileAttributes
        #region SetPropertyStatusForAll
        private void SetPropertyStatusForAll (PropertyStatus propertyStatus)
        {
            //isDirtyFileDate = value; //ReadOnly
            FileSmartDatePropertyStatus = propertyStatus;
            FileDateCreatedPropertyStatus = propertyStatus;
            FileDateModifiedPropertyStatus = propertyStatus;
            FileTypePropertyStatus = propertyStatus;
            FileNamePropertyStatus = propertyStatus; //This can't become dirty
            FileDirectoryPropertyStatus = propertyStatus;
            FileSizePropertyStatus = propertyStatus;
            FileStatusPropertyStatus = propertyStatus;
            MediaPropertyStatusDimensions = propertyStatus;
            CameraMakePropertyStatus = propertyStatus;
            CameraModelPropertyStatus = propertyStatus;
            MediaDateTakenPropertyStatus = propertyStatus;
            MediaAlbumPropertyStatus = propertyStatus;
            MediaTitlePropertyStatus = propertyStatus;
            MediaDescriptionPropertyStatus = propertyStatus;
            MediaCommentPropertyStatus = propertyStatus;
            MediaAuthorPropertyStatus = propertyStatus;
            MediaRatingPropertyStatus = propertyStatus;
            LocationDateTimePropertyStatus = propertyStatus;
            LocationTimeZonePropertyStatus = propertyStatus;
            LocationNamePropertyStatus = propertyStatus;
            LocationRegionStatePropertyStatus = propertyStatus;
            LocationCityPropertyStatus = propertyStatus;
            LocationCountryPropertyStatus = propertyStatus;
        }
        #endregion

        private bool editing;
        #endregion

        #region Properties

        #region Color BackColor
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
        #endregion

        #region CacheState ThumbnailCacheState
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
        #endregion

        #region Color ForeColor
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
        #endregion

        #region internal Guid Guid
        /// <summary>
        /// Gets the unique identifier for this item.
        /// </summary>
        [Category("Behavior"), Browsable(false), Description("Gets the unique identifier for this item.")]
        internal Guid Guid { get { return mGuid; } private set { mGuid = value; } }
        #endregion

        #region object VirtualItemKey
        /// <summary>
        /// Gets the virtual item key associated with this item.
        /// Returns null if the item is not a virtual item.
        /// </summary>
        [Category("Behavior"), Browsable(false), Description("Gets the virtual item key associated with this item.")]
        public object VirtualItemKey { get { return mVirtualItemKey; } }
        #endregion

        #region ImageListView ImageListView
        /// <summary>
        /// Gets the ImageListView owning this item.
        /// </summary>
        [Category("Behavior"), Browsable(false), Description("Gets the ImageListView owning this item.")]
        public ImageListView ImageListView { get { return mImageListView; } private set { mImageListView = value; } }
        #endregion

        #region int Index
        /// <summary>
        /// Gets the index of the item.
        /// </summary>
        [Category("Behavior"), Browsable(false), Description("Gets the index of the item."), EditorBrowsable(EditorBrowsableState.Advanced)]
        public int Index { get { return mIndex; } }
        #endregion

        #region bool Selected
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
        #endregion

        #region object Tag
        /// <summary>
        /// Gets or sets the user-defined data associated with the item.
        /// </summary>
        [Category("Data"), Browsable(true), Description("Gets or sets the user-defined data associated with the item.")]
        public object Tag { get; set; }
        #endregion

        #region string Text
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
        #endregion

        #region Image ThumbnailImage
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
        #endregion

        #region int ZOrder
        /// <summary>
        /// Gets or sets the draw order of the item.
        /// </summary>
        [Category("Appearance"), Browsable(true), Description("Gets or sets the draw order of the item."), DefaultValue(0)]
        public int ZOrder { get { return mZOrder; } set { mZOrder = value; } }
        #endregion

        //JTN: Added more column types
        //JTN: MediaFileAttributes
        #region DateTime Date
        /// <summary>
        /// Gets the creation date of the image file represented by this item.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the date of the image file represented by this item.")]
        public DateTime Date { get { UpdateFileInfo(FileDatePropertyStatus); return mFileDate; } }
        #endregion

        #region DateTime SmartDate
        /// <summary>
        /// Gets the creation date of the image file represented by this item.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the creation date of the image file represented by this item.")]
        public DateTime SmartDate { get { UpdateFileInfo(FileSmartDatePropertyStatus); return mFileSmartDate; } }
        #endregion

        #region DateTime DateCreated
        /// <summary>
        /// Gets the creation date of the image file represented by this item.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the creation date of the image file represented by this item.")]
        public DateTime DateCreated { get { UpdateFileInfo(FileDateCreatedPropertyStatus); return mFileDateCreated; } }
        #endregion

        #region DateTime DateModified
        /// <summary>
        /// Gets the modification date of the image file represented by this item.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the modification date of the image file represented by this item.")]        
        public DateTime DateModified { get { UpdateFileInfo(FileDateModifiedPropertyStatus); return mFileDateModified; } }
        #endregion

        #region string FileType
        /// <summary>
        /// Gets the shell type of the image file represented by this item.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the shell type of the image file represented by this item.")]
        public string FileType { get { UpdateFileInfo(FileTypePropertyStatus); return mFileType; } }
        #endregion

        #region string FileFullPath
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
                        SetPropertyStatusForAll(PropertyStatus.IsDirty);
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
        #endregion

        #region string FileDirectory
        /// <summary>
        /// Gets the path of the image fie represented by this item.
        /// </summary>        
        [Category("Data"), Browsable(false), Description("Gets the path of the image fie represented by this item.")]
        public string FileDirectory { get { UpdateFileInfo(FileDirectoryPropertyStatus); return mFileDirectory; } }
        #endregion

        #region long FileSize
        /// <summary>
        /// Gets file size in bytes.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets file size in bytes.")]
        public long FileSize { get { UpdateFileInfo(FileSizePropertyStatus); return mFileSize; } }
        #endregion

        #region FileStatus
        /// <summary> FileStatus
        /// Gets or sets the text file status associated with this item. 
        /// </summary>
        [Category("Appearance"), Browsable(true), Description("Gets or sets the file status associated with this item.")]
        public FileStatus FileStatus 
        {  
            get { 
                UpdateFileInfo(FileSizePropertyStatus); 
                return mFileStatus; 
            } 
            set
            {
                mFileStatus = value;
                FileSizePropertyStatus = PropertyStatus.IsSet;
            } 
        }
        #endregion

        #region Size Dimensions
        /// <summary>
        /// Gets image dimensions.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets image dimensions.")]
        public Size Dimensions { get { UpdateFileInfo(MediaPropertyStatusDimensions); return mMediaDimensions; } }
        #endregion

        #region string CameraMake
        /// <summary>
        /// Gets the camera model.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the camera make.")]
        public string CameraMake { get { UpdateFileInfo(CameraMakePropertyStatus); return mCameraMake; } }
        #endregion

        #region string CameraModel
        /// <summary>
        /// Gets the camera model.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the camera model.")]
        public string CameraModel { get { UpdateFileInfo(CameraModelPropertyStatus); return mCameraModel; } }
        #endregion

        #region DateTime DateTaken
        /// <summary>
        /// Gets the date and time the image was taken.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the date and time the image was taken.")]
        public DateTime DateTaken { get { UpdateFileInfo(MediaDateTakenPropertyStatus); return mMediaDateTaken; } }
        #endregion

        //JTN: Added more column types
        #region string MediaAlbum
        /// <summary>
        /// Gets media album.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets media album.")]
        public string MediaAlbum { get { UpdateFileInfo(MediaAlbumPropertyStatus); return mMediaAlbum; } }
        #endregion

        #region string MediaTitle
        /// <summary>
        /// Gets media title.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets media title.")]
        public string MediaTitle { get { UpdateFileInfo(MediaTitlePropertyStatus); return mMediaTitle; } }
        #endregion

        #region string MediaDescription
        /// <summary>
        /// Gets media description.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets media description.")]
        public string MediaDescription { get { UpdateFileInfo(MediaDescriptionPropertyStatus); return mMediaDescription; } }
        #endregion

        #region string MediaComment
        /// <summary>
        /// Gets user comments.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets user comments.")]
        public string MediaComment { get { UpdateFileInfo(MediaCommentPropertyStatus); return mMediaComment; } }
        #endregion

        #region string MediaAuthor
        /// <summary>
        /// Gets the name of the artist.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the name of the media file author.")]
        public string MediaAuthor { get { UpdateFileInfo(MediaAuthorPropertyStatus); return mMediaAuthor; } }
        #endregion

        #region byte MediaRating
        /// <summary>
        /// Gets the media rating.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the media rating.")]
        public byte MediaRating { get { UpdateFileInfo(MediaRatingPropertyStatus); return mMediaRating; } }
        #endregion

        #region DateTime LocationDateTime
        /// <summary>
        /// Gets the date and time the image was taken.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the UTC date and time the image was taken.")]
        public DateTime LocationDateTime { get { UpdateFileInfo(LocationDateTimePropertyStatus); return mLocationDateTime; } }
        #endregion

        #region string LocationTimeZone
        /// <summary>
        /// Gets the media location time zone.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the name of the media location time zone.")]
        public string LocationTimeZone { get { UpdateFileInfo(LocationTimeZonePropertyStatus); return mLocationTimeZone; } }
        #endregion

        #region string LocationName
        /// <summary>
        /// Gets the media location name.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the name of the media location name.")]
        public string LocationName { get { UpdateFileInfo(LocationNamePropertyStatus); return mLocationName; } }
        #endregion

        #region string LocationRegionState
        /// <summary>
        /// Gets the media location region or state.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the name of the media Llocation region or state.")]
        public string LocationRegionState { get { UpdateFileInfo(LocationRegionStatePropertyStatus); return mLocationRegionState; } }
        #endregion

        #region string LocationCity
        /// <summary>
        /// Gets the media location city.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the name of the media location city.")]
        public string LocationCity { get { UpdateFileInfo(LocationCityPropertyStatus); return mLocationCity; } }
        #endregion

        #region string LocationCountry
        /// <summary>
        /// Gets the media location country.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the name of the media location country.")]
        public string LocationCountry { get { UpdateFileInfo(LocationCountryPropertyStatus); return mLocationCountry; } }
        #endregion

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

            SetPropertyStatusForAll(PropertyStatus.IsDirty); ;
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

        #region BeginEdit()
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

            UpdateFileInfo(PropertyStatus.IsSet);
            mImageListView.cacheManager.BeginItemEdit(mGuid, mFileName); //if Thumbnail not exist it will trigger -> Image img = RetrieveImageFromExternaThenFromFile(filename))
            
            editing = true;
        }
        #endregion

        #region EndEdit(bool update)
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
        #endregion

        #region EndEdit()
        /// <summary>
        /// Ends editing and updates the item.
        /// </summary>
        public void EndEdit()
        {
            EndEdit(true);
        }
        #endregion

        #region Update()
        /// <summary>
        /// Updates item thumbnail and item details.
        /// </summary>
        public void Update()
        {

            Dirty();
            if (mImageListView != null)
            {
                mImageListView.cacheManager.Remove(mGuid, true);
                mImageListView.itemCacheManager.Add(this);
                //mImageListView.Refresh();
                mImageListView.Invalidate();
            }
        }
        #endregion

        public void Invalidate()
        {
            mImageListView.Invalidate();
        }

        #region Dirty()
        /// <summary>
        /// Updates item thumbnail and item details.
        /// </summary>
        public void Dirty()
        {
            SetPropertyStatusForAll(PropertyStatus.IsDirty);
        }
        #endregion

        #region GetSubItemText(ColumnType type)
        /// <summary>
        /// Returns the sub item item text corresponding to the specified column type.
        /// </summary>
        /// <param name="type">The type of information to return.</param>
        /// <returns>Formatted text for the given column type.</returns>
        public string GetSubItemText(ColumnType type)
        {
            //JTN: MediaFileAttributes
            //JTN: Added more column types
            #region Return text depend of type
            switch (type)
            {
                case ColumnType.FileDate:
                    if (Date == DateTime.MinValue) return "";
                    else return Date.ToString("yyyy-MM-dd HH:mm:ss");
                case ColumnType.FileSmartDate:
                    if (SmartDate == DateTime.MinValue) return "";
                    else return SmartDate.ToString("yyyy-MM-dd HH:mm:ss");
                case ColumnType.FileDateCreated:
                    if (DateCreated == DateTime.MinValue) return "";
                    else return DateCreated.ToString("yyyy-MM-dd HH:mm:ss");
                case ColumnType.FileDateModified:
                    if (DateModified == DateTime.MinValue) return "";
                    else return DateModified.ToString("yyyy-MM-dd HH:mm:ss");
                case ColumnType.MediaDateTaken:
                    if (DateTaken == DateTime.MinValue) return "";
                    else return DateTaken.ToString("yyyy-MM-dd HH:mm:ss"); //DateTaken.ToString("g"); 
                case ColumnType.LocationDateTime:
                    if (LocationDateTime == DateTime.MinValue) return "";
                    else return LocationDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "Z"; 

                case ColumnType.FileFullPath:
                    return FileFullPath;
                case ColumnType.FileName:
                    return Text;
                case ColumnType.FileDirectory:
                    return FileDirectory;
                case ColumnType.FileSize:
                    if (FileSize <= 0) return "";
                    else return Utility.FormatSize(FileSize);
                case ColumnType.FileType:
                    return FileType;
                case ColumnType.MediaDimensions:
                    if (Dimensions == Size.Empty) return "";
                    else return string.Format("{0} x {1}", Dimensions.Width, Dimensions.Height);
                case ColumnType.FileStatus:
                    string fileStatusText = "";
                    
                    #region Exists
                    if (!FileStatus.FileExists) fileStatusText = fileStatusText + (string.IsNullOrWhiteSpace(fileStatusText) ? "" : ",") + "Not exist";
                    if (FileStatus.FileErrorOrInaccessible) fileStatusText = fileStatusText + (string.IsNullOrWhiteSpace(fileStatusText) ? "" : ",") + "Inaccessible";
                    if (FileStatus.IsDirty) fileStatusText = fileStatusText + (string.IsNullOrWhiteSpace(fileStatusText) ? "" : ",") + "Checking...";
                    #endregion

                    #region Located
                    if (FileStatus.IsInCloudOrVirtualOrOffline)
                        fileStatusText = fileStatusText + (string.IsNullOrWhiteSpace(fileStatusText) ? "" : ",") + "Offline (" +
                        (FileStatus.IsInCloud ? "C" : "") +
                        (FileStatus.IsVirtual ? "V" : "") +
                        (FileStatus.IsOffline ? "O" : "") + ")";
                    #endregion

                    #region Access
                    if (!FileStatus.IsInCloudOrVirtualOrOffline)
                    {
                        if (FileStatus.IsFileLockedReadAndWrite) fileStatusText = fileStatusText + (string.IsNullOrWhiteSpace(fileStatusText) ? "" : ",") + "Locked RW";
                        if (FileStatus.IsFileLockedForRead) fileStatusText = fileStatusText + (string.IsNullOrWhiteSpace(fileStatusText) ? "" : ",") + "Locked R";
                    }
                    if (FileStatus.IsReadOnly) fileStatusText = fileStatusText + (string.IsNullOrWhiteSpace(fileStatusText) ? "" : ",") + "ReadOnly";                    
                    #endregion

                    

                    #region Processes
                    switch (FileStatus.FileProcessStatus)
                    {
                        case FileProcessStatus.WaitAction:
                            //fileStatusText = fileStatusText + (string.IsNullOrWhiteSpace(fileStatusText) ? "" : ",") + "Waiting action";
                            break;
                        case FileProcessStatus.InExiftoolReadQueue:
                            break;
                        case FileProcessStatus.WaitOfflineBecomeLocal:
                            fileStatusText = fileStatusText + (string.IsNullOrWhiteSpace(fileStatusText) ? "" : ",") + "Downloading";
                            break;
                        case FileProcessStatus.ExiftoolProcessing:
                            fileStatusText = fileStatusText + (string.IsNullOrWhiteSpace(fileStatusText) ? "" : ",") + "Exiftool";
                            break;
                        case FileProcessStatus.FileInaccessible:
                            fileStatusText = fileStatusText + (string.IsNullOrWhiteSpace(fileStatusText) ? "" : ",") + "Inaccessible";
                            break;
                        case FileProcessStatus.DoNotUpdate:
                            //fileStatusText = fileStatusText + (string.IsNullOrWhiteSpace(fileStatusText) ? "" : ",") + "DoNotUpdate";
                            break;
                        case FileProcessStatus.ExiftoolWillNotProcessingFileInCloud:
                            fileStatusText = fileStatusText + (string.IsNullOrWhiteSpace(fileStatusText) ? "" : ",") + "Skipped offline file";
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    #endregion 
                    
                    if (string.IsNullOrWhiteSpace(fileStatusText)) fileStatusText = "Normal";
                    return fileStatusText;
                case ColumnType.LocationTimeZone:
                    return LocationTimeZone;
                case ColumnType.CameraMake:
                    return CameraMake;
                case ColumnType.CameraModel:
                    return CameraModel;

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
            #endregion
        }
        #endregion

        #endregion

        #region Helper Methods

        #region UpdateFileInfo(PropertyStatus propertyStatus)
        /// <summary>
        /// Updates file info for the image file represented by this item.
        /// </summary>
        private void UpdateFileInfo(PropertyStatus propertyStatus)
        {
            if (propertyStatus == PropertyStatus.IsSet) return; //IS NOT DIRTY
            if (propertyStatus == PropertyStatus.Requested) return; //IS NOT DIRTY
            
            if (!isVirtualItem)
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
        }
        #endregion

        public void UpdateDetails(Utility.ShellImageFileInfo info)
        {
            UpdateDetailsInternal(info);
        }

        //JTN: MediaFileAttribute
        #region UpdateDetailsInternal(Utility.ShellImageFileInfo info)
        /// <summary>
        /// Invoked by the worker thread to update item details.
        /// </summary>
        internal void UpdateDetailsInternal(Utility.ShellImageFileInfo info)
        {
            if (info != null)
            {
                #region Provided by FileInfo  
                if (info.FileStatusPropertyStatus == PropertyStatus.IsSet) mFileStatus = info.FileStatus;
                if (info.FileSmartDatePropertyStatus == PropertyStatus.IsSet) mFileSmartDate = info.FileSmartDate;
                if (info.FileDateCreatedPropertyStatus == PropertyStatus.IsSet) mFileDateCreated = info.FileDateCreated;
                if (info.FileDateModifiedPropertyStatus == PropertyStatus.IsSet) mFileDateModified = info.FileDateModified;

                if (info.FileSizePropertyStatus == PropertyStatus.IsSet) mFileSize = info.FileSize;
                if (info.FileStatusPropertyStatus == PropertyStatus.IsSet) mFileStatus = info.FileStatus;
                if (info.FileMimeTypePropertyStatus == PropertyStatus.IsSet) mFileType = info.FileMimeType;
                if (info.FileDirectoryPropertyStatus == PropertyStatus.IsSet) mFileDirectory = info.FileDirectory;
                #endregion

                #region Provided by ShellImageFileInfo, MagickImage                                
                if (info.CameraMakePropertyStatus == PropertyStatus.IsSet) mCameraMake = info.CameraMake;
                if (info.CameraModelPropertyStatus == PropertyStatus.IsSet) mCameraModel = info.CameraModel;
                if (info.MediaDimensionsPropertyStatus == PropertyStatus.IsSet) mMediaDimensions = info.MediaDimensions;
                if (info.MediaDateTakenPropertyStatus == PropertyStatus.IsSet) mMediaDateTaken = info.MediaDateTaken;
                #endregion

                #region Provided by MagickImage, Exiftool
                if (info.MediaTitlePropertyStatus == PropertyStatus.IsSet) mMediaTitle = info.MediaTitle;
                if (info.MediaDescriptionPropertyStatus == PropertyStatus.IsSet) mMediaDescription = info.MediaDescription;
                if (info.MediaCommentPropertyStatus == PropertyStatus.IsSet) mMediaComment = info.MediaComment;
                if (info.MediaAuthorPropertyStatus == PropertyStatus.IsSet) mMediaAuthor = info.MediaAuthor;
                if (info.MediaRatingPropertyStatus == PropertyStatus.IsSet) mMediaRating = info.MediaRating;
                #endregion

                #region Provided by Exiftool
                if (info.MediaAlbumPropertyStatus == PropertyStatus.IsSet) mMediaAlbum = info.MediaAlbum;
                if (info.LocationDateTimePropertyStatus == PropertyStatus.IsSet) mLocationDateTime = info.LocationDateTime;
                if (info.LocationTimeZonePropertyStatus == PropertyStatus.IsSet) mLocationTimeZone = info.LocationTimeZone;
                if (info.LocationNamePropertyStatus == PropertyStatus.IsSet) mLocationName = info.LocationName;
                if (info.LocationRegionStatePropertyStatus == PropertyStatus.IsSet) mLocationRegionState = info.LocationRegionState;
                if (info.LocationCityPropertyStatus == PropertyStatus.IsSet) mLocationCity = info.LocationCity;
                if (info.LocationCountryPropertyStatus == PropertyStatus.IsSet) mLocationCountry = info.LocationCountry;
                #endregion
            }        
        }
        #endregion 

        #endregion
    }
}
