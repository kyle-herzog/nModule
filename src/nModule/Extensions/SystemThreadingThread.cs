using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Threading
{
    /// <summary>
    /// Extension Method class for the System.Threading.Thread class.
    /// </summary>
    public static class SystemThreadingThread
    {
        /// <summary>
        /// Aborts the thread passed. Will log information about the abortion at level 5.
        /// </summary>
        /// <param name="thread">The current instanced Thread object.</param>
        /// <returns>The current Thread.</returns>
        public static Thread SafeAbort(this Thread thread)
        {
            try
            {
                thread.Abort();
            }
            catch (ThreadAbortException)
            {
            }
            return thread;
        }

        /// <summary>
        /// Provides a check to test against all possible ThreadState values that would count the Thread as running.
        /// </summary>
        /// <param name="thread">The current thread upon a ThreadState/Running check will be performed.</param>
        /// <returns>Whether or not the current Thread is counted as running.</returns>
        public static bool IsThreadRunning(this Thread thread)
        {
            if (thread == null)
                return false;
            else
            {
                switch (thread.ThreadState)
                {
                    case System.Threading.ThreadState.AbortRequested:
                    case System.Threading.ThreadState.Background:
                    case System.Threading.ThreadState.Running:
                    case System.Threading.ThreadState.StopRequested:
                    case System.Threading.ThreadState.SuspendRequested:
                    case System.Threading.ThreadState.WaitSleepJoin:
                    case System.Threading.ThreadState.Suspended:
                        return true;
                    case System.Threading.ThreadState.Stopped:
                    case System.Threading.ThreadState.Aborted:
                    case System.Threading.ThreadState.Unstarted:
                        return false;
                }
            }
            return false;
        }
    }
}
