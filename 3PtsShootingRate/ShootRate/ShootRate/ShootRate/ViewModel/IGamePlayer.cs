using ShootRate.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ShootRate.ViewModel
{
    public interface IGamePlayer
    {
        //Task Hit();
        //Task Miss();

        //Task Rollback();

        ObservableCollection<string> Hits { get; }

        int Tries { get; }

        Task StartSession();
        Task StopSession();
    }
}
