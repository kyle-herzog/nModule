using System.Text;

namespace System
{
    /// <summary>
    /// Provides extensions to the System.Random class
    /// </summary>
	public static class SystemRandom
	{
        /// <summary>
        /// Generates a new string based off the current random object
        /// </summary>
        /// <param name="random">the instance of the Random class this method extends</param>
        /// <param name="size">the size of the string to be generated</param>
        /// <returns>the randomly generated string</returns>
        public static string NextString(this Random random, int size)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}
