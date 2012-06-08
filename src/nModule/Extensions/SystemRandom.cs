using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// Provides extensions to the System.Random class
    /// </summary>
	public static class SystemRandom
    {
        /// <summary>
        /// The default value for the NextString Extension-Method on System.Random
        /// </summary>
        public const int DefautRandomStringSize = 10;

        /// <summary>
        /// Generates a new string based off the current random object
        /// </summary>
        /// <param name="random">the instance of the Random class this method extends</param>
        /// <param name="size">the size of the string to be generated</param>
        /// <returns>the randomly generated string</returns>
        public static string NextString(this Random random, int size = DefautRandomStringSize)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        internal static T NextEnum<T>(this Random random, IEnumerable<T> exclusions = null)
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            if (exclusions != null)
            {
                foreach (var e in exclusions)
                {
                    values.Remove(e);
                }
            }
            return values[random.Next(values.Count - 1)];
        }
    }
}
