{% include header.md %}

# Import Location History

![Import](import-location-history.png)
1. Click Import Location History button
2. Write or Select the person that owns this location history.
3. Open the File Dialog
4. Select the file KML or json file you like to import
5. The Name of the person you enter will become a position for Camera Make and Model. Then different cameras can have different location histories.
6. The Camera Owner will also be used as Author on the Meta information

## Camera Owner

See Camera Owner in [Config](../config).

![Config camera owner](../config/config_camera-owner.png)

## Location information

See [GEOtagging user guide](../map/) for more information.
See Location information in [Config](../config)

![Config Location information](../map/config_location-information.png)

## Google Takeout - Location history 
To export Google Location History go to [Google Takeout](https://takeout.google.com/) and then follow this steps.

1. Deselect all, if you only need to export Location History
![Step 01](google-takeout-step01-deselect-all.png)
2. Scroll down and select Location History
![Step 02](google-takeout-step02-select-location-history.png)
3. Then scoll down and click Next Step
![Step 03](google-takeout-step03-next-step.png)
4. Then click "Create Export", then Google will start create a zip file with the Location History
![Step 04](google-takeout-step04-create-export.png)
5. You will get a confirmation screen that the "Takeout" is started. This can take awhile.
![Step 05](google-takeout-step05-export-inprogress.png)
6. You will also get a email notification on your gmail account that the process has started.
![Step 06](google-takeout-step06-email-notification.png)
7. You will also get a security notification email from Google 
![Step 07](google-takeout-step07-email-security-notification.png)
8. When the export zip file is ready, a email will also be sent with a link for download. 
![Step 08](google-takeout-step08-email-data-is-ready.png)
9. After clicking the link, your browser will open on this page. Click "download", and save the zip file or files.
![Step 09](google-takeout-step09-download-file.png)
10. Use File Explorer or other tools to extract the zip file or files.
![Step 10](google-takeout-step10-extract-files.png)
11. Location History.json is the file you can import.
![Step 11](google-takeout-step11-import-this-file.png)


{% include footer.md %}
