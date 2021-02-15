using System;
using System.Collections.Generic;
using System.Threading;
using Verse;

namespace ThreadUtility
{
    abstract class TickTracker
    {
        public int weight
        {
            get
            {
                return this.weightInt;
            }
        }

        public void Start()
        {
            this.canStop = false;
            ThreadStart tickUpdateInThread = new ThreadStart(TickUpdate);
            this.thread = new Thread(tickUpdateInThread);
            this.thread.Start();
        }

        public void Stop()
        {
            this.canStop = true;
        }

        private void TickUpdate()
        {
            this.ticksAbsCached = GenTicks.TicksAbs;
            while (!canStop)
            {
                Thread.Sleep(2);
                if (!Find.TickManager.Paused)
                {
                    int ticksInterval = GenTicks.TicksAbs - ticksAbsCached;
                    for (int i = 0; i < ticksInterval; i++)
                    {
                        try
                        {
                            this.DoSingleTick();
                        }
                        catch (Exception e)
                        {
                            Log.Error(e.Message);
                        }
                    }
                    this.ticksAbsCached = GenTicks.TicksAbs;
                }
            }
            
        }

        protected abstract void DoSingleTick();
        public abstract void Clear();

        //Vars =============================================================

        protected int ticksAbsCached;
        protected int tickLocal;
        protected bool canStop;
        protected Thread thread;
        protected int weightInt;

    }
}
