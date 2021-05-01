using MetadataLibrary;
using System;
using System.Drawing;

namespace Thumbnails
{
    public static class RegionThumbnailHandler
    {
        public static Size FaceThumbnailSize { get; set; } = new Size(100, 100);

        #region GetSizedImageBounds
        /// <summary>
        /// Resize the Region Thumbnail
        /// </summary>
        /// <param name="size">Original size</param>
        /// <param name="fit">New size to fit</param>
        /// <param name="acceptToScaleUp">When true, size can increse, when upsize gies lower quality on thumbail</param>
        /// <returns></returns>
        public static Size GetSizedImageBounds(Size size, Size fit, bool acceptToScaleUp)
        {
            float f = Math.Max((float)size.Width / (float)fit.Width, (float)size.Height / (float)fit.Height);
            if (f < 1.0f && !acceptToScaleUp) f = 1.0f; // Do not upsize small images
            int width = (int)Math.Round((float)size.Width / f);
            int height = (int)Math.Round((float)size.Height / f);
            return new Size(width, height);
        }
        #endregion 

        #region CopyRegionFromImage
        /// <summary>
        /// Copy region from inside a image and return the thumbnail bitmap
        /// </summary>
        /// <param name="image">The image to copy region area from</param>
        /// <param name="region">Size of the recion to copy</param>
        /// <returns>Thumbnail bitma create from the region given</returns>
        public static Bitmap CopyRegionFromImage(Image image, RegionStructure region)
        {

            Bitmap regionThumbnail;
            Rectangle rectangleInImage = region.GetImageRegionPixelRectangle(image.Size);

            Size thumbnailSize = GetSizedImageBounds(rectangleInImage.Size, FaceThumbnailSize, false);
            regionThumbnail = new Bitmap(thumbnailSize.Width, thumbnailSize.Height);

            using (Graphics grD = Graphics.FromImage(regionThumbnail))
            {
                grD.DrawImage(image, new Rectangle(1, 1, regionThumbnail.Width - 3, regionThumbnail.Height - 3), rectangleInImage, GraphicsUnit.Pixel);
                grD.DrawRectangle(new Pen(Color.Black), 0, 0, regionThumbnail.Width - 1, regionThumbnail.Height - 1);
            }

            return regionThumbnail;
        }
        #endregion 

        #region SaveThumbnailsForRegioList
        /// <summary>
        /// Save all thumbnails regions from the RegionList in the metadata class
        /// </summary>
        /// <param name="metadataDatabase">Database handler</param>
        /// <param name="metadata">The media file matadata class with RegionList</param>
        /// <param name="image">The image to create region thumbnails from</param>
        /// <returns>Return true if data updated</returns>
        public static bool SaveThumbnailsForRegioList(MetadataDatabaseCache metadataDatabase, Metadata metadata, Image image)
        {
            if (metadata == null) return false; //When new directory selected, array are become empty and list of null will be created

            RegionStructure regionStructure;
            for (int i = 0; i < metadata.PersonalRegionList.Count; i++)
            {
                if (metadata.PersonalRegionList[i].Thumbnail == null)
                {
                    regionStructure = metadata.PersonalRegionList[i];
                    regionStructure.Thumbnail = CopyRegionFromImage(image, regionStructure);
                    metadataDatabase.UpdateRegionThumbnail(metadata.FileEntryBroker, regionStructure);
                }
            }
            return true;
        }
        #endregion 
    }
}
