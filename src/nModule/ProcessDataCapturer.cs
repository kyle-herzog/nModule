using System;
using System.Text;
using System.Diagnostics;

namespace nModule
{
    /// <summary>
    /// An base object for captuing the output stream of an externally running process.
    /// </summary>
    public abstract class ProcessDataCapturer
    {
        private Process _process;
        private readonly StringBuilder _processData = new StringBuilder();

        /// <summary>
        /// The currently attached Process
        /// </summary>
        public Process Process
        {
            get
            {
                return _process;
            }
            set
            {
                if (_process != null)
                {
                    UnWireProcess();
                }
                _process = value;
                if (_process != null)
                {
                    WireProcess();
                }
            }
        }

        /// <summary>
        /// The Process' name
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// Whether the ProcessDataCapturer will wait to write all the output at a single time.
        /// </summary>
        public bool WriteSingleEntry { get; set; }

        /// <summary>
        /// Instanciates a ProcessDataCapturer
        /// </summary>
        protected ProcessDataCapturer()
        {
            WriteSingleEntry = false;
        }
        /// <summary>
        /// Instanciates a ProcessDataCapturer with with the supplied Process
        /// </summary>
        /// <param name="process">The Process to which this ProcessDataCapturer shall attach and listen</param>
        protected ProcessDataCapturer(Process process) : this() { Process = process; }
        /// <summary>
        /// Instanciates a ProcessDataCapturer with with the supplied Process
        /// </summary>
        /// <param name="process">The Process to which this ProcessDataCapturer shall attach and listen</param>
        /// <param name="writeSingleEntry">Value to predefine how captured data is written</param>
        protected ProcessDataCapturer(Process process, bool writeSingleEntry)
            : this()
        {
            Process = process;
            WriteSingleEntry = writeSingleEntry;
        }

        /// <summary>
        /// A delegate method for process DataReceived from the attached Process.
        /// </summary>
        /// <param name="sender">The Process sending data</param>
        /// <param name="e">The Data from the Event triggered by said process.</param>
        protected void CaptureDelegateMethod(object sender, DataReceivedEventArgs e)
        {
            if (!WriteSingleEntry)
                _processData.AppendLine(e.Data);
            else
            {
                _processData.AppendLine(e.Data);
                Write(e.Data);
            }
        }

        /// <summary>
        /// The currently Attached Process has exited
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Exited(object sender, EventArgs e)
        {
            if (!WriteSingleEntry)
                Write(_processData.ToString());
            _process.OutputDataReceived -= CaptureDelegateMethod;
            _process.ErrorDataReceived -= CaptureDelegateMethod;
        }

        private void UnWireProcess()
        {
            _process.CancelOutputRead();
            _process.CancelErrorRead();
            _process.OutputDataReceived -= CaptureDelegateMethod;
            _process.ErrorDataReceived -= CaptureDelegateMethod;
            _process.Exited -= new EventHandler(Exited);
        }

        private void WireProcess()
        {
            _process.Exited += new EventHandler(Exited);
            _process.OutputDataReceived += CaptureDelegateMethod;
            _process.ErrorDataReceived += CaptureDelegateMethod;
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        protected abstract void Write(string value);
    }
}
