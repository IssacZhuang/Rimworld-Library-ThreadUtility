using Verse;
using HarmonyLib;

namespace ThreadUtility
{
    [HarmonyPatch(typeof(Hediff))]
    internal class Patch_Hediff
    {

        [HarmonyPostfix]
        [HarmonyPatch("ExposeData")]
        public static void RegisterThingOnLoaded(Hediff __instance)
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

            if (__instance.pawn == null)
            {
                return;
            }

            if (__instance.pawn.Dead)
            {
                return;
            }

            if (__instance is ITickThreaded thing)
            {
                ThreadManager.RegisterTick(thing);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("PostMake")]
        public static void RegisterThingOnCreated(Hediff __instance)
        {
            if (__instance == null)
            {
                Log.Error("can not find instance");
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

            if (__instance is ITickThreaded thing)
            {
                ThreadManager.RegisterTick(thing);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("PostRemoved")]
        public static void DeregisterThing(Hediff __instance)
        {
            if (__instance == null)
            {
                Log.Error("can not get instance");
                return;
            }

            if (__instance is ITickThreaded thing)
            {
                ThreadManager.DeregisterTick(thing);
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

            if (__instance is ITickThreaded thing)
            {
                ThreadManager.DeregisterTick(thing);
            }
        }
    }
}
