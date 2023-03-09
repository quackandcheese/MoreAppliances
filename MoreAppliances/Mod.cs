global using UnityEngine;
global using UnityEngine.VFX;
global using HarmonyLib;

global using KitchenMoreAppliances.Appliances;
global using KitchenMoreAppliances.Utils;

global using Kitchen;
global using KitchenLib.References;
global using KitchenLib;
global using KitchenLib.Event;
global using KitchenMods;
global using KitchenData;
global using KitchenLib.Customs;
global using KitchenLib.Utils;
global using KitchenLib.Colorblind;
global using Kitchen.Components;


global using System;
global using System.Collections.Generic;
global using System.Text;
global using System.Threading.Tasks;
global using System.Linq;
global using System.Reflection;

global using static KitchenData.ItemGroup;
global using static KitchenMoreAppliances.Utils.Helper;
global using static KitchenLib.Utils.GDOUtils;
global using static KitchenLib.Utils.KitchenPropertiesUtils;

// Namespace should have "Kitchen" in the beginning
namespace KitchenMoreAppliances
{
    public class Mod : BaseMod, IModSystem
    {
        // GUID must be unique and is recommended to be in reverse domain name notation
        // Mod Name is displayed to the player and listed in the mods menu
        // Mod Version must follow semver notation e.g. "1.2.3"
        public const string MOD_GUID = "QuackAndCheese.PlateUp.MoreAppliances";
        public const string MOD_NAME = "More Appliances";
        public const string MOD_VERSION = "0.1.0";
        public const string MOD_AUTHOR = "QuackAndCheese";
        public const string MOD_GAMEVERSION = ">=1.1.4";
        // Game version this mod is designed for in semver
        // e.g. ">=1.1.3" current and all future
        // e.g. ">=1.1.3 <=1.2.3" for all from/untill

        // Boolean constant whose value depends on whether you built with DEBUG or RELEASE mode, useful for testing
#if DEBUG
        public const bool DEBUG_MODE = true;
#else
        public const bool DEBUG_MODE = false;
#endif

        public static AssetBundle Bundle;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void OnInitialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        private void AddGameData()
        {
            LogInfo("Attempting to register game data...");

            AddGameDataObject<ResearchingQuill>();
            AddGameDataObject<ResearchingQuillProvider>();

            AddGameDataObject<UniversalPrepStation>();

            AddGameDataObject<ButcherBlock>();
            AddGameDataObject<HibachiTable>();

            LogInfo("Done loading game data.");
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            LogInfo("Attempting to load asset bundle...");
            Bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).First();
            LogInfo("Done loading asset bundle.");


            Events.BuildGameDataPreSetupEvent += delegate (object s, BuildGameDataEventArgs args)
            {
                VisualEffectHelper.SetupEffectIndex();
            };


            // Register custom GDOs
            AddGameData();



            // Perform actions when game data is built
            Events.BuildGameDataEvent += delegate (object s, BuildGameDataEventArgs args)
            {

                // QUILL
                AddEnum<DurationToolType>(11);

                if (TryRemoveComponentsFromAppliance<Appliance>(Refs.ResearchDesk.ID, new Type[] { typeof(CTakesDuration) }))
                {
                    Refs.ResearchDesk.Properties.Add(GetCTakesDuration(5, 0, false, true, false, (DurationToolType)11, InteractionMode.Items, false, true, false, false, 0));
                }

                if (TryRemoveComponentsFromAppliance<Appliance>(Refs.DiscountDesk.ID, new Type[] { typeof(CTakesDuration) }))
                {
                    Refs.DiscountDesk.Properties.Add(GetCTakesDuration(5, 0, false, true, false, (DurationToolType)11, InteractionMode.Items, false, true, false, false, 0));
                }

                if (TryRemoveComponentsFromAppliance<Appliance>(Refs.CopyingDesk.ID, new Type[] { typeof(CTakesDuration) }))
                {
                    Refs.CopyingDesk.Properties.Add(GetCTakesDuration(5, 0, false, true, false, (DurationToolType)11, InteractionMode.Items, false, true, false, false, 0));
                }

                // UNIVERSAL PREP
                Refs.PrepStation.Upgrades.Add(Refs.UniversalPrepStation);

                // TABLES
                List<Appliance> tables = new List<Appliance>()
                {
                    Refs.ButcherBlock,
                    Refs.HibachiTable
                };

                foreach (Appliance table in tables)
                {
                    Refs.DiningTable.Upgrades.Add(table);
                }    
            };
        }


        public void AddEnum<T>(int numInEnum) where T : Enum
        {
            Type enumType = typeof(T);
            object value = System.Convert.ChangeType(numInEnum, Enum.GetUnderlyingType(enumType));
            object enumValue = Enum.ToObject(enumType, value);
            T cursedEnum = (T)enumValue;
            Mod.LogInfo(cursedEnum);
        }

        private bool TryRemoveComponentsFromAppliance<T>(int id, Type[] componentTypesToRemove) where T : GameDataObject
        {
            T gDO = Find<T>(id);

            if (gDO == null)
            {
                return false;
            }

            bool success = false;
            if (typeof(T) == typeof(Appliance))
            {
                Appliance appliance = (Appliance)Convert.ChangeType(gDO, typeof(Appliance));
                for (int i = appliance.Properties.Count - 1; i > -1; i--)
                {
                    if (componentTypesToRemove.Contains(appliance.Properties[i].GetType()))
                    {
                        appliance.Properties.RemoveAt(i);
                        success = true;
                    }
                }
            }
            else if (typeof(T) == typeof(Item))
            {
                Item item = (Item)Convert.ChangeType(gDO, typeof(Item));
                for (int i = item.Properties.Count - 1; i > -1; i--)
                {
                    if (componentTypesToRemove.Contains(item.Properties[i].GetType()))
                    {
                        item.Properties.RemoveAt(i);
                        success = true;
                    }
                }
            }
            return success;
        }
        #region Logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}
