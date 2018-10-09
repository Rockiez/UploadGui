using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PlayFab.UUnit;
using UploadGui.Commands;

namespace UploadGui.ViewModels
{
    public class UpLoadWinViewModel : NotificationObject
    {


        #region Path of folder or file
        private string _folderPath;
        public string folderPath
        {
            get
            {
                return _folderPath;
            }
            set
            {
                _folderPath = value;
                NotifyPropertyChanged("folderPath");
            }
        }

        private string _currencyPath;
        public string currencyPath
        {
            get
            {
                return _currencyPath;
            }
            set
            {
                _currencyPath = value;
                NotifyPropertyChanged("currencyPath");
            }
        }

        private string _titleSettingsPath;
        public string titleSettingsPath
        {
            get
            {
                return _titleSettingsPath;
            }
            set
            {
                _titleSettingsPath = value;
                NotifyPropertyChanged("titleSettingsPath");
            }
        }

        private string _titleDataPath;
        public string titleDataPath
        {
            get
            {
                return _titleDataPath;
            }
            set
            {
                _titleDataPath = value;
                NotifyPropertyChanged("titleDataPath");
            }
        }

        private string _catalogPath;
        public string catalogPath
        {
            get
            {
                return _catalogPath;
            }
            set
            {
                _catalogPath = value;
                NotifyPropertyChanged("catalogPath");
            }
        }

        private string _dropTablesPath;
        public string dropTablesPath
        {
            get
            {
                return _dropTablesPath;
            }
            set
            {
                _dropTablesPath = value;
                NotifyPropertyChanged("dropTablesPath");
            }
        }

        private string _cloudScriptPath;
        public string cloudScriptPath
        {
            get
            {
                return _cloudScriptPath;
            }
            set
            {
                _cloudScriptPath = value;
                NotifyPropertyChanged("cloudScriptPath");
            }
        }

        private string _titleNewsPath;
        public string titleNewsPath
        {
            get
            {
                return _titleNewsPath;
            }
            set
            {
                _titleNewsPath = value;
                NotifyPropertyChanged("titleNewsPath");
            }
        }

        private string _statsDefPath;
        public string statsDefPath
        {
            get
            {
                return _statsDefPath;
            }
            set
            {
                _statsDefPath = value;
                NotifyPropertyChanged("statsDefPath");
            }
        }

        private string _storesPath;
        public string storesPath
        {
            get
            {
                return _storesPath;
            }
            set
            {
                _storesPath = value;
                NotifyPropertyChanged("storesPath");
            }
        }

        private string _cdnAssetsPath;
        public string cdnAssetsPath
        {
            get
            {
                return _cdnAssetsPath;
            }
            set
            {
                _cdnAssetsPath = value;
                NotifyPropertyChanged("cdnAssetsPath");
            }
        }

        #endregion

        #region Content of contorl
        private string _consoleTBContent;
        public string consoleTBContent
        {
            get
            {
                return _consoleTBContent;
            }
            set
            {
                _consoleTBContent = value;
                NotifyPropertyChanged("consoleTBContent");
            }
        }

        private int _progressBarValue;
        public int progressBarValue
        {
            get
            {
                return _progressBarValue;
            }
            set
            {
                _progressBarValue = value;
                NotifyPropertyChanged("progressBarValue");
            }
        }
        private bool _uploadButtonEnable;
        public bool uploadButtonEnable
        {
            get
            {
                return _uploadButtonEnable;
            }
            set
            {
                _uploadButtonEnable = value;
                NotifyPropertyChanged("uploadButtonEnable");
            }
        }
        private bool _stopUploadButtonEnable;
        public bool stopUploadButtonEnable
        {
            get
            {
                return _stopUploadButtonEnable;
            }
            set
            {
                _stopUploadButtonEnable = value;
                NotifyPropertyChanged("stopUploadButtonEnable");
            }
        }



        #endregion


        #region Commands



        //add these strings after Selected floder
        private const string CurrencyFolderPath = "/Currency.json";
        private const string TitleSettingsFolderPath = "/TitleSettings.json";
        private const string TitleDataFolderPath = "/TitleData.json";
        private const string CatalogFolderPath = "/Catalog.json";
        private const string DropTablesFolderPath = "/DropTables.json";
        private const string CloudScriptFolderPath = "/CloudScript.js";
        private const string TitleNewsFolderPath = "/TitleNews.json";
        private const string StatsDefFolderPath = "/StatisticsDefinitions.json";
        private const string StoresFolderPath = "/Stores.json";
        private const string CdnAssetsFolderPath = "/CdnData.json";

        // Select Folder
        public DelegateCommand AssetFolderSelectCommand { get; set; }
        private void AssetFolder_Select(object sender)
        {
            
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                folderPath = openFileDialog.SelectedPath;
                Directory.SetCurrentDirectory(openFileDialog.SelectedPath);

                currencyPath = Check_File_Basis_FolderPath(folderPath, CurrencyFolderPath);
                titleSettingsPath = Check_File_Basis_FolderPath(folderPath, TitleSettingsFolderPath);
                titleDataPath = Check_File_Basis_FolderPath(folderPath, TitleDataFolderPath);
                catalogPath = Check_File_Basis_FolderPath(folderPath, CatalogFolderPath);
                dropTablesPath = Check_File_Basis_FolderPath(folderPath, DropTablesFolderPath);
                cloudScriptPath = Check_File_Basis_FolderPath(folderPath, CloudScriptFolderPath);
                titleNewsPath = Check_File_Basis_FolderPath(folderPath, TitleNewsFolderPath);
                statsDefPath = Check_File_Basis_FolderPath(folderPath, StatsDefFolderPath);
                storesPath = Check_File_Basis_FolderPath(folderPath, StoresFolderPath);
                cdnAssetsPath = Check_File_Basis_FolderPath(folderPath, CdnAssetsFolderPath);

            }
        }

        //Check  filePath
        private string Check_File_Basis_FolderPath(string prefixPath, string suffixPath)
        {
            string intactPath = prefixPath + suffixPath;
            if (File.Exists(intactPath))
            {
                return intactPath;
            }
            return "";
        }

        #region Select file Functions 

        private string Open_Local_Json()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog() { Filter = "(*.json)|*.json" };
            if (folderPath != "")
            {
                openFileDialog.InitialDirectory = folderPath;
            }
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return "";
        }

        public DelegateCommand CurrencySelectCommand { get; set; }
        private void Currency_Select(object sender) => currencyPath = Open_Local_Json();

        public DelegateCommand CatalogSelectCommand { get; set; }
        private void Catalog_Select(object sender) => catalogPath = Open_Local_Json();

        public DelegateCommand TitleDataSelectCommand { get; set; }
        private void Title_Data_Select(object sender) => titleDataPath = Open_Local_Json();

        public DelegateCommand DropTablesCommand { get; set; }
        private void Drop_Tables_Select(object sender) => dropTablesPath = Open_Local_Json();

        public DelegateCommand CloudScriptSelectCommand { get; set; }
        private void Cloud_Script_Select(object sender) => cloudScriptPath = Open_Local_Json();

        public DelegateCommand TitleNewsSelectCommand { get; set; }
        private void Title_News_Select(object sender) => titleNewsPath = Open_Local_Json();

        public DelegateCommand StatisticsDefinitionsSelectCommand { get; set; }
        private void Statistics_Definitions_Select(object sender) => statsDefPath = Open_Local_Json();

        public DelegateCommand StoresSelectCommand { get; set; }
        private void Stores_Select(object sender) => storesPath = Open_Local_Json();

        public DelegateCommand CdnAssetsSelectCommand { get; set; }
        private void CDN_Assets_Select(object sender) => cdnAssetsPath = Open_Local_Json();

        #endregion


        #region Clean file Functions

        public DelegateCommand CurrencyClearCommand { get; set; }
        private void Currency_Clear(object sender)
        {
            currencyPath = "";
        }

        public DelegateCommand CatalogClearCommand { get; set; }
        private void Catalog_Clear(object sender)
        {
            catalogPath = "";

        }

        public DelegateCommand TitleDataClearCommand { get; set; }
        private void Title_Data_Clear(object sender)
        {
            titleDataPath = "";

        }

        public DelegateCommand DropTablesClearCommand { get; set; }
        private void Drop_Tables_Clear(object sender)
        {
            dropTablesPath = "";

        }

        public DelegateCommand CloudScriptClearCommand { get; set; }
        private void Cloud_Script_Clear(object sender)
        {
            cloudScriptPath = "";

        }

        public DelegateCommand TitleNewsCleanCommand { get; set; }
        private void Title_News_Clean(object sender)
        {
            titleNewsPath = "";

        }

        public DelegateCommand StatisticsDefinitionsCleanCommand { get; set; }
        private void Statistics_Definitions_Clean(object sender)
        {
            statsDefPath = "";

        }

        public DelegateCommand StoresCleanCommand { get; set; }
        private void Stores_Clean(object sender)
        {
            storesPath = "";

        }

        public DelegateCommand CdnAssetsCleanCommand { get; set; }
        private void CDN_Assets_clean(object sender)
        {
            cdnAssetsPath = "";

        }

        #endregion


        //Upload Json file with async
        private Upload _upload;
        private CancellationTokenSource _cancelUploadTokenSource;
        public  DelegateCommand UploadCommand { get; set; }
        private async void Upload(object sender)
        {
            stopUploadButtonEnable = true;
            uploadButtonEnable = false;

            _upload = new Upload(this);
            _cancelUploadTokenSource = new CancellationTokenSource();

            await _upload.UploadAllJson(_cancelUploadTokenSource.Token);

            uploadButtonEnable = true;
            stopUploadButtonEnable = false;
        }




        public DelegateCommand StopCommand { get; set; }
        private void Stop(object sender)
        {
            consoleTBContent = "Now cancelling all tasks \n" + consoleTBContent;
            _cancelUploadTokenSource.Cancel();
        }

        private bool CanUpload(object sender)
        {
            return uploadButtonEnable == true ? true : false;
        }
        private bool CanStop(object sender)
        {
            return stopUploadButtonEnable == true ? true : false;
        }


















        #endregion
        #region Binding

        public UpLoadWinViewModel()
        {
            uploadButtonEnable = true;
            stopUploadButtonEnable = false;
            //Select Command Binding
            AssetFolderSelectCommand = new DelegateCommand
            {
                ExecuteAction = AssetFolder_Select
            };

            CurrencySelectCommand = new DelegateCommand
            {
                ExecuteAction = Currency_Select
            };

            CatalogSelectCommand = new DelegateCommand
            {
                ExecuteAction = Catalog_Select
            };

            TitleDataSelectCommand = new DelegateCommand
            {
                ExecuteAction = Title_Data_Select
            };

            DropTablesCommand = new DelegateCommand
            {
                ExecuteAction = Drop_Tables_Select
            };

            CloudScriptSelectCommand = new DelegateCommand
            {
                ExecuteAction = Cloud_Script_Select
            };

            TitleNewsSelectCommand = new DelegateCommand
            {
                ExecuteAction = Title_News_Select
            };

            StatisticsDefinitionsSelectCommand = new DelegateCommand
            {
                ExecuteAction = Statistics_Definitions_Select
            };

            StoresSelectCommand = new DelegateCommand
            {
                ExecuteAction = Stores_Select
            };

            CdnAssetsSelectCommand = new DelegateCommand
            {
                ExecuteAction = CDN_Assets_Select
            };




            //Clean Command Binding
            CurrencyClearCommand = new DelegateCommand
            {
                ExecuteAction = Currency_Clear
            };

            CatalogClearCommand = new DelegateCommand
            {
                ExecuteAction = Catalog_Clear
            };

            TitleDataClearCommand = new DelegateCommand
            {
                ExecuteAction = Title_Data_Clear
            };

            DropTablesClearCommand = new DelegateCommand
            {
                ExecuteAction = Drop_Tables_Clear
            };

            CloudScriptClearCommand = new DelegateCommand
            {
                ExecuteAction = Cloud_Script_Clear
            };

            TitleNewsCleanCommand = new DelegateCommand
            {
                ExecuteAction = Title_News_Clean
            };
            StatisticsDefinitionsCleanCommand = new DelegateCommand
            {
                ExecuteAction = Statistics_Definitions_Clean
            };
            StoresCleanCommand = new DelegateCommand
            {
                ExecuteAction = Stores_Clean
            };
            CdnAssetsCleanCommand = new DelegateCommand
            {
                ExecuteAction = CDN_Assets_clean
            };

            UploadCommand = new DelegateCommand
            {
                ExecuteAction = Upload,
                CanExecutePre = CanUpload
            };

            StopCommand = new DelegateCommand
            {
                ExecuteAction = Stop,
                //CanExecutePre = CanStop

            };
        }


        #endregion
    }
}
