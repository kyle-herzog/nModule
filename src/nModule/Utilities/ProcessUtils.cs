using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace nModule.Utilities
{
    class ProcessUtils
    {
        /// <summary>
        /// Not Completely Implemented
        /// </summary>
        /// <param name="processPath"></param>
        /// <param name="processArguments"></param>
        /// <returns></returns>
        public static Process LaunchExternalProcess(string processPath, string processArguments)
        {
            return LaunchExternalProcess(processPath, processArguments, false, null);
        }

        /// <summary>
        /// Not Completely Implemented
        /// </summary>
        /// <param name="processPath"></param>
        /// <param name="processArguments"></param>
        /// <param name="waitForExit"></param>
        /// <param name="processDataCapturer"></param>
        /// <returns></returns>
        public static Process LaunchExternalProcess(string processPath, string processArguments, bool waitForExit, ProcessDataCapturer processDataCapturer)
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
            externalProcess.Start();
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
