using BepInEx;
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


    public class DivineAmulet : BaseUnityPlugin
    {

        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Lipo";
        public const string PluginName = "DivineAmulet";
        public const string PluginVersion = "1.0.0";


        private static ItemDef myItemDef;

        public void Awake()
        {
 
            Log.Init(Logger);


            myItemDef = ScriptableObject.CreateInstance<ItemDef>();

            myItemDef.name = "DIVINE_AMULET_NAME";
            myItemDef.nameToken = "Divine Amulet";
            myItemDef.pickupToken = "On every non crit attack, cripple enemies.";
            myItemDef.descriptionToken = "Branded by false gods who live among our world. You should make one yourself, in case you don't already have one.";
            myItemDef.loreToken = "DIVINE_AMULET_LORE";

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

                var duration = count;
                
                if (count > 0 && !report.damageInfo.crit)
                {
                    victimCharacterBody.AddBuff(RoR2Content.Buffs.Cripple);
                }
            }
        }



        }


}
