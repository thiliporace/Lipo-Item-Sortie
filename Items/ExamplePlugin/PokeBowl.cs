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


    public class Item : BaseUnityPlugin
    {

        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Lipo";
        public const string PluginName = "PokeBowl";
        public const string PluginVersion = "1.0.0";

        
        private static ItemDef myItemDef;

        public void Awake()
        {
            
            Log.Init(Logger);

            
            myItemDef = ScriptableObject.CreateInstance<ItemDef>();

            
            myItemDef.name = "POKE_BOWL_NAME";
            myItemDef.nameToken = "Poke Bowl";
            myItemDef.pickupToken = "30 health. When killing an enemy there is a small chance of healing.";
            myItemDef.descriptionToken = "A bowl of ingredients and spices. How did it end up here?";
            myItemDef.loreToken = "POKE_BOWL_LORE";

            myItemDef.tags = new ItemTag[] { ItemTag.Healing };


#pragma warning disable Publicizer001 
            myItemDef._itemTierDef = Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/Tier1Def.asset").WaitForCompletion();
#pragma warning restore Publicizer001


            // You can create your own icons and prefabs through assetbundles, but to keep this boilerplate brief, we'll be using question marks.
            myItemDef.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
            myItemDef.pickupModelPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();

            myItemDef.canRemove = true;

            myItemDef.hidden = false;

            var displayRules = new ItemDisplayRuleDict(null);


            ItemAPI.Add(new CustomItem(myItemDef, displayRules));


            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
            RecalculateStatsAPI.GetStatCoefficients += AddLevel;
        }


        private void AddLevel(CharacterBody sender, StatHookEventArgs args)
        {
            var inventory = sender.inventory;

            if (inventory)
            {
                var count = inventory.GetItemCount(myItemDef.itemIndex);

                args.baseHealthAdd += 30 * count;
                
            }
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport report)
        {

            if (!report.attacker || !report.attackerBody)
            {
                return;
            }

            var attackerCharacterBody = report.attackerBody;

            if (attackerCharacterBody.inventory)
            {

                var count = attackerCharacterBody.inventory.GetItemCount(myItemDef.itemIndex);

                if (count > 0 && Util.CheckRoll(10 + 5 * count,attackerCharacterBody.master))
                {
                    attackerCharacterBody.AddTimedBuff(RoR2Content.Buffs.MedkitHeal,count * 2);
                }
            }
        }


    }
}
