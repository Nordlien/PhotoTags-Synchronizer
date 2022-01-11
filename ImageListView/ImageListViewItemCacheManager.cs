﻿// ImageListView - A listview control for image files
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

#define TraceLock_RemoveThisYes

using System.Collections.Generic;
using System.Threading;
using System;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Represents the cache manager responsible for asynchronously loading
    /// item details.
    /// </summary>
    internal class ImageListViewItemCacheManager : IDisposable
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        #region Member Variables
        private readonly object lockObject;

        private ImageListView mImageListView;
        private Thread mThreadItemCacheManager;

        private Queue<CacheItem> toCache; 
        private Dictionary<Guid, bool> editCache;

        private volatile bool stopping;
        private bool stopped;
        private bool disposed;
        #endregion


        //JTN
        public bool IsBackgroundThreadsStopped()
        {
            return stopped;
        }

        #region Private Classes
        /// <summary>
        /// Represents an item in the item cache.
        /// </summary>
        private class CacheItem
        {
            private ImageListViewItem mItem;
            private string mFileName;
            private bool mIsVirtualItem;
            private object mVirtualItemKey;

            /// <summary>
            /// Gets the ImageListViewItem associated with this request.
            /// </summary>
            public ImageListViewItem Item { get { return mItem; } }
            /// <summary>
            /// Gets the name of the image file.
            /// </summary>
            public string FileName { get { return mFileName; } }
            /// <summary>
            /// Gets whether Item is a virtual item.
            /// </summary>
            
            public object VirtualItemKey { get { return mVirtualItemKey; } }

            /// <summary>
            /// Initializes a new instance of the CacheItem class.
            /// </summary>
            /// <param name="item">The ImageListViewItem associated 
            /// with this request.</param>
            public CacheItem(ImageListViewItem item)
            {
                mItem = item;
                mIsVirtualItem = item.isVirtualItem;
                mVirtualItemKey = item.mVirtualItemKey;
                mFileName = item.FileFullPath;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Determines whether the cache thread is being stopped.
        /// </summary>
        private bool Stopping
        {
            get
            {
                lock (lockObject) { return stopping; }
            }
        }
        /// <summary>
        /// Determines whether the cache thread is stopped.
        /// </summary>
        public bool Stopped { 
            get {
                #if TraceLock
                Logger.Trace("(lockObject start and stopp) - Stopped");
                #endif
                lock (lockObject) 
                { return stopped; } 
            } 
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the ImageListViewItemCacheManager class.
        /// </summary>
        /// <param name="owner">The owner control.</param>
        public ImageListViewItemCacheManager(ImageListView owner)
        {
            lockObject = new object();

            mImageListView = owner;

            toCache = new Queue<CacheItem>();
            editCache = new Dictionary<Guid, bool>();

            mThreadItemCacheManager = new Thread(new ThreadStart(DoWork));
            mThreadItemCacheManager.Priority = ThreadPriority.BelowNormal; //JTN Added
            mThreadItemCacheManager.IsBackground = true;

            stopping = false;
            stopped = false;
            disposed = false;

            mThreadItemCacheManager.Start();
            while (!mThreadItemCacheManager.IsAlive) ;
        }
        #endregion

        #region Instance Methods
        /// <summary>
        /// Starts editing an item. While items are edited,
        /// their original images will be seperately cached
        /// instead of fetching them from the file.
        /// </summary>
        /// <param name="guid">The GUID of the item</param>
        public void BeginItemEdit(Guid guid)
        {
            #if TraceLock
            Logger.Trace("(lockObject start) - BeginItemEdit");
            #endif

            lock (lockObject)
            {
                if (!editCache.ContainsKey(guid))
                    editCache.Add(guid, false);
            }
            
            #if TraceLock
            Logger.Trace("(lockObject stop) - BeginItemEdit");
            #endif

        }
        /// <summary>
        /// Ends editing an item. After this call, item
        /// image will be continued to be fetched from the
        /// file.
        /// </summary>
        /// <param name="guid"></param>
        public void EndItemEdit(Guid guid)
        {
            lock (lockObject)
            {
                if (editCache.ContainsKey(guid))
                {
                    editCache.Remove(guid);
                }
            }
        }
        /// <summary>
        /// Adds the item to the cache queue.
        /// </summary>
        public void Add(ImageListViewItem item)
        {
            lock (lockObject)
            {
                toCache.Enqueue(new CacheItem(item));
                Monitor.Pulse(lockObject);
            }
        }
        /// <summary>
        /// Stops the cache manager.
        /// </summary>
        public void Stop()
        {
            lock (lockObject)
            {
                if (!stopping)
                {
                    stopping = true;
                    Monitor.Pulse(lockObject);
                }
            }
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                // Nothing to dispose
                disposed = true;
            }
        }
        #endregion

        #region Worker Method
        /// <summary>
        /// Used by the worker thread to read item data.
        /// </summary>
        private void DoWork()
        {
            while (!Stopping)
            {

                CacheItem item = null;
                lock (lockObject)
                {
                    // Wait until we have items waiting to be cached
                    if (toCache.Count == 0) Monitor.Wait(lockObject);

                    // Get an item from the queue
                    if (toCache.Count != 0)
                    {
                        item = toCache.Dequeue();

                        // Is it being edited?
                        if (editCache.ContainsKey(item.Item.Guid)) item = null;
                    }
                }

                // Read file info
                if (item != null)
                {
                    RetrieveItemMetadataDetailsEventArgs e = new RetrieveItemMetadataDetailsEventArgs(item.FileName);
                    mImageListView.RetrieveItemMetadataDetailsInternal(e);


                    Utility.ShellImageFileInfo info;
                    if (e.FileMetadata != null)
                    {
                        info = e.FileMetadata;
                    }
                    else //If not handel outside, try internal
                    {
                        info = new Utility.ShellImageFileInfo(item.FileName);
                    }

                    // Update file info
                    if (!Stopping)
                    {
                        try
                        {
                            if (mImageListView != null && mImageListView.IsHandleCreated && !mImageListView.IsDisposed && mImageListView.Enabled)
                            {
                                mImageListView.Invoke(new UpdateItemDetailsDelegateInternal(mImageListView.UpdateItemDetailsInternal), item.Item, info);
                            }
                        }
                        catch (ObjectDisposedException ex)
                        {
                            Logger.Warn("DoWork: " + ex.Message);
                            //if (!stopping) throw;
                        }
                        catch (InvalidOperationException ex)
                        {
                            Logger.Warn("DoWork: " + ex.Message);
                            //if (!stopping) throw;
                        }
                    }
                }
            }

            lock (lockObject)
            {
                stopped = true;
            }
        }
        #endregion
    }
}
