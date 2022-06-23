using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    /// <summary>
    /// Utilitiy methods for general math functions
    /// </summary>
    public static class MathUtilities
    {
        /// <summary>
        /// Returns a value between 0 and 1 based on the parameters given.
        /// </summary>
        /// <param name="current">The value you want to convert to a percentage.</param>
        /// <param name="min">Lower limit. If current is lower than min, it will become the min</param>
        /// <param name="max">Upper limit. If current is higher than max, it will become the max</param>
        /// <returns></returns>
        public static float GetPercentage(float current, float min, float max)
        {
            current = Mathf.Clamp(current, min, max);
            float result;
            result = (min + current) / (max);
            return result;
        }
        /// <summary>
        /// Returns a value between 0 and 1 based on the parameters given.
        /// </summary>
        /// <param name="input">current(x), min(y), max(z)</param>
        /// <returns></returns>
        public static float GetPercentage(Vector3 input)
        {
            float current = input.x;
            float min = input.y;
            float max = input.z;
            current = Mathf.Clamp(current, min, max);
            float result = MathUtilities.GetPercentage(current, min, max);
            return result;
        }
        /// <summary>
        /// Returns a specific flag from a bitmask.
        /// </summary>
        /// <param name="index">The index you want access to.</param>
        /// <returns></returns>
        public static int ReturnBitmaskFlag(int index)
        {
            int result;
            result = 1 << index;
            return index;
        }
        public static bool ArrayContentsAllEqual(int[] args1, int[] args2)
        {
            if (args1.Length == args2.Length)
            {
                int dataLength = args1.Length;
                int flags = 0;
                int alltrue = 0;
                for (int i = 0; i < dataLength; i++)
                {
                    //Construct the all true variable;
                    alltrue += 1 << i;
                    if (args1[i] == args2[2])
                    {
                        //Only set the flag if the contents being compared are the same.
                        flags += 1 << i;
                    }
                }
                return flags == alltrue ? true : false;
            }
            return false;
        }

    }
}

