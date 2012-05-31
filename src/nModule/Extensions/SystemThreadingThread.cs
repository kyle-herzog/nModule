using System.Linq;

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
    }
}
