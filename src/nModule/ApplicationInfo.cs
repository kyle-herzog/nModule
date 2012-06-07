using System;
using System.Text;
using System.Reflection;
using nModule.Utilities;

namespace nModule
{
    /// <summary>
    /// Central class for information about the application.
    /// </summary>
    public static class ApplicationInfo
    {
        /// <summary>
        /// The Directory from which the application is executing.
        /// </summary>
        public static string Directory { get; set; }
        /// <summary>
        /// The Assembly/Application code from which this library system was launched/loaded.
        /// </summary>
        public static Assembly Applicaition { get; set; }
        /// <summary>
        /// The name of the executing Assembly/Application
        /// </summary>
        public static string Name { get { return Applicaition.GetName().Name; } }
        /// <summary>
        /// The fullname of the executing Assembly/Application
        /// </summary>
        public static string FullName { get { return Applicaition.FullName; } }
        /// <summary>
        /// The path of the executing Assembly/Application
        /// </summary>
        public static string Executable { get { return Applicaition.Location; } }
        /// <summary>
        /// The path where data belonging to the application shall be stored.
        /// </summary>
        public static string ApplicationData { get; private set; }
        /// <summary>
        /// The path where common data belonging to the application shall be stored.
        /// </summary>
        public static string CommonApplicationData { get; private set; }
        /// <summary>
        /// Whether the executing backend framework is Mono or MS.NET
        /// </summary>
        public static bool RunningInMono { get; private set; }

        static ApplicationInfo()
        {
            Applicaition = Assembly.GetEntryAssembly();
            ApplicationData = IOUtility.CombinePath(Environment.SpecialFolder.ApplicationData, Name);
            CommonApplicationData = IOUtility.CombinePath(Environment.SpecialFolder.CommonApplicationData, Name);
            Directory = AppDomain.CurrentDomain.BaseDirectory;
            RunningInMono = Type.GetType("Mono.Runtime") != null;
        }
    }
}
