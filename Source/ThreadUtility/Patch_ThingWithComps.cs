using Verse;
using HarmonyLib;
using System.Collections.Generic;

namespace ThreadUtility
{
    [HarmonyPatch(typeof(ThingWithComps))]
    internal class Patch_ThingWithComps
    {
        [HarmonyPostfix]
        [HarmonyPatch("ExposeData")]
        public static void RegisterThingOnLoaded(ThingWithComps __instance, ref List<ThingComp> ___comps)
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

            if (___comps == null)
            {
                return;
            }

            foreach (var comp in ___comps)
            {
                
                if (comp is ITickThreaded compThreadable)
                {
                    ThreadManager.RegisterTick(compThreadable, __instance.def.tickerType);
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("PostMake")]
        public static void RegisterThingOnCreated(ThingWithComps __instance, ref List<ThingComp> ___comps)
        {
            if (__instance == null)
            {
                Log.Error("can not find instance");
                return;
            }

            if (___comps == null)
            {
                return;
            }

            foreach (var comp in ___comps)
            {

                if (comp is ITickThreaded compThreadable)
                {
                    ThreadManager.RegisterTick(compThreadable, __instance.def.tickerType);
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("Destroy")]
        public static void DeregisterThing(ThingWithComps __instance,ref List<ThingComp> ___comps)
        {
            if (__instance == null)
            {
                Log.Error("can not get instance");
                return;
            }

            if (___comps == null)
            {
                return;
            }

            foreach (var comp in ___comps)
            {
                if (comp is ITickThreaded compThreadable)
                {
                    ThreadManager.DeregisterTick(compThreadable, __instance.def.tickerType);
                }
            }
        }
    }
}
