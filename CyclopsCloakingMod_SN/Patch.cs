using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
<<<<<<< Updated upstream
=======
using UWE;
using Logger = QModManager.Utility.Logger;
>>>>>>> Stashed changes

namespace CyclopsCloakingMod_SN
{
    class Patch
    {
        [HarmonyPatch(typeof(CyclopsNoiseManager))]
        [HarmonyPatch(nameof(CyclopsNoiseManager.RecalculateNoiseValues))]
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
        /*
        [HarmonyPatch(typeof(SubRoot))]
        [HarmonyPatch(nameof(SubRoot.OnPlayerEntered))]
        internal class PatchSubRootOnPlayerEnter
        {
            [HarmonyPrefix]
            public static bool Prefix(SubRoot __instance)
            {
                if (QMOD.Variables.isequipped && __instance.isCyclops)
                {
<<<<<<< Updated upstream
                    __instance.gameObject.GetComponent<Cloaking>().DeactivateCloak();
                    return true;
                }

                return true;
=======
                    cloak.DeactivateCloak();
                }
>>>>>>> Stashed changes
            }
        }

        [HarmonyPatch(typeof(SubRoot))]
        [HarmonyPatch(nameof(SubRoot.OnPlayerExited))]
        internal class PatchSubRootOnPlayerLeave
        {
            [HarmonyPrefix]
            public static bool Prefix(SubRoot __instance)
            {
                if (QMOD.Variables.isequipped && __instance.isCyclops)
                {
<<<<<<< Updated upstream
                    __instance.gameObject.GetComponent<Cloaking>().ActivateCloak();
                    return true;
                }

                return true;
=======
                    CoroutineHost.StartCoroutine(cloak.ActivateTimedCloak());
                }
>>>>>>> Stashed changes
            }
        }

        [HarmonyPatch(typeof(CyclopsExternalCams))]
        [HarmonyPatch(nameof(CyclopsExternalCams.EnterCameraView))]
        internal class CyclopsCameraInputActiveCameraPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(CyclopsExternalCams __instance)
            {
                if (QMOD.Variables.isequipped)
                {
                   Player.main.currentSub.gameObject.GetComponent<Cloaking>().ActivateCloak();
                }
<<<<<<< Updated upstream

                return true;
=======
>>>>>>> Stashed changes
            }

        }

        [HarmonyPatch(typeof(CyclopsExternalCams))]
        [HarmonyPatch(nameof(CyclopsExternalCams.ExitCamera))]
        internal class CyclopsCameraInputDeactivateCameraPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(CyclopsExternalCams __instance)
            {
                if (QMOD.Variables.isequipped)
                {
                   Player.main.currentSub.gameObject.GetComponent<Cloaking>().DeactivateCloak();
                }
<<<<<<< Updated upstream
                return true;
=======
>>>>>>> Stashed changes
            }
        }
        */
    }
}