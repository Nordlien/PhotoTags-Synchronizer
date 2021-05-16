using System.IO;
using System.Collections.Generic;
using System.Linq;
using MetadataLibrary;
using ImageMagick;
using System.Drawing;
using System;
using System.Text;
using System.Globalization;
using Manina.Windows.Forms;
using LibVLCSharp.Shared;

namespace ImageAndMovieFileExtentions
{
    public static class ImageAndMovieFileExtentionsUtility 
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static MagickFormat ConvertFromMimeFormat (string contentFormat)
        {
            MagickFormat magickFormat = MagickFormat.Jpeg;
            switch ((contentFormat.StartsWith(".") ? "" : ".") + contentFormat.ToLower())
            {
                case ".apng":
                    magickFormat = MagickFormat.APng;
                    break;
                case ".bmp":
                    magickFormat = MagickFormat.Bmp;
                    break;
                case ".gif":
                    magickFormat = MagickFormat.Gif;
                    break;
                case ".jpeg":
                    magickFormat = MagickFormat.Jpeg;
                    break;
                case ".png":
                    magickFormat = MagickFormat.Png;
                    break;
                case ".webp":
                    magickFormat = MagickFormat.WebP;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return magickFormat;
        }

        public static byte[] LoadAndConvertImage(string fullFilename, string contentFormat, int width, int height, double rotateDegress)
        {
            byte[] jpegImage = null;
            try
            {
                // Read image from file
                using (var image = new MagickImage(fullFilename))
                {
                    // Sets the output format to jpeg
                    image.Format = ConvertFromMimeFormat(contentFormat);

                    if (width > 1 && height > 1)
                    {
                        MagickGeometry size = new MagickGeometry(width, height); //new MagickGeometry(720, 480);
                        size.IgnoreAspectRatio = false;
                        image.Resize(size);                        
                    }
                    if (rotateDegress != 0) image.Rotate(rotateDegress);

                    jpegImage = image.ToByteArray();
                }
            }
            catch (Exception ex)
            {
                Logger.Warn("Failed to convert image." + ex.Message);
            }
            return jpegImage;
        }

        public static Image LoadImage(string fullFilename)
        {
            Bitmap imageReturn = null;
            using (var image = new MagickImage(fullFilename))
            {
                imageReturn = image.ToBitmap();
            }
            return imageReturn;
        }

        public static Image LoadImageAndRotate(string fullFilename, double degress)
        {
            Bitmap imageReturn = null;
            using (var image = new MagickImage(fullFilename))
            {
                image.Rotate(degress);
                imageReturn = image.ToBitmap();
            }
            return imageReturn;
        }

        public static void RoateImage(string fullFilename, double degress)
        {
            using (MagickImage image = new MagickImage(fullFilename))
            {
                image.Rotate(degress);
                image.Write(fullFilename);
            }
        }

        public static Image ThumbnailFromFile(string fullFilename, Size maxSize, bool allowFailoverReadFullFille)
        {
            Image thumbnailReturn = null;
            using (MagickImage image = new MagickImage(fullFilename))
            {
                var profile = image.GetExifProfile();
                // Create thumbnail from exif information
                if (profile != null)
                {
                    using (var thumbnail = profile.CreateThumbnail())
                    {
                        if (thumbnail != null) thumbnailReturn = thumbnail.ToBitmap();
                    }
                }
                else
                {
                    if (allowFailoverReadFullFille)
                    {

                        image.Thumbnail(new MagickGeometry(maxSize.Width, maxSize.Height));
                        thumbnailReturn = image.ToBitmap();
                    }
                }
            }
            return thumbnailReturn;
        }

        public static DateTime? ConvertDateTimeFromString(String dateTimeToConvert)
        {
            String[] dateFormats = { "yyyy:MM:dd HH:mm", "yyyy:MM:dd HH:mm:ss", "yyyy:MM:dd HH:mm:sszzz", "yyyy:MM:dd HH:mm:ss.fff", "yyyy:MM:dd HH:mm:ss.ff", "yyyy:MM:dd HH:mm:ss.ffff", "yyyy:MM:dd HH:mm:ss.fffff", "yyyy:MM:dd HH:mm:ss.ffffff" };
            
            try
            {
                DateTime dateTime;
                if (DateTime.TryParseExact(dateTimeToConvert, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out dateTime))
                {
                    return dateTime;
                }
                else return null;
            }
            catch 
            {
                //Logger.Warn(dateTimeToConvert + " " + e.Message); //TODO: Need to fix problems with date formats
                return null;
            }
        }

        public static string CovertByteArrayToString(byte[] byteArray)
        {
            return Encoding.UTF8.GetString(byteArray);
        }

        public static Utility.ShellImageFileInfo GetExif(string fullFilename)
        {
            if (IsImageFormat(fullFilename)) return GetExifImage(fullFilename);
            if (IsVideoFormat(fullFilename)) return GetExifVideo(fullFilename);
            return null;
        }

        public static Utility.ShellImageFileInfo GetExifVideo(string fullFilename)
        {
            Utility.ShellImageFileInfo shellImageFileInfo = null;
            try
            {
                shellImageFileInfo = new Utility.ShellImageFileInfo();

                FileInfo fileInfo = new FileInfo(fullFilename);
                if (fileInfo != null)
                {                    
                    #region shellImageFileInfo.CreationTime
                    shellImageFileInfo.CreationTime = fileInfo.CreationTime;
                    #endregion

                    #region shellImageFileInfo.DirectoryName
                    shellImageFileInfo.DirectoryName = fileInfo.DirectoryName;
                    #endregion

                    #region shellImageFileInfo.DisplayName
                    shellImageFileInfo.DisplayName = fileInfo.Name;
                    #endregion

                    #region shellImageFileInfo.Extension
                    shellImageFileInfo.Extension = fileInfo.Extension.Trim().ToUpper();
                    #endregion

                    #region shellImageFileInfo.FileAttributes
                    shellImageFileInfo.FileAttributes = fileInfo.Attributes;
                    #endregion

                    #region shellImageFileInfo.LastAccessTime
                    shellImageFileInfo.LastAccessTime = fileInfo.LastAccessTime;
                    #endregion

                    #region shellImageFileInfo.LastWriteTime
                    shellImageFileInfo.LastWriteTime = fileInfo.LastWriteTime;
                    #endregion

                    #region shellImageFileInfo.Size
                    shellImageFileInfo.Size = fileInfo.Length;
                    #endregion
                }
            }
            catch { }

            return shellImageFileInfo;
        }

        public static Utility.ShellImageFileInfo GetExifImage(string fullFilename)
        {
            Utility.ShellImageFileInfo shellImageFileInfo = null;  
            try
            {
                
                using (MagickImage image = new MagickImage(fullFilename))
                {
                    shellImageFileInfo = new Utility.ShellImageFileInfo();
                    
                    FileInfo fileInfo = new FileInfo(fullFilename);
                    if (fileInfo != null)
                    {
                        #region shellImageFileInfo.CreationTime
                        shellImageFileInfo.CreationTime = fileInfo.CreationTime;
                        #endregion

                        #region shellImageFileInfo.DirectoryName
                        shellImageFileInfo.DirectoryName = fileInfo.DirectoryName;
                        #endregion

                        #region shellImageFileInfo.DisplayName
                        shellImageFileInfo.DisplayName = fileInfo.Name;
                        #endregion

                        #region shellImageFileInfo.Extension
                        shellImageFileInfo.Extension = fileInfo.Extension.Trim().ToUpper();
                        #endregion

                        #region shellImageFileInfo.FileAttributes
                        shellImageFileInfo.FileAttributes = fileInfo.Attributes;
                        #endregion

                        #region shellImageFileInfo.LastAccessTime
                        shellImageFileInfo.LastAccessTime = fileInfo.LastAccessTime;
                        #endregion

                        #region shellImageFileInfo.LastWriteTime
                        shellImageFileInfo.LastWriteTime = fileInfo.LastWriteTime;
                        #endregion

                        #region shellImageFileInfo.Size
                        shellImageFileInfo.Size = fileInfo.Length;
                        #endregion
                    }

                    IExifProfile profile = image.GetExifProfile();
                    if (profile != null)
                    {
                        #region shellImageFileInfo.ApertureValue / ExifTag.ApertureValue
                        var valueRational = profile.GetValue(ExifTag.ApertureValue);
                        if (valueRational != null) shellImageFileInfo.ApertureValue = valueRational.ToString();
                        #endregion

                        #region shellImageFileInfo.Artist / ExifTag.Artist
                        var valueString = profile.GetValue(ExifTag.Artist);
                        var valueByteArray = profile.GetValue(ExifTag.XPAuthor);
                        if (valueString != null) shellImageFileInfo.Artist = valueString.Value;
                        else if (valueByteArray != null) shellImageFileInfo.Artist = CovertByteArrayToString(valueByteArray.Value);
                        #endregion

                        #region shellImageFileInfo.Copyright / ExifTag.Copyright
                        valueString = profile.GetValue(ExifTag.Copyright);
                        if (valueString != null) shellImageFileInfo.Copyright = valueString.Value;
                        #endregion

                        #region shellImageFileInfo.DateTaken / ExifTag.DateTimeDigitized
                        var valueDateTime = profile.GetValue(ExifTag.DateTimeDigitized);
                        if (valueDateTime != null && ConvertDateTimeFromString(valueDateTime.Value) != null) shellImageFileInfo.DateTaken = (DateTime)ConvertDateTimeFromString(valueDateTime.Value);
                        #endregion

                        #region shellImageFileInfo.Dimensions / ExifTag.PixelXDimension, ExifTag.PixelYDimension
                        var valueNumberX = profile.GetValue(ExifTag.PixelXDimension);
                        var valueNumberY = profile.GetValue(ExifTag.PixelYDimension);
                        if (valueNumberX != null && valueNumberY != null) shellImageFileInfo.Dimensions = new Size((int)valueNumberX.Value, (int)valueNumberX.Value);
                        #endregion

                        #region shellImageFileInfo.EquipmentModel / ExifTag.Model + ExifTag.Make
                        valueString = profile.GetValue(ExifTag.Model);
                        if (valueString != null) shellImageFileInfo.EquipmentModel = valueString.Value;

                        valueString = profile.GetValue(ExifTag.Make);
                        if (valueString != null) shellImageFileInfo.EquipmentModel += (shellImageFileInfo.EquipmentModel == null ? "" : " ") + valueString.Value;
                        #endregion

                        #region shellImageFileInfo.ExposureTime / ExifTag.ExposureTime
                        valueRational = profile.GetValue(ExifTag.ExposureTime);
                        if (valueRational != null) shellImageFileInfo.ExposureTime = valueRational.Value.ToString();
                        #endregion

                        #region shellImageFileInfo.FNumber / ExifTag.FNumber
                        valueRational = profile.GetValue(ExifTag.FNumber);
                        if (valueRational != null) shellImageFileInfo.FNumber = (float)valueRational.Value.ToDouble();
                        #endregion

                        #region shellImageFileInfo.ImageDescription / ExifTag.ImageDescription
                        valueString = profile.GetValue(ExifTag.ImageDescription);
                        if (valueString != null) shellImageFileInfo.ImageDescription = valueString.Value;
                        #endregion

                        #region shellImageFileInfo.ISOSpeed / ExifTag.ISOSpeed
                        var valueuInt = profile.GetValue(ExifTag.ISOSpeed);
                        if (valueuInt != null) shellImageFileInfo.ISOSpeed = (ushort)valueuInt.Value;
                        #endregion

                        #region shellImageFileInfo.Resolution / ExifTag.PixelScale (image.Density.X + image.Density.Y)
                        var valueDouble = profile.GetValue(ExifTag.PixelScale);
                        if (valueDouble != null)
                        {
                            image.Density.ChangeUnits(DensityUnit.PixelsPerInch);
                            shellImageFileInfo.Resolution = new SizeF((float)image.Density.X, (float)image.Density.Y);
                        }
                        #endregion

                        #region shellImageFileInfo.ShutterSpeed / ExifTag.ShutterSpeedValue
                        var valueSignedRational = profile.GetValue(ExifTag.ShutterSpeedValue);
                        if (valueSignedRational != null) shellImageFileInfo.ShutterSpeed = valueSignedRational.Value.ToString();
                        #endregion

                        #region shellImageFileInfo.TypeName
                        shellImageFileInfo.TypeName = shellImageFileInfo.GetFileType(fullFilename, shellImageFileInfo.Extension);
                        #endregion

                        #region shellImageFileInfo.UserComment / ExifTag.UserComment or ExifTag.XPComment
                        valueByteArray = profile.GetValue(ExifTag.UserComment);
                        if (valueByteArray == null) valueByteArray = profile.GetValue(ExifTag.XPComment);
                        if (valueByteArray != null) shellImageFileInfo.UserComment = CovertByteArrayToString(valueByteArray.Value);
                        #endregion
                    }
                }
            }
            catch { }

            return shellImageFileInfo;
        }

        


        #region Video and Image Formats
        private static List<string> imageFormats = new List<string> {
            //Tag	        Mode	Description	Notes
            ".AAI", //	    RW	AAI Dune image	
            ".APNG", //		RW	Animated Portable Network Graphics	Note, you must use an explicit image format specifier to read an APNG (apng:myImage.apng) image sequence, otherwise it assumes a PNG image and only reads the first frame.
            ".ART", //		RW	PFS: 1st Publisher	Format originally used on the Macintosh (MacPaint?) and later used for PFS: 1st Publisher clip art.
            ".ARW", //		R	Sony Digital Camera Alpha Raw Image Format	Set -define dng:use-camera-wb=true to use the RAW-embedded color profile for Sony cameras. You can also set these options: use-auto-wb, use-auto-bright, and output-color.
            ".AVI", //		R	Microsoft Audio/Visual Interleaved	
            ".AVS", //		RW	AVS X image	
            ".BPG", //		RW	Better Portable Graphics	Use -quality to specify the image compression quality. To meet the requirements of BPG, the quality argument divided by 2 (e.g. -quality 92 assigns 46 as the BPG compression.
            ".BMP", //	 
            ".BMP2", //	 
            ".BMP3", //		RW	Microsoft Windows bitmap	By default the BMP format is version 4. Use BMP3 and BMP2 to write versions 3 and 2 respectively. Use -define bmp:ignore-filesize to ignore the filesize check.
            ".BRF", //		W	Braille Ready Format	Uses juxtaposition of 6-dot braille patterns (thus 6x2 dot matrices) to reproduce images, using the BRF ASCII Braille encoding.
            ".CALS", //		R	Continuous Acquisition and Life-cycle Support Type 1 image	Specified in MIL-R-28002 and MIL-PRF-28002. Standard blueprint archive format as used by the US military to replace microfiche.
            ".CGM", //		R	Computer Graphics Metafile	Requires ralcgm to render CGM files.
            ".CIN", //		RW	Kodak Cineon Image Format	Use -set to specify the image gamma or black and white points (e.g. -set gamma 1.7, -set reference-black 95, -set reference-white 685). Properties include cin:file.create_date, cin:file.create_time, cin:file.filename, cin:file.version, cin:film.count, cin:film.format, cin:film.frame_id, cin:film.frame_position, cin:film.frame_rate, cin:film.id, cin:film.offset, cin:film.prefix, cin:film.slate_info, cin:film.type, cin:image.label, cin:origination.create_date, cin:origination.create_time, cin:origination.device, cin:origination.filename, cin:origination.model, cin:origination.serial, cin:origination.x_offset, cin:origination.x_pitch, cin:origination.y_offset, cin:origination.y_pitch, cin:user.data.
            ".CIP", //		W	Cisco IP phone image format	
            ".CMYK", //		RW	Raw cyan, magenta, yellow, and black samples	Use -size and -depth to specify the image width, height, and depth. To specify a single precision floating-point format, use -define quantum:format=floating-point. Set the depth to 32 for single precision floats, 64 for double precision, and 16 for half-precision.
            ".CMYKA", //		RW	Raw cyan, magenta, yellow, black, and alpha samples	Use -size and -depth to specify the image width, height, and depth. To specify a single precision floating-point format, use -define quantum:format=floating-point. Set the depth to 32 for single precision floats, 64 for double precision, and 16 for half-precision.
            ".CR2", //		R	Canon Digital Camera Raw Image Format	Requires an explicit image format otherwise the image is interpreted as a TIFF image (e.g. cr2:image.cr2).
            ".CRW", //		R	Canon Digital Camera Raw Image Format	
            ".CUBE", //		R	Cube Color lookup table converted to a HALD image	Select levels like this: cube:Vibrant.cube[8] for level 8
            ".CUR", //		R	Microsoft Cursor Icon	
            ".CUT", //		R	DR Halo	
            ".DCM", //		R	Digital Imaging and Communications in Medicine (DICOM) image	Used by the medical community for images like X-rays. ImageMagick sets the initial display range based on the Window Center (0028,1050) and Window Width (0028,1051) tags. Use -define dcm:display-range=reset to set the display range to the minimum and maximum pixel values. Use -define dcm:rescale=true to enable interpretation of the rescale slope and intercept settings in the file. Use -define dcm:window=centerXwidth to override the center and width settings in the file with your own values.
            ".DCR", //		R	Kodak Digital Camera Raw Image File	
            ".DCX", //		RW	ZSoft IBM PC multi-page Paintbrush image	
            ".DDS", //		RW	Microsoft Direct Draw Surface	Use -define to specify the compression (e.g. -define dds:compression={dxt1, dxt5, none}). Other defines include dds:cluster-fit={true,false}, dds:weight-by-alpha={true,false}, dds:fast-mipmaps={true,false}, and use dds:mipmaps to set the number of mipmaps (use fromlist to use the image list).
            ".DIB", //		RW	Microsoft Windows Device Independent Bitmap	DIB is a BMP file without the BMP header. Used to support embedded images in compound formats like WMF.
            ".DJVU", //		R		
            ".DNG", //		R	Digital Negative	Requires an explicit image format otherwise the image is interpreted as a TIFF image (e.g. dng:image.dng).
            ".DOT", //		R	Graph Visualization	Use -define to specify the layout engine (e.g. -define dot:layout-engine=twopi).
            ".DPX", //		RW	SMPTE Digital Moving Picture Exchange 2.0 (SMPTE 268M-2003)	Use -set to specify the image gamma or black and white points (e.g. -set gamma 1.7, -set reference-black 95, -set reference-white 685).
            ".EMF", //		R	Microsoft Enhanced Metafile (32-bit)	Only available under Microsoft Windows. Use -size command line option to specify the maximum width and height.
            ".EPDF", //		RW	Encapsulated Portable Document Format	
            ".EPI", //		RW	Adobe Encapsulated PostScript Interchange format	Requires Ghostscript to read.
            ".EPS", //		RW	Adobe Encapsulated PostScript	Requires Ghostscript to read.
            ".EPS2", //		W	Adobe Level II Encapsulated PostScript	Requires Ghostscript to read.
            ".EPS3", //		W	Adobe Level III Encapsulated PostScript	Requires Ghostscript to read.
            ".EPSF", //		RW	Adobe Encapsulated PostScript	Requires Ghostscript to read.
            ".EPSI", //		RW	Adobe Encapsulated PostScript Interchange format	Requires Ghostscript to read.
            ".EPT", //		RW	Adobe Encapsulated PostScript Interchange format with TIFF preview	Requires Ghostscript to read.
            ".EXR", //		RW	High dynamic-range (HDR) file format developed by Industrial Light & Magic	See High Dynamic-Range Images for details on this image format. To specify the output color type, use -define exr:color-type={RGB,RGBA,YC,YCA,Y,YA,R,G,B,A}. Use -sampling-factor to specify the sampling rate for YC(A) (e.g. 2x2 or 4:2:0). Requires the OpenEXR delegate library.
            ".FARBFELD", //	RW	Farbfeld lossless image format	sRGB 16-bit RGBA lossless image format
            ".FAX", //		RW	Group 3 TIFF	This format is a fixed width of 1728 as required by the standard. See TIFF format. Note that FAX machines use non-square pixels which are 1.5 times wider than they are tall but computer displays use square pixels so FAX images may appear to be narrow unless they are explicitly resized using a resize specification of 100x150%.
            //".FIG", //		R	FIG graphics format	Requires TransFig.
            ".FITS", //		RW	Flexible Image Transport System	To specify a single-precision floating-point format, use -define quantum:format=floating-point. Set the depth to 64 for a double-precision floating-point format.
            ".FL32", //		RW	FilmLight floating point image format	
            ".FLIF", //		RW	Free Lossless Image Format	
            ".FPX", //		RW	FlashPix Format	FlashPix has the option to store mega- and giga-pixel images at various resolutions in a single file which permits conservative bandwidth and fast reveal times when displayed within a Web browser. Requires the FlashPix SDK. Specify the FlashPix viewing parameters with the -define fpx:view.
            ".GIF", //		RW	CompuServe Graphics Interchange Format	8-bit RGB PseudoColor with up to 256 palette entries. Specify the format GIF87 to write the older version 87a of the format. Use -transparent-color to specify the GIF transparent color (e.g. -transparent-color wheat).
            //".GPLT", //		R	Gnuplot plot files	Requires gnuplot4.0.tar.Z or later.
            ".GRAY", //		RW	Raw gray samples	Use -size and -depth to specify the image width, height, and depth. To specify a single precision floating-point format, use -define quantum:format=floating-point. Set the depth to 32 for single precision floats, 64 for double precision, and 16 for half-precision. For signed pixel data, use -define quantum:format=signed.
            ".GRAYA", //	RW	Raw gray and alpha samples	Use -size and -depth to specify the image width, height, and depth. To specify a single precision floating-point format, use -define quantum:format=floating-point. Set the depth to 32 for single precision floats, 64 for double precision, and 16 for half-precision.
            ".HDR", //		RW	Radiance RGBE image format	
            ".HDR", //		RW	Radiance RGBE image format	
            //".HEIC", //		R	Apple High efficiency Image Format	Set the quality to 100 to produce lossless HEIC images. Requires the libheif delegate library.
            //".HPGL", //		R	HP-GL plotter language	Requires hp2xx-3.4.4.tar.gz
            ".HRZ", //		RW	Slow Scane TeleVision	
            //".HTML", //		RW	Hypertext Markup Language with a client-side image map	Also known as HTM. Requires html2ps to read.
            ".ICO", //		R	Microsoft icon	Also known as ICON.
            //".INFO", //		W	Format and characteristics of the image	
            ".ISOBRL", //	W	ISO/TR 11548-1 BRaiLle	Uses juxtaposition of 8-dot braille patterns (thus 8x2 dot matrices) to reproduce images, using the ISO/TR 11548-1 Braille encoding.
            ".ISOBRL6", //	W	ISO/TR 11548-1 BRaiLle 6 dots	Uses juxtaposition of 6-dot braille patterns (thus 6x2 dot matrices) to reproduce images, using the ISO/TR 11548-1 Braille encoding.
            //".JBIG", //		RW	Joint Bi-level Image experts Group file interchange format	Also known as BIE and JBG. Requires jbigkit-1.6.tar.gz.
            //".JNG", //		RW	Multiple-image Network Graphics	JPEG in a PNG-style wrapper with transparency. Requires libjpeg and libpng-1.0.11 or later, libpng-1.2.5 or later recommended.
            ".JP2", //		RW	JPEG-2000 JP2 File Format Syntax	Specify the encoding options with the -define option. See JP2 Encoding Options for more details.
            ".JPT", //		RW	JPEG-2000 Code Stream Syntax	Specify the encoding options with the -define option See JP2 Encoding Options for more details.
            ".J2C", //		RW	JPEG-2000 Code Stream Syntax	Specify the encoding options with the -define option See JP2 Encoding Options for more details.
            ".J2K", //		RW	JPEG-2000 Code Stream Syntax	Specify the encoding options with the -define option See JP2 Encoding Options for more details.
            ".JPG",
            ".JPEG", //		RW	Joint Photographic Experts Group JFIF format	Note, JPEG is a lossy compression. In addition, you cannot create black and white images with JPEG nor can you save transparency.
            //                  Requires jpegsrc.v8c.tar.gz. You can set quality scaling for luminance and chrominance separately (e.g. -quality 90,70). You can optionally define the DCT method, for example to specify the float method, use -define jpeg:dct-method=float. By default we compute optimal Huffman coding tables. Specify -define jpeg:optimize-coding=false to use the default Huffman tables. Two other options include -define jpeg:block-smoothing and -define jpeg:fancy-upsampling. Set the sampling factor with -define jpeg:sampling-factor. You can size the image with jpeg:size, for example -define jpeg:size=128x128. To restrict the maximum file size, use jpeg:extent, for example -define jpeg:extent=400KB. To define one or more custom quantization tables, use -define jpeg:q-table=filename. These values are multiplied by -quality argument divided by 100.0. To avoid reading a particular associated image profile, use -define profile:skip=name (e.g. profile:skip=ICC).
            //".JXR", //		RW	JPEG extended range	Requires the jxrlib delegate library. Put the JxrDecApp and JxrEncApp applications in your execution path.
            ".JSON", //		W	JavaScript Object Notation, a lightweight data-interchange format	Include additional attributes about the image with these defines: -define json:locate, -define json:limit, -define json:moments, or -define json:features. Specify the JSON model schema version with -define json:version. The current version is 1.0. Any version less than 1.0, returns the original JSON output which included misspelled labels.
            //".JXL", //		RW	JPEG XL image coding system	Requires the JPEG XL delegate library.
            ".KERNEL", //	W	Morphology kernel format	format suitable for a morphology kernel
            //".MAN", //		R	Unix reference manual pages	Requires that GNU groff and Ghostcript are installed.
            ".MAT", //		R	MATLAB image format	
            ".MIFF", //		RW	Magick image file format	This format persists all image attributes known to ImageMagick. To specify a single precision floating-point format, use -define quantum:format=floating-point. Set the depth to 32 for single precision floats, 64 for double precision, and 16 for half-precision.
            ".MONO", //		RW	Bi-level bitmap in least-significant-byte first order	
            //".MNG", //		RW	Multiple-image Network Graphics	A PNG-like Image Format Supporting Multiple Images, Animation and Transparent JPEG. Requires libpng-1.0.11 or later, libpng-1.2.5 or later recommended. An interframe delay of 0 generates one frame with each additional layer composited on top. For motion, be sure to specify a non-zero delay.
            //".M2V", //		RW	Motion Picture Experts Group file interchange format (version 2)	Requires ffmpeg.
            //".MPEG", //		RW	Motion Picture Experts Group file interchange format (version 1)	Requires ffmpeg.
            //".MPC", //		RW	Magick Persistent Cache image file format	The most efficient data processing pattern is a write-once, read-many-times pattern. The image is generated or copied from source, then various analyses are performed on the image pixels over time. MPC supports this pattern. MPC is the native in-memory ImageMagick uncompressed file format. This file format is identical to that used by ImageMagick to represent images in memory and is read by mapping the file directly into memory. The MPC format is not portable and is not suitable as an archive format. It is suitable as an intermediate format for high-performance image processing. The MPC format requires two files to support one image. Image attributes are written to a file with the extension .mpc, whereas, image pixels are written to a file with the extension .cache.
            //".MPR", //		RW	Magick Persistent Registry	This format permits you to write to and read images from memory. The image persists until the program exits. For example, let's use the MPR to create a checkerboard:
            //                  magick \( -size 15x15 canvas:black canvas:white -append \) \
            //                  \( +clone -flip \) +append -write mpr:checkers +delete \
            //                  -size 240x240 tile:mpr:checkers board.png
            ".MRW", //		R	Sony (Minolta) Raw Image File	Set -define dng:use-camera-wb=true to use the RAW-embedded color profile for Sony cameras.
            ".MSL", //		RW	Magick Scripting Language	MSL is the XML-based scripting language supported by the conjure utility. MSL requires the libxml2 delegate library.
            ".MTV", //		RW	MTV Raytracing image format	
            //".MVG", //		RW	Magick Vector Graphics.	The native ImageMagick vector metafile format. A text file containing vector drawing commands accepted by magick's -draw option.
            ".NEF", //		R	Nikon Digital SLR Camera Raw Image File	
            ".ORF", //		R	Olympus Digital Camera Raw Image File	
            ".ORA", //		R	open exchange format for layered raster based graphics	
            ".OTB", //		RW	On-the-air Bitmap	
            ".P7", //		RW	Xv's Visual Schnauzer thumbnail format	
            ".PALM", //		RW	Palm pixmap	
            ".PAM", //		W	Common 2-dimensional bitmap format	
            ".CLIPBOARD", //RW	Windows Clipboard	Only available under Microsoft Windows.
            ".PBM", //		RW	Portable bitmap format (black and white)	
            ".PCD", //		RW	Photo CD	The maximum resolution written is 768x512 pixels since larger images require huffman compression (which is not supported). Use -bordercolor to specify the border color (e.g. -bordercolor black).
            ".PCDS", //		RW	Photo CD	Decode with the sRGB color tables.
            ".PCL", //		W	HP Page Control Language	Use -define to specify fit to page option (e.g. -define pcl:fit-to-page=true).
            ".PCX", //		RW	ZSoft IBM PC Paintbrush file	
            ".PDB", //		RW	Palm Database ImageViewer Format	
            ".PDF", //		RW	Portable Document Format	Requires Ghostscript to read. By default, ImageMagick sets the page size to the MediaBox. Some PDF files, however, have a CropBox or TrimBox that is smaller than the MediaBox and may include white space, registration or cutting marks outside the CropBox or TrimBox. To force ImageMagick to use the CropBox or TrimBox rather than the MediaBox, use -define (e.g. -define pdf:use-cropbox=true or -define pdf:use-trimbox=true). Use -density to improve the appearance of your PDF rendering (e.g. -density 300x300). Use -alpha remove to remove transparency. To specify direct conversion from Postscript to PDF, use -define delegate:bimodel=true. Use -define pdf:fit-page=true to scale to the page size. To immediately stop processing upon an error, set -define pdf:stop-on-error to true. To set the page direction preferences to right-to-left, try -define pdf:page-direction=right-to-left. Use -alpha remove to remove transparency. When writing to a PDF, thumbnails are included by default. To skip generating thumbnails, -define pdf:thumbnail=false. To enable interpolation when rendering, use -define pdf:interpolate=true.
            ".PEF", //		R	Pentax Electronic File	Requires an explicit image format otherwise the image is interpreted as a TIFF image (e.g. pef:image.pef).
            ".PES", //		R	Embrid Embroidery Format	
            ".PFA", //		R	Postscript Type 1 font (ASCII)	Opening as file returns a preview image.
            ".PFB", //		R	Postscript Type 1 font (binary)	Opening as file returns a preview image.
            ".PFM", //		RW	Portable float map format	
            ".PGM", //		RW	Portable graymap format (gray scale)	
            ".PHM", //		RW	Portable float map format 16-bit half	
            ".PICON", //	RW	Personal Icon	
            ".PICT", //		RW	Apple Macintosh QuickDraw/PICT file	
            ".PIX", //		R	Alias/Wavefront RLE image format	
            ".PNG", //		RW	Portable Network Graphics	Requires libpng-1.0.11 or later, libpng-1.2.5 or later recommended. The PNG specification does not support pixels-per-inch units, only pixels-per-centimeter. To avoid reading a particular associated image profile, use -define profile:skip=name (e.g. profile:skip=ICC).
            ".PNG8", //		RW	Portable Network Graphics	8-bit indexed with optional binary transparency
            ".PNG00", //	RW	Portable Network Graphics	PNG inheriting subformat from original if possible
            ".PNG24", //	RW	Portable Network Graphics	opaque or binary transparent 24-bit RGB
            ".PNG32", //	RW	Portable Network Graphics	opaque or transparent 32-bit RGBA
            ".PNG48", //	RW	Portable Network Graphics	opaque or binary transparent 48-bit RGB
            ".PNG64", //	RW	Portable Network Graphics	opaque or transparent 64-bit RGB
            ".PNM", //		RW	Portable anymap	PNM is a family of formats supporting portable bitmaps (PBM) , graymaps (PGM), and pixmaps (PPM). There is no file format associated with pnm itself. If PNM is used as the output format specifier, then ImageMagick automagically selects the most appropriate format to represent the image. The default is to write the binary version of the formats. Use -compress none to write the ASCII version of the formats.
            ".POCKETMOD", //RW	Pocketmod personal organizer format	Example usage: magick -density 300 pages?.pdf pocketmod:organize.pdf
            ".PPM", //		RW	Portable pixmap format (color)	
            ".PS", //		RW	Adobe PostScript file	Requires Ghostscript to read. To force ImageMagick to respect the crop box, use -define (e.g. -define eps:use-cropbox=true). Use -density to improve the appearance of your Postscript rendering (e.g. -density 300x300). Use -alpha remove to remove transparency. To specify direct conversion from PDF to Postscript, use -define delegate:bimodel=true.
            ".PS2", //		RW	Adobe Level II PostScript file	Requires Ghostscript to read.
            ".PS3", //		RW	Adobe Level III PostScript file	Requires Ghostscript to read.
            ".PSB", //		RW	Adobe Large Document Format	
            ".PSD", //		RW	Adobe Photoshop bitmap file	Use -define psd:alpha-unblend=off to disable alpha blenning in the merged image. Use -define psd:additional-info=all|selective to transfer additional information from the input PSD file to output PSD file. The 'selective' option will preserve all additional information that is not related to the geometry of the image. The 'all' option should only be used when the geometry of the image has not been changed. This option is helpful when transferring non-simple layers, such as adjustment layers from the input PSD file to the output PSD file. This define is available as of Imagemagick version 6.9.5-8. Use -define psd:preserve-opacity-mask=true to preserve the opacity mask of a layer and add it back to the layer when the image is saved.
            ".PTIF", //		RW	Pyramid encoded TIFF	Multi-resolution TIFF containing successively smaller versions of the image down to the size of an icon.
            ".PWP", //		R	Seattle File Works multi-image file	
            ".RAD", //		R	Radiance image file	Requires that ra_ppm from the Radiance software package be installed.
            ".RAF", //		R	Fuji CCD-RAW Graphic File	
            ".RGB", //		RW	Raw red, green, and blue samples	Use -size and -depth to specify the image width, height, and depth. To specify a single precision floating-point format, use -define quantum:format=floating-point. Set the depth to 32 for single precision floats, 64 for double precision, and 16 for half-precision.
            ".RGB565", //	R	Raw red, green, blue pixels in the 5-6-5 format	Use -size to specify the image width and height.
            ".RGBA", //		RW	Raw red, green, blue, and alpha samples	Use -size and -depth to specify the image width, height, and depth. To specify a single precision floating-point format, use -define quantum:format=floating-point. Set the depth to 32 for single precision floats, 64 for double precision, and 16 for half-precision.
            ".RGF", //		RW	LEGO Mindstorms EV3 Robot Graphics File	
            ".RLA", //		R	Alias/Wavefront image file	
            ".RLE", //		R	Utah Run length encoded image file	
            ".SCT", //		R	Scitex Continuous Tone Picture	
            ".SFW", //		R	Seattle File Works image	
            ".SGI", //		RW	Irix RGB image	
            ".SHTML", //	W	Hypertext Markup Language client-side image map	Used to write HTML clickable image maps based on a the output of montage or a format which supports tiled images such as MIFF.
            ".SID", //	
            ".MrSID", //	R	Multiresolution seamless image	Requires the mrsidgeodecode command line utility that decompresses MG2 or MG3 SID image files.
            ".SPARSE-COLOR", 
                    //		W	Raw text file	Format compatible with the -sparse-color option. Lists only non-fully-transparent pixels.
            ".SUN", //		RW	SUN Rasterfile	
            ".SVG", //		RW	Scalable Vector Graphics	ImageMagick utilizes inkscape if its in your execution path otherwise RSVG. If neither are available, ImageMagick reverts to its internal SVG renderer. The default resolution is 96 DPI. Use -size command line option to specify the maximum width and height. If you want to render a very large SVG and you trust the source, enable this option: -define svg:xml-parse-huge=true.
            //".TEXT", //		R	text file	Requires an explicit format specifier to read, e.g. text:README.txt.
            ".TGA", //		RW	Truevision Targa image	Also known as formats ICB, VDA, and VST.
            ".TIFF", //		RW	Tagged Image File Format	Also known as TIF. Requires tiff-v3.6.1.tar.gz or later. Use -define to specify the rows per strip (e.g. -define tiff:rows-per-strip=8). To define the tile geometry, use for example, -define tiff:tile-geometry=128x128. To specify a signed format, use -define quantum:format=signed. To specify a single-precision floating-point format, use -define quantum:format=floating-point. Set the depth to 64 for a double-precision floating-point format. Use -define quantum:polarity=min-is-black or -define quantum:polarity=min-is-white toggle the photometric interpretation for a bilevel image. Specify the extra samples as associated or unassociated alpha with, for example, -define tiff:alpha=unassociated. Set the fill order with -define tiff:fill-order=msb|lsb. Set the TIFF endianness with -define tiff:endian=msb|lsb. Use -define tiff:exif-properties=false to skip reading the EXIF properties. You can set a number of TIFF software attributes including document name, host computer, artist, timestamp, make, model, software, and copyright. For example, -set tiff:software "My Company". If you want to ignore certain TIFF tags, use this option: -define tiff:ignore-tags=comma-separated-list-of-tag-IDs. Since version 6.9.1-4 there is support for reading photoshop layers in TIFF files, this can be disabled with -define tiff:ignore-layers=true. To preserve compression of the source image, use: -define tiff:preserve-compression=true.
            ".TIM", //		R	PSX TIM file	
            //".TTF", //		R	TrueType font file	Requires freetype 2. Opening as file returns a preview image. Use -set if you do not want to hint glyph outlines after their scaling to device pixels (e.g. -set type:hinting off).
            //".TXT", //		RW	Raw text file	Use -define to specify the color compliance (e.g. -define txt:compliance=css).
            ".UBRL", //		W	Unicode BRaiLle	Uses juxtaposition of 8-dot braille patterns (thus 8x2 dot matrices) to reproduce images, using the Unicode Braille encoding.
            ".UBRL6", //	W	Unicode BRaiLle 6 dots	Uses juxtaposition of 6-dot braille patterns (thus 6x2 dot matrices) to reproduce images, using the Unicode Braille encoding.
            ".UIL", //		W	X-Motif UIL table	
            ".UYVY", //		RW	Interleaved YUV raw image	Use -size and -depth command line options to specify width and height. Use -sampling-factor to set the desired subsampling (e.g. -sampling-factor 4:2:2).
            ".VICAR", //	RW	VICAR rasterfile format	
            ".VIFF", //		RW	Khoros Visualization Image File Format	
            ".WBMP", //		RW	Wireless bitmap	Support for uncompressed monochrome only.
            //".WDP", //		RW	JPEG extended range	Requires the jxrlib delegate library. Put the JxrDecApp and JxrEncApp applications in your execution path.
            ".WEBP", //		RW	Weppy image format	Requires the WEBP delegate library. Specify the encoding options with the -define option See WebP Encoding Options for more details.
            ".WMF", //		R	Windows Metafile	Requires libwmf. By default, renders WMF files using the dimensions specified by the metafile header. Use the -density option to adjust the output resolution, and thereby adjust the output size. The default output resolution is 72DPI so -density 144 results in an image twice as large as the default. Use -background color to specify the WMF background color (default white) or -texture filename to specify a background texture image.
            ".WPG", //		R	Word Perfect Graphics File	
            ".X", //		RW	display or import an image to or from an X11 server	Use -define to obtain the image from the root window (e.g. -define x:screen=true). Set x:silent=true to turn off the beep when importing an image.
            ".XBM", //		RW	X Windows system bitmap, black and white only	Used by the X Windows System to store monochrome icons.
            ".XCF", //		R	GIMP image	
            ".XPM", //		RW	X Windows system pixmap	Also known as PM. Used by the X Windows System to store color icons.
            ".XWD", //		RW	X Windows system window dump	Used by the X Windows System to save/display screen dumps.
            ".X3F", //		R	Sigma Camera RAW Picture File	
            ".YAML", //		W	human-readable data-serialization language	Include additional attributes about the image with these defines: -define yaml:locate, -define yaml:limit, -define yaml:moments, or -define yaml:features. Specify the JSON model schema version with -define yaml:version. The current version is 1.0.
            ".YCbCr", //	RW	Raw Y, Cb, and Cr samples	Use -size and -depth to specify the image width, height, and depth.
            ".YCbCrA", //	RW	Raw Y, Cb, Cr, and alpha samples	Use -size and -depth to specify the image width, height, and depth.
            ".YUV" //		RW	CCIR 601 4:1:1	Use -size and -depth command line options to specify width, height, and depth. Use -sampling-factor to set the desired subsampling (e.g. -sampling-factor 4:2:2).
        };

        //3GP, ASF, AVCHD, AVI,*.mkv, mov, .mpeg, .mpg, .mpe, mp4, WMV  
        //Video file formats by file extension 
        public static List<string> videoFormats = new List<string>
        {
            //VLC Supported video extetions
            ".ASX", //	Advanced Stream Redirector
            ".DTS", //	Digital Theater Systems Audio File
            ".GXF", //	General eXchange Format
            ".M2V", //	MPEG-2 Video
            ".M3U", //	MP3 Uniform Resource Locator
            ".M4V", //	MPEG-4 Video File
            ".MPEG1", //	MPEG-1 Video
            ".MPEG2", //	MPEG-2 Video
            ".MTS", //	AVCHD Video File
            ".MXF", //	Material eXchange Format
            ".OGM", //	Ogg Multimedia Container File
            ".DIVX", //	DivX Movie
            ".DV", //	Digital Video
            ".FLV", //	Flash Video
            ".M1V", //	MPEG-1 Video
            ".M2TS", //	MPEG-2 Transport Stream Videos
            ".MKV", //	Matroska Video Stream
            ".MOV", //	Apple QuickTime Movie
            ".MPEG4", //	MPEG-4 Video
            ".TS", //	DVD Video
            ".VLC", //	VLC Media Player Data
            ".VOB", //	DVD Video Object
            ".DAT", //	VCD Video
            ".BIN", //	Binary DVD Video
            ".3G2", //	3G Mobile Phone Video
            ".AVI", //	Audio Video Interleave
            ".MPEG", //	MPEG Video
            ".MPG", //	MPEG Video
            ".OGG", //	Ogg Multimedia Container File
            ".3GP", //	3G Mobile Phone Video
            ".WMV", //	Windows Media Video
            ".ASF", //	Advanced Systems Format Video
            ".MOD", //	MOD Audio File
            ".MP4" //	MPEG-4 Part 14 Multimedia Container
            /*
            ".AAF", // – mostly intended to hold edit decisions and rendering information, but can also contain compressed media essence
            ".3GP", // – the most common video format for cell phones
            ".GIF", // – Animated GIF (simple animation; until recently often avoided because of patent problems)
            ".ASF", // – container (enables any form of compression to be used; MPEG-4 is common; video in ASF-containers is also called Windows Media Video (WMV))
            ".AVCHD", // – Advanced Video Codec High Definition
            ".AVI", // – container (a shell, which enables any form of compression to be used)
            ".BIK", // (.bik) – Bink Video file. A video compression system developed by RAD Game Tools
            ".CAM", // – aMSN webcam log file
            ".COLLAB", // – Blackboard Collaborate session recording
            ".DAT", // – video standard data file (automatically created when we attempted to burn as video file on the CD)
            ".DSH", //
            ".DVR-MS", // – Windows XP Media Center Edition's Windows Media Center recorded television format
            ".FLV", // – Flash video (encoded to run in a flash animation)
            ".M1V", // MPEG-1 – Video
            ".M2V", // MPEG-2 – Video
            ".FLA", // – Macromedia Flash (for producing)
            ".FLR", // – (text file which contains scripts extracted from SWF by a free ActionScript decompiler named FLARE)
            ".SOL", // – Adobe Flash shared object ("Flash cookie")
            ".M4V", // – video container file format developed by Apple
            ".MKV", // – Matroska is a container format, which enables any video format such as MPEG-4 ASP or AVC to be used along with other content such as subtitles and detailed meta information
            ".WRAP", //WRAP – MediaForge (*.wrap)
            ".MNG", // – mainly simple animation containing PNG and JPEG objects, often somewhat more complex than animated GIF
            ".MOV", //QuickTime (.mov) – container which enables any form of compression to be used; Sorenson codec is the most common; QTCH is the filetype for cached video and audio streams
            ".MPEG",
            ".MPG",
            ".MPE",
            ".THP", //– Nintendo proprietary movie/video format
            ".MP4", //MPEG-4 Part 14, shortened "MP4" – multimedia container (most often used for Sony's PlayStation Portable and Apple's iPod)
            ".MXF", // – Material Exchange Format (standardized wrapper format for audio/visual material developed by SMPTE)
            ".ROQ", // – used by Quake 3
            ".NSV", // – Nullsoft Streaming Video (media container designed for streaming video content over the Internet)
            ".OGG", // – container, multimedia
            ".RM", // – RealMedia
            ".SVI", // – Samsung video format for portable players
            ".SMI", // – SAMI Caption file (HTML like subtitle for movie files)
            ".SMK", // (.smk) – Smacker video file. A video compression system developed by RAD Game Tools
            ".SWF", // – Macromedia Flash (for viewing)
            ".WMV", // – Windows Media Video (See ASF)
            ".WTV", // – Windows Vista's and up Windows Media Center recorded television format
            ".YUV", // – raw video format; resolution (horizontal x vertical) and sample structure 4:2:2 or 4:2:0 must be known explicitly
            ".WebM" // – video file format for web video using HTML5*/
        };


        private static List<string> GetAllMediaExtentions()
        {
            List<string> allFiles = new List<string>();
            allFiles.AddRange(imageFormats);
            allFiles.AddRange(videoFormats);
            return allFiles;
        }

        public static FileInfo[] ListAllMediaFiles(string directory, bool recursive)
        {
            return GetFilesByExtensions(directory, GetAllMediaExtentions(), recursive);
        }

        public static List<FileEntry> ListAllMediaFileEntries(string directory, bool recursive)
        {
            FileInfo[] filesFoundInDirectory = GetFilesByExtensions(directory, GetAllMediaExtentions(), recursive);
            List<FileEntry> fileEntries = new List<FileEntry>();
            foreach (FileInfo fileInfo in filesFoundInDirectory) fileEntries.Add(new FileEntry(fileInfo.DirectoryName, fileInfo.Name, fileInfo.LastWriteTime));
            return fileEntries;
        }


        public static FileInfo[] GetFilesByExtensions(string folder, List<string> extensions, bool recursive)
        {
            SearchOption searchOption;
            if (recursive) searchOption = SearchOption.AllDirectories;
            else searchOption = SearchOption.TopDirectoryOnly;

            DirectoryInfo dirInfo = new DirectoryInfo(folder);
            FileInfo[] files = dirInfo.GetFiles("*", searchOption).Where(f => extensions.Contains(f.Extension.ToUpper())).ToArray();
            /*
            List<FileEntry> fileList = new List<FileEntry>();
            for (int i = 0; i < files.Length; i++)
            {
                fileList.Add(new FileEntryImage(files[i].FullName, files[i].LastWriteTime));
            }
            fileList.Sort();
            */
            return files;
        }

        public static bool IsVideoFormat(string filename)
        {
            return videoFormats.Contains(Path.GetExtension(filename).ToUpper());
        }

        public static bool IsImageFormat(string filename)
        {
            return imageFormats.Contains(Path.GetExtension(filename).ToUpper());
        }

        public static bool IsMediaFormat(string filename)
        {
            if (IsVideoFormat(filename)) return true;
            return IsImageFormat(filename);
        }
        #endregion

        public static List<string> GetVideoExtension(List<Metadata> metadatas)
        {
            List<string> extentions = new List<string>();

            foreach (Metadata metadata in metadatas)
            {
                if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(metadata.FileFullPath))
                {
                    string extention = Path.GetExtension(metadata.FileFullPath);
                    if (!extentions.Contains(extention)) extentions.Add(extention);
                }
            }
            return extentions;
        }

        public static List<string> GetImageExtension(List<Metadata> metadatas)
        {
            List<string> extentions = new List<string>();

            foreach (Metadata metadata in metadatas)
            {
                if (ImageAndMovieFileExtentionsUtility.IsImageFormat(metadata.FileFullPath))
                {
                    string extention = Path.GetExtension(metadata.FileFullPath);
                    if (!extentions.Contains(extention)) extentions.Add(extention);
                }
            }
            return extentions;
        }
    }
}
