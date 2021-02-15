using System;
using Verse;
using HarmonyLib;

namespace ThreadUtility
{
    [HarmonyPatch(typeof(HediffWithComps))]
    internal class Patch_HediffWithComps
    {
        [HarmonyPostfix]
        [HarmonyPatch("ExposeData")]
        public static void RegisterHediffOnLoaded(HediffWithComps __instance)
        {
            if (Scribe.mode != LoadSaveMode.LoadingVars)
            {
                return;
            }

            if (__instance == null)
            {
                Log.Error("can not find instance");
                return;
            }

            if (__instance.comps == null)
            {
                return;
            }

            if (__instance.pawn == null)
            {
                return;
            }

            if (__instance.pawn.Dead)
            {
                return;
            }

            foreach (var comp in __instance.comps) 
            {
                if (comp is ITickThreaded compThreadable)
                {
                    ThreadManager.RegisterTick(compThreadable);
                }
            }
        }


        [HarmonyPostfix]
        [HarmonyPatch("PostMake")]
        public static void RegisterHediffOnCreated(HediffWithComps __instance)
        {
            if (__instance == null)
            {
                Log.Error("can not find instance");
                return;
            }

            if (__instance.comps == null)
            {
                return;
            }

            if (__instance.pawn == null)
            {
                return;
            }

            if (__instance.pawn.Dead)
            {
                return;
            }

            foreach (var comp in __instance.comps)
            {
                if (comp is ITickThreaded compThreadable)
                {
                    ThreadManager.RegisterTick(compThreadable);
                }
            }
        }


        [HarmonyPostfix]
        [HarmonyPatch("PostRemoved")]
        public static void DeregisterHediffOnRemoved(HediffWithComps __instance)
        {
            if (__instance == null)
            {
                Log.Error("can not get instance");
                return;
            }

            if (__instance.comps == null)
            {
                return;
            }

            foreach (var comp in __instance.comps)
            {
                if (comp is ITickThreaded compThreadable)
                {
                    ThreadManager.DeregisterTick(compThreadable);
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("Notify_PawnDied")]
        public static void DeregisterHediffOnKilled(HediffWithComps __instance)
        {
            if (__instance == null)
            {
                Log.Error("can not get instance");
                return;
            }

            if (__instance.comps == null)
            {
                return;
            }

            foreach (var comp in __instance.comps)
            {
                if (comp is ITickThreaded compThreadable)
                {
                    ThreadManager.DeregisterTick(compThreadable);
                }
            }
        }
    }
}
