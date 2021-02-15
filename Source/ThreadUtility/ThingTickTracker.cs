using System;
using System.Collections.Generic;
using System.Threading;
using Verse;

namespace ThreadUtility
{
	class ThingTickTracker : TickTracker
    {
		public ThingTickTracker()
        {
			this.tickNormal = new List<ITickThreaded>();
			this.tickRare = new List<ITickThreaded>();
			this.tickLong = new List<ITickThreaded>();
			this.tickLocal = 0;
		}

		public void RegisterTick(ITickThreaded tick, TickerType type = TickerType.Normal)
		{
            switch (type)
            {
				case TickerType.Normal:
					this.tickNormal.Add(tick);
					break;

				case TickerType.Rare:
					this.tickRare.Add(tick);
					break;

				case TickerType.Long:
					this.tickLong.Add(tick);
					break;
			}
			this.weightInt++;
		}

		public void DeregisterTick(ITickThreaded tick, TickerType type = TickerType.Normal)
        {
			switch (type)
			{
				case TickerType.Normal:
					this.tickNormal.Remove(tick);
					break;

				case TickerType.Rare:
					this.tickRare.Remove(tick);
					break;

				case TickerType.Long:
					this.tickLong.Remove(tick);
					break;
			}
			this.weightInt--;
		}

		public override void Clear()
        {
			this.tickNormal.Clear();
			this.tickRare.Clear();
			this.tickLong.Clear();
        }

		protected override void DoSingleTick()
        {
			//Log.Message("tick");
			foreach (ITickThreaded tick in tickNormal)
            {
				tick.TickThreaded();
            }

			//tick rare
            if (tickLocal % 250 == 0)
            {
				foreach (ITickThreaded tick in tickRare)
				{
					tick.TickThreaded();
				}
			}

			//tick long
			if (tickLocal % 2000 == 0)
			{
				foreach (ITickThreaded tick in tickLong)
				{
					tick.TickThreaded();
				}
				tickLocal = 0;
			}

			this.tickLocal++;
        }

		//Vars =============================================================

		private List<ITickThreaded> tickNormal;
		private List<ITickThreaded> tickRare;
		private List<ITickThreaded> tickLong;
	}
}