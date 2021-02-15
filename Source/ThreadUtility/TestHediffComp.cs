using System;
using Verse;

namespace ThreadUtility
{
    class TestHediffComp : HediffComp, ITickThreaded
    {
        private int logTick;
        public void TickThreaded()
        {
            logTick++;
            if (logTick>=360) 
            {
                Log.Message("Hediff comp from sub-thread");
                logTick = 0;
            }
        }
    }
}
