namespace KitchenMoreAppliances.Appliances
{
    public class ResearchingQuillProvider : CustomAppliance
    {
        public override string UniqueNameID => "Researching Quill Provider";
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Researching Quill Provider");
        public override PriceTier PriceTier => PriceTier.VeryExpensive;
        public override RarityTier RarityTier => RarityTier.Uncommon;
        public override bool IsPurchasable => true;
        public override ShoppingTags ShoppingTags => ShoppingTags.Misc;

        public override List<(Locale, ApplianceInfo)> InfoList => new()
        {
            ( Locale.English, LocalisationUtils.CreateApplianceInfo("Researching Quill", "Makes you seem smarter", new() 
            { new Appliance.Section
                {
                    Title = "Scholarly",
                    Description = "Hold this to <sprite name=\"upgrade\" color=#A8FF1E> 2x faster"
                } 
            }, new()) )
        };

        public override List<Process> RequiresProcessForShop => new()
        {
            Refs.Research
        };

        public override List<IApplianceProperty> Properties => new()
        {

            GetCItemProvider(Refs.ResearchingQuill.ID, 1, 1, false, false, false, false, false, false, false)
        };

        public override void OnRegister(GameDataObject gameDataObject)
        {
            var parent = Prefab.GetChild("GameObject");
            var inkJar = parent.GetChild("Ink Jar");

            SetupThinCounter(Prefab);
            SetupThinCounterLimitedItem(Prefab, GetPrefab("Researching Quill"), false);

            parent.ApplyMaterialToChild("Researching Quill/Quill/Cube.003", "Plastic - White", "Plastic - Grey");
            inkJar.ApplyMaterialToChild("Jar", "Oven Glass");
            inkJar.ApplyMaterialToChild("Ink", "Piano Black");
        }
    }
}
