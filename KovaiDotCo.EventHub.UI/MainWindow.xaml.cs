using KovaiDotCo.Model;
using Lamar;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using PubSub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KovaiDotCo.EventHub.UI.ViewModel;

namespace KovaiDotCo.EventHub.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region Private Fields
        /// <summary>
        /// Hub to subsribe to AppMessageModel
        /// </summary>
        private Hub _hub = Hub.Default;
        #endregion

        #region Constructor
        /// <summary>
        /// The MainWindow constructor
        /// </summary>
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            Subscribe();

            DataContext = mainViewModel;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Subscribe to AppMessageModel to show Message Box
        /// </summary>
        private void Subscribe()
        {
            _hub.Subscribe<AppMessageModel>(OnShowMessage);

            // Hook to the Closing event to unsubscribe
            Closing += MainWindow_Closing;
        }

        /// <summary>
        /// Handler for Window Closing event.
        /// Use this method to release/unsubsribe objects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Unsubscribe 
            _hub.Unsubscribe<AppMessageModel>();
        }

        /// <summary>
        /// Displays a message box based on the input data
        /// </summary>
        /// <param name="message"></param>
        private async void OnShowMessage(AppMessageModel message)
        {
            MetroDialogSettings metroDialogSettings = new MetroDialogSettings();
            metroDialogSettings.ColorScheme = message.IsError ? MetroDialogColorScheme.Accented : MetroDialogColorScheme.Accented;
            await this.ShowMessageAsync(message.Title ?? "Machine Test", message.Message, MessageDialogStyle.Affirmative, metroDialogSettings);
        }
        #endregion
    }
}
