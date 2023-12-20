using BepInEx;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ExamplePlugin
{
    [BepInDependency(ItemAPI.PluginGUID)]

    [BepInDependency(LanguageAPI.PluginGUID)]

    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]


    public class BleedingLotto : BaseUnityPlugin
    {

        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Lipo";
        public const string PluginName = "BleedingLotto";
        public const string PluginVersion = "1.0.0";


        private static ItemDef myItemDef;

 
        public void Awake()
        {

            Log.Init(Logger);


            myItemDef = ScriptableObject.CreateInstance<ItemDef>();


            myItemDef.name = "TONIC_JAR_NAME";
            myItemDef.nameToken = "Bleeding Lotto";
            myItemDef.pickupToken = "Chance to apply hellfire on kill.";
            myItemDef.descriptionToken = "'In my eyes an experiment gone wrong, in yours?'. 'It looks cute!'.";
            myItemDef.loreToken = "TONIC_JAR_LORE";
            
            myItemDef.tags = new ItemTag[] { ItemTag.Damage };


#pragma warning disable Publicizer001 
            myItemDef.deprecatedTier = ItemTier.Lunar;
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
            var victimCharacterBody = report.victimBody;

            
            if (attackerCharacterBody.inventory)
            {

                var garbCount = attackerCharacterBody.inventory.GetItemCount(myItemDef.itemIndex);

                var duration = 2 + garbCount;

                if (garbCount > 0 && garbCount < 4 &&
                    
                    Util.CheckRoll(40 + (10 * garbCount), attackerCharacterBody.master))
                {
                    victimCharacterBody.AddHelfireDuration(duration * 2);
                }
                else if (garbCount > 4 &&
                    Util.CheckRoll(65 + garbCount, attackerCharacterBody.master))
                {
                    victimCharacterBody.AddHelfireDuration(duration * 2);
                }
            }
        }


    }
}
