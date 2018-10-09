using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using PlayFab;
using PlayFab.AdminModels;
using PlayFab.Json;
using System.ComponentModel;
using System.Threading;
using UploadGui.ViewModels;


namespace UploadGui
{
    public class Upload
    {
        private string defaultCatalog = null; // Determined by TitleSettings.json
        private bool hitErrors;


        public string currencyPath = "./Currency.json";
        public string titleSettingsPath = "./TitleSettings.json";
        public string titleDataPath = "./TitleData.json";
        public string catalogPath = "./Catalog.json";
        public string dropTablesPath = "./DropTables.json";
        public string cloudScriptPath = "./CloudScript.js";
        public string titleNewsPath = "./TitleNews.json";
        public string statsDefPath = "./StatisticsDefinitions.json";
        public string storesPath = "./Stores.json";
        public string cdnAssetsPath = "./CdnData.json";
        public string cdnPath = "./AssetBundles/";

        private UpLoadWinViewModel upLoadWinViewModel;

        public Upload(UpLoadWinViewModel upLoadWinViewModel)
        {
            this.upLoadWinViewModel = upLoadWinViewModel;

            currencyPath = upLoadWinViewModel.currencyPath;
            catalogPath = upLoadWinViewModel.catalogPath;
            titleDataPath = upLoadWinViewModel.titleDataPath;
            dropTablesPath = upLoadWinViewModel.dropTablesPath;
            cloudScriptPath = upLoadWinViewModel.cloudScriptPath;
            titleNewsPath = upLoadWinViewModel.titleNewsPath;
            statsDefPath = upLoadWinViewModel.statsDefPath;
            storesPath = upLoadWinViewModel.storesPath;
            cdnAssetsPath = upLoadWinViewModel.cdnAssetsPath;

        }

        // log file details
        private FileInfo logFile;
        private StreamWriter logStream;

        private CancellationToken token;

        public async Task UploadAllJson(CancellationToken token)
        {
            this.token = token;
            try
            {
                // setup the log file
                logFile = new FileInfo("PreviousUploadLog.txt");
                logStream = logFile.CreateText();

                // get the destination title settings

                if (!GetTitleSettings())
                {
                    throw new Exception("\tFailed to load Title Settings");
                }

                // start uploading
                if (token.IsCancellationRequested)  return;
                upLoadWinViewModel.progressBarValue += !await UploadTitleData()
                    ? throw new Exception("\tFailed to upload TitleData.")
                    : 10;
                if (token.IsCancellationRequested) return;

                upLoadWinViewModel.progressBarValue += !await UploadEconomyData()
                    ? throw new Exception("\tFailed to upload Economy Data.")
                    : 20;
                if (token.IsCancellationRequested) return;

                upLoadWinViewModel.progressBarValue += !await UploadCloudScript()
                    ? throw new Exception("\tFailed to upload CloudScript.")
                    : 20;
                if (token.IsCancellationRequested) return;

                upLoadWinViewModel.progressBarValue += !await UploadTitleNews()
                    ? throw new Exception("\tFailed to upload TitleNews.")
                    : 20;
                if (token.IsCancellationRequested) return;

                upLoadWinViewModel.progressBarValue += !await UploadStatisticDefinitions()
                    ? throw new Exception("\tFailed to upload Statistics Definitions.")
                    : 20;
                if (token.IsCancellationRequested) return;

                upLoadWinViewModel.progressBarValue += !await UploadCdnAssets()
                    ? throw new Exception("\tFailed to upload CDN Assets.")
                    : 10;
            }
            catch (Exception ex)
            {
                hitErrors = true;
                LogToFile("\tAn unexpected error occurred: " + ex.Message, ConsoleColor.Red);
            }
            finally
            {
                var status = hitErrors
                    ? "ended with errors. See PreviousUploadLog.txt for details"
                    : "ended successfully!";
                var color = hitErrors ? ConsoleColor.Red : ConsoleColor.White;

                LogToFile("UB_Uploader.exe " + status, color);
                logStream.Close();
            }
        }

        // CDN
        public enum CdnPlatform
        {
            Desktop,
            iOS,
            Android
        }

        public readonly Dictionary<CdnPlatform, string> cdnPlatformSubfolder = new Dictionary<CdnPlatform, string>
        {
            {CdnPlatform.Desktop, ""},
            {CdnPlatform.iOS, "iOS/"},
            {CdnPlatform.Android, "Android/"},
        };

        /// <summary>
        /// This app parses the textfiles(defined above) and uploads the contents into a PlayFab title (defined in titleSettingsPath);
        /// </summary>
        /// <param name="args"></param>

        public bool GetTitleSettings()
        {
            var parsedFile = ParseFile(titleSettingsPath);

            var titleSettings = JsonWrapper.DeserializeObject<Dictionary<string, string>>(parsedFile);

            if (titleSettings != null &&
                titleSettings.TryGetValue("TitleId", out PlayFabSettings.TitleId) &&
                !string.IsNullOrEmpty(PlayFabSettings.TitleId) &&
                titleSettings.TryGetValue("DeveloperSecretKey", out PlayFabSettings.DeveloperSecretKey) &&
                !string.IsNullOrEmpty(PlayFabSettings.DeveloperSecretKey) &&
                titleSettings.TryGetValue("CatalogName", out defaultCatalog))
            {
                LogToFile("Setting Destination TitleId to: " + PlayFabSettings.TitleId);
                LogToFile("Setting DeveloperSecretKey to: " + PlayFabSettings.DeveloperSecretKey);
                LogToFile("Setting defaultCatalog name to: " + defaultCatalog);
                return true;
            }

            LogToFile("An error occurred when trying to parse TitleSettings.json", ConsoleColor.Red);
            return false;
        }

        #region Uploading Functions -- these are straightforward calls that push the data to the backend

        private async Task<bool> UploadEconomyData()
        {
            ////MUST upload these in this order so that the economy data is properly imported: VC -> Catalogs -> DropTables -> Stores

            if (!(await UploadVc()))
                return false;

            var reUploadList = new List<CatalogItem>();
            if (!(await UploadCatalog(reUploadList)))
                return false;

            if (!await UploadDropTables())
                return false;

            if (!(await UploadStores()))
                return false;

            // workaround for the DropTable conflict
            if (reUploadList.Count > 0)
            {
                LogToFile("Re-uploading [" + reUploadList.Count + "] CatalogItems due to DropTable conflicts...");
                await UpdateCatalog(reUploadList);
            }

            return true;
        }

        public async Task<bool> UploadStatisticDefinitions()
        {
            if (string.IsNullOrEmpty(statsDefPath))
                return false;

            LogToFile("Updating Player Statistics Definitions ...");
            var parsedFile = ParseFile(statsDefPath);

            var statisticDefinitions = JsonWrapper.DeserializeObject<List<PlayerStatisticDefinition>>(parsedFile);

            foreach (var item in statisticDefinitions)
            {
                LogToFile("\tUploading: " + item.StatisticName);

                var request = new CreatePlayerStatisticDefinitionRequest()
                {
                    StatisticName = item.StatisticName,
                    VersionChangeInterval = item.VersionChangeInterval,
                    AggregationMethod = item.AggregationMethod
                };

                if (token.IsCancellationRequested) return true;

                var createStatTask = await PlayFabAdminAPI.CreatePlayerStatisticDefinitionAsync(request);

                if (createStatTask.Error != null)
                {
                    if (createStatTask.Error.Error == PlayFabErrorCode.StatisticNameConflict)
                    {
                        LogToFile("\tStatistic Already Exists, Updating values: " + item.StatisticName,
                            ConsoleColor.DarkYellow);
                        var updateRequest = new UpdatePlayerStatisticDefinitionRequest()
                        {
                            StatisticName = item.StatisticName,
                            VersionChangeInterval = item.VersionChangeInterval,
                            AggregationMethod = item.AggregationMethod
                        };

                        if (token.IsCancellationRequested) return true;

                        var updateStatTask = await PlayFabAdminAPI.UpdatePlayerStatisticDefinitionAsync(updateRequest);
                        //updateStatTask.Wait();
                        if (updateStatTask.Error != null)
                            OutputPlayFabError("\t\tStatistics Definition Error: " + item.StatisticName,
                                updateStatTask.Error);
                        else
                            LogToFile("\t\tStatistics Definition:" + item.StatisticName + " Updated",
                                ConsoleColor.Green);
                    }
                    else
                    {
                        OutputPlayFabError("\t\tStatistics Definition Error: " + item.StatisticName,
                            createStatTask.Error);
                    }
                }
                else
                {
                    LogToFile("\t\tStatistics Definition: " + item.StatisticName + " Created", ConsoleColor.Green);
                }
            }

            return true;
        }

        public async Task<bool> UploadTitleNews()
        {
            if (string.IsNullOrEmpty(titleNewsPath))
                return false;

            LogToFile("Uploading TitleNews...");
            var parsedFile = ParseFile(titleNewsPath);

            var titleNewsItems = JsonWrapper.DeserializeObject<List<PlayFab.ServerModels.TitleNewsItem>>(parsedFile);

            foreach (var item in titleNewsItems)
            {
                LogToFile("\tUploading: " + item.Title);

                var request = new AddNewsRequest()
                {
                    Title = item.Title,
                    Body = item.Body
                };
                if (token.IsCancellationRequested) return true;

                var addNewsTask = await PlayFabAdminAPI.AddNewsAsync(request);

                if (addNewsTask.Error != null)
                    OutputPlayFabError("\t\tTitleNews Upload: " + item.Title, addNewsTask.Error);
                else
                    LogToFile("\t\t" + item.Title + " Uploaded.", ConsoleColor.Green);
            }

            return true;
        }

        public async Task<bool> UploadCloudScript()
        {
            if (string.IsNullOrEmpty(cloudScriptPath))
                return false;

            LogToFile("Uploading CloudScript...");
            var parsedFile = ParseFile(cloudScriptPath);

            if (parsedFile == null)
            {
                LogToFile("\tAn error occurred deserializing the CloudScript.js file.", ConsoleColor.Red);
                return false;
            }

            var files = new List<CloudScriptFile>
            {
                new CloudScriptFile
                {
                    Filename = "CloudScript.js",
                    FileContents = parsedFile
                }
            };

            var request = new UpdateCloudScriptRequest()
            {
                Publish = true,
                Files = files
            };
            if (token.IsCancellationRequested) return true;

            var updateCloudScriptTask = await PlayFabAdminAPI.UpdateCloudScriptAsync(request);
            //updateCloudScriptTask.Wait();

            if (updateCloudScriptTask.Error != null)
            {
                OutputPlayFabError("\tCloudScript Upload Error: ", updateCloudScriptTask.Error);
                return false;
            }

            LogToFile("\tUploaded CloudScript!", ConsoleColor.Green);
            return true;
        }

        public async Task<bool> UploadTitleData()
        {
            if (string.IsNullOrEmpty(titleDataPath))
                return false;

            LogToFile("Uploading Title Data Keys & Values...");
            var parsedFile = ParseFile(titleDataPath);
            var titleDataDict = JsonWrapper.DeserializeObject<Dictionary<string, string>>(parsedFile);

            foreach (var kvp in titleDataDict)
            {
                LogToFile("\tUploading: " + kvp.Key);

                var request = new SetTitleDataRequest()
                {
                    Key = kvp.Key,
                    Value = kvp.Value
                };

                if (token.IsCancellationRequested) return true;

                var setTitleDataTask = await PlayFabAdminAPI.SetTitleDataAsync(request);
                //setTitleDataTask.Wait();

                if (setTitleDataTask.Error != null)
                    OutputPlayFabError("\t\tTitleData Upload: " + kvp.Key, setTitleDataTask.Error);
                else
                    LogToFile("\t\t" + kvp.Key + " Uploaded.", ConsoleColor.Green);
            }

            return true;
        }

        public async Task<bool> UploadVc()
        {
            LogToFile("Uploading Virtual Currency Settings...");
            var parsedFile = ParseFile(currencyPath);
            var vcData = JsonWrapper.DeserializeObject<List<VirtualCurrencyData>>(parsedFile);
            var request = new AddVirtualCurrencyTypesRequest
            {
                VirtualCurrencies = vcData
            };
            if (token.IsCancellationRequested) return true;

            var updateVcTask = await PlayFabAdminAPI.AddVirtualCurrencyTypesAsync(request);
            //updateVcTask.Wait();

            if (updateVcTask.Error != null)
            {
                OutputPlayFabError("\tVC Upload Error: ", updateVcTask.Error);
                return false;
            }

            LogToFile("\tUploaded VC!", ConsoleColor.Green);
            return true;
        }

        public async Task<bool> UploadCatalog(List<CatalogItem> reUploadList)
        {
            if (string.IsNullOrEmpty(catalogPath))
                return false;

            LogToFile("Uploading CatalogItems...");
            var parsedFile = ParseFile(catalogPath);

            var catalogWrapper = JsonWrapper.DeserializeObject<CatalogWrapper>(parsedFile);
            if (catalogWrapper == null)
            {
                LogToFile("\tAn error occurred deserializing the Catalog.json file.", ConsoleColor.Red);
                return false;
            }

            for (var z = 0; z < catalogWrapper.Catalog.Count; z++)
            {
                if (catalogWrapper.Catalog[z].Bundle != null || catalogWrapper.Catalog[z].Container != null)
                {
                    var original = catalogWrapper.Catalog[z];
                    var strippedClone = CloneCatalogItemAndStripTables(original);

                    reUploadList.Add(original);
                    catalogWrapper.Catalog.Remove(original);
                    catalogWrapper.Catalog.Add(strippedClone);
                }
            }

            return await UpdateCatalog(catalogWrapper.Catalog);
        }

        public async Task<bool> UploadDropTables()
        {
            if (string.IsNullOrEmpty(dropTablesPath))
                return false;

            LogToFile("Uploading DropTables...");
            var parsedFile = ParseFile(dropTablesPath);

            var dtDict = JsonWrapper.DeserializeObject<Dictionary<string, RandomResultTableListing>>(parsedFile);
            if (dtDict == null)
            {
                LogToFile("\tAn error occurred deserializing the DropTables.json file.", ConsoleColor.Red);
                return false;
            }

            var dropTables = new List<RandomResultTable>();
            foreach (var kvp in dtDict)
            {
                dropTables.Add(new RandomResultTable()
                {
                    TableId = kvp.Value.TableId,
                    Nodes = kvp.Value.Nodes
                });
            }

            var request = new UpdateRandomResultTablesRequest()
            {
                CatalogVersion = defaultCatalog,
                Tables = dropTables
            };
            if (token.IsCancellationRequested) return true;

            var updateResultTableTask = await PlayFabAdminAPI.UpdateRandomResultTablesAsync(request);
            //updateResultTableTask.Wait();

            if (updateResultTableTask.Error != null)
            {
                OutputPlayFabError("\tDropTable Upload Error: ", updateResultTableTask.Error);
                return false;
            }

            LogToFile("\tUploaded DropTables!", ConsoleColor.Green);
            return true;
        }

        public async Task<bool> UploadStores()
        {
            if (string.IsNullOrEmpty(storesPath))
                return false;

            LogToFile("Uploading Stores...");
            var parsedFile = ParseFile(storesPath);

            var storesList = JsonWrapper.DeserializeObject<List<StoreWrapper>>(parsedFile);

            foreach (var eachStore in storesList)
            {
                LogToFile("\tUploading: " + eachStore.StoreId);

                var request = new UpdateStoreItemsRequest
                {
                    CatalogVersion = defaultCatalog,
                    StoreId = eachStore.StoreId,
                    Store = eachStore.Store,
                    MarketingData = eachStore.MarketingData
                };
                if (token.IsCancellationRequested) return true;

                var updateStoresTask = await PlayFabAdminAPI.SetStoreItemsAsync(request);
                //updateStoresTask.Wait();

                if (updateStoresTask.Error != null)
                    OutputPlayFabError("\t\tStore Upload: " + eachStore.StoreId, updateStoresTask.Error);
                else
                    LogToFile("\t\tStore: " + eachStore.StoreId + " Uploaded. ", ConsoleColor.Green);
            }

            return true;
        }

        public async Task<bool> UploadCdnAssets()
        {
            if (string.IsNullOrEmpty(cdnAssetsPath))
                return false;

            LogToFile("Uploading CDN AssetBundles...");
            var parsedFile = ParseFile(cdnAssetsPath);
            var bundleNames =
                JsonWrapper.DeserializeObject<List<string>>(
                    parsedFile); // TODO: This could probably just read the list of files from the directory

            if (bundleNames != null)
            {
                foreach (var bundleName in bundleNames)
                {
                    foreach (CdnPlatform eachPlatform in Enum.GetValues(typeof(CdnPlatform)))
                    {
                        var key = cdnPlatformSubfolder[eachPlatform] + bundleName;
                        var path = cdnPath + key;
                        await UploadAsset(key, path);
                    }
                }
            }
            else
            {
                LogToFile("\tAn error occurred deserializing CDN Assets: ", ConsoleColor.Red);
                return false;
            }

            return true;
        }

        #endregion

        #region Helper Functions -- these functions help the main uploading functions

        void LogToFile(string content, ConsoleColor color = ConsoleColor.White)
        {
            upLoadWinViewModel.consoleTBContent = content + "\n"+ upLoadWinViewModel.consoleTBContent ;
            Console.ForegroundColor = color;
            Console.WriteLine(content);
            logStream.WriteLine(content);

            Console.ForegroundColor = ConsoleColor.White;
        }

        void OutputPlayFabError(string context, PlayFabError err)
        {
            hitErrors = true;
            LogToFile("\tAn error occurred during: " + context, ConsoleColor.Red);

            var details = string.Empty;
            if (err.ErrorDetails != null)
            {
                foreach (var kvp in err.ErrorDetails)
                {
                    details += (kvp.Key + ": ");
                    foreach (var eachIssue in kvp.Value)
                        details += (eachIssue + ", ");
                    details += "\n";
                }
            }

            LogToFile(string.Format("\t\t[{0}] -- {1}: {2} ", err.Error, err.ErrorMessage, details), ConsoleColor.Red);
        }

        string ParseFile(string path)
        {
            var s = File.OpenText(path);
            var contents = s.ReadToEnd();
            s.Close();
            return contents;
        }

        CatalogItem CloneCatalogItemAndStripTables(CatalogItem strip)
        {
            if (strip == null)
                return null;

            return new CatalogItem
            {
                ItemId = strip.ItemId,
                ItemClass = strip.ItemClass,
                CatalogVersion = strip.CatalogVersion,
                DisplayName = strip.DisplayName,
                Description = strip.Description,
                VirtualCurrencyPrices = strip.VirtualCurrencyPrices,
                RealCurrencyPrices = strip.RealCurrencyPrices,
                Tags = strip.Tags,
                CustomData = strip.CustomData,
                Consumable = strip.Consumable,
                Container = null, //strip.Container, // Clearing this is the point
                Bundle = null, //strip.Bundle, // Clearing this is the point
                CanBecomeCharacter = strip.CanBecomeCharacter,
                IsStackable = strip.CanBecomeCharacter,
                IsTradable = strip.IsTradable,
                ItemImageUrl = strip.ItemImageUrl
            };
        }

        private async Task<bool> UpdateCatalog(List<CatalogItem> catalog)
        {
            var request = new UpdateCatalogItemsRequest
            {
                CatalogVersion = defaultCatalog,
                Catalog = catalog
            };
            if (token.IsCancellationRequested) return true;

            var updateCatalogItemsTask = await PlayFabAdminAPI.UpdateCatalogItemsAsync(request);
            //updateCatalogItemsTask.Wait();

            if (updateCatalogItemsTask.Error != null)
            {
                OutputPlayFabError("\tCatalog Upload Error: ", updateCatalogItemsTask.Error);
                return false;
            }

            LogToFile("\tUploaded Catalog!", ConsoleColor.Green);
            return true;
        }

        private async Task<bool> UploadAsset(string key, string path)
        {
            var request = new GetContentUploadUrlRequest()
            {
                Key = key,
                ContentType = "application/x-gzip"
            };

            LogToFile("\tFetching CDN endpoint for " + key);
            if (token.IsCancellationRequested) return true;

            var getContentUploadTask = await PlayFabAdminAPI.GetContentUploadUrlAsync(request);
            //getContentUploadTask.Wait();

            if (getContentUploadTask.Error != null)
            {
                OutputPlayFabError("\t\tAcquire CDN URL Error: ", getContentUploadTask.Error);
                return false;
            }

            var destUrl = getContentUploadTask.Result.URL;
            LogToFile("\t\tAcquired CDN Address: " + key, ConsoleColor.Green);

            byte[] fileContents = File.ReadAllBytes(path);

            return await PutFile(key, destUrl, fileContents);
        }

        private async Task<bool> PutFile(string key, string url, byte[] payload)
        {
            LogToFile("\t\tStarting HTTP PUT for: " + key);

            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = "PUT";
            request.ContentType = "application/x-gzip";

            if (payload != null)
            {
                var dataStream = request.GetRequestStream();
                dataStream.Write(payload, 0, payload.Length);
                dataStream.Close();
            }
            else
            {
                LogToFile("\t\t\tERROR: Byte array was empty or null", ConsoleColor.Red);
                return false;
            }

            var response = await request.GetResponseAsync();

            if (response != null)
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    LogToFile("\t\t\tHTTP PUT Successful:" + key, ConsoleColor.Green);
                    return true;
                }
            }
            else
            {
                LogToFile("\t\t\tHTTP PUT Successful:" + key, ConsoleColor.Red);
                return false;
            }

            #endregion
        }

        public class CatalogWrapper
        {
            public string CatalogVersion;
            public List<CatalogItem> Catalog;
        }

        public class StoreWrapper
        {
            public string StoreId;
            public List<StoreItem> Store;
            public StoreMarketingModel MarketingData;
        }

    }
}