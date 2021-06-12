{% include header.md %}

# User interface

## Bulding blocks

ToolStrip
![Toolstrip](userinterface-layout-toolstrip.png)

1. Filters | 2. Image List View | 3. Grid view
--|--|--
1. Start here. Select your filer. Folder or database search.<br>  ![Filter](userinterface-layout-filter.png) | 2. A view of the result from filter<br> ![ImageListView](userinterface-layout-imeglistview.png) | 3. After select files in the List View, then they will presented in this Details view<br> ![GridView](userinterface-layout-girdview.png)

Statusbar
![Statusbar](userinterface-layout-statusbar.png)

## 1. Filters

There two ways to find your meida files. Select your folder or search for the media files.

You can even filter the result you get in the filter tab.

![Filter](userinterface-layout-filter.png)

Find | Filter
--|--
Find in folder <br> ![Folder](userinterface-filer-folder.png) | After filtering what found in folder <br>![Folder with filter](userinterface-filer-folder-filter.png)
Find with search <br> ![Search](userinterface-filer-search.png) | After filter search result <br> ![Search with filter](userinterface-filer-search-filter.png)

## 2. Image List View

The image list view will present a set of thumbnails for selected folder or result of your search, and if filters are added thumbnails presented will be updated according to the added filter.

![ImageListView](userinterface-layout-imeglistview.png)

### Thumbnail sizes

You can adjust the thumbnail size so it fits your needs. There are 5 different size to choose from.

![Thumbnail Size ToolStrip](userinterface-imagelistview-thumbnail-size-toolbar.png)

Example of diffrent thumbnail sizes.

Extra small | Small | Medium | Large | Extra Large
--|--|--|--|--
![Extra small](userinterface-imagelistview-extra-small.png) | ![Small](userinterface-imagelistview-small.png) | ![Medium](userinterface-imagelistview-medium.png) | ![Large](userinterface-imagelistview-large.png) | ![Extra large](userinterface-imagelistview-extra-large.png)

### Image List View layout

![Thumbnail Size ToolStrip](userinterface-imagelistview-thumbnail-viewtype-toolbar.png)

## 3. Grid View (For all Grid Views thruout the application)

When select what media files you want to work with, it will presented in this view. In some tabs you can edit and view information, in other tabs you can only view information.

List of tabs to select from:
- [Tag](..\keywords)
- [People](..\people)
- [Map](..\map)
- [Date](..\date)
- [ExifTool](..\exiftool)
- [Warnings](..\warnings)
- [Properties](..\properties)
- [Rename](..\renametool)
- [Convert & Merge](..\convert-and-merge)

![GridView](userinterface-layout-girdview.png)

### Fast copy and paste between media files
You are able to copy blocks and paste them for many blocks, as long as number of row or columns are equal.

Select what to copy | Select where to paste | Result
--|--|--
![Copy](userinterface-copy.png) | ![Paste](userinterface-paste.png) | ![Result](userinterface-copy_and_pasted.png)

### Show and hide historical and errors columns
Every time meta information is read the ole information will be keep in the database and new infomration will be created in the database.

If for some reason meta information was not written correctly, the data will be store in the database as an historical column and marked as error

![Thumbnail Size ToolStrip](userinterface-grid_columns-show-and_hide-toolbar.png)

History | Errors | History and errors
--|--|--
![History](userinterface-grid_columns-show_history.png) | ![Errors](userinterface-grid_columns-show_errors.png) | ![History and errors](userinterface-grid_columns-show_history_and_errors.png)


### Mark favorites and hide rows with Equal values
Hide all rows where values for each columns are equal
- Easy compare meta infromation between media files or find changes in historical meta information.

Menu | Result
--|--
1. Mark as favorite <br> 2. Remove as favorite <br> 3. Toggle favorite <br> 4. Show only favorite rows <br> ![Context menu](userinterface-favorite-contextmenu.png) | The favorite rows has a heart symbol and are highlighted <br> ![Favorite highlight](userinterface-favorite-highlight.png)
If you want to compare content between columns, you can select "Hide equal rows". ![Context menu](userinterface-equal-rows-contextmenu.png) | When you hide equal rows, only rows with diffrent value are shown. ![](userinterface-equal-rows-contextmenu-result.png)

### Grid size

There is 3 diffrent grid view sizes that can easly switch between. Small for getting an overview or larg to see more details, and medium for a combine overview and details.

![Grid View sizes](userinterface-gridsize_toolbar.png)

Small | Medium | Large
--|--|--
![Small](userinterface-gridsize_small.png) | ![Medium](userinterface-gridsize_medium.png) | ![Large](userinterface-gridsize_large.png)



{% include footer.md %}
