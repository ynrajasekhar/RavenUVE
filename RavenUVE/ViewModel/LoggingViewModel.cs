using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using Common.Logging;
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

        #region Classes

        internal class LogFilter
        {
            public String PropertyName { get; set; }

            public bool Enabled { get; set; }
        }

        #endregion

        #region Fields

        private const string className = "LoggingViewModel";
        private ICollectionView loggingView;
        private readonly Dictionary<NLog.LogLevel, LogFilter> logLevelFilter;
        private readonly ILog logger;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the LoggingViewModel class.
        /// </summary>
        public LoggingViewModel(ILog logger) : this(logger, new CollectionView(new Collection<String>()))
        {
            Contract.Requires(null != logger);

            logger.Trace(m => m("{0}: Creating.", className));
            
            if (IsInDesignMode)
            {
                LogCollection.Add(new LogEventInfo(NLog.LogLevel.Error, "DBLogger", "Lorem Ipsum"));
                LogCollection.Add(new LogEventInfo(NLog.LogLevel.Warn, "DBLogger", "Lorem Ipsum"));
                LogCollection.Add(new LogEventInfo(NLog.LogLevel.Info, "DBLogger", "Lorem Ipsum"));
                LogCollection.Add(new LogEventInfo(NLog.LogLevel.Debug, "DBLogger", "Lorem Ipsum"));
                LogCollection.Add(new LogEventInfo(NLog.LogLevel.Info, "DBLogger", "Lorem Ipsum"));
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

                SetupLogCollectionView(CollectionViewSource.GetDefaultView(LogCollection));
            }

            logger.Trace(m => m("{0}: Created {0}", className, className));
        }

        internal LoggingViewModel(ILog logger, ICollectionView collectionView)
        {
            Contract.Requires(null != collectionView);

            this.logger = logger;
            SetupLogCollectionView(collectionView);

            logLevelFilter = new Dictionary<NLog.LogLevel, LogFilter> 
            { 
                { NLog.LogLevel.Fatal, new LogFilter { PropertyName = "FilterFatal", Enabled = false} }, 
                { NLog.LogLevel.Error, new LogFilter { PropertyName = "FilterError", Enabled = false} }, 
                { NLog.LogLevel.Warn, new LogFilter { PropertyName = "FilterWarn", Enabled = false} }, 
                { NLog.LogLevel.Info, new LogFilter { PropertyName = "FilterInfo", Enabled = false} }, 
                { NLog.LogLevel.Debug, new LogFilter { PropertyName = "FilterDebug", Enabled = false} }, 
                { NLog.LogLevel.Trace, new LogFilter { PropertyName = "FilterTrace", Enabled = false} } 
            };

            LogCollection = new ObservableCollection<LogEventInfo>();
        }

        #endregion

        #region Properties

        public ObservableCollection<LogEventInfo> LogCollection { get; set; }

        public Visibility FatalVisible
        {
            get
            {
                logger.Trace(m => m("{0}: Fetch FatalVisible Property, this is set to {1}!", className, logger.IsFatalEnabled));
                return logger.IsFatalEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
            set
            {
                logger.Debug(m => m("{0}: Tried to set FatalVisible Property, this should not happen!", className));
            }
        }

        public bool FilterFatal
        {
            get
            {
                return logLevelFilter[NLog.LogLevel.Fatal].Enabled;
            }
            set
            {
                SetLogLevelProperty(NLog.LogLevel.Fatal, value);
            }
        }

        public Visibility ErrorVisible
        {
            get
            {
                logger.Trace(m => m("{0}: Fetch ErrorVisible Property, this is set to {1}!", className, logger.IsErrorEnabled));
                return logger.IsErrorEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
            set
            {
                logger.Debug(m => m("{0}: Tried to set ErrorVisible Property, this should not happen!", className));
            }
        }

        public bool FilterError
        {
            get
            {
                return logLevelFilter[NLog.LogLevel.Error].Enabled;
            }
            set
            {
                SetLogLevelProperty(NLog.LogLevel.Error, value);
            }
        }

       public Visibility WarningVisible
        {
            get
            {
                logger.Trace(m => m("{0}: Fetch WarningVisible Property, this is set to {1}!", className, logger.IsWarnEnabled));
                return logger.IsWarnEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
            set
            {

                logger.Debug(m => m("{0}: Tried to set WarningVisible Property, this should not happen!", className));
            }
        }

       public bool FilterWarn
       {
           get
           {
               return logLevelFilter[NLog.LogLevel.Warn].Enabled;
           }
           set
           {
               SetLogLevelProperty(NLog.LogLevel.Warn, value);
           }
       }

        public Visibility InfoVisible
        {
            get
            {
                logger.Trace(m => m("{0}: Fetch InfoVisible Property, this is set to {1}!", className, logger.IsInfoEnabled));
                return logger.IsInfoEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
            set
            {
                logger.Debug(m => m("{0}: Tried to set InfoVisible Property, this should not happen!", className));
            }
        }

        public bool FilterInfo
        {
            get
            {
                return logLevelFilter[NLog.LogLevel.Info].Enabled;
            }
            set
            {
                SetLogLevelProperty(NLog.LogLevel.Info, value);
            }
        }

        public Visibility DebugVisible
        {
            get
            {
                logger.Trace(m => m("{0}: Fetch DebugVisible Property, this is set to {1}!", className, logger.IsDebugEnabled));
                return logger.IsDebugEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
            set
            {
                logger.Debug(m => m("{0}: Tried to set DebugVisible Property, this should not happen!", className));
            }
        }

        public bool FilterDebug
        {
            get
            {
                return logLevelFilter[NLog.LogLevel.Debug].Enabled;
            }
            set
            {
                SetLogLevelProperty(NLog.LogLevel.Debug, value);
            }
        }

        public Visibility TraceVisible
        {
            get
            {
                logger.Trace(m => m("{0}: Fetch TraceVisiböle Property, this is set to {1}!", className, logger.IsTraceEnabled));
                return logger.IsDebugEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
            set
            {
                logger.Debug(m => m("{0}: Tried to set TraceVisible Property, this should not happen!", className));
            }
        }

        public bool FilterTrace
        {
            get
            {
                return logLevelFilter[NLog.LogLevel.Trace].Enabled;
            }
            set
            {
                SetLogLevelProperty(NLog.LogLevel.Trace, value);
            }
        }

        #endregion

        #region Methods

        private void SetupLogCollectionView(ICollectionView logCollectionView)
        {
            Contract.Requires(null != logCollectionView);

            loggingView = logCollectionView;
            if (loggingView != null)
            {
                loggingView.Filter = LogFilterMethod;
            }
        }

        private void SetLogLevelProperty(NLog.LogLevel logLevel, bool value)
        {
            Contract.Requires(null != logLevel);

            logger.Trace(m => m("{0}: Entering SetLogLevelProperty", className));
            if (logLevelFilter[logLevel].Enabled != value)
            {
                logger.Debug(m => m("{0}: Setting filtering of {1} level to {2}.", className, logLevel, value));
                logLevelFilter[logLevel].Enabled = value;
                RaisePropertyChanged(logLevelFilter[logLevel].PropertyName);
                loggingView.Refresh();
            }

            logger.Trace(m => m("{0}: Leaving SetLogLevelProperty", className));
        }

        private void EventReceived(NLog.LogEventInfo eventLog)
        {
            Contract.Requires(null != eventLog);

            Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
            {
                if (LogCollection.Count >= 500)
                {
                    LogCollection.RemoveAt(LogCollection.Count - 1);
                }
                LogCollection.Add(eventLog);
            }));
        }

        private bool LogFilterMethod(object obj)
        {
            Contract.Requires(null != obj);

            var logEventInfo = obj as LogEventInfo;
            if (logEventInfo != null)
            {
                return !logLevelFilter[logEventInfo.Level].Enabled;
            }
            logger.Debug(m => m("{0}: Object is not of LogEventInfo type.", className));

            return false;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(null != logLevelFilter);
            Contract.Invariant(logLevelFilter.Count == 6);
            Contract.Invariant(null != loggingView);
            Contract.Invariant(null != LogCollection); 
        }

        #endregion

    }
}