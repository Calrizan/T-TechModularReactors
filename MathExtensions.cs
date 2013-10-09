using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTechModularReactors
{
    public static class MathExtensions
    {
        /// <summary>
        /// Clamp the input value between a given minimum and maximum
        /// </summary>
        /// <param name="val">The value to be clamped.</param>
        /// <param name="min">The minimum value that can be returned.</param>
        /// <param name="max">The maximum value that can be returned.</param>
        /// <returns></returns>
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
    }
}
