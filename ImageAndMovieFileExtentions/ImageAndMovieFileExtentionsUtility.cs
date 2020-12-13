using System.IO;
using System.Collections.Generic;
using System.Linq;
using MetadataLibrary;

namespace ImageAndMovieFileExtentions
{
    public static class ImageAndMovieFileExtentionsUtility {
        #region Video and Image Formats
        private static List<string> imageFormats = new List<string> {
            ".ART", //"America Online proprietary format", 
            ".BLP", //"Blizzard Entertainment proprietary texture format"
            ".BMP", //"Microsoft Windows Bitmap formatted image"
            ".BTI", // – Nintendo proprietary texture format
            ".CD5", // – Chasys Draw IES image
            ".CIT", // – Intergraph is a monochrome bitmap format
            ".CPT", // – Corel PHOTO-PAINT image
            ".CR2", // – Canon camera raw format; photos have this on some Canon cameras if the quality RAW is selected in camera settings
            ".CLIP", // – CLIP STUDIO PAINT format
            ".CPL", // – Windows control panel file
            ".DDS", // – DirectX texture file
            ".DIB", // – Device-Independent Bitmap graphic
            ".DJVU", // – DjVu for scanned documents
            ".EGT", // – EGT Universal Document, used in EGT SmartSense to compress PNG files to yet a smaller file
            ".GIF", // – CompuServe's Graphics Interchange Format
            ".GRF", // – Zebra Technologies proprietary format}
            ".ICNS", // – format for icons in macOS. Contains bitmap images at multiple resolutions and bitdepths with alpha channel.
            ".ICO", // – a format used for icons in Microsoft Windows. Contains small bitmap images at multiple resolutions and bitdepths with 1-bit transparency or alpha channel.
            ".IFF",
            ".ILBM",
            ".LBM", // – ILBM
            ".JNG", // – a single-frame MNG using JPEG compression and possibly an alpha channel
            ".JFIF",
            ".JPG",
            ".JEPG", // – Joint Photographic Experts Group; a lossy image format widely used to display photographic images
            ".JP2", // – JPEG2000
            ".JPS", // – JPEG Stereo
            ".LBM", // – Deluxe Paint image file
            ".MAX", // – ScanSoft PaperPort document
            ".MIFF", // – ImageMagick's native file format
            ".MNG", // – Multiple-image Network Graphics, the animated version of PNG
            ".MSP", // – a format used by old versions of Microsoft Paint; replaced by BMP in Microsoft Windows 3.0
            ".NITF", // – A U.S. Government standard commonly used in Intelligence systems
            ".OTB", // – Over The Air bitmap, a specification designed by Nokia for black and white images for mobile phones
            ".PBM", // – Portable bitmap
            ".PC1", // – Low resolution, compressed Degas picture file
            ".PC2", // – Medium resolution, compressed Degas picture file
            ".PC3", // – High resolution, compressed Degas picture file
            ".PCF", // – Pixel Coordination Format
            ".PCX", // – a lossless format used by ZSoft's PC Paint, popular for a time on DOS systems.
            ".PDN", // – Paint.NET image file
            ".PGM", // – Portable graymap
            ".PI1", // – Low resolution, uncompressed Degas picture file
            ".PI2", // – Medium resolution, uncompressed Degas picture file; also Portrait Innovations encrypted image format
            ".PI3", // – High resolution, uncompressed Degas picture file
            ".PICT",
            ".PCT", // – Apple Macintosh PICT image
            ".PNG", // – Portable Network Graphic (lossless, recommended for display and edition of graphic images)
            ".PNM", // – Portable anymap graphic bitmap image
            ".PNS", // – PNG Stereo
            ".PPM", // – Portable Pixmap (Pixel Map) image
            ".PSB", // – Adobe Photoshop Big image file (for large files)
            ".PSD",
            ".PDD", // – Adobe Photoshop Drawing
            ".PSP", // – Paint Shop Pro image
            ".PX", // – Pixel image editor image file
            ".PXM", // – Pixelmator image file
            ".PXR", // – Pixar Image Computer image file
            ".QFX", // – QuickLink Fax image
            ".RAW", // – General term for minimally processed image data (acquired by a digital camera)
            ".RLE", // – a run-length encoding image
            ".SCT", // – Scitex Continuous Tone image file
            ".SGI",
            ".RGB",
            ".INT",
            ".BW", // – Silicon Graphics Image
            ".TGA",
            ".TARGA",
            ".ICB",
            ".VDA",
            ".VST",
            ".PIX", // – Truevision TGA (Targa) image
            ".TIF",
            ".TIFF", // – Tagged Image File Format (usually lossless, but many variants exist, including lossy ones)
                     //(.tif or .tiff) – Tag Image File Format / Electronic Photography, ISO 12234-2; tends to be used as a basis for other formats rather than in its own right.
            ".VTF", // – Valve Texture Format
            ".XBM", // – X Window System Bitmap
            ".XCF", // – GIMP image (from Gimp's origin at the eXperimental Computing Facility of the University of California)
            ".XPM", // – X Window System Pixmap
            ".ZIF" // – Zoomable/Zoomify Image Format (a web-friendly, TIFF-based, zoomable image format) 
        };

        //3GP, ASF, AVCHD, AVI,*.mkv, mov, .mpeg, .mpg, .mpe, mp4, WMV  
        //Video file formats by file extension 
        private static List<string> videoFormats = new List<string>
        {
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
            ".WebM" // – video file format for web video using HTML5
        };

        private static List<string> GetAllMediaExtentions()
        {
            List<string> allFiles = new List<string>();
            allFiles.AddRange(imageFormats);
            allFiles.AddRange(videoFormats);
            return allFiles;
        }

        public static FileEntryImage[] ListAllMediaFiles(string directory, bool recursive)
        {
            return GetFilesByExtensions(directory, GetAllMediaExtentions(), recursive);
        }

        public static FileEntryImage[] GetFilesByExtensions(string folder, List<string> extensions, bool recursive)
        {
            SearchOption searchOption;
            if (recursive) searchOption = SearchOption.AllDirectories;
            else searchOption = SearchOption.TopDirectoryOnly;

            DirectoryInfo dirInfo = new DirectoryInfo(folder);
            FileInfo[] files = dirInfo.GetFiles("*", searchOption).Where(f => extensions.Contains(f.Extension.ToUpper())).ToArray();

            List<FileEntryImage> fileList = new List<FileEntryImage>();
            for (int i = 0; i < files.Length; i++)
            {
                fileList.Add(new FileEntryImage(files[i].FullName, files[i].LastWriteTime));
            }
            //fileList.Sort();

            return fileList.ToArray();
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


/////////////////////////////////////////////////////////////////
//https://github.com/radioman/greatmaps / Maps library
//https://www.nuget.org/packages/Nominatim.API/ Address lockup

//Image reading
//https://imageprocessor.org/imageprocessor/imagefactory/tint/ Image Processor

//Exiftool
//Start Exiftool Process in a better way
//https://github.com/madelson/MedallionShell/ //Star commadn Shell and support encoding
//Another option would be to HTML encode the problematic characters and use exiftool's -E (escapeHTML) option. For example -E -City="&#x158;&#xED;&#x10D;any"
//https://github.com/Ruslan-B/FFmpeg.AutoGen Exiftool Wrapper
//https://github.com/AerisG222/NExifTool Exiftool Wrapper

//FFMPEG
//https://xabe.net/product/xabe-ffmpeg/ FFMPEG Wrapper
//https://www.nrecosite.com/video_converter_net.aspx FFMPEG Embedded
//var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
//ffMpeg.GetVideoThumbnail(pathToVideoFile, thumbJpegStream,5);
/*
    * NReco.VideoConverter (FFMpeg wrapper)
-------------------------------------
Website (release notes, examples etc): https://www.nrecosite.com/video_converter_net.aspx
API documentation: https://www.nrecosite.com/doc/NReco.VideoConverter/
Nuget package: https://www.nuget.org/packages/NReco.VideoConverter/

NReco.VideoConverter (FFMpeg wrapper) - customer settinings / args
https://stackoverflow.com/questions/34234263/how-to-use-nreco-ffmpeg-convertmedia-with-filter-complex
ffMpeg.ConvertMedia(this.Video + ".mov", 
    null, // autodetect by input file extension 
    outPutVideo1 + ".mp4", 
    null, // autodetect by output file extension 
    new NReco.VideoConverter.ConvertSettings() {
        CustomOutputArgs = " -filter_complex \"[0] yadif=0:-1:0,scale=iw*sar:ih,scale='if(gt(a,16/9),1280,-2)':'if(gt(a,16/9),-2,720)'[scaled];[scaled] pad=1280:720:(ow-iw)/2:(oh-ih)/2:black \" -c:v libx264 -c:a mp3 -ab 128k "
    }
);

License
-------
VideoConverter can be used for FREE in single-deployment projects (websites, intranet/extranet) or applications for company's internal business purposes (redistributed only internally inside the company). 
Commercial license (included into enterprise source code pack) is required for:
1) Applications for external redistribution (ISV)
2) SaaS deployments

How to use
----------
var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
ffMpeg.ConvertMedia("input.mov", "output.mp4", Format.mp4);
*/

//Metadata
//Taglib.sharp
//https://github.com/mono/taglib-sharp _ Metadata Read and Write ***** LOT OF FORMATS *****
//Example: Get thumbnail from file -- https://stackoverflow.com/questions/17904184/using-taglib-to-display-the-cover-art-in-a-image-box-in-wpf
//
//https://github.com/drewnoakes/metadata-extractor




