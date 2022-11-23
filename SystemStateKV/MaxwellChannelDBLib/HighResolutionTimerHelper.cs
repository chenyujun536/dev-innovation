using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MaxwellChannelDBLib
{
    public static class HighResolutionTimerHelper
    {
        [DllImport("Winmm.dll")]
        private static extern int timeBeginPeriod(uint uPeriod);

        [DllImport("Winmm.dll")]
        private static extern int timeEndPeriod(uint uPeriod);

        public static void EnableHighResolutionTimer()
        {
            timeBeginPeriod(1);
        }

        public static void DisableHighResolutionTimer()
        {
            timeEndPeriod(1);
        }
    }
}
