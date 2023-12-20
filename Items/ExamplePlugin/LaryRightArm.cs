using BepInEx;
using JetBrains.Annotations;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static R2API.RecalculateStatsAPI;

namespace ExamplePlugin
{

    [BepInDependency(ItemAPI.PluginGUID)]

    [BepInDependency(LanguageAPI.PluginGUID)]

    [BepInDependency(RecalculateStatsAPI.PluginGUID)]

    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]



    public class LaryRightArm : BaseUnityPlugin
    {

        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Lipo";
        public const string PluginName = "LaryRightArm";
        public const string PluginVersion = "1.0.0";


        private static ItemDef myItemDef;


        public void Awake()
        {

            Log.Init(Logger);

            myItemDef = ScriptableObject.CreateInstance<ItemDef>();

            myItemDef.name = "LARY_ARM_NAME";
            myItemDef.nameToken = "Lary's Right Arm";
            myItemDef.pickupToken = "On crit, apply bleeding.";
            myItemDef.descriptionToken = "Don't stick with it.";
            myItemDef.loreToken = "test";

            myItemDef.tags = new ItemTag[] { ItemTag.Damage };


#pragma warning disable Publicizer001
            myItemDef._itemTierDef = Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/Tier3Def.asset").WaitForCompletion();
#pragma warning restore Publicizer001

 


            myItemDef.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
            myItemDef.pickupModelPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();


            myItemDef.canRemove = true;

            myItemDef.hidden = false;


            var displayRules = new ItemDisplayRuleDict(null);

            ItemAPI.Add(new CustomItem(myItemDef, displayRules));


            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onCritDamage;

            RecalculateStatsAPI.GetStatCoefficients += AddCritChance;
        }


        private void GlobalEventManager_onCritDamage(DamageReport report)
        {
  
            if (!report.attacker || !report.attackerBody)
            {
                return;
            }

            var attackerCharacterBody = report.attackerBody;
            var victimCharacterBody = report.victimBody;

            if (attackerCharacterBody.inventory)
            {
                var count = attackerCharacterBody.inventory.GetItemCount(myItemDef.itemIndex);
                
                if (count > 0 && report.damageInfo.crit)
                {
                    victimCharacterBody.AddBuff(RoR2Content.Buffs.Bleeding);
                }
            }
        }

        private void AddCritChance(CharacterBody sender, StatHookEventArgs args)
        {
            var inventory = sender.inventory;

            if (inventory)
            {
                var count = inventory.GetItemCount(myItemDef.itemIndex);
                args.critAdd += 5 * count;
                
            }
        }


        }


}
