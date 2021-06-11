{% include header.md %}

# AutoCorrect

## GPS Location - The algorithm

### Location History for Camera owner
When importing GPS location history, you will import GPS location history for a given person. For example Name of Wife, Name of Husband, Name of Child, etc.

When you shoot a picture or video with a camera, a Camera Make and Model tag is set by the manufacturer of the digital camera. That means pictures and videos using this camera or phone will use one set of GPS history and pictures and videos using another camera will use another GPS history. In short, your camera, your GPS history, someone else's camera, someone else GPS history.

### Media taken - UTC date time
GPS locations are saved with UTC date and time, and GPS location for that time. Therefore we will need to have the UTC date time for when the picture was taken. If UTC date and time is missing, then we need to start guessing.

Based on your GPS location history, we will search for GPS locations nearby date and time picture was taken, if we GPS location was found then this will lookup the Time Zone for Latitude and Longitude found using [TimeZoneConverter, TimeZoneNames and
GeoTimeZone by Matt Johnson-Pint](https://github.com/mattjohnsonpint).

NB: this will only work, if you have GPS location history, and the time zone found will be most likely. If you always are in the time zone, the time zone will be correct.

### Find you GPS location
When we have UTC date and time for when a picture or video was taken, we know the camera owner and have GPS location history for this camera owne, we can start to estimate the GPS location for where this picture or video was taken.

Using the UTC date and time, we will lock-up to locations closed to given UTC date and time in the GPS location history. Then we calculate a location point between these two locations we found. If media files UTC date and time are 10 seconds from location point A and B. Then the location will become in the middle, if the UTC date and time are only 1 second from point A and 19 second from point B, the new location much closer to point A.

## Date and Time Digitized
Updating the data and time media was taken. The date and time taken for the media file will use the first field where data and time exist according to your preferences.

Date Taken  (Date and Time Digitized)
UTC Date and Time when have GPS location and using location time zone
First date and time found in the filename
Last date and time found in the filename

## Location name, city, region, country
All locations found will be saved in the database.

If the new location is found, the Location name, city, region, country will be looked up using [Nominatim.API](https://nominatim.org/)

In the config, you can change what Location name, city, region, country you what to be used when for a given GEO location. Next time a media with this GEOlocation is found, the Location name, city, region, country from the database will be used.

## People / Region
A region is often used to mark a face and name of the person, but in theory can be used to mark anything of interest in the picture. You can set in config, how much the region can differ in size to be accepted as a match.

- When a Region from Microsoft Photos doesn't already exist, then this will be added.
- When a Region from Windows live Photo Gallery doesn't already exist, then this will be added.
- When a Region name from Web Scraping doesn’t exist, this region with size of the picture will be added. The region will not be correct, but it will be searchable and you can resize it later.

## Keywords
In config you can select what source you accept new keywords from.
- When a Keyword from Microsoft Photos doesn't already exist, then this will be added. Keywords from Microsoft Photos that will be added are also configured by Confidence setting.
- When a Keyword from Windows live Photo Gallery doesn't already exist, then this will be added.
- When a Keyword from Web Scraping doesn’t exist, this keyword will be added.

### Backup - Date Taken and UTC date and time
- If configure to do backup, extra keywords will be create with Date Taken
- If configure to do backup, extra keywords will be create with UTC date and time

## Title
Depending what priority of source is in config, Title information will be set accordingly.

- Title already in the media file
- Microsoft Photos
- Windows Live Photo Gallery
- Web Scraping

## Album
Depending what priority of source is in config, Album information will be set accordingly.

- Title already in the media file
- Microsoft Photos
- Windows Live Photo Gallery
- Web Scraping
- Folder name
  - Example: C:\Pictures\My Album 1\picture.jpg will create album name: “My Album 1”

## Author
Whem config is set to upded, and when Camera Make and Model are assigned to a person, this name will be used as the Author on the picture or video recording.

## Rename tool

After the media files have been updated the [Rename tool](userguide/renametool) will be run.

{% include footer.md %}
