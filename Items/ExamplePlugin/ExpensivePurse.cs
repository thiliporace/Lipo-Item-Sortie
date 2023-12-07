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



    public class ExpensivePurse : BaseUnityPlugin
    {

        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Lipo";
        public const string PluginName = "ExpensivePurse";
        public const string PluginVersion = "1.0.0";

        private static ItemDef myItemDef;


        public void Awake()
        {

            Log.Init(Logger);


            myItemDef = ScriptableObject.CreateInstance<ItemDef>();


            myItemDef.name = "EXPENSIVE_PURSE_NAME";
            myItemDef.nameToken = "Expensive Purse";
            myItemDef.pickupToken = "Increase jump height.";
            myItemDef.descriptionToken = "Now available in a new color! Purple Moss™! ";
            myItemDef.loreToken = "EXPENSIVE_PURSE_LORE";

            myItemDef.tags = new ItemTag[] { ItemTag.Damage };

#pragma warning disable Publicizer001 
            myItemDef._itemTierDef = Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/Tier2Def.asset").WaitForCompletion();
#pragma warning restore Publicizer001


            myItemDef.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
            myItemDef.pickupModelPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();

            myItemDef.canRemove = true;

            myItemDef.hidden = false;

            var displayRules = new ItemDisplayRuleDict(null);


            ItemAPI.Add(new CustomItem(myItemDef, displayRules));


            RecalculateStatsAPI.GetStatCoefficients += AddDamage;
        }


        private void AddDamage(CharacterBody sender, StatHookEventArgs args)
        {
            var inventory = sender.inventory;

            if (inventory)
            {
                var count = inventory.GetItemCount(myItemDef.itemIndex);

                args.jumpPowerMultAdd += count/2;
            }
        }






        //Remove later
        private void Update()
        {
            // This if statement checks if the player has currently pressed F2.
            if (Input.GetKeyDown(KeyCode.F10))
            {
                // Get the player body to use a position:
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

                // And then drop our defined item in front of the player.

                Log.Info($"Player pressed F2. Spawning our custom item at coordinates {transform.position}");
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(myItemDef.itemIndex), transform.position, transform.forward * 20f);
            }
        }
    }
}
