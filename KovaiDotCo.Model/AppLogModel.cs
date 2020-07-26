using System;
using System.Collections.Generic;
using System.Text;

namespace KovaiDotCo.Model
{
    /// <summary>
    /// POCO class stored an application log detail
    /// </summary>
    public class AppLogModel
    {
        #region Properties
        /// <summary>
        /// The time when the log message was created
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Set true if the message is an error
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// Gets or sets the Message
        /// </summary>
        public string Message { get; set; }
        #endregion

        #region Cosntructors
        public AppLogModel(string message, bool isError = false)
        {
            Time = DateTime.Now;
            IsError = isError;
            Message = message;
        }
        #endregion
    }
}
