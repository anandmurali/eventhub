using KovaiDotCo.Model;
using Microsoft.Extensions.DependencyInjection;
using PubSub;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using KovaiDotCo.EventHub.UI.ViewModel;
using MahApps.Metro.Controls.Dialogs;
using KovaiDotCo.EventHub.Contracts;
using System.IO;
using Newtonsoft.Json;

namespace KovaiDotCo.EventHub.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Private Fields
        private Hub _hub = Hub.Default;

        private ServiceProvider _serviceProvider;
        #endregion

        #region Constructors
        /// <summary>
        /// 1. Configure IOC container
        /// </summary>
        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Override OnStartup to hook up exception handling
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CatchUnhanledExceptions();
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Attach event hanlders for UnhandledException event
        /// </summary>
        private void CatchUnhanledExceptions()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) => HandleException((Exception)e.ExceptionObject);

            DispatcherUnhandledException += (s, e) =>
            {
                HandleException(e.Exception);
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                HandleException(e.Exception);
                e.SetObserved();
            };
        }

        /// <summary>
        /// Handles exception by
        /// 1. Logging exception detals to Logs tab
        /// 2. Displays a message to user
        /// </summary>
        /// <param name="ex"></param>
        private void HandleException(Exception ex)
        {
            _hub.Publish(new AppLogModel($"Exception: \r\n {ex.Message} \r\n {ex.StackTrace}"));
            _hub.Publish(new AppMessageModel($"{ex.Message} \r\nPlease check the Logs tab for more details", "An error occurred") { IsError = true });
        }

        /// <summary>
        /// Container configuration
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureServices(IServiceCollection services)
        {
            // Views
            services.AddSingleton<MainWindow>();
            // Models
            services.AddSingleton(GetSettingsModelInstance);
            
            // ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<LogViewModel>();
            services.AddSingleton<MachineTestViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<CreateEventViewModel>();

            services.AddSingleton((x) => DialogCoordinator.Instance);
            services.AddSingleton<IEventHubCommunicator, EventHubCommunicator>();
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Load settings model from settings.json file
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        private SettingsModel GetSettingsModelInstance(IServiceProvider serviceProvider)
        {
            var settingsModel = new SettingsModel();

            // Read from settins.json
            try
            {
                var fileSettingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText("settings.json"));
                if (!string.IsNullOrWhiteSpace(fileSettingsModel.EventHubConnectionString))
                {
                    // Vaiue is there
                    settingsModel.EventHubConnectionString = fileSettingsModel.EventHubConnectionString;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return settingsModel;
        }

        /// <summary>
        /// STartup Event Hanlder. Loads main window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
        #endregion
    }
}
