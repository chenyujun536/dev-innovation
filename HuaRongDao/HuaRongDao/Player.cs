using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HuaRongDao
{
    internal class Player
    {
        private readonly int[] defaultNums = new int[Util.SIZE];
        private int _step;
        private List<Station> _candidates = new List<Station>();
        private HashSet<Station> _markedStations = new HashSet<Station>();

        private List<Station> _reversedCandidates = new List<Station>();
        private HashSet<Station> _reversedMarkedStations = new HashSet<Station>();

        private bool _stopped;
        private int _reverseStep;
        private bool _reverseSucceed;
        private Station _reversedSucceedStation;
        private Station _succeedStation;
        private object _lockObj = new object();
        private object _reverseLockObj = new object();
        private bool _succeed;
        private Thread reverseWorker;
        private Thread reverseMonitor;

        public Station Current
        {
            get
            {
                return _candidates.FirstOrDefault(s=>!s.Walked)?? new Station(defaultNums);
            }
        }

        public int CurrentStep
        {
            get { return Current.Step; }
        }

        public int TotalStep
        {
            get { return _step; }
        }

        public bool Success
        {
            get { return _succeed || _reverseSucceed; }
        }

        public bool Stopped
        {
            get { return _stopped; }
        }

        public void Init()
        {
            _candidates.Clear();
            _markedStations.Clear();
            _reversedCandidates.Clear();
            _reversedMarkedStations.Clear();

            _stopped = false;
            _step = 0;
            _reverseStep = 0;
            _reverseSucceed = false;
            _reversedSucceedStation = null;
            _succeed = false;

            int[] _currentNums = new int[Util.SIZE];
            var random = new Random();
            HashSet<int> indices = new HashSet<int>() {-1};
            for (int i = 0; i < Util.SIZE; i++)
            {
                var index = -1;
                while (indices.Contains(index))
                {
                    index = random.Next(0, Util.SIZE);
                }

                _currentNums[i] = Util.TargetsNums[index];
                indices.Add(index);
            }

            _candidates.Add(new Station(_currentNums, _step));

            reverseMonitor = new Thread(ReverseMonitor);
            reverseMonitor.Start();
        }

        public async Task Next()
        {
            await Task.Run(
                () => {
                    var station1 = _candidates.FirstOrDefault(s => !s.Walked);
                    //Console.WriteLine($@"step {station1.Step}, distance {station1.Distance}");
              
                    _markedStations.Add(station1);
                    _candidates.Remove(station1);
                    var stations = Station.Walk(station1);
                    stations.ForEach(
                        s =>
                        {
                            if (_markedStations.Contains(s)&&!_candidates.Contains(s))
                                _candidates.Add(s);
                        });
                    _candidates.Sort(Comparison);
                    _step += 1;
                });
        }

        private int Comparison(Station station, Station station2)
        {
            return station.Distance - station2.Distance;
        }

        public void Stop()
        {
            _stopped = true;
        }

        public async Task Play()
        {

            reverseWorker = new Thread(ReversePlay);
            reverseWorker.Start();

            await Task.Run(() =>
                {
                    _stopped = false;
                    while (true)
                    {
                        lock (_lockObj)
                        {
                            var station1 = _candidates.FirstOrDefault(s => !s.Walked);

                            _markedStations.Add(station1);
                            _candidates.Remove(station1);
                            if (station1.Distance == 0)
                            {
                                _succeedStation = station1;
                                _succeed = true;
                                break;
                            }

                            var stations = Station.Walk(station1);
                            stations.ForEach(
                                s =>
                                {
                                    if (!_markedStations.Contains(s) && !_candidates.Contains(s))
                                        _candidates.Add(s);
                                });
                            _candidates.Sort(Comparison);
                            _step += 1;
                            if (Success || Stopped)
                                break;
                        }
                    }
                }
            );

            reverseWorker.Join();

        }

        private void ReverseMonitor()
        {
            while (true)
            {
                Thread.Sleep(1000);
                lock(_lockObj)
                    lock(_reverseLockObj)
                    {
                        var intesection = _markedStations.Intersect(_reversedMarkedStations);
                        if (intesection != null && intesection.Any())
                        {
                            _reversedSucceedStation = intesection.First();
                            _reverseSucceed = true;
                        }
                    }
            }

        }

        private void ReversePlay()
        {
            _reversedCandidates.Add(new Station(Util.MoveAbsent(Util.TargetsNums, Util.SIZE-1, 0)));
            while (true)
            {
                lock (_reverseLockObj)
                {
                    var station1 = _reversedCandidates.FirstOrDefault(s => !s.Walked);

                    _reversedMarkedStations.Add(station1);
                    _reversedCandidates.Remove(station1);
                    var stations = Station.Walk(station1);
                    stations.ForEach(
                        s =>
                        {
                            if (!_reversedMarkedStations.Contains(s) && !_reversedCandidates.Contains(s))
                                _reversedCandidates.Add(s);
                        });
                    _reversedCandidates.Sort(ReverseComparison);
                    _reverseStep += 1;
                    if (Success || Stopped)
                        break;
                }
            }
        }

        private int ReverseComparison(Station x, Station y)
        {
            return y.Distance - x.Distance;
        }
    }
}
