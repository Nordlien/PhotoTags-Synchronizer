# Keyword Tags

## Media information

- Media information
  - Media
  - Album
  - Title
  - Description
  - Comments
  - Rating(1-5 stars)
  - Author

### Copy selected values to media with or without overwrite
- When source is Album, it will be pasted into Album field.
- When source is Title, it will be pasted into Title field.
- When source is Rating, it will be pasted into Rating field.

Select | Copy & Paste selection
--|--
![Copy](keywords-copy-and-paste_copy.png) <br>Select your soure | ![Paste and not overwrite](keywords-copy-and-paste_paste-without-overwrite.png) <br> Copy & Paste selection without overwrite exising data
![Copy](keywords-copy-and-paste_copy-webscraping.png) <br> Select your soure | ![Paste and overwrite](keywords-copy-and-paste_paste-webscraping_and_overwrite.png) <br> Copy & Paste selection and overwrite exising data

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

- TriState switch - On a keyword cells
  - ![Keyword do exist exist in media file](../tristate/tri_state_switch_on.png) Keyword do exist exist in media file
  - ![Keyword do exist in media but will be removed](../tristate/tri_state_switch_off_remove.png) Keyword do exist in media but will be removed
  - ![Keyword do not exist in media file](../tristate/tri_state_switch_off.png) Keyword do not exist in media file
  - ![Keyword do not exist in media file but will be added](../tristate/tri_state_switch_on_add.png) Keyword do not exist in media file but will be added
- TriState switch - On a column or row
  - ![Row and column are unchanged](../tristate/tri_state_switch_partial.png) Row and column are unchanged
  - ![Something added and deleted in row or column](../tristate/tri_state_switch_add_delete.png) Something added and deleted in row or column
  - ![Some keywords are removed in row or column](../tristate/tri_state_switch_off_remove.png) Some keywords are removed in row or column
  - ![The keyword are added for all cells in row or column ](../tristate/tri_state_switch_on_add_all.png) The keyword are added for all cells in row or column
  - ![The keyword are delete for all cells in row or column](../tristate/tri_state_switch_on_delete_all.png) The keyword are delete for all cells in row or column

### This view show what keywords are currenly saved in the media files
This you can easily see on top row and left column on the TriState switch: ![Row and column are unchanged](tri_state_switch_partial.png)

![As in file](keywords-tristate-switch_as-in-file.png)


### Example of TriState switch in used
Added tags | Deleted tags
--|--
![Keywords added](tri_state_switch_on_add.png) Keyword(s) are added  ![Keywords added](keywords-tristate-switch_something_added.png) | ![Keywords removed](tri_state_switch_off_remove.png) Keyword(s) are deleted ![Keywords deleted](keywords-tristate-switch_something_deleted.png)
![All added](tri_state_switch_on_add_all.png) All keywords are added ![All added](keywords-tristate-switch_all_added.png) | ![All deleted](tri_state_switch_on_delete_all.png) All keywords are deleted ![All deleted](keywords-tristate-switch_all_deleted.png)

## Add your own keywords
In the last empty row, you can enter a new keyword you want to add.
![Add your own keywords](keywords-add-you-own-keywords.png)
