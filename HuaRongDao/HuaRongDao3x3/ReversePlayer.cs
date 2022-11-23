using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HuaRongDao
{
    internal class ReversePlayer
    {
        private int[] _currentNums;

        public ReversePlayer(int[] _currentNums)
        {
            this._currentNums = _currentNums;
        }

        private List<Station> _reversedCandidates = new List<Station>();
        private HashSet<Station> _reversedMarkedStations = new HashSet<Station>();

        private bool _succeed;
        private bool _stopped;
        private int _reverseStep;
        private bool _reverseSucceed;
        private Station _reversedSucceedStation;
        private object _reverseLockObj = new object();
        
        
        public void Stop()
        {
            _stopped = true;
        }

        public void ReversePlay()
        {
            //_reversedCandidates.Add(new Station(Util.MoveAbsent(Util.TargetsNums, Util.SIZE - 1, 0)));
            //while (true)
            //{
            //    lock (_reverseLockObj)
            //    {
            //        var station1 = _reversedCandidates.FirstOrDefault(s => !s.Walked);

            //        _reversedMarkedStations.Add(station1);
            //        _reversedCandidates.Remove(station1);
            //        var stations = Station.Walk(station1);
            //        stations.ForEach(
            //            s =>
            //            {
            //                if (!_reversedMarkedStations.Contains(s) && !_reversedCandidates.Contains(s))
            //                    _reversedCandidates.Add(s);
            //            });
            //        _reversedCandidates.Sort(ReverseComparison);
            //        _reverseStep += 1;
            //        if (_succeed || _stopped)
            //            break;
            //    }
            //}
        }

        private int ReverseComparison(Station x, Station y)
        {
            return y.Cost - x.Cost;
        }
    }
}