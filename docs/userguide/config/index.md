# Config

- Application
- Metadata Read
- WebScraper
- Metadata Write
- File date formats
- AutoCorrect
- Camera owner
- Location names
- Convert and merge
- Chromecast

## Application
Here you can change general settings about the applications.
- Thumbnail size. The smaller the thumbnail is, the faster the application will run and small the database will become. But the thumbnail will also become blury when thumbnail will be upsized.
  - Poster Thumbnail size. Size of media file thumbnail. A small version of the picture, or a small version of a frame from the video.
  - Region is often a face of person inside the picture.
- Nominatim look-up
  - What language is prefrerred when looking up location name, region/state, city and contry.
- Search result
  - When search for media files, this will be maximum of media files found. This to avoid working with to media files at once.
- Region name suggestions. See [People/Regions](\userguide\people) for more information.
  - Number of days. When tagging names for people, a shortcut for most likly names are create. People you been together with during last x days, will apper in this list.
  - Number of most common.  When tagging names for people, a shortcut for most used names will apper in this list. Here you can set maximum nuber of names you want to see in this list.
- Region media and list view
  - Avoid read media files from Cloud. If media files are only stored in the cloud, when checked, the application will not load the thumbnail, this avoid for exmaple bid vide files to be download, just to get the get thumbnail.
  - Load Image Thumbnails "on demand". When present thumbnails in the image view, when checked, thumnbail will only be loaded when it will be presented on the screen, otherwise, it will contine the work in background.
- GPS Location Accuracy
  - Latitude
  - Longitude
- Cache logic
  - Number of posters

![Application](config_application.png)

## Metadata Read
All meta information read in this application will be assiged to a value internaly in the application.

![Metadata Read](config_metadata-read.png)

### Example of diffrent standards

As an exmaple: Athur has a few standards where this meta information can be stored.

- Single
  - EXIF:IFD0, XPAuthor
  - IPTC. By-line
  - QuickTime:ItemList, Artist
  - QuickTime:ItemList, Author
- List
  - PDF, Creator
  - XMP:XMP-dc | Creator

### PhotoTags Syncronizer internally overview

Here is an overview what the application use internally:

- File
  - FileName
  - FileDirectory
  - FileSize
  - FileDateCreated
  - FileDateModified
  - FileLastAccessed
  - FileMimeType
- Personal
  - PersonalTitle
  - PersonalDescription
  - PersonalComments
  - PersonalRating
  - PersonalRatingPercent
  - PersonalAuthor
  - PersonalAlbum
  - PersonalRegionList
    - Name
    - Type
    - Region Structure Standard
    - AreaX
    - AreaY
    - AreaWidth
    - AreaHeight
    - Thumbnail
  - PersonalTagList
    - Keyword
    - Confidence (Used by Microsoft Photos, how confidence are they about this keyword)
- Camera
  - CameraMake
  - CameraModel
- Media
  - MediaDateTaken
  - MediaWidth
  - MediaHeight
  - MediaOrientation
  - MediaVideoLength
- Location
  - LocationAltitud
  - LocationLatitude
  - LocationLongitude
  - LocationDateTime
  - LocationName
  - LocationCountry
  - LocationCity
  - LocationRegion/State


### Change how meta infomration is read and used

In theory you can use any meta information where you want. Just assign to the fields you want to use.

Please also note, that there are few diffrent standards data is stored.

As example:
- Just as as text
- List of texts stored in Structured format
- List of texts stored in XML format

### Priority

Because mulitple standards, meta information can become out of sync, depending on how camera and software you used saving the meta information.

If the softwate don't updated all meta information that in theory should be "equal", then you will get data out of sync.

Example:
- Software 1: Updated this EXIF:IFD0, XPAuthor with Name1
- Software 2: Updates IPTC. By-line with Name2
- Software 3: Updateds QuickTime:ItemList, Artist with Name3

By settning all fields as Author, and what priority each meta information has you can solve this.

If you want QuickTime to win, just set hight priority than IPTC and EXIF.

#### warnings
When there are mismatch between meta information that in theory should be equal, a warning will be saved and you can see all warnings in the [Warnings tab](/userguide/warnings)

#### Assign meta information and priority

Drag and Drop From | Drag and drop to
--|--
![Text](config_metadata-read-drag-and-drop-done.png) | ![Text](config_metadata-read-drag-and-drop.png)

Use context menu | Enter priority
--|--
![Text](config_metadata-context-menu.png) | ![Text](config_metadata-read-priority.png) <br> Bigger number = higher priority

#### Easy access in Exiftool and Warnings tab
For easy access and set the values where you see them in the Exiftool and Warnings tab.

Exiftool | Warnings
--|--
![Exiftool tooltip](config_metadata-read-exiftool-tooltip.png) | ![Warnings](config_metadata-read-warnings-tooltip.png)
![Exiftool contextmenu](config_metadata-read-exiftool-contextmenu.png) | ![Warnings](config_metadata-read-warnings-contextmenu.png)

## WebScraper
![WebScraper](config_webscraper.png)

## Metadata Write
![Metadata Write](config_metadata-write.png)

## File date formats
![File date formats](config_file-date-formats.png)

## AutoCorrect
![AutoCorrect](config_autocorrect.png)

## Camera owner
![Camera owner](config_camera-owner.png)

## Location names
![Location names](config_location-names.png)

## Convert and merge
![Convert and merge](config_convert-and-merge.png)

## Chromecast
![Chromecast](config_chromecast.png)
