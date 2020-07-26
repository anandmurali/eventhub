using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using MahApps.Metro.Controls.Dialogs;
using PubSub;
using KovaiDotCo.Model;
using KovaiDotCo.EventHub.Contracts;

namespace KovaiDotCo.EventHub.UI.ViewModel
{
    /// <summary>
    /// MainViewModel is first ViewModel that will be loaded
    /// </summary>
    public class MainViewModel : BindableBase
    {
        #region Private Fields
        /// <summary>
        /// Backing field for MachineTestViewModel property
        /// </summary>
        private MachineTestViewModel _machineTestViewModel;

        /// <summary>
        /// Backing field for CreateEventViewModel property
        /// </summary>
        private CreateEventViewModel _createEventViewModel;

        /// <summary>
        /// Backing field for LogViewModel property
        /// </summary>
        private LogViewModel _logViewModel;

        /// <summary>
        /// Backing field SettingsViewModel property
        /// </summary>
        private SettingsViewModel _settingsViewModel;

        /// <summary>
        /// PubSub Hub instance
        /// </summary>
        private Hub _hub = Hub.Default;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the MachineTestViewModel
        /// </summary>
        public MachineTestViewModel MachineTestViewModel
        {
            get { return _machineTestViewModel; }
            set
            {
                _machineTestViewModel = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the CreateEventViewModel
        /// </summary>
        public CreateEventViewModel CreateEventViewModel
        {
            get { return _createEventViewModel; }
            set
            {
                _createEventViewModel = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the LogViewModel
        /// </summary>
        public LogViewModel LogViewModel
        {
            get { return _logViewModel; }
            set { _logViewModel = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the SettingsViewModel
        /// </summary>
        public SettingsViewModel SettingsViewModel
        {
            get { return _settingsViewModel; }
            set { _settingsViewModel = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Cosntructor

        public MainViewModel(LogViewModel logViewModel, MachineTestViewModel machineTestViewModel, CreateEventViewModel createEventViewModel, SettingsViewModel settingsViewModel)
        {
            LogViewModel = logViewModel;
            MachineTestViewModel = machineTestViewModel;
            CreateEventViewModel = createEventViewModel;
            SettingsViewModel = settingsViewModel;

            _hub.Publish(new AppLogModel("View Models Initialized"));
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Called when window is closing.
        /// Release all objects used by VM here.
        /// </summary>
        public void OnClosing()
        {
            MachineTestViewModel.OnClosing();
            LogViewModel.OnClosing();
        }

        /// <summary>
        /// Called when window is loaded
        /// </summary>
        public void OnLoaded()
        {
            MachineTestViewModel.StartReceiving();
        }
        #endregion
    }
}
