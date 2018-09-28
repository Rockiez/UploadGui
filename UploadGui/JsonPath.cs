using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace UploadGui
{
    class JsonPath : INotifyPropertyChanged
    {


        #region public property for Binding
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
    }
}
