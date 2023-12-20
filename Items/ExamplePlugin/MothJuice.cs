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


    public class MothJuice : BaseUnityPlugin
    {

        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Lipo";
        public const string PluginName = "MothJuice";
        public const string PluginVersion = "1.0.0";


        private static ItemDef myItemDef;


        public void Awake()
        {
            
            Log.Init(Logger);

            
            myItemDef = ScriptableObject.CreateInstance<ItemDef>();

            myItemDef.name = "MOTH_NAME";
            myItemDef.nameToken = "Moth Juice";
            myItemDef.pickupToken = "When levelling up reduce all cooldowns.";
            myItemDef.descriptionToken = "'You may feel the following symptoms: Nausea, Vomiting and Diarrhea.'. 'You seeing this?'.";
            myItemDef.loreToken = "MOTH_LORE";

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


            GlobalEventManager.onCharacterLevelUp += GlobalEventManager_onCharacterLevelUp;
        }

        private void GlobalEventManager_onCharacterLevelUp(CharacterBody body)
        {
            
            if (body.inventory)
            {
                var count = body.inventory.GetItemCount(myItemDef.itemIndex);

                if (count > 0)
                {
                    body.AddTimedBuff(RoR2Content.Buffs.NoCooldowns, 2 + count);
                } 
            }
        }


    }
}
