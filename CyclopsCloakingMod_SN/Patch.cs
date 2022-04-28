using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace CyclopsCloakingMod_SN
{
    class Patch
    {
        [HarmonyPatch(typeof(CyclopsNoiseManager))]
        [HarmonyPatch("RecalculateNoiseValues")]
        internal class PatchCyclopsNoiseManagerRecalculateNoise
        {
            [HarmonyPrefix]
            public static bool Prefix(CyclopsNoiseManager __instance, ref float __result)
            {
                if (QMOD.Variables.isequipped)
                {
                    Traverse.Create(__instance).Field("noiseScalar").SetValue(0f);
                    __result = 0f;
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(SubRoot))]
        [HarmonyPatch("OnPlayerEntered")]
        internal class PatchSubRootOnPlayerEnter
        {
            [HarmonyPrefix]
            public static bool Prefix(SubRoot __instance)
            {
                if (QMOD.Variables.isequipped && __instance.isCyclops)
                {
                    __instance.gameObject.GetComponent<Cloaking>().DeactivateCloak();
                    return true;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(SubRoot))]
        [HarmonyPatch("OnPlayerExited")]
        internal class PatchSubRootOnPlayerLeave
        {
            [HarmonyPrefix]
            public static bool Prefix(SubRoot __instance)
            {
                if (QMOD.Variables.isequipped && __instance.isCyclops)
                {
                    __instance.gameObject.GetComponent<Cloaking>().ActivateCloak();
                    return true;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(CyclopsExternalCams))]
        [HarmonyPatch("EnterCameraView")]
        internal class CyclopsCameraInputActiveCameraPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(CyclopsExternalCams __instance)
            {
                if (QMOD.Variables.isequipped)
                {
                   Player.main.currentSub.gameObject.GetComponent<Cloaking>().ActivateCloak();
                }

                return true;
            }

        }

        [HarmonyPatch(typeof(CyclopsExternalCams))]
        [HarmonyPatch("ExitCamera")]
        internal class CyclopsCameraInputDeactivateCameraPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(CyclopsExternalCams __instance)
            {
                if (QMOD.Variables.isequipped)
                {
                   Player.main.currentSub.gameObject.GetComponent<Cloaking>().DeactivateCloak();
                }
                return true;
            }
        }
    }
}