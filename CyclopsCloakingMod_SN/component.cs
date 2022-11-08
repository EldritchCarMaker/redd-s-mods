using System;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using Logger = QModManager.Utility.Logger;
using static CraftNode;

namespace CyclopsCloakingMod_SN
{
    public class Cloaking : MonoBehaviour
    {
        public static Transform CameraParent { get; private set; }

        public bool isCloaked = false;
        private Dictionary<GameObject, Material[]> oldcyclopsstuff = new Dictionary<GameObject, Material[]>();
<<<<<<< Updated upstream
        private SubRoot cyclops;
=======
        private Dictionary<GameObject, Material[]> playerMaterials = new Dictionary<GameObject, Material[]>();

        public static List<string> autoFilterObjectNames = new List<string>()//filters the specific object with this string in its name
        {
            "WorldMeshes",
            "light",
            "DamagePoint"
        };

        public static List<string> autoDeactivateObjectNames = new List<string>()//same as above except deactivates object instead of filtering
        {
            "Hud",//capitilization doesn't matter, it's just easier to read
            "LightsControl",
            "SonarMap",
            "HealthBar",
            "CyclopsVehicleStorageTerminal",
            "SubName",
            "ProximityWarning"
        };

        public SubRoot cyclops;
        public CloakUpgradeHandler handler;

        public bool ShouldCloak => (!PlayerInSub || !PlayerCamNormal) && handler != null && handler.HasUpgrade;
        public bool PlayerCamNormal => SNCameraRoot.main.transform.localPosition == Vector3.zero && SNCameraRoot.main.transform.parent == CameraParent;
        public bool PlayerInSub => Player.main.currentSub == cyclops;

>>>>>>> Stashed changes
        private void Awake()
        {
            cyclops = GetComponent<SubRoot>();
            foreach (Renderer objectRenderer in GetComponentsInChildren<Renderer>())
            { 
                oldcyclopsstuff.Add(objectRenderer.gameObject,objectRenderer.materials);
            }
<<<<<<< Updated upstream
            
=======
            if(!CameraParent)
                CameraParent = SNCameraRoot.main.transform.parent;
        }

        public void Update()
        {
            var shouldCloak = ShouldCloak;
            if(shouldCloak != isCloaked)
            {
                if (shouldCloak)
                    ActivateCloak();
                else
                    DeactivateCloak();
            }
        }

        public IEnumerator ActivateTimedCloak()
        {
            yield return new WaitForFixedUpdate();
            ActivateCloak();
>>>>>>> Stashed changes
        }

        public void ActivateCloak()
        {
<<<<<<< Updated upstream
            QMOD.Variables.oldcyclopsstuff.Clear();
            Parallel.ForEach(cyclops.GetComponentsInChildren<Renderer>(), objrender =>
            {
                if (!oldcyclopsstuff.ContainsKey(objrender.gameObject))
                {
                    oldcyclopsstuff.Add(objrender.gameObject, objrender.materials);
=======
            if (isCloaked) return;


            SetObjectsRecursive(transform, true);

            if(PlayerInSub)
                CloakPlayer();


            isCloaked = true;
        }
        public void SetObjectsRecursive(Transform startTransform, bool cloaked, int depth = 0)
        {
            if(depth >= 15)
            {
                throw new Exception($"Excessive depth found, final transform {startTransform}");
                //would prefer to throw an error than to lock up
                //and would prefer to throw an error than have a simple easily missed message
            }
            else if(depth >= 10)
            {
                Logger.Log(Logger.Level.Warn, $"Large depth on SetObjectsRecursive found on transform {startTransform}! depth of {depth}. Please avoid having large depth in recursive calls");
            }

            foreach (Transform transform in startTransform)
            {
                if (IsObjectFiltered(transform.gameObject))
                    continue;

                if (cloaked)
                    SetObjectCloaked(transform.gameObject);
                else
                    SetObjectUncloaked(transform.gameObject);

                if (transform.childCount > 0)
                {
                    SetObjectsRecursive(transform, cloaked, depth + 1);
>>>>>>> Stashed changes
                }
            }
        }
        public void SetObjectCloaked(GameObject obj)
        {
            foreach (var name in autoDeactivateObjectNames)
            {
                if (obj.name.ToLower().Contains(name.ToLower()))
                {
<<<<<<< Updated upstream
                    oldcyclopsstuff[objrender.gameObject] = objrender.materials;
                }
            });
            foreach(Renderer objectRenderer in cyclops.GetComponentsInChildren<Renderer>())
            {
                if (objectRenderer.gameObject.layer != 1 << LayerMask.NameToLayer("BaseClipProxy"))
=======
                    obj.gameObject.SetActive(false);
                    break;
                }
            }
            if (!obj.gameObject.activeInHierarchy)
                return;


            var objectRenderer = obj.gameObject.GetComponent<Renderer>();

            if (!objectRenderer || objectRenderer is ParticleSystemRenderer) return;


            if (!oldcyclopsstuff.ContainsKey(objectRenderer.gameObject))
            {
                oldcyclopsstuff.Add(objectRenderer.gameObject, objectRenderer.materials);
            }
            else
            {
                oldcyclopsstuff[objectRenderer.gameObject] = objectRenderer.materials;
            }

            if (objectRenderer.gameObject.layer != 1 << LayerMask.NameToLayer("BaseClipProxy"))
            {
                var mats = objectRenderer.materials;
                for (int i = 0; i < mats.Length; i++)
>>>>>>> Stashed changes
                {
                    mats[i] = QMOD.Variables.StealthEffect;
                }

                objectRenderer.materials = mats;
            }
        }
        public void SetObjectUncloaked(GameObject obj)
        {
            foreach (var name in autoDeactivateObjectNames)
            {
                if (obj.name.ToLower().Contains(name.ToLower()))
                    obj.gameObject.SetActive(true);
            }

            var objectRenderer = obj.GetComponent<Renderer>();

            if (!objectRenderer) return;

            if (oldcyclopsstuff.TryGetValue(obj.gameObject, out var materials))
            {
                objectRenderer.materials = materials;
            }
        }
        public bool IsObjectFiltered(GameObject obj)
        {
            foreach (var filter in autoFilterObjectNames)
            {
                foreach(var disableFilter in autoDeactivateObjectNames)
                {
                    if(obj.name.ToLower().Contains(disableFilter.ToLower()))
                    {
                        //disable filter should be used even for otherwise filtered objects
                        return false;
                    }
                }

                if (obj.name.ToLower().Contains(filter.ToLower()))
                {
                    return true;
                }
            }
            if (obj.transform.IsChildOf(Player.main.transform)) return true;

            return false;
        }

        public void DeactivateCloak()
        {
            if (!isCloaked) return;


            SetObjectsRecursive(transform, false);

            UnCloakPlayer();


            isCloaked = false;
        }
        public void CloakPlayer()
        {
            var bodyTransform = Player.main.transform.Find("body");

            if (bodyTransform == null)
            {
                Logger.Log(Logger.Level.Warn, "Could not find player body");
                return;
            }

            var body = bodyTransform.gameObject;
            foreach (Renderer renderer in body.GetComponentsInChildren<Renderer>())
            {
                if (playerMaterials.ContainsKey(renderer.gameObject))
                    playerMaterials[renderer.gameObject] = renderer.materials;
                else
                    playerMaterials.Add(renderer.gameObject, renderer.materials);

<<<<<<< Updated upstream
                foreach (KeyValuePair<GameObject,Material[]> kvp in oldcyclopsstuff)
=======
                var mats = renderer.materials;
                for (int i = 0; i < mats.Length; i++)
>>>>>>> Stashed changes
                {
                    mats[i] = QMOD.Variables.StealthEffect;
                }

                renderer.materials = mats;
            }
        }
        public void UnCloakPlayer()
        {
            var bodyTransform = Player.main.transform.Find("body");

<<<<<<< Updated upstream
            isCloaked = false;
            
        }

        public void DestroySelf()
        {
            Destroy(GetComponent<Cloaking>());
=======
            if (bodyTransform == null)
            {
                Logger.Log(Logger.Level.Warn, "Could not find player body");
                return;
            }

            var body = bodyTransform.gameObject;
            foreach (Renderer renderer in body.GetComponentsInChildren<Renderer>())
            {
                if(playerMaterials.TryGetValue(renderer.gameObject, out var materials))
                {
                    renderer.materials = materials;
                }
                else
                {
                    Logger.Log(Logger.Level.Warn, $"Found renderer on player without a corresponding entry in dictionary");
                }
            }
>>>>>>> Stashed changes
        }
    }
}