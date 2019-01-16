# 简介 UploadGui
该工具致力于利用简洁直观的GUI界面使用户更加方便地将Json文件上传到PlayFab服务器配置Title相关数据。

项目部分代码来自[UB_Uploader](
https://github.com/PlayFab/UnicornBattle/tree/master/UB_Uploader)。

### 下载

点击该[链接](https://github.com/Rockiez/UploadGui/releases)下载最新版本。

### 如何使用

1. 解压并运行.exe文件。
2. 输入您的邮箱及密码，并点击Login按钮。

![login](http://github.com/rockiez/UploadGui/raw/master/images/login.jpg)

3. 选择您账户下的Studio以及Title，该工具回自动获取其Developer Secret Key.

![selectTitle](http://github.com/rockiez/UploadGui/raw/master/images/selectTitle.jpg)

4. 如果选择文件夹，工具会自动寻找该文件夹下**具备对应文件名**的文件。您也可以针对某单一资源选择对应的文件。

![upload](http://github.com/rockiez/UploadGui/raw/master/images/upload.jpg)

## （可选）链接数据库
新建并链接数据库可以记录您的登录信息以供下次登陆的自动填充。您可以在常用且安全的设备上使用该功能。
您需要下载完整的项目源文件，在本地建立数据库，并在App.config中更新您的数据库信息。数据库表格设计需要符合UserAuthenticateModels.cs中的User类，更多信息可以参看[linq2db](https://github.com/linq2db/linq2db)项目中的信息。