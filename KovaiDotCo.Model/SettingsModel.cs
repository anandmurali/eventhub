using System;
using Prism.Mvvm;

namespace KovaiDotCo.Model
{
    /// <summary>
    /// POCO class for storing the application settings
    /// </summary>
    public class SettingsModel : BindableBase
    {
        #region Private Fields
        private string _eventHubConnectionString;
        #endregion

        #region Properties
        public string EventHubConnectionString
        {
            get { return _eventHubConnectionString; }
            set
            {
                _eventHubConnectionString = value;
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}
