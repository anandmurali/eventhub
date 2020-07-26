using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;
using KovaiDotCo.Model;
using System.Collections.ObjectModel;
using KovaiDotCo.EventHub.Contracts;
using System.Configuration;
using System.Threading.Tasks;
using PubSub;
using System.Windows.Threading;
using System.Windows.Input;
using Prism.Commands;

namespace KovaiDotCo.EventHub.UI.ViewModel
{
    public class MachineTestViewModel : BindableBase
    {
        #region Private Fields
        /// <summary>
        /// Backing field for Property ActivityLogModels
        /// </summary>
        private ObservableCollection<AzureDiagnosticGridModel> _activityLogModels;

        /// <summary>
        /// Instance of IEventHubCommunicator implementation to communicate with EventHubClient
        /// </summary>
        private IEventHubCommunicator _eventHubOneReceiver;

        /// <summary>
        /// PUbSub Hub instance
        /// </summary>
        private Hub _hub = Hub.Default;

        /// <summary>
        /// DispatcherTimer to update the time ago field for every one minute
        /// </summary>
        private DispatcherTimer _calculateTimeDispatcherTimer;

        /// <summary>
        /// Holds the instance of SettingsModel
        /// </summary>
        private SettingsModel _settingsModel;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ActivityLogModels
        /// </summary>
        public ObservableCollection<AzureDiagnosticGridModel> ActivityLogModels
        {
            get { return _activityLogModels; }
            set
            {
                _activityLogModels = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Constructor
        public MachineTestViewModel(IEventHubCommunicator eventHubOneReceiver, SettingsModel settingsModel)
        {
            _eventHubOneReceiver = eventHubOneReceiver;
            _settingsModel = settingsModel;

            ActivityLogModels = new ObservableCollection<AzureDiagnosticGridModel>();

            _hub.Subscribe<List<AzureDiagnosticGridModel>>(DataReceived);

            // Trigger timer for every one minute
            _calculateTimeDispatcherTimer = new DispatcherTimer();
            _calculateTimeDispatcherTimer.Tick += new EventHandler(TimeElapsedEventHandler);
            _calculateTimeDispatcherTimer.Interval = new TimeSpan(0, 1, 0);
            _calculateTimeDispatcherTimer.Start();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Starts communication with EventHub
        /// </summary>
        public async void StartReceiving()
        {
            try
            {
                // If connection string is empty, then use value from App.config file
                await _eventHubOneReceiver.ReceiveAsync(_settingsModel.EventHubConnectionString ?? ConfigurationManager.AppSettings["EventHubConnectionString"]);
            }
            catch (Exception ex)
            {
                _hub.Publish(new AppLogModel($"Error whlle setting up EventHubClient\r\n\r\n" +
                    $"{ex.Message}\r\n{ex.StackTrace}"));
                _hub.Publish(new AppMessageModel($"{ex.Message}\r\nPlease check the Logs tab for more information.\r\n\r\nPossible problems could be\r\n" +
                    $"- Invalid EventHub connection string\r\n" +
                    $"- Insufficient privileges (Send and Listen required)\r\n" +
                    $"- No internet\r\n", "Event Hub Client - Error")
                { IsError = true });
            }
        }

        /// <summary>
        /// Adds the list to ObservableCollection.
        /// The method will be triggered from a background thread so calling on Dispatcher
        /// </summary>
        /// <param name="list"></param>
        public void DataReceived(List<AzureDiagnosticGridModel> list)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                foreach (var model in list)
                {
                    _activityLogModels.Insert(0, model);
                }
            });
        }

        /// <summary>
        /// Called when window is closing.
        /// Release all object used by the VM here.
        /// </summary>
        public async void OnClosing()
        {
            // Close Receiver
            await _eventHubOneReceiver.CloseReceiver();
            // Unsubscribe
            _hub.Unsubscribe<List<AzureDiagnosticGridModel>>();
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Calculate Time Dispatcher Timer Elapsed.
        /// Recalculates the time ago field shown in UI every one minute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeElapsedEventHandler(object sender, EventArgs e)
        {
            try
            {
                if (_activityLogModels != null)
                {
                    for (int index = 0; index < _activityLogModels.Count; index++)
                    {
                        var model = _activityLogModels[index];
                        var timeDifference = DateTime.Now - model.ReceivedTime;
                        if (timeDifference.TotalMinutes < 60)
                        {
                            var totalMinutes = Math.Truncate(timeDifference.TotalMinutes);
                            model.Time = $"{totalMinutes} mins ago";
                        }
                        else if (timeDifference.TotalHours < 24)
                        {
                            var totalHours = Math.Truncate(timeDifference.TotalHours);
                            model.Time = $"{totalHours} hours ago";
                        }
                        else
                        {
                            var totalDays = Math.Truncate(timeDifference.TotalDays);
                            model.Time = $"{totalDays} days ago";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _hub.Publish(new AppLogModel(ex.Message, true));
            }
        }
        #endregion
    }
}
