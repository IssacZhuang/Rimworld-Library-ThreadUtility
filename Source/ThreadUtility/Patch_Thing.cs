using Verse;
using HarmonyLib;

namespace ThreadUtility
{
    [HarmonyPatch(typeof(Thing))]
    internal class Patch_Thing
    {
        [HarmonyPostfix]
        [HarmonyPatch("ExposeData")]
        public static void RegisterThingOnLoaded(Thing __instance)
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

            if (__instance is ITickThreaded thing)
            {
                ThreadManager.RegisterTick(thing);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("PostMake")]
        public static void RegisterThingOnCreated(Thing __instance)
        {
            if (__instance == null)
            {
                Log.Error("can not find instance");
                return;
            }

            if (__instance is ITickThreaded thing)
            {
                ThreadManager.RegisterTick(thing);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("Destroy")]
        public static void DeregisterThing(Thing __instance)
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
    }
}
