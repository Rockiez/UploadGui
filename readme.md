# Introduction UploadGui

The tool has a  simple and intuitive GUI interface and designs around to make it easier for users to upload the configuration files to the PlayFab to setup your new title.


The project Refactoring from [UB_Uploader](
https://github.com/PlayFab/UnicornBattle/tree/master/UB_Uploader).

### Download

Click the [link](https://github.com/Rockiez/UploadGui/releases) to download the latest version.

### How to use

1. Unzip and run the .exe file.
2. Enter your email address and password, then click the Login button.

![Login](http://github.com/rockiez/UploadGui/raw/master/images/login.jpg)

3. Select Studio and Title your account have, and the tool will automatically get its developer key from PlayFab.

![selectTitl](http://github.com/rockiez/UploadGui/raw/master/images/selectTitle.jpg)

4. If you select a folder, the tool will automatically find the file which file name map resource in the folder. You can also choose a corresponding data file for a single resource.

![Upload](http://github.com/rockiez/UploadGui/raw/master/images/upload.jpg)

##(Optional) Link to Database
Create a database and link to it can record your login information for fill information automatically next time. You can use this feature on popular and secure devices.
You need to download the source code of the project, build the database locally, and update your database information in App.config. The table design of database needs to map to the Class User in UserAuthenticateModels.cs. For more information, see [linq2db](HTTPS://github.com/linq2db/linq2db).
