using QModManager.API.ModLoading;
using HarmonyLib;
using SMLHelper.V2.Assets;
using System.Collections.Generic;
using UWE;
using UnityEngine;
using System.Collections;
using System.IO;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using MoreCyclopsUpgrades.API.Upgrades;
using System.Reflection;
using MoreCyclopsUpgrades.API;
using RadicalLibrary;
using SMLHelper.V2.Utility;
using UnityEngine.Experimental.Rendering;
using Logger = QModManager.Utility.Logger;

public class CyclopsCloakingModule: Equipable
{
    public CyclopsCloakingModule() : base(
        classId: "CyclopsCloakingModule",
        friendlyName: "Cyclops Cloaking Module",
        description: "Cloaks the Cyclops to match the surrounding terrain to avoid aggressive creatures.")
    {
    }
    public override EquipmentType EquipmentType => EquipmentType.CyclopsModule;
    public override TechType RequiredForUnlock => TechType.CyclopsHullModule1;
    public override TechGroup GroupForPDA => TechGroup.Cyclops;
    public override TechCategory CategoryForPDA => TechCategory.CyclopsUpgrades;
    public override CraftTree.Type FabricatorType => CraftTree.Type.CyclopsFabricator;
    public override string[] StepsToFabricatorTab => MCUServices.CrossMod.StepsToCyclopsModulesTabInCyclopsFabricator;
    public override QuickSlotType QuickSlotType => QuickSlotType.Passive;
    public override GameObject GetGameObject()
    {
        var path = "WorldEntities/Tools/SeamothElectricalDefense";
        var prefab = Resources.Load<GameObject>(path);
        var obj = Object.Instantiate(prefab);

        var techTag = obj.GetComponent<TechTag>();
        var prefabIdentifier = obj.GetComponent<PrefabIdentifier>();

        techTag.type = TechType;
        prefabIdentifier.ClassId = ClassID;

        return obj;
    }
    protected override TechData GetBlueprintRecipe()
    {
        return new TechData()
        {
            Ingredients = new List<Ingredient>()
            {
            new Ingredient(TechType.CyclopsShieldModule, 1),
            new Ingredient(TechType.Sulphur, 1),
            new Ingredient(TechType.PrecursorIonCrystal, 3)
            },
            craftAmount = 1
        };
    }
    protected  override Atlas.Sprite GetItemSprite()
    {
        return ImageUtils.LoadSpriteFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "icon.png"));
    }
}
namespace CyclopsCloakingMod_SN
{

    [QModCore]
    public class QMOD
    {
        public static class Variables
        {
            public static Material StealthEffect;
        }   
        [QModPatch]
        public static void MyInitializationMethod()
        {
            var assembly = Assembly.GetExecutingAssembly();
            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Info, "Patching redd_CyclopsCloakingModule_SN");
            Harmony harmony = new Harmony("redd_CyclopsCloakingModule_SN");
            harmony.PatchAll(assembly);
            var item = new CyclopsCloakingModule();
            item.Patch(); 
            

            var bundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(assembly.Location),
                    "cyclopscloakingmod.shaders"));
            var StealthEffect = bundle.LoadAsset<Material>("Cloak_Material_mtl");
            if (StealthEffect == null)
            {
                QModManager.Utility.Logger.Log(Logger.Level.Error, "StealthEffect null");
            }
            var StealthEffectShader = bundle.LoadAsset<Shader>("Invisibility");
            if (StealthEffectShader == null)
            {
                QModManager.Utility.Logger.Log(Logger.Level.Error, "StealthEffectShader null");
            }
            StealthEffect.shader = StealthEffectShader;
            bundle.Unload(false);
            Variables.StealthEffect = StealthEffect;


            MoreCyclopsUpgrades.API.MCUServices.Register.CyclopsUpgradeHandler((SubRoot cyclops) =>
            {
                return new CloakUpgradeHandler(item.TechType, cyclops);
            });

            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Info, "Patched Successfully!");
        }
    }
}