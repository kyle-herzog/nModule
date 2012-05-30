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
        public static void AbortSafely(this Thread thread)
        {
            try
            {
                thread.Abort();
            }
            catch (ThreadAbortException) { }
        }

        /// <summary>
        /// Provides a check to test against all possible ThreadState values that would count the Thread as running.
        /// </summary>
        /// <param name="thread">The current thread upon a ThreadState/Running check will be performed.</param>
        /// <returns>Whether or not the current Thread is counted as running.</returns>
        public static bool IsThreadRunning(this Thread thread)
        {
            var threadState = ThreadState.Unstarted;
            try
            {
                threadState = thread.ThreadState;
            }
            catch (ThreadAbortException) { }
            switch (threadState)
            {
                case ThreadState.AbortRequested:
                case ThreadState.Background:
                case ThreadState.Running:
                case ThreadState.StopRequested:
                case ThreadState.SuspendRequested:
                case ThreadState.WaitSleepJoin:
                case ThreadState.Suspended:
                    return true;
            }
            return false;
        }
    }
}
