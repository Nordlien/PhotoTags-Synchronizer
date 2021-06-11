{% include header.md %}

# User interface

## Bulding blocks

ToolStrip
![Toolstrip](userinterface-layout-toolstrip.png)

Filters<br>![Filter](userinterface-layout-filter.png) | Result from filter<br> ![ImageListView](userinterface-layout-imeglistview.png) | Details view and edit for selected media files<br> ![GridView](userinterface-layout-girdview.png)
--|--|--

Statusbar
![Statusbar](userinterface-layout-statusbar.png)


## Fast copy and paste between media files
You are able to copy blocks and paste them for many blocks, as long as number of row or columns are equal.

Select what to copy | Select where to paste | Result
--|--|--
![Copy](userinterface-copy.png) | ![Paste](userinterface-paste.png) | ![Result](userinterface-copy_and_pasted.png)

## Show and hide historical and errors columns
Every time meta information is read the ole information will be keep in the database and new infomration will be created in the database.

If for some reason meta information was not written correctly, the data will be store in the database as an historical column and marked as error

![Thumbnail Size ToolStrip](userinterface-grid_columns-show-and_hide-toolbar.png)

History | Errors | History and errors
--|--|--
![History](userinterface-grid_columns-show_history.png) | ![Errors](userinterface-grid_columns-show_errors.png) | ![History and errors](userinterface-grid_columns-show_history_and_errors.png)


## Mark favorites and hide rows with Equal values
Hide all rows where values for each columns are equal
- Easy compare meta infromation between media files or find changes in historical meta information.

## Grid size

There is 3 diffrent grid view sizes that can easly switch between. Small for getting an overview or larg to see more details, and medium for a combine overview and details.

Small | Medium | Large
--|--|--
![Small](userinterface-gridsize_small.png) | ![Medium](userinterface-gridsize_medium.png) | ![Large](userinterface-gridsize_large.png)

## Thumbnail sizes

You can adjust the thumbnail size so it fits your needs. There are 5 different size to choose from.

![Thumbnail Size ToolStrip](userinterface-imagelistview-thumbnail-size-toolbar.png)

Example of diffrent thumbnail sizes.

Extra small | Small | Medium | Large | Extra Large
--|--|--|--|--
![Extra small](userinterface-imagelistview-extra-small.png) | ![Small](userinterface-imagelistview-small.png) | ![Medium](userinterface-imagelistview-medium.png) | ![Large](userinterface-imagelistview-large.png) | ![Extra large](userinterface-imagelistview-extra-large.png)

## Image List View layout

![Thumbnail Size ToolStrip](userinterface-imagelistview-thumbnail-viewtype-toolbar.png)

{% include footer.md %}
