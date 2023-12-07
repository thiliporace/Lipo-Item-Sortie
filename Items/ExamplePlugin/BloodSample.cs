using BepInEx;
using BepInEx.Configuration;
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

    [BepInDependency(VoidItemAPI.VoidItemAPI.MODGUID)]


    public class BloodSample : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Lipo";
        public const string PluginName = "BloodSample";
        public const string PluginVersion = "1.0.0";

 
        private static ItemDef myItemDef;


        public void Awake()
        {
   
            Log.Init(Logger);

            myItemDef = ScriptableObject.CreateInstance<ItemDef>();


            myItemDef.name = "BLOOD_SAMPLE_NAME";
            myItemDef.nameToken = "Blood Sample";
            myItemDef.pickupToken = "Add crit chance and crit damage but lower health.";
            myItemDef.descriptionToken = "Living in the danger zone makes you feel alive.";
            myItemDef.loreToken = "BLOOD_SAMPLE_LORE";

            myItemDef.tags = new ItemTag[] { ItemTag.Damage };



#pragma warning disable Publicizer001
            //myItemDef._itemTierDef = Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/Tier2Def.asset").WaitForCompletion();
            myItemDef.deprecatedTier = ItemTier.VoidTier2;
#pragma warning restore Publicizer001


            myItemDef.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
            myItemDef.pickupModelPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();


            myItemDef.canRemove = true;


            myItemDef.hidden = false;


            VoidItemAPI.VoidTransformation.CreateTransformation(myItemDef, "Infusion");

            var displayRules = new ItemDisplayRuleDict(null);

            ItemAPI.Add(new CustomItem(myItemDef, displayRules));


            RecalculateStatsAPI.GetStatCoefficients += AddCritChance;

            RecalculateStatsAPI.GetStatCoefficients += LowerHealth;





        }



        private void AddCritChance(CharacterBody sender, StatHookEventArgs args)
        {
            var inventory = sender.inventory;

            if (inventory)
            {
                var count = inventory.GetItemCount(myItemDef.itemIndex);

                args.critAdd += 5 * count;
                args.critDamageMultAdd += count;

            }
        }



        private void LowerHealth(CharacterBody sender, StatHookEventArgs args)
        {
            var inventory = sender.inventory;

            if (inventory)
            {
                var count = inventory.GetItemCount(myItemDef.itemIndex);

                // +1 is +100%, always use += or -= with args or it will fuck up other recalculatestatsapi subscriptions
                args.baseHealthAdd -= sender.maxHealth/4 * count;
            }
        }





        //Remove later
        private void Update()
        {
            // This if statement checks if the player has currently pressed F2.
            if (Input.GetKeyDown(KeyCode.F6))
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
