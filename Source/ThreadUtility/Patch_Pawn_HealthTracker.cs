using System;
using Verse;
using HarmonyLib;

namespace ThreadUtility
{
    [HarmonyPatch(typeof(Pawn_HealthTracker))]
    internal class Patch_Pawn_HealthTracker
    {
        [HarmonyPostfix]
        [HarmonyPatch("Notify_Resurrected")]
        public static void RegisterTickOnResurrected(Pawn_HealthTracker __instance)
        {
            if (__instance == null)
            {
                Log.Error("can not find instance");
                return;
            }

            if (__instance.hediffSet == null)
            {
                return;
            }


            foreach (var hediff in __instance.hediffSet.GetHediffs<Hediff>())
            {
                if (hediff is ITickThreaded tick)
                {
                    ThreadManager.RegisterTick(tick);
                }

                if (hediff is HediffWithComps hediffWithComps)
                {
                    if (hediffWithComps.comps == null)
                    {
                        return;
                    }

                    foreach (var comp in hediffWithComps.comps)
                    {
                        if (comp is ITickThreaded compTick)
                        {
                            ThreadManager.RegisterTick(compTick);
                        }
                    }
                }
            }
        }
    }
}
