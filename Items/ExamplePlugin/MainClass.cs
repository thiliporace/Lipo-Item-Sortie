using BepInEx;
using EntityStates.Treebot.Weapon;
using R2API;
using RoR2;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static R2API.RecalculateStatsAPI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;


namespace ExamplePlugin
{
    [BepInDependency(ItemAPI.PluginGUID)]

    [BepInDependency(LanguageAPI.PluginGUID)]

    [BepInDependency(RecalculateStatsAPI.PluginGUID)]

    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class MainClass : BaseUnityPlugin
{
    public static PluginInfo PInfo { get; private set; }
    public void Awake()
    {
            Log.Init(Logger);

            PInfo = Info;
    }
}
}
