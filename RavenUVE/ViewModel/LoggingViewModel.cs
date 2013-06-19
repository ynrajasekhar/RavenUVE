using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Common.Logging;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using NLog;
using NlogEventTarget;

namespace RavenUVE.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class LoggingViewModel : ViewModelBase
    {

        /// <summary>
        /// Initializes a new instance of the LoggingViewModel class.
        /// </summary>
        public LoggingViewModel()
        {
            LogCollection = new ObservableCollection<LogEventInfo>();

            if (IsInDesignMode)
            {
                LogCollection.Add(new LogEventInfo(NLog.LogLevel.Error, "Lorem Ipsum"));
                LogCollection.Add(new LogEventInfo(NLog.LogLevel.Warn, "Lorem Ipsum"));
                LogCollection.Add(new LogEventInfo(NLog.LogLevel.Info, "Lorem Ipsum"));
                LogCollection.Add(new LogEventInfo(NLog.LogLevel.Debug, "Lorem Ipsum"));
                LogCollection.Add(new LogEventInfo(NLog.LogLevel.Info, "Lorem Ipsum"));
            }
            else
            {
                foreach (var target in NLog.LogManager.Configuration.AllTargets)
                {
                    if (target.Name.Equals("Event"))
                    {
                        var eventTarget = target as EventTarget;
                        eventTarget.EventReceived += EventReceived;
                        eventTarget.Flush();
                    }
                }
            }

            
        }

        public static ObservableCollection<LogEventInfo> LogCollection { get; set; }

        public Visibility ErrorVisible
        {
            get
            {
                return Logger.IsErrorEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
            set
            {
            }
        }

       public Visibility WarningVisible
        {
            get
            {
                return Logger.IsWarnEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
            set
            {
            }
        }

        public Visibility InfoVisible
        {
            get
            {
                return Logger.IsInfoEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
            set
            {
            }
        }

        private void EventReceived(NLog.LogEventInfo eventLog)
        {
            Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
            {
                if (LogCollection.Count >= 500)
                {
                    LogCollection.RemoveAt(LogCollection.Count - 1);
                }
                LogCollection.Add(eventLog);
            }));
        }

        private ILog Logger
        {
            get { return ServiceLocator.Current.GetInstance<ILog>(); }
        }

    }
}