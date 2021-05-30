# What problems PhotoTags Synchronizer want to solve

## Windows Live Photo Gallery
Windows Live Photo Gallery is saving most of the meta information into the media files. That's good, however, we never know when it is not saved. If Windows Live Photo Gallery failed to save the meta information it only stores it on the local database.

PC 1 <br> Windows Live Photo Gallery  | PC 2 <br> Windows Live Photo Gallery
--|--
Looks as all Caption meta information saved properly on PC 1 ![Microsoft Windows Live Gallery - PC1](problem-windows-live-photo-gallery_not-saving-tags2.png) | However, not visble on PC 2, because data is only stored in database  ![Microsoft Windows Live Gallery - PC1](problem-windows-live-photo-gallery_not-saving-tags1.png)

## Microsoft Xtra Atoms
Microsoft Xtra Atoms is Microsoft own standard to write attitonal information to the file. Few programs are able to read it, but almost none are able to write it back except Microsoft.

Exiftool does not support write back Microsoft Xtra Atoms. However, PhotoTags Synchronizer allows you to so, if you want.

## Windows Explorer
![Microsoft Windows Live Gallery - PC1](problem-windows-explorer-windows-live-photo-gallery_not-consistent.png)
1. Windows Live Photo Gallery are not consistent where the field "Caption" is written back. In File Exporter you will sometimes see it "Title" and "subject" field.
2. Sometimes "Caption" filed from Windows Live Photo Gallery are stored "only" on "Title" field
3. "Tags" are "Categories" fields are unsync.
4. Sometimes it's never stored in the media file, but only in local database.


## Cloud Photo and Video Gallerys
It's great having photos and videos automaticlly saved in the cloud. Also to be able to tag the photos and videos. The problems uccures when you want to change cloud proivder.


## Mismatch between Meta information
Mismatch beween meta information that in theroy should be equal


## No standards tags


## Date
