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
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections.Generic;


namespace Manina.Windows.Forms
{
    #region Event Delegates
    /// <summary>
    /// Represents the method that will handle the DropFiles event. 
    /// </summary>
    /// <param name="sender">The ImageListView object that is the source of the event.</param>
    /// <param name="e">A DropFileEventArgs that contains event data.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate void DropFilesEventHandler(object sender, DropFileEventArgs e);
    /// <summary>
    /// Represents the method that will handle the ColumnClick event. 
    /// </summary>
    /// <param name="sender">The ImageListView object that is the source of the event.</param>
    /// <param name="e">A ColumnClickEventArgs that contains event data.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate void ColumnClickEventHandler(object sender, ColumnClickEventArgs e);
    /// <summary>
    /// Represents the method that will handle the ColumnHover event. 
    /// </summary>
    /// <param name="sender">The ImageListView object that is the source of the event.</param>
    /// <param name="e">A ColumnHoverEventArgs that contains event data.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate void ColumnHoverEventHandler(object sender, ColumnHoverEventArgs e);
    /// <summary>
    /// Represents the method that will handle the ColumnWidthChanged event. 
    /// </summary>
    /// <param name="sender">The ImageListView object that is the source of the event.</param>
    /// <param name="e">A ColumnEventArgs that contains event data.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate void ColumnWidthChangedEventHandler(object sender, ColumnEventArgs e);
    /// <summary>
    /// Represents the method that will handle the ItemClick event. 
    /// </summary>
    /// <param name="sender">The ImageListView object that is the source of the event.</param>
    /// <param name="e">A ItemClickEventArgs that contains event data.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate void ItemClickEventHandler(object sender, ItemClickEventArgs e);
    /// <summary>
    /// Represents the method that will handle the ItemHover event. 
    /// </summary>
    /// <param name="sender">The ImageListView object that is the source of the event.</param>
    /// <param name="e">A ItemHoverEventArgs that contains event data.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate void ItemHoverEventHandler(object sender, ItemHoverEventArgs e);
    /// <summary>
    /// Represents the method that will handle the ItemDoubleClick event. 
    /// </summary>
    /// <param name="sender">The ImageListView object that is the source of the event.</param>
    /// <param name="e">A ItemClickEventArgs that contains event data.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate void ItemDoubleClickEventHandler(object sender, ItemClickEventArgs e);
    /// <summary>
    /// Represents the method that will handle the ThumbnailCaching event. 
    /// </summary>
    /// <param name="sender">The ImageListView object that is the source of the event.</param>
    /// <param name="e">A ItemEventArgs that contains event data.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate void ThumbnailCachingEventHandler(object sender, ItemEventArgs e);

    //JTN Added
    /// <summary>
    /// Represents the method that will handle the RetrieveItemImage event. 
    /// </summary>
    /// <param name="sender">The ImageListView object that is the source of the event.</param>
    /// <param name="e">A RetrieveItemImageEventArgs that contains event data.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate void RetrieveItemImageEventHandler(object sender, RetrieveItemImageEventArgs e);

    //JTN Added
    /// <summary>
    /// Represents the method that will handle the RetrieveItemThumbnail event. 
    /// </summary>
    /// <param name="sender">The ImageListView object that is the source of the event.</param>
    /// <param name="e">A RetrieveItemThumbnailEventArgs that contains event data.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate void RetrieveItemThumbnailEventHandler(object sender, RetrieveItemThumbnailEventArgs e);

    //JTN Added RetrieveItemMetadataDetails
    /// <summary>
    /// Represents the method that will handle the RetrieveItemMetadataDetails event. 
    /// </summary>
    /// <param name="sender">The ImageListView object that is the source of the event.</param>
    /// <param name="e">A RetrieveItemMetadataDetailsEventArgs that contains event data.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate void RetrieveItemMetadataDetailsEventHandler(object sender, RetrieveItemMetadataDetailsEventArgs e);

    /// <summary>
    /// Represents the method that will handle the ThumbnailCached event. 
    /// </summary>
    /// <param name="sender">The ImageListView object that is the source of the event.</param>
    /// <param name="e">A ThumbnailCachedEventArgs that contains event data.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate void ThumbnailCachedEventHandler(object sender, ThumbnailCachedEventArgs e);

    /// <summary>
    /// Represents the method that will handle the RetrieveVirtualItemThumbnail event. 
    /// </summary>
    /// <param name="sender">The ImageListView object that is the source of the event.</param>
    /// <param name="e">A VirtualItemThumbnailEventArgs that contains event data.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate void RetrieveVirtualItemThumbnailEventHandler(object sender, VirtualItemThumbnailEventArgs e);
    /// <summary>
    /// Represents the method that will handle the RetrieveVirtualItemImage event. 
    /// </summary>
    /// <param name="sender">The ImageListView object that is the source of the event.</param>
    /// <param name="e">A VirtualItemImageEventArgs that contains event data.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate void RetrieveVirtualItemImageEventHandler(object sender, VirtualItemImageEventArgs e);
    /// <summary>
    /// Represents the method that will handle the RetrieveVirtualItemDetails event. 
    /// </summary>
    /// <param name="sender">The ImageListView object that is the source of the event.</param>
    /// <param name="e">A VirtualItemDetailsEventArgs that contains event data.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate void RetrieveVirtualItemDetailsEventHandler(object sender, VirtualItemDetailsEventArgs e);
    #endregion

    #region Internal Delegates
    /// <summary>
    /// Updates item details.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal delegate void UpdateItemDetailsDelegateInternal(ImageListViewItem item, Utility.ShellImageFileInfo info);
    /// <summary>
    /// Updates item details.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal delegate void UpdateVirtualItemDetailsDelegateInternal(ImageListViewItem item, VirtualItemDetailsEventArgs e);
    /// <summary>
    /// Determines if the given item is visible.
    /// </summary>
    /// <param name="guid">The guid of the item to check visibility.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal delegate bool CheckItemVisibleDelegateInternal(Guid guid);
    /// <summary>
    /// Gets the guids of visible items.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal delegate Dictionary<Guid, bool> GetVisibleItemsDelegateInternal();
    /// <summary>
    /// Refreshes the owner control.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal delegate void RefreshDelegateInternal();

    //JTN Changed added parameters
    /// <summary>
    /// Represents the method that will handle the ThumbnailCached event. 
    /// </summary>
    /// <param name="guid">The guid of the item whose thumbnail is cached.</param>
    /// <param name="thumbnail">Requested thumbnail, not size Thumbnail in Item Thumbnail.</param>
    /// <param name="error">Determimes whether an error occurred during thumbnail extraction.</param>
    /// <param name="requestedSize">Requested size of thumbnail, not size after adapeted size.</param>
    /// <param name="wasThumbnailReadFromFile">Determimes whether an thumbnail was read from File or other sources.</param>
    /// <param name="didThumbnailReadErrorOccur">Determimes whether an thumbnail is Error icon or not.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal delegate void ThumbnailCachedEventHandlerInternal(Guid guid, Image thumbnail, Size requestedSize, bool error, bool wasThumbnailReadFromFile, bool didThumbnailReadErrorOccur);
    #endregion

    #region Event Arguments
    /// <summary>
    /// Represents the event arguments for column related events.
    /// </summary>
    [Serializable, ComVisible(true)]
    public class DropFileEventArgs
    {
        private bool mCancel;
        private int mIndex;
        private string[] mFileNames;

        /// <summary>
        /// Gets or sets whether default event code will be processed.
        /// When set to true, the control will automatically insert the new items.
        /// Otherwise, the control will not process the dropped files.
        /// </summary>
        public bool Cancel { get { return mCancel; } set { mCancel = value; } }
        /// <summary>
        /// Gets the position of the insertion caret.
        /// This determines where the new items will be inserted.
        /// </summary>
        public int Index { get { return mIndex; } }
        /// <summary>
        /// Gets the array of filenames droppped on the control.
        /// </summary>
        public string[] FileNames { get { return mFileNames; } }

        /// <summary>
        /// Initializes a new instance of the DropFileEventArgs class.
        /// </summary>
        /// <param name="index">The position of the insertion caret.</param>
        /// <param name="fileNames">The array of filenames droppped on the control.</param>
        public DropFileEventArgs(int index, string[] fileNames)
        {
            mCancel = false;
            mIndex = index;
            mFileNames = fileNames;
        }
    }
    /// <summary>
    /// Represents the event arguments for column related events.
    /// </summary>
    [Serializable, ComVisible(true)]
    public class ColumnEventArgs
    {
        private ImageListView.ImageListViewColumnHeader mColumn;

        /// <summary>
        /// Gets the ImageListViewColumnHeader that is the target of the event.
        /// </summary>
        public ImageListView.ImageListViewColumnHeader Column { get { return mColumn; } }

        /// <summary>
        /// Initializes a new instance of the ColumnEventArgs class.
        /// </summary>
        /// <param name="column">The column that is the target of this event.</param>
        public ColumnEventArgs(ImageListView.ImageListViewColumnHeader column)
        {
            mColumn = column;
        }
    }
    /// <summary>
    /// Represents the event arguments for column click related events.
    /// </summary>
    [Serializable, ComVisible(true)]
    public class ColumnClickEventArgs
    {
        private ImageListView.ImageListViewColumnHeader mColumn;
        private Point mLocation;
        private MouseButtons mButtons;

        /// <summary>
        /// Gets the ImageListViewColumnHeader that is the target of the event.
        /// </summary>
        public ImageListView.ImageListViewColumnHeader Column { get { return mColumn; } }
        /// <summary>
        /// Gets the coordinates of the cursor.
        /// </summary>
        public Point Location { get { return mLocation; } }
        /// <summary>
        /// Gets the x-coordinates of the cursor.
        /// </summary>
        public int X { get { return mLocation.X; } }
        /// <summary>
        /// Gets the y-coordinates of the cursor.
        /// </summary>
        public int Y { get { return mLocation.Y; } }
        /// <summary>
        /// Gets the state of the mouse buttons.
        /// </summary>
        public MouseButtons Buttons { get { return mButtons; } }

        /// <summary>
        /// Initializes a new instance of the ColumnClickEventArgs class.
        /// </summary>
        /// <param name="column">The column that is the target of this event.</param>
        /// <param name="location">The location of the mouse.</param>
        /// <param name="buttons">One of the System.Windows.Forms.MouseButtons values 
        /// indicating which mouse button was pressed.</param>
        public ColumnClickEventArgs(ImageListView.ImageListViewColumnHeader column, Point location, MouseButtons buttons)
        {
            mColumn = column;
            mLocation = location;
            mButtons = buttons;
        }
    }
    /// <summary>
    /// Represents the event arguments for column hover related events.
    /// </summary>
    [Serializable, ComVisible(true)]
    public class ColumnHoverEventArgs
    {
        private ImageListView.ImageListViewColumnHeader mPreviousColumn;
        private ImageListView.ImageListViewColumnHeader mColumn;

        /// <summary>
        /// Gets the ImageListViewColumnHeader that was previously hovered.
        /// Returns null if there was no previously hovered column.
        /// </summary>
        public ImageListView.ImageListViewColumnHeader PreviousColumn { get { return mPreviousColumn; } }
        /// <summary>
        /// Gets the currently hovered ImageListViewColumnHeader.
        /// Returns null if there is no hovered column.
        /// </summary>
        public ImageListView.ImageListViewColumnHeader Column { get { return mColumn; } }

        /// <summary>
        /// Initializes a new instance of the ColumnHoverEventArgs class.
        /// </summary>
        /// <param name="column">The currently hovered column.</param>
        /// <param name="previousColumn">The previously hovered column.</param>
        public ColumnHoverEventArgs(ImageListView.ImageListViewColumnHeader column, ImageListView.ImageListViewColumnHeader previousColumn)
        {
            mColumn = column;
            mPreviousColumn = previousColumn;
        }
    }
    /// <summary>
    /// Represents the event arguments for item related events.
    /// </summary>
    [Serializable, ComVisible(true)]
    public class ItemEventArgs
    {
        private ImageListViewItem mItem;

        /// <summary>
        /// Gets the ImageListViewItem that is the target of the event.
        /// </summary>
        public ImageListViewItem Item { get { return mItem; } }

        /// <summary>
        /// Initializes a new instance of the ItemEventArgs class.
        /// </summary>
        /// <param name="item">The item that is the target of this event.</param>
        public ItemEventArgs(ImageListViewItem item)
        {
            mItem = item;
        }
    }
    /// <summary>
    /// Represents the event arguments for item click related events.
    /// </summary>
    [Serializable, ComVisible(true)]
    public class ItemClickEventArgs
    {
        private ImageListViewItem mItem;
        private Point mLocation;
        private MouseButtons mButtons;

        /// <summary>
        /// Gets the ImageListViewItem that is the target of the event.
        /// </summary>
        public ImageListViewItem Item { get { return mItem; } }
        /// <summary>
        /// Gets the coordinates of the cursor.
        /// </summary>
        public Point Location { get { return mLocation; } }
        /// <summary>
        /// Gets the x-coordinates of the cursor.
        /// </summary>
        public int X { get { return mLocation.X; } }
        /// <summary>
        /// Gets the y-coordinates of the cursor.
        /// </summary>
        public int Y { get { return mLocation.Y; } }
        /// <summary>
        /// Gets the state of the mouse buttons.
        /// </summary>
        public MouseButtons Buttons { get { return mButtons; } }

        /// <summary>
        /// Initializes a new instance of the ItemClickEventArgs class.
        /// </summary>
        /// <param name="item">The item that is the target of this event.</param>
        /// <param name="location">The location of the mouse.</param>
        /// <param name="buttons">One of the System.Windows.Forms.MouseButtons values 
        /// indicating which mouse button was pressed.</param>
        public ItemClickEventArgs(ImageListViewItem item, Point location, MouseButtons buttons)
        {
            mItem = item;
            mLocation = location;
            mButtons = buttons;
        }
    }
    /// <summary>
    /// Represents the event arguments for item hover related events.
    /// </summary>
    [Serializable, ComVisible(true)]
    public class ItemHoverEventArgs
    {
        private ImageListViewItem mPreviousItem;
        private ImageListViewItem mItem;

        /// <summary>
        /// Gets the ImageListViewItem that was previously hovered.
        /// Returns null if there was no previously hovered item.
        /// </summary>
        public ImageListViewItem PreviousItem { get { return mPreviousItem; } }
        /// <summary>
        /// Gets the currently hovered ImageListViewItem.
        /// Returns null if there is no hovered item.
        /// </summary>
        public ImageListViewItem Item { get { return mItem; } }

        /// <summary>
        /// Initializes a new instance of the ItemEventArgs class.
        /// </summary>
        /// <param name="item">The currently hovered item.</param>
        /// <param name="previousItem">The previously hovered item.</param>
        public ItemHoverEventArgs(ImageListViewItem item, ImageListViewItem previousItem)
        {
            mItem = item;
            mPreviousItem = previousItem;
        }
    }
    /// <summary>
    /// Represents the event arguments related to control layout.
    /// </summary>
    [Serializable, ComVisible(true)]
    public class LayoutEventArgs
    {
        /// <summary>
        /// Gets or sets the rectangle bounding the item area.
        /// </summary>
        public Rectangle ItemAreaBounds { get; set; }

        /// <summary>
        /// Initializes a new instance of the LayoutEventArgs class.
        /// </summary>
        /// <param name="itemAreaBounds">The rectangle bounding the item area.</param>
        public LayoutEventArgs(Rectangle itemAreaBounds)
        {
            ItemAreaBounds = itemAreaBounds;
        }
    }

    //JTN Added
    /// <summary>
    /// Represents the event arguments for the image before load from file. If Image are set, load from fill will not occure
    /// </summary>
    [Serializable, ComVisible(true)]
    public class RetrieveItemImageEventArgs
    {
        private string fullFilePath;
        private Image image;
        private bool didErrorOccureOnReadMedia;
        private bool wasImageReadFromFile;

        /// <summary>
        /// Use this Image instead
        /// </summary>
        public Image LoadedImage { get { return image; } set { image = value; } }
        /// <summary>
        /// Get Filename for Image that are become to be loaded
        /// </summary>
        public String FullFilePath { get { return fullFilePath; } }
        /// <summary>
        /// Get and set if picture information was loaded correctly 
        /// </summary>
        public bool DidErrorOccourLoadMedia { get => didErrorOccureOnReadMedia; set => didErrorOccureOnReadMedia = value; }
        public bool WasImageReadFromFile { get => wasImageReadFromFile; set => wasImageReadFromFile = value; }


        /// <summary>
        /// Initializes a new instance of the RetrieveImageEventArgs class.
        /// </summary>
        /// <param name="fullFilePath">The item that is the target of this event.</param>
        public RetrieveItemImageEventArgs(string fullFilePath)
        {
            this.fullFilePath = fullFilePath;
        }
    }

    //JTN Added
    /// <summary>
    /// Represents the event arguments for the thumbnail before load from file. If Thumbnail set, load from file will not occure
    /// </summary>
    [Serializable, ComVisible(true)]
    public class RetrieveItemThumbnailEventArgs
    {
        private string mFileName;
        private Image mImage;
        private DateTime dateModified;
        private Size mThumbnailSize;

        /// <summary>
        /// Use this Image instead
        /// </summary>
        public Image Thumbnail
        {
            get { return mImage; }
            set { mImage = value; }
        }

        public DateTime DateModified
        {
            get { return dateModified; }
            //set { dateModified = value; }
        }

        /// <summary>
        /// Get Filename for Image that are become to be loaded
        /// </summary>
        public String FullFileName
        {
            get { return mFileName; }
            //set { mFileName = value; }
        }


        /// <summary>
        /// Initializes a new instance of the RetrieveImageEventArgs class.
        /// </summary>
        /// <param name="filename">The item that is the target of this event.</param>
        /// <param name="dateModified">The size of thumbnail to fetch or create that is the target of this event.</param>
        public RetrieveItemThumbnailEventArgs(string filename, DateTime dateModified)
        {
            mFileName = filename;
            dateModified = DateModified;
        }
    }

    //RetrieveItemMetadataDetails
    //JTN Added
    /// <summary>
    /// Represents the event arguments for the thumbnail before load from file. If Thumbnail set, load from fill will not occure
    /// </summary>
    [Serializable, ComVisible(true)]
    public class RetrieveItemMetadataDetailsEventArgs
    {
        private string mFileName;
        private Utility.ShellImageFileInfo mFileMetadata;
        
        /// <summary>
        /// Get Filename for Image that are become to be loaded
        /// </summary>
        public String FileName
        {
            get { return mFileName; }
            //set { mFileName = value; }
        }

        /// <summary>
        /// Get and Set FileMetadata for Image that are become to be loaded
        /// </summary>
        public Utility.ShellImageFileInfo FileMetadata { get => mFileMetadata; set => mFileMetadata = value; }


        /// <summary>
        /// Initializes a new instance of the RetrieveImageEventArgs class.
        /// </summary>
        /// <param name="filename">The item that is the target of this event.</param>
        public RetrieveItemMetadataDetailsEventArgs(string filename)
        {
            mFileName = filename;
        }
    }

    /// <summary>
    /// Represents the event arguments for the thumbnail cached event.
    /// </summary>
    [Serializable, ComVisible(true)]
    public class ThumbnailCachedEventArgs
    {
        private ImageListViewItem mItem;
        private Image mThumbnail;
        private bool mError;
        private bool mWasThumbnailReadFromFile;
        private bool mDidErrorOccureReadFromFile;
        private Size mRequestedSize;

        /// <summary>
        /// Gets the ImageListViewItem that is the target of the event.
        /// </summary>
        public ImageListViewItem Item { get { return mItem; } }
        /// <summary>
        /// Gets the Thumbnail image created, not stored in Item Thumbnail.
        /// </summary>
        public Image Thumbnail { get { return mThumbnail; } }
        /// <summary>
        /// Gets whether an error occurred during thumbnail extraction.
        /// </summary>
        public bool Error { get { return mError; } }
        /// <summary>
        /// Gets whether an error occurred during thumbnail extraction.
        /// </summary>
        public bool WasThumbnailReadFromFile { get { return mWasThumbnailReadFromFile; } }
        /// <summary>
        /// Gets requested thumbnail size, not size after converted.
        /// </summary>
        public Size RequestedSize { get { return mRequestedSize; } }

        public bool DidErrorOccureReadFromFile { get => mDidErrorOccureReadFromFile; set => mDidErrorOccureReadFromFile = value; }

        /// <summary>
        /// Initializes a new instance of the ItemEventArgs class.
        /// </summary>
        /// <param name="item">The item that is the target of this event.</param>
        /// <param name="thumbnail">Excact thumbnail. Not only item version.</param>
        /// <param name="requestedSize">The item that is the target of this event.</param>
        /// <param name="error">Determines whether an error occurred during thumbnail extraction.</param>
        /// <param name="wasThumbnailReadFromFile">Determines whether an error occurred during thumbnail extraction.</param>
        public ThumbnailCachedEventArgs(ImageListViewItem item, Image thumbnail, Size requestedSize, bool error, bool wasThumbnailReadFromFile, bool didErrorOccureReadFromFile)
        {
            mItem = item;
            mThumbnail = thumbnail;
            mRequestedSize = requestedSize;
            mError = error;
            mWasThumbnailReadFromFile = wasThumbnailReadFromFile;
            mDidErrorOccureReadFromFile = didErrorOccureReadFromFile;
        }
    }
    /// <summary>
    /// Represents the event arguments related to virtual item 
    /// thumbnail requests.
    /// </summary>
    [Serializable, ComVisible(true)]
    public class VirtualItemThumbnailEventArgs
    {
        /// <summary>
        /// Gets the key of the virtual item.
        /// </summary>
        public object Key { get; private set; }
        /// <summary>
        /// Gets the size of the thumbnail image for the virtual item
        /// represented by Key.
        /// </summary>
        public Size ThumbnailDimensions { get; private set; }
        /// <summary>
        /// Gets or sets the thumbnail image for the virtual item
        /// represented by Key.
        /// </summary>
        public Image ThumbnailImage { get; set; }

        /// <summary>
        /// Initializes a new instance of the LayoutEventArgs class.
        /// </summary>
        /// <param name="key">The key of the virtual item.</param>
        /// <param name="thumbnailDimensions">Requested thumbnail pixel dimensions.</param>
        public VirtualItemThumbnailEventArgs(object key, Size thumbnailDimensions)
        {
            Key = key;
            ThumbnailDimensions = thumbnailDimensions;
        }
    }
    /// <summary>
    /// Represents the event arguments related to virtual item images.
    /// </summary>
    [Serializable, ComVisible(true)]
    public class VirtualItemImageEventArgs
    {
        /// <summary>
        /// Gets the key of the virtual item.
        /// </summary>
        public object Key { get; private set; }
        /// <summary>
        /// Gets or sets the full path to the source image for the virtual item
        /// represented by Key.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Initializes a new instance of the LayoutEventArgs class.
        /// </summary>
        /// <param name="key">The key of the virtual item.</param>
        public VirtualItemImageEventArgs(object key)
        {
            Key = key;
        }
    }
    /// <summary>
    /// Represents the event arguments related to virtual item details.
    /// </summary>
    [Serializable, ComVisible(true)]
    public class VirtualItemDetailsEventArgs
    {
        /// <summary>
        /// Gets the key of the virtual item.
        /// </summary>
        public object Key { get; private set; }
        /// <summary>
        /// Gets or sets the last access date of the image file represented by this item.
        /// </summary>
        public DateTime DateAccessed { get; set; }
        /// <summary>
        /// Gets or sets the creation date of the image file represented by this item.
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Gets or sets the modification date of the image file represented by this item.
        /// </summary>
        public DateTime DateModified { get; set; }
        /// <summary>
        /// Gets or sets the shell type of the image file represented by this item.
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// Gets or sets the name of the image fie represented by this item.
        /// </summary>        
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets the path of the image fie represented by this item.
        /// </summary>        
        public string FileDirectory { get; set; }
        /// <summary>
        /// Gets or sets file size in bytes.
        /// </summary>
        public long FileSize { get; set; }
        /// <summary>
        /// Gets or sets image dimensions.
        /// </summary>
        public Size Dimensions { get; set; }
        /// <summary>
        /// Gets or sets image resolution in pixels per inch.
        /// </summary>
        public SizeF Resolution { get; set; }
        /// <summary>
        /// Gets or sets image deascription.
        /// </summary>
        public string ImageDescription { get; set; }
        /// <summary>
        /// Gets or sets the camera model.
        /// </summary>
        public string EquipmentModel { get; set; }
        /// <summary>
        /// Gets or sets the date and time the image was taken.
        /// </summary>
        public DateTime DateTaken { get; set; }
        /// <summary>
        /// Gets or sets the name of the artist.
        /// </summary>
        public string Artist { get; set; }
        /// <summary>
        /// Gets or sets image copyright information.
        /// </summary>
        public string Copyright { get; set; }
        /// <summary>
        /// Gets or sets the exposure time in seconds.
        /// </summary>
        public string ExposureTime { get; set; }
        /// <summary>
        /// Gets or sets the F number.
        /// </summary>
        public float FNumber { get; set; }
        /// <summary>
        /// Gets or sets the ISO speed.
        /// </summary>
        public ushort ISOSpeed { get; set; }
        /// <summary>
        /// Gets or sets the shutter speed.
        /// </summary>
        public string ShutterSpeed { get; set; }
        /// <summary>
        /// Gets or sets the lens aperture value.
        /// </summary>
        public string Aperture { get; set; }
        /// <summary>
        /// Gets or sets user comments.
        /// </summary>
        public string UserComment { get; set; }

        /// <summary>
        /// Initializes a new instance of the LayoutEventArgs class.
        /// </summary>
        /// <param name="key">The key of the virtual item.</param>
        public VirtualItemDetailsEventArgs(object key)
        {
            Key = key;
        }
    }
    #endregion
}
