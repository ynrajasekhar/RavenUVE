/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:RavenUVE"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Common.Logging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using Raven.Client;
using Raven.Client.Document;
using RavenUVE.Model;

namespace RavenUVE.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                if (!SimpleIoc.Default.IsRegistered<ILog>())
                    SimpleIoc.Default.Register<ILog>(() => LogManager.GetLogger("DBLogger"));
                if (!SimpleIoc.Default.IsRegistered<IMessenger>())
                    SimpleIoc.Default.Register<IMessenger>(() => Messenger.Default);
                if (!SimpleIoc.Default.IsRegistered<IMessenger>())
                    SimpleIoc.Default.Register<IConfiguration, Configuration>();
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Register<ILog>(() => LogManager.GetLogger("DBLogger"));
                SimpleIoc.Default.Register<IMessenger>(() => Messenger.Default);
                SimpleIoc.Default.Register<IConfiguration, Configuration>();
            }

            SimpleIoc.Default.Register<IConfiguration, Configuration>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoggingViewModel>();
            //SimpleIoc.Default.Register<ConnectViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                Logger.Info(m => m("{0}: Requesting instance of {1}", GetType().Name, typeof(MainViewModel).Name));
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Gets the LoggerView property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public LoggingViewModel LoggerView
        {
            get
            {
                Logger.Info(m => m("{0}: Requesting instance of {1}", GetType().Name, typeof(LoggingViewModel).Name));
                return ServiceLocator.Current.GetInstance<LoggingViewModel>();
            }
        }

        /// <summary>
        /// Gets the ConnectDialog property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ConnectViewModel ConnectDialog
        {
            get
            {
                Logger.Info(m => m("{0}: Requesting instance of {1}", GetType().Name, typeof(ConnectViewModel).Name));
                return new ConnectViewModel(ServiceLocator.Current.GetInstance<ILog>(), 
                    ServiceLocator.Current.GetInstance<IMessenger>(), 
                    ServiceLocator.Current.GetInstance<IConfiguration>());
            }
        }

        private ILog Logger
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ILog>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}