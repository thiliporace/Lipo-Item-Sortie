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



    public class WeddingRing : BaseUnityPlugin
    {

        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Lipo";
        public const string PluginName = "WeddingRing";
        public const string PluginVersion = "1.0.0";


        private static ItemDef myItemDef;


        public void Awake()
        {

            Log.Init(Logger);

            myItemDef = ScriptableObject.CreateInstance<ItemDef>();

            myItemDef.name = "WEDDING_RING_NAME";
            myItemDef.nameToken = "Wedding Ring";
            myItemDef.pickupToken = "On kill, there is a chance to receive a small armor boost.";
            myItemDef.descriptionToken = "Forever together.";
            myItemDef.loreToken = "WEDDING_RING_LORE";

            myItemDef.tags = new ItemTag[] { ItemTag.Utility };

#pragma warning disable Publicizer001 
            myItemDef._itemTierDef = Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/Tier2Def.asset").WaitForCompletion();
#pragma warning restore Publicizer001

            myItemDef.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
            myItemDef.pickupModelPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();


            myItemDef.canRemove = true;

            myItemDef.hidden = false;

            var displayRules = new ItemDisplayRuleDict(null);

            ItemAPI.Add(new CustomItem(myItemDef, displayRules));


            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
        }



        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport report)
        {
            if (!report.attacker || !report.attackerBody)
            {
                return;
            }

            var attackerCharacterBody = report.attackerBody;

            var count = attackerCharacterBody.inventory.GetItemCount(myItemDef.itemIndex);

            var duration = (count * 2) + 2;

            if (count > 0 && 
                Util.CheckRoll(35 + 5 * count, attackerCharacterBody.master))
            {
                attackerCharacterBody.AddTimedBuff(RoR2Content.Buffs.SmallArmorBoost, duration);
            }
        }


    }


}
