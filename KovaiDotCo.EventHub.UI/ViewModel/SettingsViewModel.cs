using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;
using KovaiDotCo.Model;
using Prism.Commands;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Input;
using PubSub;
using Newtonsoft.Json;
using System.IO;

namespace KovaiDotCo.EventHub.UI.ViewModel
{
    /// <summary>
    /// ViewModel for Settings View
    /// </summary>
    public class SettingsViewModel : BindableBase
    {
        #region Private Fields

        /// <summary>
        /// Holds the instance of SettingsModel
        /// </summary>
        private SettingsModel _model;

        /// <summary>
        /// PubSub Default Hub instance
        /// </summary>
        private Hub _hub = Hub.Default;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the model
        /// </summary>
        public SettingsModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets the SaveCommand
        /// </summary>
        public ICommand SaveCommand { get; }
        #endregion

        #region Constructor
        /// <summary>
        /// SettingsViewModel
        /// </summary>
        /// <param name="model">Singleton Settings Model</param>
        public SettingsViewModel(SettingsModel model)
        {
            Model = model;

            SaveCommand = new DelegateCommand(OnSave);

        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Click Event Hanlder for Save 
        /// Deserializes the model and saves to disk
        /// </summary>
        public void OnSave()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Model.EventHubConnectionString))
                {
                    _hub.Publish(new AppMessageModel("Please enter a valid connection string", "Validation - Error") { IsError = true });
                    return;
                }

                string fileName = "settings.json";
                var jsonData = JsonConvert.SerializeObject(Model);
                File.WriteAllText(fileName, jsonData);

                _hub.Publish(new AppMessageModel("Please restart the app for the changes to take effect", "Saved Successfully"));
            }
            catch (Exception ex)
            {
                _hub.Publish(new AppLogModel($"{ex.Message} \r\n{ex.StackTrace}"));

                _hub.Publish(new AppMessageModel($"{ex.Message}\r\nPlesae check Logs tab for more details.", "Save - Error")
                { IsError = true });
            }
        }
        #endregion

       
    }
}
