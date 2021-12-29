using System.Windows.Forms;
using MetadataLibrary;
using DataGridViewGeneric;
using Manina.Windows.Forms;
using static Manina.Windows.Forms.ImageListView;
using System;
using System.IO;
using FileDateTime;
using System.Collections.Generic;
using NLog;
using ApplicationAssociations;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using FileHandeling;

namespace PhotoTagsSynchronizer
{
    public static class DataGridViewHandlerConvertAndMerge
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static string headerConvertAndMergeFilename = "Filename";
        public static string headerConvertAndMergeInfo = "Drag and drop to re-order";

        public static bool HasBeenInitialized { get; set; } = false;
        public static MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        public static FileDateTimeReader FileDateTimeFormats { get; set; }

        public static FilesCutCopyPasteDrag FilesCutCopyPasteDrag { get; set; }

        public static string RenameVaribale { get; set; }

        #region ffmpeg Information
        /*
        Container: MP4
        -c:v libx264
        -c:v libx264rgb 		-> If your input files are RGB, it's the RGB to YUV color space conversion. Use -c:v libx264rgb instead
        -movflags +faststart 	-> YouTube ​recommends using faststart

        Audio bitrates : 
        -b:a 384k

	        Stereo	384 kbps
	        5.1	512 kbps
	
        Audio codec: AAC-LC 
        -c:a aac
        -ac 2
        -ar 48000

	        Channels: Stereo or Stereo + 5.1
	        Sample rate 96 khz or 48 khz

        Video codec: H.264
        -pix_fmt yuv420p -> 4:2:0 chroma subsampling


	        Progressive scan (no interlacing)
	        High Profile
        -profile:v baseline / high
	        2 consecutive B frames
	        Closed GOP. GOP of half the frame rate.
	        CABAC
	        Variable bitrate. No bitrate limit required, although we offer recommended bitrates below for reference
	        Chroma subsampling: 4:2:0
	
        Frame rate
	        Content should be encoded and uploaded using the same frame rate that was used during recording.
	        Common frame rates include: 24, 25, 30, 48, 50 and 60 frames per second (other frame rates are also acceptable).
	        Interlaced content should be deinterlaced before uploading. For example, 1080i60 content should be deinterlaced to 1080p30. 60 interlaced fields per second should be deinterlaced to 30 progressive frames per second.

        -vsync vfr -r 24
	        -filter:v fps=24
	        -crf 24 <- output	‘ntsc’ 30000/1001, ‘pal’ 25/1
	        -framerate is an input per-file option
	        -r can be either an input or output option. As an input option
	        fps filter allows one to alter a stream's framerate while filtering by dropping or duplicating frames to achieve the given rate
	
	
        Bitrate
        -maxrate 8M -bufsize 10M

	        The bitrates below are recommendations for uploads. Audio playback bitrate is not related to video resolution.	
	        Type		Video Bitrate, 		Video Bitrate, 
				        Standard Frame Rate	High Frame Rate
				        (24, 25, 30)		(48, 50, 60)
	        2160p (4K)	35–45 Mbps			53–68 Mbps
	        1440p (2K)	16 Mbps				24 Mbps
	        1080p		8 Mbps				12 Mbps
	        720p		5 Mbps				7.5 Mbps
	        480p		2.5 Mbps			4 Mbps
	        360p		1 Mbps				1.5 Mbps
	


        Resolution and aspect ratio
        -vf "scale=1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2"
	        16:9
	        2160p: 3840 x 2160
	        1440p: 2560 x 1440
	        1080p: 1920 x 1080 -> ‘hd1080’
	        1920x1080
	        720p: 1280 x 720
	        480p: 854 x 480
	        360p: 640 x 360
	        240p: 426 x 240

        Colour space
	        BT.709	BT.709 (H.273 value: 1)	BT.709 (H.273 value 1)	BT.709 (H.273 value 1)


        -y 
        -i "{AudioFileFullPath}" 
        -f concat -safe 0 
        -i "{ArgumentFileFullPath}" 
        -c:v libx264rgb
        -movflags +faststart 
        -b:a 384k
        -c:a aac
        -ac 2
        -ar 48000
        -pix_fmt yuv420p
        -profile:v baseline
        -vsync vfr -r 24
        -maxrate 8M -bufsize 10M
        -vf "scale=1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2"
        -shortest 
        "{TempFileFullPath}"

        Merge Images - Argument
        -y -i "{AudioFileFullPath}" -f concat -safe 0 -i "{ArgumentFileFullPath}" -c:v libx264 -movflags +faststart -b:a 384k -c:a aac -ac 2 -ar 48000 -pix_fmt yuv420p -profile:v baseline -vsync vfr -r 24 -maxrate 8M -bufsize 10M -vf "scale=1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2" -shortest "{TempFileFullPath}"

        Merge Images - ArguFile
        file '{ImageFileFullPath}'
        duration {Duration}

        Convert video - Arguments
        -y -i "{VideoFileFullPath}" -c:v copy -c:v libx264 -movflags +faststart -b:a 384k -c:a aac -ac 2 -ar 48000 -pix_fmt yuv420p -profile:v baseline -vsync vfr -r 24 -maxrate 8M -bufsize 10M -vf "scale=1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2" "{TempFileFullPath}"

        Concat videos - Arguments 
        -y -f concat -safe 0 -i "{ArgumentFileFullPath}" -c:v copy "{TempFileFullPath}"

        Concat videos - ArguFile 
        file '{VideoFileFullPath}' 


        //Problems to solve:
        https://stackoverflow.com/questions/32604886/ffmpeg-concat-protocol-does-not-combine-video-files/32605139
        FFMPEG Concat protocol does not combine video files
        
        So I've tried using the following command to combine 2 video files with the same codec:
        ffmpeg -i "concat:/home/mike/downloads/a1.mp4|/home/mike/downloads/a2.mp4" -c copy "/home/mike/downloads/output.mp4"
        The the result: output.mp4 only contains video from a1.mp4. I also tried 2 or more file but the result is the same. What could be the possible cause for this? Please help
         
        //Solution:
        You cannot concatenate mp4 files directly with the concat protocol because the format doesn't support it. This is intended for mpg or mpeg-ts and similar.

        You can do it if you pass by one of those formats:

        ffmpeg -i input1.mp4 -c copy -bsf:v h264_mp4toannexb -f mpegts intermediate1.ts
        ffmpeg -i input2.mp4 -c copy -bsf:v h264_mp4toannexb -f mpegts intermediate2.ts 
        
        The alternative is to use the concat demuxer which is more flexible (you still need the same codecs for the input files but it can use different containers):   
        ffmpeg -f concat -i mylist.txt -c copy output
         */
        #endregion 

        #region Parameter Arguments
        public static readonly string[] parameterAgruments = {
            "--help",
            "-?",
            "-L",
            "-ab",
            "-abort_on flags",
            "-absf",
            "-ac",
            "-accurate_seek",
            "-acodec",
            "-adrift_threshold",
            "-af",
            "-aframes",
            "-an",
            "-apad",
            "-apre",
            "-aq",
            "-ar",
            "-aspect",
            "-async",
            "-atag",
            "-attach",
            "-autorotate",
            "-b",
            "-benchmark",
            "-benchmark_all",
            "-bitexact",
            "-bits_per_raw_sample",
            "-bsf",
            "-bsfs",
            "-buildconf",
            "-c",
            "-canvas_size",
            "-channel_layout",
            "-chroma_intra_matrix",
            "-codec",
            "-codecs",
            "-colors",
            "-copy_unknown",
            "-copyinkf",
            "-copypriorss",
            "-copytb",
            "-copyts",
            "-cpuflags",
            "-dcodec",
            "-debug_ts",
            "-decoders",
            "-deinterlace",
            "-demuxers",
            "-devices",
            "-dframes",
            "-discard",
            "-disposition",
            "-dn",
            "-dts_delta_threshold",
            "-dts_error_threshold",
            "-dump",
            "-dump_attachment",
            "-enc_time_base",
            "-encoders",
            "-f",
            "-filter",
            "-filter_complex",
            "-filter_complex_script",
            "-filter_complex_threads",
            "-filter_hw_device",
            "-filter_script",
            "-filter_threads",
            "-filters",
            "-find_stream_info",
            "-fix_sub_duration",
            "-force_fps",
            "-force_key_frames timestamps",
            "-formats",
            "-fpre",
            "-frame_drop_threshold",
            "-frames",
            "-fs",
            "-guess_layout_max",
            "-h",
            "-help",
            "-hex",
            "-hide_banner",
            "-hwaccel",
            "-hwaccel_device devicename",
            "-hwaccel_output_format",
            "-hwaccels",
            "-ignore_unknown",
            "-init_hw_device",
            "-inter_matrix matrix",
            "-intra",
            "-intra1",
            "-intra_matrix matrix",
            "-isync",
            "-itsoffset",
            "-itsscale",
            "-lavfi",
            "-layouts",
            "-loglevel",
            "-map",
            "-map_channel",
            "-map_chapters",
            "-map_metadata",
            "-max_alloc bytes",
            "-max_error_rate maximum",
            "-max_muxing_queue_size",
            "-metadata",
            "-muxdelay",
            "-muxers",
            "-muxpreload",
            "-n",
            "-pass",
            "-passlogfile",
            "-pix_fmt",
            "-pix_fmts",
            "-pre",
            "-profile",
            "-program",
            "-progress",
            "-protocols",
            "-psnr",
            "-q",
            "-qphist",
            "-qscale",
            "-qsv_device",
            "-r",
            "-rc_override",
            "-re",
            "-reinit_filter",
            "-report",
            "-s",
            "-same_quant",
            "-sameq",
            "-sample_fmt",
            "-sample_fmts",
            "-scodec",
            "-sdp_file",
            "-seek_timestamp",
            "-shortest",
            "-sinks",
            "-sn",
            "-sources",
            "-spre preset",
            "-ss",
            "-sseof",
            "-stag",
            "-start_at_zero",
            "-stats",
            "-stdin",
            "-stream_loop",
            "-streamid",
            "-t",
            "-tag",
            "-target",
            "-thread_queue_size",
            "-time_base",
            "-timecode",
            "-timelimit",
            "-timestamp",
            "-to",
            "-top",
            "-tvstd",
            "-tvstd standard",
            "-v",
            "-vbsf",
            "-vc",
            "-vcodec",
            "-version",
            "-vf",
            "-vframes",
            "-vn",
            "-vol",
            "-vpre",
            "-vstats",
            "-vstats_file",
            "-vstats_file file",
            "-vstats_version",
            "-vsync",
            "-vtag",
            "-xerror error",
            "-y"
        };
        #endregion

        #region Parameter Values
        public static readonly string[] parameterValues = {
            "1.0",
            "1.0b",
            "1.1",
            "1.2",
            "1.3",
            "1200",
            "1300",
            "1400",
            "16",
            "1600",
            "170m",
            "1b",
            "2.0",
            "2.1",
            "2.2",
            "2020",
            "2020_10",
            "2020_12",
            "2020_cl",
            "2020_ncl",
            "240m",
            "2k_24",
            "2k_48",
            "2level",
            "2nd",
            "2x",
            "3.0",
            "3.1",
            "3.2",
            "3200",
            "4.0",
            "4.1",
            "4.2",
            "4444xq",
            "470bg",
            "470m",
            "4k_24",
            "4level",
            "4th",
            "4thrt",
            "4x",
            "5.0",
            "5.1",
            "5.2",
            "50fm",
            "50kf",
            "5_3",
            "5thrt",
            "6.0",
            "6.1",
            "6.2",
            "601",
            "700B",
            "700C",
            "75fm",
            "75kf",
            "8.5",
            "8level",
            "8th",
            "9_7",
            "A53_CC",
            "AFD",
            "AUDIO_SERVICE_TYPE",
            "Bit_depth",
            "CONTENT_LIGHT_LEVEL",
            "Crest_factor",
            "DC_offset",
            "DISPLAYMATRIX",
            "DOWNMIX_INFO",
            "DYNAMIC_HDR_PLUS",
            "Dynamic_range",
            "Flat_factor",
            "GOP_TIMECODE",
            "ICC_PROFILE",
            "LUFS",
            "MASTERING_DISPLAY_METADATA",
            "MATRIXENCODING",
            "MOTION_VECTORS",
            "Max_difference",
            "Max_level",
            "Mean_difference",
            "Min_difference",
            "Min_level",
            "Number_of_Infs",
            "Number_of_NaNs",
            "Number_of_denormals",
            "Number_of_samples",
            "PANSCAN",
            "Peak_count",
            "Peak_level",
            "QP_TABLE_DATA",
            "QP_TABLE_PROPERTIES",
            "REGIONS_OF_INTEREST",
            "REPLAYGAIN",
            "RMS_difference",
            "RMS_level",
            "RMS_peak",
            "RMS_trough",
            "S12M_TIMECOD",
            "SKIP_SAMPLES",
            "SPHERICAL",
            "STEREO3D",
            "Zero_crossings",
            "Zero_crossings_rate",
            "a_dither",
            "aac_eld",
            "aac_he",
            "aac_he_v2",
            "aac_ld",
            "aac_low",
            "aac_ltp",
            "aac_main",
            "aac_seq_header_detect",
            "aac_ssr",
            "ab2l",
            "ab2r",
            "abl",
            "abr",
            "absolute",
            "ac",
            "accurate_rnd",
            "acolor",
            "adaptive",
            "add",
            "add_keyframe_index",
            "addition",
            "addition128",
            "advanced",
            "advanced_codec_digital_hdtv",
            "advanced_codec_digital_radio",
            "advanced_codec_digital_sdtv",
            "af",
            "aflat",
            "aggressive",
            "agmc",
            "agmd",
            "agmg",
            "agmh",
            "aic",
            "al",
            "album",
            "alg",
            "all",
            "allow_high_depth",
            "allow_profile_mismatch",
            "altivec",
            "altref",
            "always",
            "am",
            "amplitude",
            "amv",
            "and",
            "anmr",
            "any",
            "aobmc",
            "append_list",
            "apple",
            "ar",
            "arbg",
            "arcc",
            "arcd",
            "arcg",
            "arch",
            "area",
            "argg",
            "arib-std-b67",
            "arm",
            "ass",
            "ass_ro_flush_noop",
            "ass_with_timings",
            "atan",
            "audio",
            "auto",
            "autobsf",
            "autodetect",
            "autovariance",
            "autovariance-biased",
            "average",
            "avg",
            "aybc",
            "aybd",
            "aybg",
            "aybh",
            "backward",
            "balance",
            "balanced",
            "bar",
            "bartlett",
            "baseline",
            "basic",
            "bayer",
            "bb",
            "bd",
            "be",
            "begin_only",
            "best",
            "bf",
            "bff",
            "bhann",
            "bharris",
            "bicubic",
            "bicublin",
            "bidir",
            "bilat",
            "bilinear",
            "binary",
            "bit",
            "bitexact",
            "bits",
            "bitstream",
            "black",
            "blackman",
            "blackman_nuttall",
            "blank",
            "blend",
            "blue",
            "blur",
            "bnuttall",
            "bob",
            "bohman",
            "both",
            "bottom",
            "bottomleft",
            "bp",
            "bpm",
            "bradford",
            "brng",
            "brown",
            "bruteforce",
            "bsi",
            "bt",
            "bt1358",
            "bt1361",
            "bt1361e",
            "bt2020",
            "bt2020-10",
            "bt2020-12",
            "bt2020_10bit",
            "bt2020_12bit",
            "bt2020_cl",
            "bt2020_ncl",
            "bt2020c",
            "bt2020nc",
            "bt2020ncl",
            "bt470",
            "bt470bg",
            "bt470m",
            "bt601",
            "bt601-6-525",
            "bt601-6-625",
            "bt709",
            "buffer",
            "buffers",
            "bugs",
            "burn",
            "cabac",
            "cache",
            "canny",
            "captions",
            "careful",
            "cauchy",
            "cavlc",
            "cbp_rd",
            "cbr",
            "cbr_hq",
            "cbr_ld_hq",
            "cbrt",
            "cd",
            "cdt",
            "center",
            "centered",
            "cgop",
            "channel",
            "checkerboard",
            "chl",
            "cholesky",
            "chr",
            "chroma",
            "chroma-derived-c",
            "chroma-derived-nc",
            "chunks",
            "cie1931",
            "cinema2k",
            "cinema4k",
            "cividis",
            "clamp",
            "cm",
            "co",
            "col",
            "color",
            "color2",
            "color3",
            "color4",
            "color5",
            "color_negative",
            "colormix",
            "column",
            "columns",
            "combined",
            "compact",
            "complex",
            "complexity",
            "compliant",
            "component",
            "constqp",
            "constrained",
            "constrained_baseline",
            "constrained_high",
            "convergence",
            "cool",
            "copy",
            "cosine",
            "cover",
            "cprl",
            "cqp",
            "crccheck",
            "crop_bitmap",
            "cross_process",
            "csv",
            "cub",
            "cubic",
            "custom_io",
            "cyclic",
            "darken",
            "darker",
            "dash",
            "data",
            "datetime",
            "dbl",
            "dc",
            "dc_clip",
            "dct",
            "dct264",
            "dct_coeff",
            "dctmax",
            "deblock",
            "decode_copy",
            "decode_drop",
            "decrease",
            "default",
            "default_base_moof",
            "deflate",
            "deinterleave",
            "delay_moov",
            "delete",
            "delete_segments",
            "descriptions",
            "dese",
            "desi",
            "destination",
            "di",
            "dia",
            "diff",
            "difference",
            "difference128",
            "digital_radio",
            "digital_tv",
            "direct",
            "direct_blocksize",
            "disable",
            "disable_chpl",
            "disabled",
            "discardcorrupt",
            "discont_start",
            "divide",
            "dnxhd",
            "dnxhr_444",
            "dnxhr_hq",
            "dnxhr_hqx",
            "dnxhr_lb",
            "dnxhr_sq",
            "do_nothing",
            "dodge",
            "dolby",
            "dolph",
            "dot",
            "dotcrawl",
            "double",
            "down",
            "downward",
            "dplii",
            "dpliiz",
            "drawing",
            "drop",
            "drop_changed",
            "drop_even",
            "drop_odd",
            "ds",
            "dts",
            "dts_96_24",
            "dts_es",
            "dts_hd_hra",
            "dts_hd_ma",
            "dual_channel",
            "dup",
            "dwt53",
            "dwt97",
            "dwt97int",
            "each",
            "ebu",
            "ed",
            "edge",
            "ef",
            "em",
            "emi",
            "empty_moov",
            "endall",
            "epoch",
            "epzs",
            "equal",
            "equator360",
            "er",
            "error_diffusion",
            "error_resilient",
            "esa",
            "esin",
            "estimation",
            "event",
            "exclusion",
            "exhaustive",
            "exp",
            "experimental",
            "explode",
            "export_mvs",
            "expr",
            "ext",
            "extra_slow",
            "extremity",
            "f_weighted",
            "faan",
            "faani",
            "fast",
            "fast_bilinear",
            "faster",
            "fastint",
            "fastseek",
            "faststart",
            "favor_inter",
            "fcc",
            "fdiff",
            "ffconcat",
            "ffmpeg",
            "fiery",
            "film",
            "filter_src",
            "final",
            "fire",
            "first",
            "fixed",
            "flat",
            "flattop",
            "float",
            "floyd_steinberg",
            "flt",
            "flush_packets",
            "fmp4",
            "force_autohint",
            "format",
            "forward",
            "fp",
            "frag_custom",
            "frag_discont",
            "frag_every_frame",
            "frag_keyframe",
            "frame",
            "frame_count_in",
            "frame_count_out",
            "frameseq",
            "freeze",
            "freq",
            "fruit",
            "fsb",
            "fss",
            "full",
            "full_chroma_inp",
            "full_chroma_int",
            "fullframe",
            "gamma",
            "gamma22",
            "gamma28",
            "gauss",
            "gbr",
            "gbrp",
            "generic",
            "genpts",
            "glob",
            "glob_sequence",
            "global_header",
            "global_sidx",
            "glow",
            "gm",
            "gn",
            "good",
            "gop",
            "gradient",
            "grainextract",
            "grainmerge",
            "gray",
            "greater",
            "green",
            "green_metadata",
            "guess_mvs",
            "h264_mode0",
            "h264_mp4toannexb",
            "haar",
            "haar_noshift",
            "hamming",
            "hann",
            "hanning",
            "hap",
            "hap_alpha",
            "hap_q",
            "hard",
            "hardlight",
            "hardmix",
            "hdcd",
            "hdmi",
            "hdtv",
            "heat",
            "heckbert",
            "hevc_digital_hdtv",
            "hevc_hw",
            "hevc_sw",
            "hex",
            "hexbs",
            "hi",
            "high",
            "high444p",
            "high_shibata",
            "hls",
            "hm",
            "horizontal",
            "hp",
            "hpel_chroma",
            "hq",
            "hsin",
            "http",
            "https",
            "hz",
            "i16",
            "i32",
            "icl",
            "icon",
            "icr",
            "ictcp",
            "identity",
            "idr",
            "iec61966-2-1",
            "iec61966-2-4",
            "iec61966_2_1",
            "iec61966_2_4",
            "iedge",
            "if",
            "iframes_only",
            "igndts",
            "ignidx",
            "ignore",
            "ignore_err",
            "ignore_global_advance_width",
            "ignore_level",
            "ignore_transform",
            "ignorecrop",
            "ihsin",
            "ildct",
            "ilme",
            "improved_e_weighted",
            "in",
            "increase",
            "increase_contrast",
            "indep",
            "independent_segments",
            "inf",
            "info",
            "init",
            "initial_discontinuity",
            "input",
            "insert",
            "instant",
            "int",
            "intensity",
            "interlaced",
            "interlacex2",
            "interleave",
            "interleave_bottom",
            "interleave_top",
            "ipar",
            "iqsin",
            "irl",
            "irr",
            "isml",
            "iter",
            "iteration_count",
            "j2k",
            "jedec-p22",
            "joint_stereo",
            "jp2",
            "jpeg",
            "jpeg2000",
            "json",
            "ka",
            "kaiser",
            "keep",
            "keepside",
            "keyframe",
            "lanczos",
            "large",
            "latm",
            "left",
            "left_side",
            "less",
            "levinson",
            "lighten",
            "lighter",
            "limited",
            "lin",
            "line",
            "linear",
            "linear_contrast",
            "linear_design",
            "linearlight",
            "lines",
            "linlin",
            "linlog",
            "lipshitz",
            "list",
            "listen",
            "live",
            "ll",
            "ll_2pass_quality",
            "ll_2pass_size",
            "lle",
            "llhp",
            "llhq",
            "local_header",
            "log",
            "log100",
            "log316",
            "log_sqrt",
            "logarithmic",
            "loglin",
            "loglog",
            "longest",
            "loop",
            "loro",
            "losi",
            "lossless",
            "losslesshp",
            "low_delay",
            "low_power",
            "low_shibata",
            "lowdelay",
            "lowlatency",
            "lowpass",
            "lr>l+r",
            "lr>ll",
            "lr>lr",
            "lr>ms",
            "lr>rl",
            "lr>rr",
            "lrcp",
            "lt",
            "ltrt",
            "luv",
            "lzw",
            "m3u8",
            "ma",
            "mac",
            "magma",
            "main",
            "main10",
            "mainsp",
            "make_non_negative",
            "make_zero",
            "maximum",
            "mb_type",
            "mci",
            "median",
            "medium",
            "medium_contrast",
            "merge",
            "mergex2",
            "metadata",
            "mid",
            "mid_side",
            "middle",
            "mincol",
            "mirror",
            "mixed",
            "ml",
            "mmco",
            "mmx",
            "mnuttall3",
            "modified_e_weighted",
            "modify",
            "momentary",
            "mono",
            "monochrome",
            "moreland",
            "mp4",
            "mpeg",
            "mpeg2_aac_he",
            "mpeg2_aac_low",
            "mpeg2_digital_hdtv",
            "mpeg4_asp",
            "mpeg4_core",
            "mpeg4_main",
            "mpeg4_sp",
            "mpegts",
            "mr",
            "ms",
            "ms>ll",
            "ms>lr",
            "ms>rr",
            "msad",
            "msbc",
            "multich",
            "multiply",
            "multiply128",
            "mv0",
            "mv4",
            "n",
            "n128",
            "n16",
            "n256",
            "n32",
            "n64",
            "naq",
            "native",
            "near",
            "nearest",
            "nebulae",
            "negation",
            "negative",
            "negative_cts_offsets",
            "neighbor",
            "never",
            "new",
            "nns_iterative",
            "nns_recursive",
            "no",
            "no_autohint",
            "no_bitmap",
            "no_duration_filesize",
            "no_hinting",
            "no_metadata",
            "no_padding",
            "no_recurse",
            "no_scale",
            "no_sequence_end",
            "nobuffer",
            "nofade",
            "nofillin",
            "nointra",
            "nokey",
            "nomc",
            "none",
            "noout",
            "noparse",
            "noref",
            "normal",
            "normalized_iteration_count",
            "notindicated",
            "ns",
            "nsse",
            "ntsc",
            "ntss",
            "nuttall",
            "nuttall3",
            "obmc",
            "off",
            "offsetting",
            "omit_endlist",
            "omit_tfhd_offset",
            "on",
            "once",
            "only",
            "opt",
            "optimal",
            "or",
            "orange",
            "original",
            "out",
            "output_corrupt",
            "outz",
            "overlay",
            "overwrite",
            "packbits",
            "pad",
            "paeth",
            "pal",
            "par",
            "parade",
            "partitions",
            "parzen",
            "pass",
            "pat_pmt_at_frames",
            "pc",
            "pc_n",
            "pc_n_ub",
            "pc_u",
            "pcn",
            "pcn_ub",
            "pcnub",
            "pcrl",
            "pd",
            "pe",
            "peak",
            "peak+instant",
            "pedantic",
            "period",
            "periodic_rekey",
            "pf",
            "phoenix",
            "photo",
            "pict",
            "picture",
            "pink",
            "pinlight",
            "plane",
            "plasma",
            "pm",
            "poisson",
            "power",
            "pr",
            "pre_decoder",
            "prefer_tcp",
            "premultiplied",
            "print",
            "print_info",
            "production",
            "prog",
            "program_date_time",
            "progressive",
            "proxy",
            "psnr",
            "pts",
            "qm",
            "qp",
            "qp_rd",
            "qpel",
            "qpel_chroma",
            "qpel_chroma2",
            "qsin",
            "qua",
            "quadratic",
            "quality",
            "queue",
            "quintic",
            "rainbow",
            "rainbows",
            "random",
            "range_def",
            "range_tab",
            "rate",
            "raw",
            "rc",
            "rd",
            "read",
            "realtime",
            "rec2020",
            "rec709",
            "recorded",
            "rect",
            "rectangle",
            "rectangular",
            "reflect",
            "relative",
            "remove",
            "render",
            "repeat",
            "replace",
            "reproduction",
            "res",
            "resend_headers",
            "rext",
            "rfc2190",
            "rgb",
            "riaa",
            "rice",
            "right",
            "right_side",
            "rlcp",
            "rle",
            "rlog",
            "rms",
            "ro",
            "round",
            "round_durations",
            "row",
            "rpcl",
            "rscroll",
            "rtcp_to_source",
            "rtphint",
            "rw",
            "s16x4",
            "s16x6",
            "s32x4",
            "s32x6",
            "s48x6",
            "s8x4",
            "s8x6",
            "sad",
            "same_str",
            "sample",
            "satd",
            "sawdown",
            "sawup",
            "sbs",
            "sbs2l",
            "sbs2r",
            "sbsl",
            "sbsr",
            "sc",
            "scd",
            "scene_change_detect",
            "screen",
            "scroll",
            "search",
            "sec",
            "secam",
            "second_level_segment_duration",
            "second_level_segment_index",
            "second_level_segment_size",
            "select",
            "send_bye",
            "send_field",
            "send_frame",
            "separate",
            "separate_moof",
            "sequence",
            "shibata",
            "shortest",
            "shortterm",
            "showall",
            "side",
            "sierra2",
            "sierra2_4a",
            "simple",
            "simplearm",
            "simplearmv5te",
            "simplearmv6",
            "simpleauto",
            "simplemmx",
            "simpleneon",
            "sin",
            "sinc",
            "sine",
            "single",
            "single_file",
            "sinusoidal",
            "size",
            "skip",
            "skip_manual",
            "skip_rd",
            "skip_rtcp",
            "skip_sidx",
            "skip_trailer",
            "slice",
            "slow",
            "slower",
            "small",
            "smart",
            "smear",
            "smpte",
            "smpte170",
            "smpte170m",
            "smpte2084",
            "smpte2085",
            "smpte240",
            "smpte2400m",
            "smpte240m",
            "smpte274m",
            "smpte296m",
            "smpte347m",
            "smpte349m",
            "smpte428",
            "smpte428_1",
            "smpte431",
            "smpte432",
            "snappy",
            "soft",
            "softlight",
            "sortdts",
            "source",
            "soxr",
            "spatial",
            "speed",
            "spline",
            "split_by_time",
            "sqrt",
            "squ",
            "square",
            "srgb",
            "sse",
            "ssim",
            "stack",
            "standard",
            "start",
            "startcode",
            "starts_with",
            "std_qpel",
            "stereo",
            "stop",
            "straight",
            "strftime",
            "strict",
            "strict_gop",
            "strong",
            "strong_contrast",
            "sub",
            "subtitle",
            "subtitles",
            "subtract",
            "summary",
            "swr",
            "system_b",
            "tab",
            "tanh",
            "tb",
            "tcp",
            "tdls",
            "teletext",
            "temp_file",
            "temporal",
            "terrain",
            "tesa",
            "tetrahedral",
            "text",
            "tf",
            "tff",
            "tgm",
            "thread_ops",
            "time",
            "timebase",
            "timestamped",
            "toggle",
            "top",
            "topleft",
            "tout",
            "track",
            "transcoding",
            "transdiff",
            "tri",
            "triangle",
            "triangular",
            "triangular_hp",
            "trilinear",
            "true",
            "trunc",
            "truncated",
            "ts",
            "tss",
            "tt",
            "tukey",
            "tv",
            "twoloop",
            "ucs",
            "udp",
            "udp_multicast",
            "uhdtv",
            "ultralowlatency",
            "umh",
            "ump4",
            "unaligned",
            "uniform_color",
            "unknown",
            "unofficial",
            "unspecified",
            "up",
            "upward",
            "use_metadata_tags",
            "variance",
            "vbr",
            "vbr_2pass",
            "vbr_hq",
            "vbr_latency",
            "vbr_minqp",
            "vbr_peak",
            "verbose",
            "vertical",
            "vertical_layout",
            "very",
            "veryfast",
            "veryslow",
            "vi",
            "video",
            "vintage",
            "violet",
            "viridis",
            "vividlight",
            "vlc",
            "vo",
            "vod",
            "voip",
            "vonkries",
            "vrep",
            "vsad",
            "vsse",
            "w53",
            "w97",
            "wallclock",
            "weak",
            "weave",
            "webcam",
            "webm",
            "welch",
            "white",
            "widergb",
            "wires",
            "wrap",
            "write",
            "write_colr",
            "write_gama",
            "x_dither",
            "xflat",
            "xml",
            "xone",
            "xor",
            "xvid",
            "xvid_ilace",
            "xvidmmx",
            "xvycc",
            "xyy",
            "ycgco",
            "ycocg",
            "yuv420",
            "yuv420p",
            "yuv420p10",
            "yuv420p12",
            "yuv422",
            "yuv422p",
            "yuv422p10",
            "yuv422p12",
            "yuv444",
            "yuv444p",
            "yuv444p10",
            "yuv444p12",
            "zero",
            "zp"
        };

        #endregion

        #region VariableListType
        public enum VariableListType
        {
            ExeConcatFilesArguments,
            ExeConvertFileArguments,
            ImageArgumentFileListing,
            VideoArgumentFileListing
        }
        #endregion

        #region string[] ListVariables(VariableListType variableListType)
        public static string[] ListVariables(VariableListType variableListType)
        {
            List<string> variables = new List<string>();
            switch (variableListType)
            {
                case VariableListType.ExeConcatFilesArguments:
                    variables.Add("{AudioFileFullPath}");
                    variables.Add("{ArgumentFileFullPath}");
                    variables.Add("{TempFileFullPath}");
                    variables.Add("{TempFilename}");
                    variables.Add("{TempFileWithoutExtension}");
                    variables.Add("{TempFileExtension}");                    
                    variables.Add("{TempFileDirectory}");                   
                    variables.Add("{ResolutionWidth}");
                    variables.Add("{ResolutionHeight}");                    
                    break;

                case VariableListType.ExeConvertFileArguments:
                    variables.Add("{AudioFileFullPath}");
                    
                    variables.Add("{VideoFileFullPath}");
                    variables.Add("{VideoFilename}");
                    variables.Add("{VideoFileWithoutExtension}");
                    variables.Add("{VideoFileExtension}");
                    variables.Add("{VideoFileDirectory}");

                    variables.Add("{TempFileFullPath}");
                    variables.Add("{TempFilename}");
                    variables.Add("{TempFileWithoutExtension}");
                    variables.Add("{TempFileExtension}");
                    variables.Add("{TempFileDirectory}");
                    variables.Add("{ResolutionWidth}");
                    variables.Add("{ResolutionHeight}");

                    break;
                case VariableListType.VideoArgumentFileListing:                    
                    variables.Add("{VideoFileFullPath}");
                    break;
                case VariableListType.ImageArgumentFileListing:           
                    variables.Add("{ImageFileFullPath}");
                    variables.Add("{Duration}");
                    break;
            }
            return variables.ToArray();
        }
        #endregion

        #region ReplaceVariablesInString
        public static string ReplaceVariablesInString(string stringWithVariables, string arguFilename, string musicFileFullPath, string videoFileFullPath, int resolutionWidth, int resolutionHeight, string tempOutputfile)
        {
            string variableReplaced = stringWithVariables;
            variableReplaced = variableReplaced.Replace("{ArgumentFileFullPath}", arguFilename);
            variableReplaced = variableReplaced.Replace("{AudioFileFullPath}", musicFileFullPath);
            
            variableReplaced = variableReplaced.Replace("{VideoFileFullPath}", videoFileFullPath);
            variableReplaced = variableReplaced.Replace("{VideoFilename}", Path.GetFileName(videoFileFullPath));
            variableReplaced = variableReplaced.Replace("{VideoFileWithoutExtension}", Path.GetFileNameWithoutExtension(videoFileFullPath));
            variableReplaced = variableReplaced.Replace("{VideoFileExtension}", Path.GetExtension(videoFileFullPath));
            variableReplaced = variableReplaced.Replace("{VideoFileDirectory}", Path.GetDirectoryName(videoFileFullPath));

            variableReplaced = variableReplaced.Replace("{TempFileFullPath}", tempOutputfile);
            variableReplaced = variableReplaced.Replace("{TempFilename}", Path.GetFileName(tempOutputfile));
            variableReplaced = variableReplaced.Replace("{TempFileWithoutExtension}", Path.GetFileNameWithoutExtension(tempOutputfile));
            variableReplaced = variableReplaced.Replace("{TempFileExtension}", Path.GetExtension(tempOutputfile));
            variableReplaced = variableReplaced.Replace("{TempFileDirectory}", Path.GetDirectoryName(tempOutputfile));

            variableReplaced = variableReplaced.Replace("{ResolutionWidth}", resolutionWidth.ToString());
            variableReplaced = variableReplaced.Replace("{ResolutionHeight}", resolutionHeight.ToString());
            
            return variableReplaced;
        }
        #endregion

        #region ReplaceVariablesInArguFile
        public static string ReplaceVariablesInArguFile(string stringWithVariables, string file, int duration)
        {
            string variableReplaced = stringWithVariables;
            variableReplaced = variableReplaced.Replace("{VideoFileFullPath}", file);
            variableReplaced = variableReplaced.Replace("{ImageFileFullPath}", file);
            variableReplaced = variableReplaced.Replace("{Duration}", duration.ToString());
            return variableReplaced;
        }
        #endregion

        #region ConvertImages
        private static void ConvertImages(PhotoTagsCommonComponets.FormTerminalWindow formTerminalWindow, List<string> imagesFiles,
            string executeFile, string imageArgument,
            string arguFilename, string imageArguFile, int duration)
        {
            int exitCode = int.MinValue;

            if (imagesFiles.Count > 0)
            {
                using (StreamWriter sw = new StreamWriter(arguFilename, false, new UTF8Encoding(false)))
                {
                    foreach (string file in imagesFiles)
                    {                        
                        string arguFileLines = ReplaceVariablesInArguFile(imageArguFile, file, duration);
                        sw.WriteLine(arguFileLines);
                    }
                    //(Due to a quirk, the last image has to be specified twice - the 2nd time without any duration directive)
                    sw.WriteLine("file '"+ imagesFiles[imagesFiles.Count-1] + "'");
                }

                String path = NativeMethods.GetFullPathOfFile(executeFile);
                
                
                string exiftoolOutput = "";

                formTerminalWindow.LogWarning("Command: " + path + " " + imageArgument + "\r\n");

                using (var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = path,
                        Arguments = imageArgument,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        RedirectStandardInput = true,
                        StandardOutputEncoding = Encoding.UTF8
                    }
                })
                {
                    formTerminalWindow.SetProcssToFollow(process);
                    bool result = process.Start();
                    string line;

                    while (!process.StandardError.EndOfStream)
                    {

                        line = process.StandardError.ReadLine();
                        exiftoolOutput += line + "\r\n";
                        formTerminalWindow.LogError(line + "\r\n");
                        //if (!line.StartsWith("Warning")) hasExiftoolErrorMessage = true;
                        Logger.Error("EXIFTOOL WRITE ERROR: " + line);
                        //Application.DoEvents();
                    }

                    while (!process.StandardOutput.EndOfStream)
                    {
                        line = process.StandardOutput.ReadLine();
                        exiftoolOutput += line + "\r\n";
                        formTerminalWindow.LogInfo(line + "\r\n");
                        //if (line.StartsWith("Error")) hasExiftoolErrorMessage = true;
                        Logger.Info("EXIFTOOL WRITE OUTPUT: " + line);
                        //Application.DoEvents();
                    }


                    process.WaitForExit();
                    if ((exitCode = process.ExitCode) != 0)
                    {
                        Logger.Info("process.WaitForExit() " + exitCode);
                    }

                    int count = 100;
                    while (!process.HasExited && count-- > 0) Task.Delay(100).Wait();

                    formTerminalWindow.SetProcssToFollow(null);
                    process.Close();
                    process.Dispose();
                }
                
                formTerminalWindow.LogWarning("Exitcode: " + exitCode + "\r\n");
            }
     
        }
        #endregion

        #region MergeVideos
        private static void MergeVideos(PhotoTagsCommonComponets.FormTerminalWindow formTerminalWindow, List<string> videoFiles,
            string executeFile, string videoArgument,
            string arguFilename, string videoArguFile)
        {
            int exitCode = -1;

            if (videoFiles.Count > 0)
            {
                using (StreamWriter sw = new StreamWriter(arguFilename, false, new UTF8Encoding(false)))
                {
                    foreach (string file in videoFiles)
                    {
                        
                        string arguFileLines = ReplaceVariablesInArguFile(videoArguFile, file, 0);
                        sw.WriteLine(arguFileLines);
                    }
                }

                String path = NativeMethods.GetFullPathOfFile(executeFile);


                string exiftoolOutput = "";

                formTerminalWindow.LogWarning("Command: " + path + " " + videoArgument + "\r\n");

                using (var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = path,
                        Arguments = videoArgument,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        RedirectStandardInput = true,
                        StandardOutputEncoding = Encoding.UTF8
                    }
                })
                {
                    formTerminalWindow.SetProcssToFollow(process);

                    bool result = process.Start();
                    string line;

                    while (!process.StandardError.EndOfStream)
                    {

                        line = process.StandardError.ReadLine();
                        exiftoolOutput += line + "\r\n";
                        formTerminalWindow.LogError(line + "\r\n");
                        //if (!line.StartsWith("Warning")) hasExiftoolErrorMessage = true;
                        Logger.Error("EXIFTOOL WRITE ERROR: " + line);
                        //Application.DoEvents();
                    }

                    while (!process.StandardOutput.EndOfStream)
                    {
                        line = process.StandardOutput.ReadLine();
                        exiftoolOutput += line + "\r\n";
                        formTerminalWindow.LogInfo(line + "\r\n");
                        //if (line.StartsWith("Error")) hasExiftoolErrorMessage = true;
                        Logger.Info("EXIFTOOL WRITE OUTPUT: " + line);
                        //Application.DoEvents();
                    }

                    process.WaitForExit();
                    if ((exitCode = process.ExitCode) != 0)
                    {
                        Logger.Info("process.WaitForExit() " + exitCode);
                    }

                    int count = 100;
                    while (!process.HasExited && count-- > 0) Task.Delay(100).Wait();

                    formTerminalWindow.SetProcssToFollow(null);
                    process.Close();
                    process.Dispose();
                }
                formTerminalWindow.LogWarning("Exitcode: " + exitCode + "\r\n");
            }

        }
        #endregion 

        #region ConvertVideo
        private static void ConvertVideo(PhotoTagsCommonComponets.FormTerminalWindow formTerminalWindow, string executeFile, string videoConvertArgument)
        {
            int exitCode = -1;

            String path = NativeMethods.GetFullPathOfFile(executeFile);

            string exiftoolOutput = "";

            formTerminalWindow.LogWarning("Command: " + path + " " + videoConvertArgument + "\r\n");

            using (var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = path,
                    Arguments = videoConvertArgument,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    StandardOutputEncoding = Encoding.UTF8
                }
            })
            {
                formTerminalWindow.SetProcssToFollow(process);

                bool result = process.Start();
                string line;

                while (!process.StandardError.EndOfStream)
                {

                    line = process.StandardError.ReadLine();
                    exiftoolOutput += line + "\r\n";
                    formTerminalWindow.LogError(line + "\r\n");
                    Logger.Error("EXIFTOOL WRITE ERROR: " + line);
                    //Application.DoEvents();
                }

                while (!process.StandardOutput.EndOfStream)
                {
                    line = process.StandardOutput.ReadLine();
                    exiftoolOutput += line + "\r\n";
                    formTerminalWindow.LogInfo(line + "\r\n");
                    //if (line.StartsWith("Error")) hasExiftoolErrorMessage = true;
                    Logger.Info("EXIFTOOL WRITE OUTPUT: " + line);
                    //Application.DoEvents();
                }

                process.WaitForExit();
                if ((exitCode = process.ExitCode) != 0)
                {
                    Logger.Info("process.WaitForExit() " + exitCode);
                }

                int count = 100;
                while (!process.HasExited && count-- > 0) Task.Delay(100).Wait();

                formTerminalWindow.SetProcssToFollow(null);
                process.Close();
                process.Dispose();
            }
            formTerminalWindow.LogWarning("Exitcode: " + exitCode + "\r\n");


        }
        #endregion

        #region ConvertAndMerge
        public static void ConvertAndMerge(
            List<string> files, string executeFile, string musicFile, int duration,
            int resolutionWidth, int resolutionHeight, string tempfileExtension,
            string videoMergeArgument, string videoMergeArguFile, 
            string imageConcatArgument, string imageConcatArguFile,
            string videoCovertArgument, string outputFile)
        {

            string arguFilename = FileHandler.GetLocalApplicationDataPath("ffmpeg_arg.txt", true, null);
            string musicFileFullPath = NativeMethods.GetFullPathOfFile(musicFile);
            string outputFolder = Path.GetDirectoryName(outputFile);

            PhotoTagsCommonComponets.FormTerminalWindow formTerminalWindow = new PhotoTagsCommonComponets.FormTerminalWindow();
            formTerminalWindow.Show();

            
            List<string> tempFiles = new List<string>();
            List<string> videoFiles = new List<string>();

            int indexStartNext = 0;
            while (indexStartNext < files.Count && !formTerminalWindow.GetWasProcessKilled())
            {
                if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsVideoFormat(files[indexStartNext]))
                {
                    bool converted = true;
                    string tempOutputfile = Path.Combine(outputFolder, "temp_" + Guid.NewGuid().ToString() + tempfileExtension);
                    string videoMergeArgumentReplaced = ReplaceVariablesInString(videoCovertArgument, "WRONG VARIABLE - NO ARGU FILE", musicFileFullPath, files[indexStartNext], resolutionWidth, resolutionHeight, tempOutputfile);
                    ConvertVideo(formTerminalWindow, executeFile, videoMergeArgumentReplaced);

                    if (converted)
                    {
                        videoFiles.Add(tempOutputfile); //Add converted video file to merge queue
                        tempFiles.Add(tempOutputfile); //Add temp file to delete queue
                    }
                    else videoFiles.Add(files[indexStartNext]); //If NOT NEED not conveted video file add to merge queue

                    indexStartNext++;
                }
                else
                {
                    //If image, convert to video and and to merge queue
                    List<string> imagesFiles = new List<string>();
                    for (int indexMediaFile = indexStartNext; indexMediaFile < files.Count; indexMediaFile++)
                    {
                        indexStartNext = indexMediaFile + 1;
                        if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsImageFormat(files[indexMediaFile])) imagesFiles.Add(files[indexMediaFile]);
                        if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsVideoFormat(files[indexMediaFile]))
                        {
                            indexStartNext--;
                            break;
                        }
                    }

                    string tempOutputfile = Path.Combine(outputFolder, "temp_" + Guid.NewGuid().ToString() + tempfileExtension);
                    videoFiles.Add(tempOutputfile);
                    tempFiles.Add(tempOutputfile);

                    string imageArgumentReplace = ReplaceVariablesInString(imageConcatArgument, arguFilename, musicFileFullPath, "WRONG VARIABLE - USE ARGU FILE", resolutionWidth, resolutionHeight, tempOutputfile);
                    ConvertImages(formTerminalWindow, imagesFiles, executeFile, imageArgumentReplace, arguFilename, imageConcatArguFile, duration);
                    
                }
            }

            if (!formTerminalWindow.GetWasProcessKilled())
            {
                /*if (tempFiles.Count == -1)
                {
                    try
                    {
                        File.Delete(outputFile);
                        File.Move(tempFiles[0], outputFile);
                        formTerminalWindow.LogInfo("Rename file from: " + tempFiles[0] + " to: " + outputFile + "\r\n");
                    }
                    catch (Exception ex)
                    {
                        formTerminalWindow.LogInfo(ex.Message + "\r\n");
                    }
                }
                else 
                { */
                    string videoArgumentReplace = ReplaceVariablesInString(videoMergeArgument, arguFilename, "WRONG VARIABLE - NO ARGU FILE",  musicFileFullPath, resolutionWidth, resolutionHeight, outputFile);
                    MergeVideos(formTerminalWindow, videoFiles, executeFile, videoArgumentReplace, arguFilename, videoMergeArguFile);

                    foreach (string tempfile in tempFiles)
                    {
                        try
                        {
                            File.Delete(tempfile);
                            formTerminalWindow.LogInfo("Temp file deleted: " + tempfile + "\r\n");
                        }
                        catch (Exception ex)
                        {
                            formTerminalWindow.LogInfo(ex.Message + "\r\n");
                        }
                    }
                //}
            }

            formTerminalWindow.LogInfo("\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\nThe process is complete, you can close the window.\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n");
            formTerminalWindow.GoEnd();
        }
        #endregion

        #region Write
        /*(int)Properties.Settings.Default.ConvertAndMergeOutputWidth,
                    (int)Properties.Settings.Default.ConvertAndMergeOutputHeight,
                    Properties.Settings.Default.ConvertAndMergeOutputTempfileExtension,*/
        public static void Write(DataGridView dataGridView,
            string executeFile, string musicFile, int duration,
            int resolutionWidth, int resolutionHeight, string tempfileExtension,
            string videoMergeArgument, string videoMergeArguFile,
            string imageConcatArgument, string imageConcatArguFile,
            string videoConvertArgument, string outputFile)
        {
            int columnIndex = DataGridViewHandler.GetColumnIndexFirst(dataGridView, headerConvertAndMergeFilename);
            if (columnIndex == -1) return;

            List<string> files = new List<string>();

            for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                //DataGridViewGenericCell cellGridViewGenericCell = DataGridViewHandler.GetCellDataGridViewGenericCellCopy(dataGridView, columnIndex, rowIndex);
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                if (dataGridViewGenericRow != null && dataGridViewGenericRow.IsHeader == false) files.Add(dataGridViewGenericRow.RowName);
            }
           
            ConvertAndMerge(files, executeFile, musicFile, duration, resolutionWidth, resolutionHeight, tempfileExtension, videoMergeArgument, videoMergeArguFile, imageConcatArgument, imageConcatArguFile, videoConvertArgument, outputFile);
        }
        #endregion

        #region AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, bool sort)
        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, bool sort)
        {
            return DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, sort);
        }
        #endregion

        #region AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, bool cellReadOnly)
        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, bool cellReadOnly)
        {
            return AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, value, new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, cellReadOnly));
        }
        #endregion

        #region AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefaults)
        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefaults)
        {
            int rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, value, dataGridViewGenericCellStatusDefaults, true);
            return rowIndex;
        }
        #endregion 

        #region PopulateFile
        public static int PopulateFile(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute)
        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return -1;
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return -1;      //Not default columns or rows added
            if (DataGridViewHandler.GetIsPopulatingFile(dataGridView)) return -1;  //In progress doing so

            //Check if file is in DataGridView, and needs updated
            if (!DataGridViewHandler.DoesColumnFilenameExist(dataGridView, headerConvertAndMergeFilename)) return -1;

            //When file found, Tell it's populating file, avoid two process updates
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, true);
            //-----------------------------------------------------------------

            
            int columnIndex = DataGridViewHandler.GetColumnIndexFirst(dataGridView, headerConvertAndMergeFilename);

            AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerConvertAndMergeInfo), true);

            if (columnIndex != -1)
            {
                Metadata metadata = DatabaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryAttribute.GetFileEntryBroker(MetadataBrokerType.ExifTool));
                //string directory = fileEntryAttribute.Directory;
                string filename = fileEntryAttribute.FileName;
                //Media
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerConvertAndMergeInfo, fileEntryAttribute.FileFullPath, metadata, fileEntryAttribute), filename, true);
            }

            //-----------------------------------------------------------------
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, false);
            //-----------------------------------------------------------------
            return columnIndex;
        }
        #endregion 

        #region PopulateSelectedFiles
        public static void PopulateSelectedFiles(DataGridView dataGridView, HashSet<FileEntry> imageListViewSelectItems, DataGridViewSize dataGridViewSize, ShowWhatColumns showWhatColumns)
        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return;
            if (DataGridViewHandler.GetIsAgregated(dataGridView)) return;
            if (DataGridViewHandler.GetIsPopulating(dataGridView)) return;
            //Tell that work in progress, can start a new before done.
            DataGridViewHandler.SetIsPopulating(dataGridView, true);
            //Clear current DataGridView
            DataGridViewHandler.Clear(dataGridView, dataGridViewSize);
            //Add Columns for all selected files, one column per select file
            
            DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute(headerConvertAndMergeFilename, DateTime.Now, FileEntryVersion.CurrentVersionInDatabase), null, null,
                ReadWriteAccess.AllowCellReadAndWrite, showWhatColumns, new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true), out _);

            //Tell data default columns and rows are agregated
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //-----------------------------------------------------------------

            foreach (FileEntry imageListViewItem in imageListViewSelectItems)
            {
                PopulateFile(dataGridView, new FileEntryAttribute(imageListViewItem.FileFullPath, imageListViewItem.LastWriteDateTime, FileEntryVersion.CurrentVersionInDatabase));
            }


            //-----------------------------------------------------------------
            //Unlock
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //Tell that work is done
            DataGridViewHandler.SetIsPopulating(dataGridView, false);
            //-----------------------------------------------------------------
        }
        #endregion 
    }
}
