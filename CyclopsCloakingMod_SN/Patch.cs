using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using Logger = QModManager.Utility.Logger;

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
                var cloak = __instance.subRoot.GetComponent<Cloaking>();
                if (cloak != null && cloak.handler.HasUpgrade)
                {
                    __instance.noiseScalar = 0f;
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
            [HarmonyPostfix]
            public static void Prefix(SubRoot __instance)
            {
                var cloak = __instance.GetComponent<Cloaking>();
                if (cloak != null && cloak.handler.HasUpgrade)
                {
                    cloak.DeactivateCloak();
                    return;
                }
                else { Logger.Log(Logger.Level.Info, "cloak: " + (cloak != null), null, true); }

                return;
            }
        }

        [HarmonyPatch(typeof(SubRoot))]
        [HarmonyPatch("OnPlayerExited")]
        internal class PatchSubRootOnPlayerLeave
        {
            [HarmonyPostfix]
            public static void Prefix(SubRoot __instance)
            {
                var cloak = __instance.GetComponent<Cloaking>();
                if (cloak != null && cloak.handler.HasUpgrade)
                {
                    cloak.ActivateCloak();
                    return;
                }
                else { Logger.Log(Logger.Level.Info, "handler: " + (cloak != null), null, true); }

                return;
            }
        }

        [HarmonyPatch(typeof(CyclopsExternalCams))]
        [HarmonyPatch("EnterCameraView")]
        internal class CyclopsCameraInputActiveCameraPatch
        {
            [HarmonyPostfix]
            public static void Prefix(CyclopsExternalCams __instance)
            {
                var subRoot = __instance.GetComponentInParent<SubRoot>();
                if (subRoot == null) return;
                var cloak = subRoot.GetComponent<Cloaking>();
                if (cloak != null && cloak.handler.HasUpgrade)
                {
                    cloak.ActivateCloak();
                }
                else { Logger.Log(Logger.Level.Info, "handler: " + (cloak != null), null, true); }

                return;
            }

        }

        [HarmonyPatch(typeof(CyclopsExternalCams))]
        [HarmonyPatch("ExitCamera")]
        internal class CyclopsCameraInputDeactivateCameraPatch
        {
            [HarmonyPostfix]
            public static void Prefix(CyclopsExternalCams __instance)
            {
                var subRoot = __instance.GetComponentInParent<SubRoot>();
                if (subRoot == null) return;
                var cloak = subRoot.GetComponent<Cloaking>();
                if (cloak != null && cloak.handler.HasUpgrade)
                {
                    cloak.DeactivateCloak();
                }
                else { Logger.Log(Logger.Level.Info, "handler: " + (cloak != null), null, true); }
                return;
            }
        }
    }
}