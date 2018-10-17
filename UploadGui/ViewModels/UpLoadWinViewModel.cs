using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using UploadGui.Commands;
using UploadGui.Models;
using UploadGui.Services;

namespace UploadGui.ViewModels
{
    public class UpLoadWinViewModel : NotificationObject
    {


        #region Path of folder or file
        private string _folderPath;
        public string FolderPath
        {
            get
            {
                return _folderPath;
            }
            set
            {
                _folderPath = value;
                NotifyPropertyChanged(nameof(FolderPath));
            }
        }

        private string _currencyPath;
        public string CurrencyPath
        {
            get
            {
                return _currencyPath;
            }
            set
            {
                _currencyPath = value;
                NotifyPropertyChanged(nameof(CurrencyPath));
            }
        }

        private string _titleSettingsPath;
        public string TitleSettingsPath
        {
            get
            {
                return _titleSettingsPath;
            }
            set
            {
                _titleSettingsPath = value;
                NotifyPropertyChanged(nameof(TitleSettingsPath));
            }
        }

        private string _titleDataPath;
        public string TitleDataPath
        {
            get
            {
                return _titleDataPath;
            }
            set
            {
                _titleDataPath = value;
                NotifyPropertyChanged(nameof(TitleDataPath));
            }
        }

        private string _catalogPath;
        public string CatalogPath
        {
            get
            {
                return _catalogPath;
            }
            set
            {
                _catalogPath = value;
                NotifyPropertyChanged(nameof(CatalogPath));
            }
        }

        private string _dropTablesPath;
        public string DropTablesPath
        {
            get
            {
                return _dropTablesPath;
            }
            set
            {
                _dropTablesPath = value;
                NotifyPropertyChanged(nameof(DropTablesPath));
            }
        }

        private string _cloudScriptPath;
        public string CloudScriptPath
        {
            get
            {
                return _cloudScriptPath;
            }
            set
            {
                _cloudScriptPath = value;
                NotifyPropertyChanged(nameof(CloudScriptPath));
            }
        }

        private string _titleNewsPath;
        public string TitleNewsPath
        {
            get
            {
                return _titleNewsPath;
            }
            set
            {
                _titleNewsPath = value;
                NotifyPropertyChanged(nameof(TitleNewsPath));
            }
        }

        private string _statsDefPath;
        public string StatsDefPath
        {
            get
            {
                return _statsDefPath;
            }
            set
            {
                _statsDefPath = value;
                NotifyPropertyChanged(nameof(StatsDefPath));
            }
        }

        private string _storesPath;
        public string StoresPath
        {
            get
            {
                return _storesPath;
            }
            set
            {
                _storesPath = value;
                NotifyPropertyChanged(nameof(StoresPath));
            }
        }

        private string _cdnAssetsPath;
        public string CdnAssetsPath
        {
            get
            {
                return _cdnAssetsPath;
            }
            set
            {
                _cdnAssetsPath = value;
                NotifyPropertyChanged(nameof(CdnAssetsPath));
            }
        }

        #endregion

        #region Content of contorl
        private string _consoleTBContent;
        public string ConsoleTbContent
        {
            get
            {
                return _consoleTBContent;
            }
            set
            {
                _consoleTBContent = value;
                NotifyPropertyChanged(nameof(ConsoleTbContent));
            }
        }

        private int _progressBarValue;
        public int ProgressBarValue
        {
            get
            {
                return _progressBarValue;
            }
            set
            {
                _progressBarValue = value;
                NotifyPropertyChanged(nameof(ProgressBarValue));
            }
        }
        private bool _uploadButtonEnable;
        public bool UploadButtonEnable
        {
            get
            {
                return _uploadButtonEnable;
            }
            set
            {
                _uploadButtonEnable = value;
                NotifyPropertyChanged(nameof(UploadButtonEnable));
            }
        }
        private bool _stopUploadButtonEnable;
        public bool StopUploadButtonEnable
        {
            get
            {
                return _stopUploadButtonEnable;
            }
            set
            {
                _stopUploadButtonEnable = value;
                NotifyPropertyChanged(nameof(StopUploadButtonEnable));
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
                FolderPath = openFileDialog.SelectedPath;
                Directory.SetCurrentDirectory(openFileDialog.SelectedPath);

                CurrencyPath = Check_File_Basis_FolderPath(FolderPath, CurrencyFolderPath);
                TitleSettingsPath = Check_File_Basis_FolderPath(FolderPath, TitleSettingsFolderPath);
                TitleDataPath = Check_File_Basis_FolderPath(FolderPath, TitleDataFolderPath);
                CatalogPath = Check_File_Basis_FolderPath(FolderPath, CatalogFolderPath);
                DropTablesPath = Check_File_Basis_FolderPath(FolderPath, DropTablesFolderPath);
                CloudScriptPath = Check_File_Basis_FolderPath(FolderPath, CloudScriptFolderPath);
                TitleNewsPath = Check_File_Basis_FolderPath(FolderPath, TitleNewsFolderPath);
                StatsDefPath = Check_File_Basis_FolderPath(FolderPath, StatsDefFolderPath);
                StoresPath = Check_File_Basis_FolderPath(FolderPath, StoresFolderPath);
                CdnAssetsPath = Check_File_Basis_FolderPath(FolderPath, CdnAssetsFolderPath);

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

        

        private string Open_Local_Json()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog() { Filter = "(*.json)|*.json" };
            if (FolderPath != "")
            {
                openFileDialog.InitialDirectory = FolderPath;
            }
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return "";
        }
        #region Select file Functions 
        //public DelegateCommand CurrencySelectCommand { get; set; }
        //private void Currency_Select(object sender) => currencyPath = Open_Local_Json();

        //public DelegateCommand CatalogSelectCommand { get; set; }
        //private void Catalog_Select(object sender) => catalogPath = Open_Local_Json();

        //public DelegateCommand TitleDataSelectCommand { get; set; }
        //private void Title_Data_Select(object sender) => titleDataPath = Open_Local_Json();

        //public DelegateCommand DropTablesCommand { get; set; }
        //private void Drop_Tables_Select(object sender) => dropTablesPath = Open_Local_Json();

        //public DelegateCommand CloudScriptSelectCommand { get; set; }
        //private void Cloud_Script_Select(object sender) => cloudScriptPath = Open_Local_Json();

        //public DelegateCommand TitleNewsSelectCommand { get; set; }
        //private void Title_News_Select(object sender) => titleNewsPath = Open_Local_Json();

        //public DelegateCommand StatisticsDefinitionsSelectCommand { get; set; }
        //private void Statistics_Definitions_Select(object sender) => statsDefPath = Open_Local_Json();

        //public DelegateCommand StoresSelectCommand { get; set; }
        //private void Stores_Select(object sender) => storesPath = Open_Local_Json();
        //public DelegateCommand CdnAssetsSelectCommand { get; set; }
        //private void CDN_Assets_Select(object sender) => cdnAssetsPath = Open_Local_Json();

        #endregion

        // No MVVM but Encapsulation
        public DelegateCommand JsonSelectCommand { get; set; }
        private void Json_Select(object sender)
        {
            var tb = sender as TextBox;
            tb.Text = Open_Local_Json();
        }


        #region Clean file Functions

        //public DelegateCommand CurrencyClearCommand { get; set; }
        //private void Currency_Clear(object sender)
        //{
        //    currencyPath = "";
        //}

        //public DelegateCommand CatalogClearCommand { get; set; }
        //private void Catalog_Clear(object sender)
        //{
        //    catalogPath = "";

        //}

        //public DelegateCommand TitleDataClearCommand { get; set; }
        //private void Title_Data_Clear(object sender)
        //{
        //    titleDataPath = "";

        //}

        //public DelegateCommand DropTablesClearCommand { get; set; }
        //private void Drop_Tables_Clear(object sender)
        //{
        //    dropTablesPath = "";

        //}

        //public DelegateCommand CloudScriptClearCommand { get; set; }
        //private void Cloud_Script_Clear(object sender)
        //{
        //    cloudScriptPath = "";

        //}

        //public DelegateCommand TitleNewsCleanCommand { get; set; }
        //private void Title_News_Clean(object sender)
        //{
        //    titleNewsPath = "";

        //}

        //public DelegateCommand StatisticsDefinitionsCleanCommand { get; set; }
        //private void Statistics_Definitions_Clean(object sender)
        //{
        //    statsDefPath = "";

        //}

        //public DelegateCommand StoresCleanCommand { get; set; }
        //private void Stores_Clean(object sender)
        //{
        //    storesPath = "";

        //}

        //public DelegateCommand CdnAssetsCleanCommand { get; set; }
        //private void CDN_Assets_clean(object sender)
        //{
        //    cdnAssetsPath = "";

        //}
        #endregion

        // No MVVM but Encapsulation
        public DelegateCommand CleanCommand { get; set; }
        private void Json_Clean(object sender)
        {
            var tb = sender as TextBox;
            tb.Text = "";
        }


        //Upload Json file with async
        private UploadService _upload;
        private CancellationTokenSource _cancelUploadTokenSource;
        public  DelegateCommand UploadCommand { get; set; }
        private async void Upload(object sender)
        {
            StopUploadButtonEnable = true;
            UploadButtonEnable = false;

            _upload = new UploadService(this);
            _cancelUploadTokenSource = new CancellationTokenSource();

            await _upload.UploadAllJson(_cancelUploadTokenSource.Token);

            UploadButtonEnable = true;
            StopUploadButtonEnable = false;
        }




        public DelegateCommand StopCommand { get; set; }
        private void Stop(object sender)
        {
            ConsoleTbContent = "\n Now cancelling all tasks \n" + ConsoleTbContent;
            _cancelUploadTokenSource.Cancel();
        }

        private bool CanUpload(object sender)
        {
            return UploadButtonEnable == true ? true : false;
        }



        public Title sTitle;

        #endregion
        #region Binding

        public UpLoadWinViewModel(Title sTitle)
        {
            UploadButtonEnable = true;
            StopUploadButtonEnable = false;
            this.sTitle = sTitle;
            AssetFolderSelectCommand = new DelegateCommand
            {
                ExecuteAction = AssetFolder_Select
            };

            //Select Command Binding

            //CurrencySelectCommand = new DelegateCommand
            //{
            //    ExecuteAction = Currency_Select
            //};

            //CatalogSelectCommand = new DelegateCommand
            //{
            //    ExecuteAction = Catalog_Select
            //};

            //TitleDataSelectCommand = new DelegateCommand
            //{
            //    ExecuteAction = Title_Data_Select
            //};

            //DropTablesCommand = new DelegateCommand
            //{
            //    ExecuteAction = Drop_Tables_Select
            //};

            //CloudScriptSelectCommand = new DelegateCommand
            //{
            //    ExecuteAction = Cloud_Script_Select
            //};

            //TitleNewsSelectCommand = new DelegateCommand
            //{
            //    ExecuteAction = Title_News_Select
            //};

            //StatisticsDefinitionsSelectCommand = new DelegateCommand
            //{
            //    ExecuteAction = Statistics_Definitions_Select
            //};

            //StoresSelectCommand = new DelegateCommand
            //{
            //    ExecuteAction = Stores_Select
            //};

            //CdnAssetsSelectCommand = new DelegateCommand
            //{
            //    ExecuteAction = CDN_Assets_Select
            //};

            JsonSelectCommand = new DelegateCommand
            {
                ExecuteAction = Json_Select
            };



            ////Clean Command Binding
            //CurrencyClearCommand = new DelegateCommand
            //{
            //    ExecuteAction = Currency_Clear
            //};

            //CatalogClearCommand = new DelegateCommand
            //{
            //    ExecuteAction = Catalog_Clear
            //};

            //TitleDataClearCommand = new DelegateCommand
            //{
            //    ExecuteAction = Title_Data_Clear
            //};

            //DropTablesClearCommand = new DelegateCommand
            //{
            //    ExecuteAction = Drop_Tables_Clear
            //};

            //CloudScriptClearCommand = new DelegateCommand
            //{
            //    ExecuteAction = Cloud_Script_Clear
            //};

            //TitleNewsCleanCommand = new DelegateCommand
            //{
            //    ExecuteAction = Title_News_Clean
            //};
            //StatisticsDefinitionsCleanCommand = new DelegateCommand
            //{
            //    ExecuteAction = Statistics_Definitions_Clean
            //};
            //StoresCleanCommand = new DelegateCommand
            //{
            //    ExecuteAction = Stores_Clean
            //};
            //CdnAssetsCleanCommand = new DelegateCommand
            //{
            //    ExecuteAction = CDN_Assets_clean
            //};

            UploadCommand = new DelegateCommand
            {
                ExecuteAction = Upload,
                CanExecutePre = CanUpload
            };

            StopCommand = new DelegateCommand
            {
                ExecuteAction = Stop,

            };
            CleanCommand = new DelegateCommand
            {
                ExecuteAction = Json_Clean,

            };
        }


        #endregion
    }
}
