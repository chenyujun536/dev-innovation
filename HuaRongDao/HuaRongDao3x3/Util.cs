using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaRongDao
{
    internal class Util
    {
        static Util()
        {
            TargetsNums = new int[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                TargetsNums[i] = i + 1;
            }

            TargetsNums[SIZE - 1] = ABSENT;
        }

        internal static int SIZE
        {
            get { return Rank * Rank; }
        }

        internal static int ABSENT
        {
            get { return SIZE; }
        }

        internal const int Rank = 4;

        internal static int[] TargetsNums;
        
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
        public static Station DefaultNums()
        {
            return new Station(new int[Util.SIZE]);
        }

        public static int Distance(int[] nums)
        {
            var distance = 0;
            for (int i = 0; i < SIZE; i++)
            {
                var manHattanDist = Math.Abs(nums[i] - (i + 1));
                distance += (manHattanDist % Rank) + (manHattanDist / Rank);
                //distance += Math.Abs(nums[i] - TargetsNums[i]);
            }

            return distance;
        }

        internal static int[] MoveAbsent(int[] nums, int absentIdx, int move)
        {
            int[] newNums = new int[SIZE];

            nums.CopyTo(newNums, 0);

            var leftIdx = absentIdx + move;
            newNums[absentIdx] = newNums[leftIdx];
            newNums[leftIdx] = ABSENT;

            return newNums;
        }
    }
}
