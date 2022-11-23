using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HuaRongDao
{
    internal class Station
    {
        public int Id { get; set; }
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
            long hashCode = 0;
            for (int i = 0; i < Util.SIZE; i++)
            {
                hashCode = hashCode*31 + Nums[i];
            }

            return hashCode.GetHashCode();
        }

        public int[] Nums { get; set; }
        public int Step { get; set; }

        private int _distance;
        private int _parent;
        private static int _gId = 1;
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

        public Station(int[] nums, int step = 0, int parent=-1)
        {
            Nums = nums;
            Step = step;
            _distance = -1;
            _parent = parent;
            Id = UniqueId();
        }

        private static int UniqueId()
        {
            return _gId++;
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

            if (absentIdx < Util.Rank) //0,1,2
                up = false;
            if (absentIdx >= (Util.SIZE - Util.Rank)) //6,7,8
                down = false;
            if (absentIdx % Util.Rank == 0)//0,3,6
                left = false;
            if (absentIdx % Util.Rank == (Util.Rank-1))//2,5,8
                right = false;

            if (left)
            {
                stations.Add(new Station(Util.MoveAbsent(nums, absentIdx, -1), station1.Step+1, station1.Id));
            }
            if (right)
            {
                stations.Add(new Station(Util.MoveAbsent(nums, absentIdx, 1), station1.Step + 1, station1.Id));
            }
            if (up)
            {
                stations.Add(new Station(Util.MoveAbsent(nums, absentIdx, -1 * Util.Rank), station1.Step + 1, station1.Id));
            }
            if (down)
            {
                stations.Add(new Station(Util.MoveAbsent(nums, absentIdx, Util.Rank), station1.Step + 1, station1.Id));
            }

            station1.Walked = true;
            return stations;
        }

    }
}