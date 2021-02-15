using HarmonyLib;
using Verse;

namespace ThreadUtility
{
    [HarmonyPatch(typeof(Game))]
    internal class Patch_Game
    {
        [HarmonyPrefix]
        [HarmonyPatch("LoadGame")]
        public static void PreGameloaded()
        {
            ThreadManager.InitThread();
        }

        [HarmonyPostfix]
        [HarmonyPatch("FinalizeInit")]
        public static void PostGameInit()
        {
            ThreadManager.StartAll();
        }
    }
}
