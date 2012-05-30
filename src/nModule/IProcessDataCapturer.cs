using System.Diagnostics;

namespace nModule
{
    /// <summary>
    /// Interface for processing data from a the System.Diagnostics.Process object
    /// </summary>
    public interface IProcessDataCapturer
    {
        /// <summary>
        /// The Process to which the Capturer will bind
        /// </summary>
        Process Process { get; set; }
        /// <summary>
        /// The name of the currently attached process
        /// </summary>
        string ProcessName { get; }
        /// <summary>
        /// Whether or not this Capturer shall write all at once or as messages arrive
        /// </summary>
        bool WriteSingleEntry { get; }
        /// <summary>
        /// The last message received from the attached Process
        /// </summary>
        string LastOutput { get; }
    }
}