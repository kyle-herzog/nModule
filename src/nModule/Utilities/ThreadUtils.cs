using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace nModule.Utilities
{
    class ThreadUtils
    {
        public static Thread CreateThread(ThreadStart ts)
        {
            return CreateThread(ts, "", true, true, ThreadPriority.Normal);
        }

        public static Thread CreateThread(ThreadStart ts, string name)
        {
            return CreateThread(ts, name, true, true, ThreadPriority.Normal);
        }

        public static Thread CreateThread(ThreadStart ts, string name, bool isBackground, bool startThread, ThreadPriority threadPriority)
        {
            Thread retVal = new Thread(ts);
            retVal.IsBackground = isBackground;
            retVal.Name = name;
            retVal.Priority = threadPriority;
            if (startThread)
                retVal.Start();
            return retVal;
        }

        public static Thread CreateThread(ParameterizedThreadStart pts, string name, object threadParameter)
        {
            return CreateThread(pts, name, threadParameter, true, true, ThreadPriority.Normal);
        }

        public static Thread CreateThread(ParameterizedThreadStart pts, string name, object threadParameter, bool isBackground, bool startThread, ThreadPriority threadPriority)
        {
            Thread retVal = new Thread(pts);
            retVal.IsBackground = isBackground;
            retVal.Name = name;
            retVal.Priority = threadPriority;
            if (startThread)
                retVal.Start(threadParameter);
            return retVal;
        }
    }
}
