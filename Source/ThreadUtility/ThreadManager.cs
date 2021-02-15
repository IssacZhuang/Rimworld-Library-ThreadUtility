using System;
using Verse;
using HarmonyLib;
using System.Collections.Generic;
using RimWorld;

namespace ThreadUtility
{
    [StaticConstructorOnStartup]
    public class ThreadManager
    {
        static ThreadManager()
        {
            ThreadManager.harmony.PatchAll();

            threadCount = 1;

            trackers = new List<ThingTickTracker>();
            for (int i = 0; i < ThreadManager.threadCount; i++)
            {
                ThingTickTracker tracker = new ThingTickTracker();
                trackers.Add(tracker);
            }

            //for debug
            TPSCounter counter = new TPSCounter();
            ThreadManager.RegisterTick(counter);
        }

        public static void InitThread()
        {
            foreach (var tracker in trackers)
            {
                tracker.Stop();
                tracker.Clear();
            }
        }

        public static void StartAll()
        {
            foreach (var tracker in trackers)
            {
                tracker.Start();
            }
        }

        public static void RegisterTick(ITickThreaded tick, TickerType type = TickerType.Normal)
        {
            GetSelectedTickTracker().RegisterTick(tick, type);
        }

        public static void DeregisterTick(ITickThreaded tick, TickerType type = TickerType.Normal)
        {
            foreach (var tracker in trackers)
            {
                tracker.DeregisterTick(tick, type);
            }
        }

        private static ThingTickTracker GetSelectedTickTracker()
        {
            ThingTickTracker trackerSelected = trackers[compsCount % threadCount];
            compsCount++;
            return trackerSelected;
        }

        private static int compsCount;
        private static int threadCount;
        private static List<ThingTickTracker> trackers;

        private static readonly Harmony harmony = new Harmony("rimdeadspace.threadable");
    }


    class TPSCounter : ITickThreaded
    {
        int count = 0;
        DateTime prevTime;
        public void TickThreaded()
        {
            var currTime = DateTime.Now;
            count++;
            if (currTime.Second != prevTime.Second)
            {
                Log.Message(count.ToString());
                prevTime = DateTime.Now;
                count = 0;
            }
        }
    }
}
