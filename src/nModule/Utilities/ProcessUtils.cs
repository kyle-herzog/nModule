using System.Diagnostics;

namespace nModule.Utilities
{
    /// <summary>
    /// Utility methods for .NET's Process objects
    /// </summary>
    public class ProcessUtils
    {
        /// <summary>
        /// Not Completely Implemented
        /// </summary>
        /// <param name="processPath"></param>
        /// <param name="processArguments"></param>
        /// <returns></returns>
        public static Process LaunchExternalProcess(string processPath, string processArguments)
        {
            return LaunchExternalProcess(processPath, processArguments, false, false, null);
        }

        /// <summary>
        /// Not Completely Implemented
        /// </summary>
        /// <param name="processPath"></param>
        /// <param name="processArguments"></param>
        /// <param name="startProcess"></param>
        /// <param name="waitForExit"></param>
        /// <param name="processDataCapturer"></param>
        /// <returns></returns>
        public static Process LaunchExternalProcess(string processPath, string processArguments, bool startProcess, bool waitForExit, ProcessDataCapturer processDataCapturer)
        {
            Process externalProcess = new Process();
            ProcessStartInfo externalProcessStartInfo = new ProcessStartInfo();
            externalProcessStartInfo.Arguments = processArguments;
            externalProcessStartInfo.FileName = processPath;
            externalProcessStartInfo.RedirectStandardInput = true;
            externalProcessStartInfo.RedirectStandardError = true;
            externalProcessStartInfo.RedirectStandardOutput = true;
            externalProcessStartInfo.CreateNoWindow = true;
            externalProcessStartInfo.UseShellExecute = false;
            externalProcess.StartInfo = externalProcessStartInfo;
            if (processDataCapturer != null)
            {
                processDataCapturer.Process = externalProcess;
            }
            if (startProcess)
            {
                externalProcess.Start();
            }
            if (processDataCapturer != null)
            {
                //processDataCapturer.ProcessName = externalProcess.ProcessName;
                externalProcess.BeginOutputReadLine();
                externalProcess.BeginErrorReadLine();
            }
            if (waitForExit)
                externalProcess.WaitForExit();
            return externalProcess;
        }
    }
}
