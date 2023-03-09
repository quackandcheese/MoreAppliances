using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace KitchenMoreAppliances.Appliances
{
    class UniversalPrepStation : CustomAppliance
    {
        public override string UniqueNameID => "Universal Prep Station";
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Universal Prep Station");
        public override PriceTier PriceTier => PriceTier.Medium;
        public override RarityTier RarityTier => RarityTier.Common;
        public override bool IsPurchasable => false;
        public override bool IsPurchasableAsUpgrade => true;
        public override ShoppingTags ShoppingTags => ShoppingTags.Misc;

        public override List<(Locale, ApplianceInfo)> InfoList => new()
        {
            ( Locale.English, LocalisationUtils.CreateApplianceInfo("Universal Prep Station", "Storing only food is outdated", new()
            {   
                new Appliance.Section
                {
                    Title = "<sprite name=\"upgrade\" color=#A8FF1E> Universal Storage",
                    Description = "Stores up to 4 of any identical item"
                }
            }, new()) )
        };

        public override List<Process> RequiresProcessForShop => new()
        {
            Refs.Research
        };

        public override List<IApplianceProperty> Properties => new()
        {

            GetCItemProvider(0, 0, 4, false, false, false, false, false, false, false),
            new CDynamicItemProvider()
            {
                StorageFlags = ItemStorage.None & ItemStorage.Small & ItemStorage.StackableFood & ItemStorage.OutsideRubbish & ItemStorage.Dish
            }
        };

        public override void OnRegister(GameDataObject gameDataObject)
        {
            GameObject parent = Prefab.GetChildFromPath("FoodPrep");
            var paintedWood = "Wood 4 - Painted";
            var defaultWood = "Wood - Default";
            parent.ApplyMaterialToChild("Base_L_Counter.blend", defaultWood, paintedWood, paintedWood);
            parent.ApplyMaterialToChild("Top", defaultWood);
            parent.ApplyMaterialToChild("Cylinder", "Plastic - Blue");
            parent.GetChild("Base_L_Counter.blend").ApplyMaterialToChild("Handle_L_Counter.blend", "Knob");

            // Limited Item Source View 

            Transform holdTransform = GameObjectUtils.GetChildObject(Prefab, "HoldPoint").transform;

            Prefab.TryAddComponent<HoldPointContainer>().HoldPoint = holdTransform;

            var sourceView = Prefab.TryAddComponent<LimitedItemSourceView>();
            //sourceView.HeldItemPosition = holdTransform;
            ReflectionUtils.GetField<LimitedItemSourceView>("Items").SetValue(sourceView, new List<GameObject>()
            {
                GameObjectUtils.GetChildObject(Prefab, "Slot (1)"),
                GameObjectUtils.GetChildObject(Prefab, "Slot (2)"),
                GameObjectUtils.GetChildObject(Prefab, "Slot (3)"),
                GameObjectUtils.GetChildObject(Prefab, "Slot (4)")
            });
        }
    }
}
