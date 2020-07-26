using System.Collections.Generic;
using Prism.Mvvm;
using KovaiDotCo.Model;
using System.Collections.ObjectModel;
using PubSub;
using System.Windows.Input;
using Prism.Commands;

namespace KovaiDotCo.EventHub.UI.ViewModel
{
    public class LogViewModel : BindableBase
    {
        #region Private Fields
        private ObservableCollection<AppLogModel> _logList;

        private Hub _hub = Hub.Default;

        #endregion

        #region Properties


        public ObservableCollection<AppLogModel> LogList
        {
            get { return _logList; }
            set
            {
                _logList = value;
                RaisePropertyChanged();
            }
        }

        public ICommand ClearLogCommand { get; }
        #endregion

        #region Constructor
        public LogViewModel()
        {
            LogList = new ObservableCollection<AppLogModel>();

            _hub.Subscribe<AppLogModel>(OnNewLogMessageReceived);

            ClearLogCommand = new DelegateCommand(OnClearLog);
        }
        #endregion

        #region Public Methods
        public void OnClearLog()
        {
            LogList.Clear();
        }

        public void OnNewLogMessageReceived(AppLogModel model)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                // Insert log in the beginning
                _logList.Insert(0, model);
            });
        }

        public void OnClosing()
        {
            // Unsubscribe
            _hub.Unsubscribe<List<AzureDiagnosticGridModel>>();
        }
        #endregion
    }
}
