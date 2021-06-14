{% include header.md %}

# Image List View

The image list view will present a set of thumbnails for selected folder or result of your search, and if filters are added thumbnails presented will be updated according to the added filter.

![ImageListView](../userinterface/userinterface-layout-imeglistview.png)

## Thumbnail sizes

You can adjust the thumbnail size so it fits your needs. There are 5 different size to choose from.

![Thumbnail Size ToolStrip](userinterface-imagelistview-thumbnail-size-toolbar.png)

Example of diffrent thumbnail sizes.

Extra small | Small | Medium | Large | Extra Large
--|--|--|--|--
![Extra small](userinterface-imagelistview-extra-small.png) | ![Small](userinterface-imagelistview-small.png) | ![Medium](userinterface-imagelistview-medium.png) | ![Large](userinterface-imagelistview-large.png) | ![Extra large](userinterface-imagelistview-extra-large.png)

## Image List View layout

![Thumbnail Size ToolStrip](userinterface-imagelistview-thumbnail-viewtype-toolbar.png)

Layout | Layout
--|--
Thumbnail <br>![Thumbnails](userinterface-imagelistview-views-thumbnail.png) | Gallery <br> ![Gallery](userinterface-imagelistview-views-gallery.png)
Pane <br> ![Pane](userinterface-imagelistview-views-pane.png) | Details <br> ![Details](userinterface-imagelistview-views-details.png)

### Thumbnail view layout

![a](../toolstrip/toolstrip-imagelistview-thumbnail-view.png)

## Rotate media file

![Rotate media file](../toolstrip/toolstrip-rotate.png)

## Select by group

Fast way to select a group of pictures, so you can edit all at once that most likly are in a group and with more or less same content.

![Select group](../toolstrip/toolstrip-select-group-by.png)

Select group by | Select group by
--|--
![Options](imagelistview-select-group-by-options.png) | ![Click next 1](imagelistview-select-group-by-next-1.png)
![Click next 2](imagelistview-select-group-by-next-2.png) | ![Click next 3](imagelistview-select-group-by-next-3.png)

## Context menu

### Sort media files by...

You can sort your media files by following fields...
- File name
- File created date and time
- File modified date and time
- Media Date Taken
- Media Album
- Media Title
- Media Description
- Media Comments
- Media Author
- Media Rating
- Location name
- Location Region/State
- Location City
- Location Country

### Cut / Copy / Paste
Copy, cut and paste media files. Drag and drop is also possible.

### Delete files
Delete selected media files, from the folder and database.

### Copy filename to clipboard
Copy select filenames to the clipboard, so you can paste it into your favorite applications.

### Refresh Folder
Read all files in the folder again and add found media files in the Image List View.

### Reload thumbnail and metadata
If media information for the selected media files exists in the database with the same “last written date” as the file, then delete this database record, then run the Exiftool again and grab the meta information once more.

### Clear thumbnail and metadata history
Delete all historical meta information in the database, and run ExifTool grabber once more and add the last version into the database.

### Select all
Select all media files.

### AutoCorrect metadata
Run the powerful [AutoCorrect](../autocorrect) tool / algorithm.

### Open
For each selected media file, a Open command with application associated with the file will be run. It can be different applications that can be run depending on file extension.

Open | Result
--|--
![Open](imagelistview-contextmenu-open.png) | ![Open result](imagelistview-contextmenu-open-result.png)

### Open media files with...

For each selected media file, an Open with and selected application will be run for each media file.

Only the applications that are associated with all selected files will be shown, as example pictures below shows.

Associated with jpg | Associated with mp4 | Associated with jpg and mp4
--|--|--
![Open with for jpg](imagelistview-contextmenu-openwith-jpg.png) | ![Open with for mp4](imagelistview-contextmenu-openwith-mp4.png) | ![Open with for jpg and mp4](imagelistview-contextmenu-openwith-mp4-jpg.png)

### Edit
For each selected media file, an Edit command with an application associated with the file will be run. There can be different applications that can be run depending on the file extension.

Edit | Result
--|--
![Open](imagelistview-contextmenu-edit.png) | ![Open result](imagelistview-contextmenu-edit-result.png)

### Run batch; app or command...

### Open and associate dialog...

Associate with | Result
--|--
![associate-with](imagelistview-contextmenu-associate-with.png) | ![associate-with result](imagelistview-contextmenu-associate-with-result.png)

### Open file location

Open file location | Result
--|--
![Open file location](imagelistview-contextmenu-open-file-location.png) | ![Open file location result](imagelistview-contextmenu-open-file-location-result.png)

### Rotate 90, 180, 270 degree

Rotate media file 90, 180, 270 degrees. Please note, the media will be rotated physically not with tags.

### Media preview and chromecast

Open the [Mediapreview & Chromecast](../mediapreview-chromecast) window, that also have possibility to play media on chromecast

{% include footer.md %}
