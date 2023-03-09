using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMoreAppliances.Appliances
{
    class HibachiTable : CustomAppliance
    {
        public override string UniqueNameID => "Hibachi Table";
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Hibachi Table");
        public override PriceTier PriceTier => PriceTier.Medium;
        public override RarityTier RarityTier => RarityTier.Rare;
        public override bool IsPurchasable => false;
        public override bool IsPurchasableAsUpgrade => true;
        public override ShoppingTags ShoppingTags => ShoppingTags.FrontOfHouse | ShoppingTags.Cooking;

        public override List<(Locale, ApplianceInfo)> InfoList => new()
        {
            ( Locale.English, LocalisationUtils.CreateApplianceInfo("Hibachi Table", "Flaming hot plate with a side of entertainment", new()
            {
                new Appliance.Section
                {
                    Title = "Hot Plate",
                    Description = "<color=#ff1111>25%</color> slower <sprite name=\"cook\">"
                },
                new Appliance.Section
                {
                    Title = "Combinable",
                    Description = "Combines with adjacent tables"
                }
            }, new()
            {
                "0.75x <sprite name=\"cook\">"
            }) )
        };

        public override List<IApplianceProperty> Properties => new()
        {

            GetCApplianceTable(false, false, false, false, false, false, Orientation.Null),
            new CItemHolder(),
            GetCItemStorage(0, 16, true, true),
            new CHolderFirstIfStorage(),
            new CCausesSpills()
            {
                ID = Refs.HobSpill.ID,
                Rate = 0.025f,
                OverwriteOtherMesses = false
            }
        };

        public override List<Appliance.ApplianceProcesses> Processes => new List<Appliance.ApplianceProcesses>()
        {
            new Appliance.ApplianceProcesses()
            {
                Process = Refs.Cook,
                Speed = 0.75f,
                IsAutomatic = true
            }
        };

        public override void OnRegister(GameDataObject gameDataObject)
        {
            SetupTable(Prefab);

            GameObject parent = Prefab.GetChildFromPath("Hibachi Table");
            parent.ApplyMaterialToChild("Top", "Wood - Autumn", "Metal Black");
            parent.ApplyMaterialToChild("Base", "Metal Black");

            Prefab.GetChild("Steam").ApplyVisualEffect("Steam");
        }
    }
}
