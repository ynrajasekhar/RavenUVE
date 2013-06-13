using System.Windows;
using System.Windows.Input;
using Common.Logging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using NLog;

namespace RavenUVE.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            ExitCommand = new RelayCommand(Exit);
        }

        private void Exit()
        {
            Logger.Info(m => m("{0}: Shutting down application.", GetType().Name));
            Application.Current.Shutdown();
        }

        public ICommand ExitCommand
        {
            get;
            private set;
        }

        private ILog Logger
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ILog>();
            }
        }

        
    }
}