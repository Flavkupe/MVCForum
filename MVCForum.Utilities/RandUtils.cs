using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCForum.Utilities
{
    public static class RandUtils
    {
        /// <summary>
        /// Gets a randomly seeded random number.
        /// </summary>
        /// <param name="min">Min value for number, inclusive</param>
        /// <param name="max">Max value for number, exclusive</param>
        /// <returns></returns>
        public static int GetRandom(int min, int max)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            return rand.Next(min, max);
        }
    }
}
