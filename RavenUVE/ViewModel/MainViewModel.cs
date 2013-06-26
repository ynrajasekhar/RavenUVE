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

        #region Fields

        private readonly ILog logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(ILog logger)
        {
            this.logger = logger;
            ExitCommand = new RelayCommand(Exit);
        }

        internal MainViewModel(ILog logger, ICommand exitCommand)
        {
            this.logger = logger;
            ExitCommand = exitCommand;
        }

        #endregion

        #region Properties

        public ICommand ExitCommand
        {
            get;
            private set;
        }

        #endregion

        #region Methods

        private void Exit()
        {
            logger.Info(m => m("{0}: Shutting down application.", GetType().Name));
            Application.Current.Shutdown();
        }

        #endregion

    }
}