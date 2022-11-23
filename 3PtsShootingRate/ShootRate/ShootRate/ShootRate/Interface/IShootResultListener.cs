using System.Collections.Generic;
using System.Text;

namespace ShootRate.Interface
{
    public interface IShootResultListener
    {
        void HearHit();
        void HearMiss();
    }
}
