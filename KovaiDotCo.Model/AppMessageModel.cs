using System;
using System.Collections.Generic;
using System.Text;

namespace KovaiDotCo.Model
{
    /// <summary>
    /// POCO class to store details which has to be shown in a message dialog box.
    /// </summary>
    public class AppMessageModel
    {
        #region Properties
        /// <summary>
        /// Gets or sets the message to display on the message box
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the title of the message box. If the titil is null, then default title will be shown.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Property to indicate if the message is an error
        /// </summary>
        public bool IsError { get; set; }
        #endregion

        #region Constructors
        public AppMessageModel(string message, string title = null)
        {
            Message = message;
            Title = title;
        }
        #endregion
    }
}
