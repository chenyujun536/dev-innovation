using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaRongDao
{
    internal class Util
    {
        internal const int SIZE = 16;
        internal const int ABSENT = 16;

        internal static readonly int[] TargetsNums = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, Util.ABSENT };
        public static bool IsAbsent(int num)
        {
            return num == ABSENT;

        }

        public static string Format(int num)
        {
            if (IsAbsent(num))
                return "";
            return num.ToString();

        }

        public static int Distance(int[] nums)
        {
            var distance = 0;
            for (int i = 0; i < SIZE; i++)
            {
                var manHattanDist = Math.Abs(nums[i] - (i + 1));
                distance += (manHattanDist % 4) + (manHattanDist / 4);
                //distance += Math.Abs(nums[i] - TargetsNums[i]);
            }

            return distance;
        }

        internal static int[] MoveAbsent(int[] nums, int absentIdx, int move)
        {
            int[] newNums = new int[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                newNums[i] = nums[i];
            }

            var leftIdx = absentIdx + move;
            newNums[absentIdx] = newNums[leftIdx];
            newNums[leftIdx] = ABSENT;

            return newNums;
        }

        
    }
}
