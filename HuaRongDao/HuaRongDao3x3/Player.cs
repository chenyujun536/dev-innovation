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
        private int _step;
        private List<Station> _candidates = new List<Station>();
        private HashSet<Station> _markedStations = new HashSet<Station>();
        private HashSet<int> _markedStationHash = new HashSet<int>();
        private HashSet<int> _candidatesHash = new HashSet<int>();
        private bool _stopped;
        private Station _succeedStation;
        private bool _succeed;
        //private ReverseMonitor _reverseMonitor;
        //private ReversePlayer _reversePlayer;
        private double _elapsedSeconds;
        private Stack<Station> _answers = new Stack<Station>();
        private Station _initStation = Util.DefaultNums();

        public Station InitialStation
        {
            get
            {
                return _initStation;
            }
        }

        public int CurrentStep
        {
            get { return Success? _succeedStation.Step : TotalStep; }
        }

        public int TotalStep
        {
            get { return _step; }
        }

        public bool Success
        {
            get { return _succeed; }
        }

        public bool Stopped
        {
            get { return _stopped; }
        }

        public double ElapsedSeconds
        {
            get { return _elapsedSeconds; }
        }

        public void Init()
        {
            _candidates.Clear();
            _markedStations.Clear();
            _markedStationHash.Clear();
            _candidatesHash.Clear();
            _answers.Clear();
            _stopped = false;
            _step = 0;
            _succeed = false;
            _elapsedSeconds = 0;
            _initStation = new Station(MergeSortAlg.NewInitNums(), _step);
            _candidates.Add(_initStation);

            //_reversePlayer = new ReversePlayer(_currentNums);
            //_reverseMonitor = new ReverseMonitor();
        }
        
        private int Comparison(Station station, Station station2)
        {
            return station.Cost - station2.Cost;
        }

        public void Stop()
        {
            _stopped = true;
        }

        public async Task Play()
        {
            //_reversePlayer.ReversePlay();
            //_reverseMonitor.Monitor();
            DateTime t1 = DateTime.Now;
            
            await Task.Run(() =>
                {
                    _stopped = false;
                    while (true)
                    {
                        //lock (_lockObj)
                        {
                            var station1 = _candidates.FirstOrDefault(s => !s.Walked);

                            _markedStationHash.Add(station1.GetHashCode());
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
                                    //if (!_markedStations.Contains(s) && !_candidates.Contains(s))
                                    if(!_markedStationHash.Contains(s.GetHashCode()) &&
                                       !_candidatesHash.Contains(s.GetHashCode()))
                                    {
                                        _candidates.Add(s);
                                        _candidatesHash.Add(s.GetHashCode());
                                    }
                                });
                            _candidates.Sort(Comparison);
                            _step += 1;
                            if (Success || Stopped)
                                break;
                        }
                    }
                }
            );

            DateTime t2 = DateTime.Now;
            
            _elapsedSeconds = TimeSpan.FromTicks(t2.Ticks-t1.Ticks).TotalSeconds; 
            
        }

        public void BeginMove()
        {
            _answers = new Stack<Station>();
            Station step = _succeedStation;
            while (step.Parent >= 0)
            {
                _answers.Push(step);
                step = _markedStations.First(s => s.Id == step.Parent);
            }
            _answers.Push(step);
        }

        public bool NextMove(out Station station)
        {
            bool hasNext = _answers.Any();

            station = hasNext? _answers.Pop() : Util.DefaultNums();

            return hasNext;
        }
    }
}
