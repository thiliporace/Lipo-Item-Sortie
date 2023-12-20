using BepInEx;
using EntityStates.Treebot.Weapon;
using R2API;
using RoR2;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static R2API.RecalculateStatsAPI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using System.Reflection;
using System.IO;

namespace ExamplePlugin
{

    [BepInDependency(ItemAPI.PluginGUID)]

    [BepInDependency(LanguageAPI.PluginGUID)]

    [BepInDependency(RecalculateStatsAPI.PluginGUID)]

    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]


    public class BulletExtravaganza : BaseUnityPlugin
    {

        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Lipo";
        public const string PluginName = "BulletExtravaganza";
        public const string PluginVersion = "1.0.0";

        private static ItemDef myItemDef;


        public void Awake()
        {

            Log.Init(Logger);

            //Carregar do 0 o assetbundle de novo :pensive:
            var myLoadedAssetBundle
            = AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.streamingAssetsPath, "assetbundle"));
            if (myLoadedAssetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                return;
            }

            var icon = myLoadedAssetBundle.LoadAsset<Sprite>("BulletExtravaganza.png");
            Instantiate(icon);


            myItemDef = ScriptableObject.CreateInstance<ItemDef>();


            myItemDef.name = "BULLET_NAME";
            myItemDef.nameToken = "Bullet Extravaganza";
            myItemDef.pickupToken = "Receive a massive dose of attack speed when attacking enemies. Overall damage reduced.";
            myItemDef.descriptionToken = "'You count how many bullets are left in your mag?'.";
            myItemDef.loreToken = "BULLET_LORE";
            
            myItemDef.tags = new ItemTag[] { ItemTag.Damage };


#pragma warning disable Publicizer001 
            myItemDef.deprecatedTier = ItemTier.Lunar;
#pragma warning restore Publicizer001


            myItemDef.pickupIconSprite = icon;
            myItemDef.pickupModelPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();

            myItemDef.canRemove = true;

 
            myItemDef.hidden = false;


            var displayRules = new ItemDisplayRuleDict(null);


            ItemAPI.Add(new CustomItem(myItemDef, displayRules));


           // GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            RecalculateStatsAPI.GetStatCoefficients += ReduceDamage;
            RecalculateStatsAPI.GetStatCoefficients += AddAttackSpeed;
        }


        private void AddAttackSpeed(CharacterBody sender, StatHookEventArgs args)
        {
            var inventory = sender.inventory;

            if (inventory)
            {
                var count = inventory.GetItemCount(myItemDef.itemIndex);

                if (count < 5)
                {
                    args.baseAttackSpeedAdd += count * 2;
                }
                else if (count > 5)
                {
                    args.baseAttackSpeedAdd += count * 3;
                }
            }
        }

        private void ReduceDamage(CharacterBody sender, StatHookEventArgs args)
        {
            var inventory = sender.inventory;

            if (inventory)
            {
                var count = inventory.GetItemCount(myItemDef.itemIndex);

                if (count < 5)
                {
                    args.baseDamageAdd -= count * 8;
                }
                else if (count > 5)
                {
                    args.baseDamageAdd -= count * 10;
                }
            }
        }


    }
}
