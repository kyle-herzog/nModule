using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace nModule.Utilities
{
    /// <summary>
    /// Common reusable utility methods for IO
    /// </summary>
    public static class IOUtility
    {
        /// <summary>
        /// Combines the paths given safely trimming the leading slash in the 2nd value
        /// </summary>
        /// <param name="path1">The first part of the path to be combined</param>
        /// <param name="path2">The second part of the path to be combined</param>
        /// <param name="escapePrependingDirectorySeparator">Whether to allow the default functionality of Path.Combine returning path2 when it is prepended with a \</param>
        /// <returns>The safely combined path</returns>
        public static string CombinePath(string path1, string path2, bool escapePrependingDirectorySeparator = true)
        {
            if (escapePrependingDirectorySeparator && path2.StartsWith(Path.DirectorySeparatorChar.ToString()))
                path2 = path2.Substring(1, path2.Length - 2);
            return Path.Combine(path1, path2);
        }

        /// <summary>
        /// Combines the paths given safely trimming the leading slash in the 2nd value
        /// </summary>
        /// <param name="paths">The parts of the path to be combined</param>
        /// <param name="escapePrependingDirectorySeparator">Whether to allow the default functionality of Path.Combine returning path2 when it is prepended with a \</param>
        /// <returns>The safely combined path</returns>
        public static string CombinePath(IEnumerable<string> paths, bool escapePrependingDirectorySeparator = true)
        {
            string combinedPath = "";
            foreach (var path in paths)
            {
                combinedPath = CombinePath(combinedPath, path, escapePrependingDirectorySeparator);
            }
            return combinedPath;
        }

        /// <summary>
        /// Combines the defined SpecialFolder with the designated path
        /// </summary>
        /// <param name="folder">The Environment Special folder to start the combined path.</param>
        /// <param name="path">The path to append to the Envitonment Special folder.</param>
        /// <param name="escapePrependingDirectorySeparator">Whether to allow the default functionality of Path.Combine returning path2 when it is prepended with a \</param>
        /// <returns>The combined path.</returns>
        public static string CombinePath(Environment.SpecialFolder folder, string path, bool escapePrependingDirectorySeparator = true)
        {
            return CombinePath(Environment.GetFolderPath(folder), path, escapePrependingDirectorySeparator);
        }

        /// <summary>
        /// Takes the path given and creates it if it dit not previously exist
        /// </summary>
        /// <param name="path">The path to check for existance.</param>
        /// <returns>Whether the path exists.</returns>
        public static bool EnsurePathExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return Directory.Exists(path);
        }

    }
}
