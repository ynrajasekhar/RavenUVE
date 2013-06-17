using System;
using System.Collections.Generic;
using NLog;
using NLog.Targets;

namespace NlogEventTarget
{
    [Target("Event")]
    public sealed class EventTarget : TargetWithLayout
    {

        #region Fields

        private Queue<LogEventInfo> logQueue;

        #endregion

        #region Events

        public event Action<LogEventInfo> EventReceived;

        #endregion

        #region Constructor

        public EventTarget()
        {
            QueueSize = 100;
            logQueue = new Queue<LogEventInfo>();
        }

        #endregion 

        #region Properties

        public int QueueSize { get; set; }

        #endregion

        #region Methods

        public void Flush()
        {
            if (EventReceived != null)
            {
                while (logQueue.Count > 0)
                {
                    EventReceived(logQueue.Dequeue());
                }
            }
        }

        protected override void Write(LogEventInfo logEvent)
        {
            if (logQueue.Count >= QueueSize)
            {
                logQueue.Dequeue();
            }

            logQueue.Enqueue(logEvent);

            Flush();
        }

        #endregion

    }
}
