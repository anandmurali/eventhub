using System;
using System.Collections.Generic;
using System.Text;

namespace KovaiDotCo.Model
{
    /// <summary>
    /// POCO model for data binded on MachineTest View data grid
    /// </summary>
    public class AzureDiagnosticGridModel : Prism.Mvvm.BindableBase
    {
        #region Private Fields
        private string _operationName;

        private string _status;

        private string _time;

        private string _timeStamp;

        private string _subscription;

        private string _eventInitiatedBy;

        private DateTime _receivedTime;
        #endregion

        #region Properties

        public string OperationName
        {
            get { return _operationName; }
            set
            {
                _operationName = value;
                RaisePropertyChanged();
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged();
            }
        }

        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                RaisePropertyChanged();
            }
        }

        public string TimeStamp
        {
            get { return _timeStamp; }
            set
            {
                _timeStamp = value;
                RaisePropertyChanged();
            }
        }

        public string Subscription
        {
            get { return _subscription; }
            set
            {
                _subscription = value;
                RaisePropertyChanged();
            }
        }

        public string EventInitiatedBy
        {
            get { return _eventInitiatedBy; }
            set
            {
                _eventInitiatedBy = value;
                RaisePropertyChanged();
            }
        }


        public DateTime ReceivedTime
        {
            get { return _receivedTime; }
            set
            {
                _receivedTime = value;
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}
