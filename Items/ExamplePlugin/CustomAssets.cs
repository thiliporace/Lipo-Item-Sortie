using BepInEx;
using EntityStates.Treebot.Weapon;
using R2API;
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
    public class CustomAssets : BaseUnityPlugin
    {
        //The mod's AssetBundle
        public static AssetBundle mainBundle;
        //A constant of the AssetBundle's name.
        public const string bundleName = "assetbundle";
        // Not necesary, but useful if you want to store the bundle on its own folder.
        // public const string assetBundleFolder = "AssetBundles";

        //The direct path to your AssetBundle
        public static string AssetBundlePath
       {
            get
           {
                //This returns the path to your assetbundle assuming said bundle is on the same folder as your DLL. If you have your bundle in a folder, you can uncomment the statement below this one.
               return Path.Combine(Path.GetDirectoryName(MainClass.PInfo.Location), bundleName);
                //return Path.Combine(Path.GetDirectoryName(MainClass.PInfo.Location), assetBundleFolder, myBundle);
            }
        }

        public void Awake()
        {
            Log.Init(Logger);

            Debug.LogError("Entrou");

            if (MainClass.PInfo.Location == null) {
                Debug.LogError("No file found");
            }

            //Loads the assetBundle from the Path, and stores it in the static field.
            mainBundle = AssetBundle.LoadFromFile(AssetBundlePath);
       }

       // public void Awake()
       // {
           // Log.Init(Logger);

            //Nao entra aqui de jeito nenhum
          //  Debug.LogError("Entrou");

           // using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RoR2Mods.assetbundle"))
          //  {
               // if (stream == null)
               // {
                //    Debug.LogError("Failed to load stream");
               // }
               // mainBundle = AssetBundle.LoadFromStream(stream);
           // }
        //}
    }
}