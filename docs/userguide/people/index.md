{% include header.md %}

# People and Region

## People and Region user interface
1. Pepole of region names
  - All People and region names with same name will be collected in this row.
  - In case of two or more names are equale new row will be added per equal name.
2. Most used names
  - Easy access to most used named to add new region
3. Added names
  - Easy access to resetly added names to add new region
4. Suggestion of names
  - Suggestions is names from nearby media files. For example you been with a group of people, then names within a short date inteval from other media files will be shown her. That gives easy access to named to add new region.
5. Free line for add new names
  - Didn't find your name, then you can add your new names here to create region for new names.
6. Thumbnails of picture or video files
  - When select a name the selected region will be shown and higlight on the thumbnail.
  - You can select new region inside the thumbnail by left click and drag a new region.
  - Or right click the mouse button and open the "Region selector window"
7. Icons showing where the region are from
  - WebScraping
  - Microsoft Photos
  - Windows Live Photo Gallery
  - Meta information from Exiftool
  - TriState button

![People](people-gui.png)

## Region selector
Select a cell to change a region for a region. A region is often a face of person, but not limited to be a face. In therory it can be any region of intrest, that you want to add name for.

Contect menu |  Select a region
--|--
![Context menu](people-context-menu.png) <br> Right click the mouse button and click "Open the Region Selector". | ![Region selector](people-select-region.png) <br> 1. Select a cell <br> 2. Change the region in thumbnail <br> 3. Or change the region in the "Region Selector"

## TriState buttons

By clicking on the TriState button you can change the state for a given cell, row, column or even all cells.

### Changing the values

- Click on the TriState button within a cell (4), to change the value for given cell (4).
- Click on the TriState button within column (2) header, to change all cells for given column (2).
- Click on the TriState button within row name (3), to change all cells for given row (3).
- Click on TriState botton on top left cell (1) to change content in all cells.

1 | 2 | 2 | 2 | 2
--|--|--|--|--
3 | 4 | 4 | 4 | 4
3 | 4 | 4 | 4 | 4
3 | 4 | 4 | 4 | 4

### Description of button states

- TriState switch - On a region cells
  - ![Region do exist exist in media file](../tristate/tri_state_switch_on.png) The region do exist exist in media file
  - ![Region do exist in media but will be removed](../tristate/tri_state_switch_off_remove.png) The region do exist in media but will be removed
  - ![The region do not exist in media file](../tristate/tri_state_switch_off.png) The region do not exist in media file
  - ![Region do not exist in media file but will be added](../tristate/tri_state_switch_on_add.png) The region do not exist in media file but will be added
- TriState switch - On a column or row
  - ![Row and column are unchanged](../tristate/tri_state_switch_partial.png) Row and column are unchanged
  - ![Something added and deleted in row or column](../tristate/tri_state_switch_add_delete.png) Something added and deleted in row or column
  - ![Some regions are removed in row or column](../tristate/tri_state_switch_off_remove.png) Some regions are removed in row or column
  - ![All regions found will be added for all cells in row or column](../tristate/tri_state_switch_on_add_all.png) All regions found will be added for all cells in row or column
  - ![All regions found will be deleted for all cells in row or column](../tristate/tri_state_switch_on_delete_all.png) All regions found will be deleted for all cells in row or column

{% include footer.md %}
