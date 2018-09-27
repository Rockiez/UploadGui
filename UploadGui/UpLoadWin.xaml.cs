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
        public UpLoadWin()
        {
            InitializeComponent();
        }
        //public string currencyPath = "./PlayFabData/Currency.json";
        //public string titleSettingsPath = "./PlayFabData/TitleSettings.json";
        //public string titleDataPath = "./PlayFabData/TitleData.json";
        //public string catalogPath = "./PlayFabData/Catalog.json";
        //public string dropTablesPath = "./PlayFabData/DropTables.json";
        //public string cloudScriptPath = "./PlayFabData/CloudScript.js";
        //public string titleNewsPath = "./PlayFabData/TitleNews.json";
        //public string statsDefPath = "./PlayFabData/StatisticsDefinitions.json";
        //public string storesPath = "./PlayFabData/Stores.json";
        //public string cdnAssetsPath = "./PlayFabData/CdnData.json";

        // Select Folder
        private void AssetFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AssetFolderTB.Text = openFileDialog.SelectedPath;
                Directory.SetCurrentDirectory(openFileDialog.SelectedPath);
            }
        }


        #region Select file Functions 

        private void Open_Local_Json(ref System.Windows.Controls.TextBox uploadTextBox, ref string uploadPath)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog() { Filter = "(*.json)|*.json" };
            if(AssetFolderTB.Text != "")
            {
                openFileDialog.InitialDirectory = AssetFolderTB.Text;
            }
            if (openFileDialog.ShowDialog() == true)
            {
                uploadTextBox.Text = openFileDialog.FileName;
                uploadPath = openFileDialog.FileName;
            }
        }

        private void Currency_Click(object sender, RoutedEventArgs e)
        {
            Open_Local_Json(ref CurrencyTB, ref Upload.currencyPath);

        }

        private void Catalog_Click(object sender, RoutedEventArgs e)
        {
            Open_Local_Json(ref CatalogTB, ref Upload.catalogPath);
        }

        private void Title_Data_Click(object sender, RoutedEventArgs e)
        {
            Open_Local_Json(ref TitleDataTB, ref Upload.titleDataPath);

        }

        private void Drop_Tables_Click(object sender, RoutedEventArgs e)
        {
            Open_Local_Json(ref DropTablesTB, ref Upload.dropTablesPath);
        }

        private void Cloud_Script_Click(object sender, RoutedEventArgs e)
        {
            Open_Local_Json(ref CloudScriptTB, ref Upload.cloudScriptPath);
        }

        private void Title_News_Click(object sender, RoutedEventArgs e)
        {
            Open_Local_Json(ref TitleNewsTB, ref Upload.titleNewsPath);

        }

        private void Statistics_Definitions_Click(object sender, RoutedEventArgs e)
        {
            Open_Local_Json(ref StatisticsDefinitionsTB, ref Upload.statsDefPath);

        }

        private void Stores_Click(object sender, RoutedEventArgs e)
        {
            Open_Local_Json(ref StoresTB, ref Upload.storesPath);

        }

        private void CDN_Assets_Click(object sender, RoutedEventArgs e)
        {
            Open_Local_Json(ref CDNAssetsTB, ref Upload.storesPath);

        }
        #endregion


        #region Clean file Functions
        private void Currency_Clear_Click(object sender, RoutedEventArgs e)
        {
            CurrencyTB.Clear();
            Upload.currencyPath = "";
        }

        private void Catalog_Clear_Click(object sender, RoutedEventArgs e)
        {
            CatalogTB.Clear();
            Upload.catalogPath = "";
        }
        private void Title_Data_Clear_Click(object sender, RoutedEventArgs e)
        {
            TitleDataTB.Clear();
            Upload.titleDataPath = "";
        }
        private void Drop_Tables_Clear_Click(object sender, RoutedEventArgs e)
        {
            DropTablesTB.Clear();
            Upload.dropTablesPath = "";
        }
        private void Cloud_Script_Clear_Click(object sender, RoutedEventArgs e)
        {
            CloudScriptTB.Clear();
            Upload.cloudScriptPath = "";
        }
        private void Title_News_Clean_Click(object sender, RoutedEventArgs e)
        {
            TitleNewsTB.Clear();
            Upload.titleNewsPath = "";
        }
        private void Statistics_Definitions_Clean_Click(object sender, RoutedEventArgs e)
        {
            StatisticsDefinitionsTB.Clear();
            Upload.statsDefPath = "";
        }
        private void Stores_Clean_Click(object sender, RoutedEventArgs e)
        {
            StoresTB.Clear();
            Upload.storesPath = "";
        }
        private void CDN_Assets_clean_Click(object sender, RoutedEventArgs e)
        {
            CDNAssetsTB.Clear();
            Upload.cdnAssetsPath = "";
        }

        #endregion

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (o, ea) =>
            {
                Upload.UploadAllJson();
            };

            worker.RunWorkerCompleted += (o, ea) =>
            {
            };

            worker.RunWorkerAsync();
                
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}