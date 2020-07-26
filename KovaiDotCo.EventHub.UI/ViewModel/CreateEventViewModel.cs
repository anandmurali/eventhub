using KovaiDotCo.EventHub.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using KovaiDotCo.Model;
using Prism.Mvvm;
using Prism.Commands;
using System.Windows.Threading;
using MahApps.Metro.Controls.Dialogs;
using PubSub;

namespace KovaiDotCo.EventHub.UI.ViewModel
{
    /// <summary>
    /// ViewModel for CreateEvent View
    /// </summary>
    public class CreateEventViewModel : BindableBase
    {
        #region Private Fields
        /// <summary>
        /// Holds instance of IEventHubCommunicator implementation to send Events
        /// </summary>
        private IEventHubCommunicator _eventHubOneReceiver;

        /// <summary>
        /// Backing field of Model Property
        /// </summary>
        private AzureDiagnosticGridModel _model;

        /// <summary>
        /// Backing field of Status property
        /// </summary>
        private string _status;

        /// <summary>
        /// Timer to clear the status message after 10 seconds
        /// </summary>
        private DispatcherTimer _statusDispatcherTimer;

        /// <summary>
        /// PUbSub Hub instance
        /// </summary>
        private Hub _hub = Hub.Default;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the Model
        /// </summary>
        public AzureDiagnosticGridModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Status
        /// </summary>
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets the CreateEvent command
        /// </summary>
        public ICommand CreateEventCommand { get; }
        #endregion

        #region Constructor
        public CreateEventViewModel(IEventHubCommunicator eventHubOneReceiver)
        {
            _eventHubOneReceiver = eventHubOneReceiver;

            Model = GetModelInstance();

            CreateEventCommand = new DelegateCommand(OnCreateEvent);

            // Initialize Status Timer
            _statusDispatcherTimer = new DispatcherTimer();
            _statusDispatcherTimer.Tick += new EventHandler(TimeElapsedEventHandler);
            _statusDispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Click Event Hanlder for Create Event Button.
        /// </summary>
        public async void OnCreateEvent()
        {
            try
            {
                await _eventHubOneReceiver.SendEventAsync(Model);

                Model = GetModelInstance();

                Status = "Event sent successfully";
                _statusDispatcherTimer.Start();
            }
            catch (Exception ex)
            {
                _hub.Publish(new AppLogModel($"{ex.Message} \r\n{ex.StackTrace}"));

                _hub.Publish(new AppMessageModel($"{ex.Message}\r\nPlesae check Logs tab for more details.", "Save - Error")
                { IsError = true });
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns an instance of AzureDiagnosticGridModel with default values
        /// </summary>
        /// <returns></returns>
        private AzureDiagnosticGridModel GetModelInstance()
        {
            var model = new AzureDiagnosticGridModel();
            //model.OperationName = "Sample Operation Name";
            //model.Status = "Sample Status";
            //model.Subscription = "Sample Subscription";
            model.EventInitiatedBy = "Desktop App";
            return model;
        }

        /// <summary>
        /// Status Dispatcher Timer Elapsed.
        /// Stops the timer and resets the Status message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeElapsedEventHandler(object sender, EventArgs e)
        {
            _statusDispatcherTimer.Stop();
            Status = "";
        }
        #endregion
    }
}
