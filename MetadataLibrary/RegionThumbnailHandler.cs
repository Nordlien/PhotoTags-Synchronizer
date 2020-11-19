using System;
using System.Drawing;

namespace MetadataLibrary
{
    public static class RegionThumbnailHandler
    {
        public static Size FaceThumbnailSize { get; set; } = new Size(100, 100);
        public static Size GetSizedImageBounds(Size size, Size fit, bool acceptToScaleUp)
        {
            float f = Math.Max((float)size.Width / (float)fit.Width, (float)size.Height / (float)fit.Height);
            if (f < 1.0f && !acceptToScaleUp) f = 1.0f; // Do not upsize small images
            int width = (int)Math.Round((float)size.Width / f);
            int height = (int)Math.Round((float)size.Height / f);
            return new Size(width, height);
        }

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
    }
}
