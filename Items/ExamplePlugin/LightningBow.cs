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



    public class LightningBow : BaseUnityPlugin
    {

        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Lipo";
        public const string PluginName = "LightningBow";
        public const string PluginVersion = "1.0.0";


        private static ItemDef myItemDef;


        public void Awake()
        {
   
            Log.Init(Logger);

            myItemDef = ScriptableObject.CreateInstance<ItemDef>();


            myItemDef.name = "LIGHTNING_BOW_NAME";
            myItemDef.nameToken = "Lightning Bow";
            myItemDef.pickupToken = "Grants a small boost to crit chance and movespeed.";
            myItemDef.descriptionToken = "Don't hit your friends' eye!";
            myItemDef.loreToken = "LIGHTNING_BOW_LORE";

            myItemDef.tags = new ItemTag[] { ItemTag.Damage };

#pragma warning disable Publicizer001 
            myItemDef._itemTierDef = Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/Tier1Def.asset").WaitForCompletion();
#pragma warning restore Publicizer001


            myItemDef.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
            myItemDef.pickupModelPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();


            myItemDef.canRemove = true;


            myItemDef.hidden = false;


            var displayRules = new ItemDisplayRuleDict(null);


            ItemAPI.Add(new CustomItem(myItemDef, displayRules));


            RecalculateStatsAPI.GetStatCoefficients += AddCritChance;

            RecalculateStatsAPI.GetStatCoefficients += ChangeMoveSpeed;
        }


        private void AddCritChance(CharacterBody sender, StatHookEventArgs args)
        {
            var inventory = sender.inventory;

            if (inventory)
            {
                var count = inventory.GetItemCount(myItemDef.itemIndex);

                if (count < 14)
                {
                    args.critAdd += 7 * count;
                }
                else
                {
                    args.critAdd += 6 * count;
                }
                
            }
        }



        private void ChangeMoveSpeed(CharacterBody sender, StatHookEventArgs args)
        {
            var inventory = sender.inventory;

            if (inventory)
            {
                var count = inventory.GetItemCount(myItemDef.itemIndex);

                // +1 is +100%, always use += or -= with args or it will fuck up other recalculatestatsapi subscriptions
                args.baseMoveSpeedAdd += count;
                
            }
        }




        //Remove later
        private void Update()
        {
            // This if statement checks if the player has currently pressed F2.
            if (Input.GetKeyDown(KeyCode.F5))
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
