using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;

namespace UploadGui
{
    /// <summary>
    /// Interaction logic for UpLoadWin.xaml
    /// </summary>
    public partial class UpLoadWin : Window
    {
        //BackgroundWorker worker = new BackgroundWorker();

        private JsonPath upJsonPath;
        public UpLoadWin()
        {
            InitializeComponent();

            //initializing JsonPath and giving Windows.DataContext
            upJsonPath = new JsonPath();
            this.DataContext = upJsonPath;

            ////initializing BackgroundWorker
            //worker.WorkerReportsProgress = true;
            //worker.WorkerSupportsCancellation = true;
            //worker.DoWork += (o, ea) =>
            //{
            //    Upload.UploadAllJson(o, ea);
            //};

            //worker.ProgressChanged += (o, ea) =>
            //{
            //    //Missing Implementation about output in console.

            //    uploadPB.Value = ea.ProgressPercentage;
            //};

            //worker.RunWorkerCompleted += (o, ea) =>
            //{
            //};
     
        }

        //add these strings after selected floder
        private string currencyFolderPath = "/Currency.json";
        private string titleSettingsFolderPath = "/TitleSettings.json";
        private string titleDataFolderPath = "/TitleData.json";
        private string catalogFolderPath = "/Catalog.json";
        private string dropTablesFolderPath = "/DropTables.json";
        private string cloudScriptFolderPath = "/CloudScript.js";
        private string titleNewsFolderPath = "/TitleNews.json";
        private string statsDefFolderPath = "/StatisticsDefinitions.json";
        private string storesFolderPath = "/Stores.json";
        private string cdnAssetsFolderPath = "/CdnData.json";

        // Select Folder
        private void AssetFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                upJsonPath.folderPath = openFileDialog.SelectedPath;
                Directory.SetCurrentDirectory(openFileDialog.SelectedPath);

                upJsonPath.currencyPath = Check_File_Basis_FolderPath(upJsonPath.folderPath, currencyFolderPath);
                upJsonPath.titleSettingsPath = Check_File_Basis_FolderPath(upJsonPath.folderPath, titleSettingsFolderPath);
                upJsonPath.titleDataPath = Check_File_Basis_FolderPath(upJsonPath.folderPath, titleDataFolderPath);
                upJsonPath.catalogPath = Check_File_Basis_FolderPath(upJsonPath.folderPath, catalogFolderPath);
                upJsonPath.dropTablesPath = Check_File_Basis_FolderPath(upJsonPath.folderPath, dropTablesFolderPath);
                upJsonPath.cloudScriptPath = Check_File_Basis_FolderPath(upJsonPath.folderPath, cloudScriptFolderPath);
                upJsonPath.titleNewsPath = Check_File_Basis_FolderPath(upJsonPath.folderPath, titleNewsFolderPath);
                upJsonPath.statsDefPath = Check_File_Basis_FolderPath(upJsonPath.folderPath, statsDefFolderPath);
                upJsonPath.storesPath = Check_File_Basis_FolderPath(upJsonPath.folderPath, storesFolderPath);
                upJsonPath.cdnAssetsPath = Check_File_Basis_FolderPath(upJsonPath.folderPath, cdnAssetsFolderPath);

            }
        }

        //Check  filePath validatable
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
            if(upJsonPath.folderPath != "")
            {
                openFileDialog.InitialDirectory = upJsonPath.folderPath;
            }
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return "";
        }

        private void Currency_Click(object sender, RoutedEventArgs e) => upJsonPath.currencyPath = Open_Local_Json();
        private void Catalog_Click(object sender, RoutedEventArgs e) => upJsonPath.catalogPath = Open_Local_Json();
        private void Title_Data_Click(object sender, RoutedEventArgs e) => upJsonPath.titleDataPath = Open_Local_Json();
        private void Drop_Tables_Click(object sender, RoutedEventArgs e) => upJsonPath.dropTablesPath = Open_Local_Json();
        private void Cloud_Script_Click(object sender, RoutedEventArgs e) => upJsonPath.cloudScriptPath = Open_Local_Json();
        private void Title_News_Click(object sender, RoutedEventArgs e) => upJsonPath.titleNewsPath = Open_Local_Json();
        private void Statistics_Definitions_Click(object sender, RoutedEventArgs e) => upJsonPath.statsDefPath = Open_Local_Json();
        private void Stores_Click(object sender, RoutedEventArgs e) => upJsonPath.storesPath = Open_Local_Json();
        private void CDN_Assets_Click(object sender, RoutedEventArgs e) => upJsonPath.cdnAssetsPath = Open_Local_Json();
        
        #endregion


        #region Clean file Functions
        private void Currency_Clear_Click(object sender, RoutedEventArgs e)
        {
            upJsonPath.currencyPath = "";
        }

        private void Catalog_Clear_Click(object sender, RoutedEventArgs e)
        {
            upJsonPath.catalogPath = "";

        }
        private void Title_Data_Clear_Click(object sender, RoutedEventArgs e)
        {
            upJsonPath.titleDataPath = "";

        }
        private void Drop_Tables_Clear_Click(object sender, RoutedEventArgs e)
        {
            upJsonPath.dropTablesPath = "";

        }
        private void Cloud_Script_Clear_Click(object sender, RoutedEventArgs e)
        {
            upJsonPath.cloudScriptPath = "";

        }
        private void Title_News_Clean_Click(object sender, RoutedEventArgs e)
        {
            upJsonPath.titleNewsPath = "";

        }
        private void Statistics_Definitions_Clean_Click(object sender, RoutedEventArgs e)
        {
            upJsonPath.statsDefPath = "";

        }
        private void Stores_Clean_Click(object sender, RoutedEventArgs e)
        {
            upJsonPath.storesPath = "";

        }
        private void CDN_Assets_clean_Click(object sender, RoutedEventArgs e)
        {
            upJsonPath.cdnAssetsPath = "";

        }

        #endregion

        //Upload Json file with async
        private async void Upload_Click(object sender, RoutedEventArgs e)
        {
            //if (!worker.IsBusy)
            //{
            //    worker.RunWorkerAsync();
            //}
            
            UploadButton.IsEnabled = false;
            await Task.Run(new Action(Upload.UploadAllJson));
            UploadButton.IsEnabled= true;

        }


        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            //worker.CancelAsync();

            ////Missing Cancel Message
        }
    }


}