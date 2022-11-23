using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HuaRongDao
{
    internal class Station
    {
        public override bool Equals(object obj)
        {
            var s2 = obj as Station;
            if (s2 == null)
                return false;

            for (int i = 0; i < Util.SIZE; i++)
            {
                if (s2.Nums[i] != Nums[i])
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            for (int i = 0; i < Util.SIZE; i++)
            {
                hashCode += Nums[i] * (Util.SIZE - i);
            }

            return hashCode;
        }

        public int[] Nums { get; set; }
        public int Step { get; set; }

        private int _distance;
        private int _parent;
        public bool Walked { get; set; }
        public int Distance
        {
            get
            {
                if(_distance <0)
                    _distance = Util.Distance(Nums);
                return _distance;
            }
        }

        public int Cost
        {
            get { return Step + Distance; }
        }

        public int Parent
        {
            get { return _parent; }
        }

        public Station(int[] nums, int step = 0, int parent=0)
        {
            Nums = nums;
            Step = step;
            _distance = -1;
            _parent = parent;
        }

        public static List<Station> Walk(Station station1)
        {
            List<Station> stations = new List<Station>(4);
            var nums = station1.Nums;
            int absentIdx = nums.TakeWhile(s => s != Util.ABSENT).Count();
            bool left = true;
            bool right = true;
            bool up = true;
            bool down = true;

            if (absentIdx < 4) //0,1,2,3
                up = false;
            if (absentIdx > 11) //12,13,14,15
                down = false;
            if (absentIdx % 4 == 0)//0,4,8,12
                left = false;
            if (absentIdx % 4 == 3)//3,7,11,15
                right = false;

            if (left)
            {
                stations.Add(new Station(Util.MoveAbsent(nums, absentIdx, -1), station1.Step+1));
            }
            if (right)
            {
                stations.Add(new Station(Util.MoveAbsent(nums, absentIdx, 1), station1.Step + 1));
            }
            if (up)
            {
                stations.Add(new Station(Util.MoveAbsent(nums, absentIdx, -4), station1.Step + 1));
            }
            if (down)
            {
                stations.Add(new Station(Util.MoveAbsent(nums, absentIdx, 4), station1.Step + 1));
            }

            station1.Walked = true;
            return stations;
        }

    }
}