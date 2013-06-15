using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
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

            foreach (var target in NLog.LogManager.Configuration.AllTargets)
            {
                if (target.Name.Equals("Event"))
                {
                    var eventTarget = target as EventTarget;
                    eventTarget.EventReceived += EventReceived;
                }
            }
        }

        public static ObservableCollection<LogEventInfo> LogCollection { get; set; }
        
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
    }
}