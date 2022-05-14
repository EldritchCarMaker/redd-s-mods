using System;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CyclopsCloakingMod_SN
{
    public class Cloaking : MonoBehaviour
    {
        public bool isCloaked = false;
        private Dictionary<GameObject, Material[]> oldcyclopsstuff = new Dictionary<GameObject, Material[]>();
        public SubRoot cyclops;
        public CloakUpgradeHandler handler;

        private void Awake()
        {
            cyclops = GetComponent<SubRoot>();
            foreach (Renderer objectRenderer in cyclops.GetComponentsInChildren<Renderer>())
            {
                oldcyclopsstuff.Add(objectRenderer.gameObject, objectRenderer.materials);
            }

        }
        public void ActivateCloak()
        {
            foreach (Renderer objectRenderer in cyclops.GetComponentsInChildren<Renderer>())
            {
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
                    {
                        mats[i] = QMOD.Variables.StealthEffect;
                    }

                    objectRenderer.materials = mats;
                }
            }

            isCloaked = true;
        }
        public void DeactivateCloak()
        {
            foreach (Renderer objectRenderer in cyclops.GetComponentsInChildren<Renderer>())
            {

                foreach (KeyValuePair<GameObject, Material[]> kvp in oldcyclopsstuff)
                {
                    if (kvp.Key.GetInstanceID() == objectRenderer.gameObject.GetInstanceID())
                    {
                        objectRenderer.materials = kvp.Value;
                    }
                }
            }

            isCloaked = false;

        }
    }
}