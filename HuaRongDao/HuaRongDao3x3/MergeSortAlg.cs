using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Microsoft.Build.Utilities;

namespace HuaRongDao
{
    public class MergeSortAlg
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(MergeSortAlg));

        public static int[] NewInitNums()
        {
            log.Debug(@"NewInitNums starts");
            while (true)
            {
                var sample = CreateOneSample();
                if (IsValidSample(sample))
                {
                    log.Debug(@"NewInitNums ends");
                    return sample;
                }
            }
        }

        private static bool IsValidSample(int[] sample)
        {
            log.Debug(@"Validates by reverse order number pair count");
            int reverseOrders = 0;
            //Sort(sample.Where(s=>!Util.IsAbsent(s)).ToList(), ref reverseOrders);
            reverseOrders = StupidCount(sample.Where(s => !Util.IsAbsent(s)).ToArray());

            log.Debug($@"Reverse order number pair count {reverseOrders}");
            if (Util.SIZE % 2 == 1)
            {
                return reverseOrders % 2 == 0;
            }
            else
            {
                int idxOfAbsent = sample.TakeWhile(s => !Util.IsAbsent(s)).Count();
                int rowIdxofAbsent = idxOfAbsent / Util.Rank +1;

                return (rowIdxofAbsent % 2) == (reverseOrders % 2);
            }
        }

        private static int StupidCount(int[] sample)
        {
            int count = 0;
            for (int i = 0; i < sample.Length; i++)
            {
                for (int j = i; j < sample.Length; j++)
                {
                    if (sample[i] > sample[j])
                        count++;
                }
            }

            return count;
        }

        public static List<int> Sort(List<int> lst, ref int reverseOrders)
        {
            if (lst.Count <= 1)
            {
                return lst;
            }
            int mid = lst.Count / 2;
            List<int> left = new List<int>();//定义左侧List
            List<int> right = new List<int>();//定义右侧List

            //以下兩個循環把lst分為左右兩個List
            for (int i = 0; i < mid; i++)
            {
                left.Add(lst[i]);
            }
            for (int j = mid; j < lst.Count; j++)
            {
                right.Add(lst[j]);
            }
            left = Sort(left, ref reverseOrders);
            right = Sort(right, ref reverseOrders);
            return MergeSorted(left, right, ref reverseOrders);
        }

        /// <summary>
        /// 合併兩個已經排好序的List
        /// </summary>
        /// <param name="left">左側List</param>
        /// <param name="right">右側List</param>
        /// <param name="reverseOrders"></param>
        /// <returns></returns>
        public static List<int> MergeSorted(List<int> left, List<int> right, ref int reverseOrders)
        {
            log.Debug($@"MergeSorted {FormatArray(left)}, {FormatArray(right)}");
            List<int> temp = new List<int>();
            int leftCount = left.Count;
            int rightCount = right.Count;
            while (left.Count > 0 && right.Count > 0)
            {
                if (left[0] <= right[0])
                {
                    temp.Add(left[0]);
                    left.RemoveAt(0);
                }
                else
                {
                    log.Debug($@"({left[0]}, {right[0]})");
                    temp.Add(right[0]);
                    right.RemoveAt(0);
                    reverseOrders++;
                }
            }
            if (left.Count > 0)
            {
                for (int i = 0; i < left.Count; i++)
                {
                    temp.Add(left[i]);
                    if (i > 0)
                    {
                            reverseOrders += rightCount;
                            log.Debug($@"{left[i]} has {rightCount} more reverse order number pairs");
                    }
                }
            }
            if (right.Count > 0)
            {
                for (int i = 0; i < right.Count; i++)
                {
                    temp.Add(right[i]);
                }
            }
            return temp;
        }

        private static int[] CreateOneSample()
        {
            //int[] _currentNums = new int[Util.SIZE];
            //var random = new Random();
            //HashSet<int> indices = new HashSet<int>() {-1};
            //for (int i = 0; i < Util.SIZE; i++)
            //{
            //    var index = -1;
            //    while (indices.Contains(index))
            //    {
            //        index = random.Next(0, Util.SIZE);
            //    }

            //    _currentNums[i] = Util.TargetsNums[index];
            //    indices.Add(index);
            //}
            int[] _currentNums = new[]
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15, 11, 12, 13, 14, 16
            };

            var result = FormatArray(_currentNums);
            log.Debug($@"Create sample {result}");

            return _currentNums;
        }

        private static string FormatArray(IEnumerable<int> _currentNums)
        {
            string result = _currentNums.Aggregate("", (s, i) => s += " " + i.ToString());
            return result;
        }
    }
}