using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenMoreAppliances.Appliances
{
    class ButcherBlock : CustomAppliance
    {
        public override string UniqueNameID => "Butcher Block";
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Butcher Block");
        public override PriceTier PriceTier => PriceTier.Medium;
        public override RarityTier RarityTier => RarityTier.Rare;
        public override bool IsPurchasable => false;
        public override bool IsPurchasableAsUpgrade => true;
        public override ShoppingTags ShoppingTags => ShoppingTags.FrontOfHouse;

        public override List<(Locale, ApplianceInfo)> InfoList => new()
        {
            ( Locale.English, LocalisationUtils.CreateApplianceInfo("Butcher Block", "Chop meat while in the seat", new()
            {
                new Appliance.Section
                {
                    Title = "Versatile",
                    Description = "Allows <sprite name=\"chop\"> and <sprite name=\"knead\">"
                },
                new Appliance.Section
                {
                    Title = "Combinable",
                    Description = "Combines with adjacent tables"
                }
            }, new()) )
        };

        public override List<IApplianceProperty> Properties => new()
        {

            GetCApplianceTable(false, false, false, false, false, false, Orientation.Null),
            new CItemHolder(),
            GetCItemStorage(0, 16, true, true),
            new CHolderFirstIfStorage()
        };

        public override List<Appliance.ApplianceProcesses> Processes => new List<Appliance.ApplianceProcesses>()
        {
            new Appliance.ApplianceProcesses()
            {
                Process = Refs.Chop,
                Speed = 0.75f,
                IsAutomatic = false
            },
            new Appliance.ApplianceProcesses()
            {
                Process = Refs.Knead,
                Speed = 0.75f,
                IsAutomatic = false
            }
        };

        public override void OnRegister(GameDataObject gameDataObject)
        {
            SetupTable(Prefab);

            GameObject parent = Prefab.GetChildFromPath("Butcher Table");
            var metal = MaterialHelper.GetMaterialArray("Metal Black");
            parent.ApplyMaterialToChild("Frame", metal);
            parent.ApplyMaterialToChild("Legs", metal);
            parent.ApplyMaterialToChild("Top", "Wood - Corkboard");
        }
    }
}
