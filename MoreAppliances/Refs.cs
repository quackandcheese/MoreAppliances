namespace KitchenMoreAppliances
{
    internal class Refs
    {
        #region Vanilla References
        // Items
        public static Appliance HobSpill => Find<Appliance>(ApplianceReferences.MessKitchen1);

        // Appliances
        public static Appliance ResearchDesk => Find<Appliance>(ApplianceReferences.BlueprintUpgradeDesk);
        public static Appliance CopyingDesk => Find<Appliance>(ApplianceReferences.BlueprintCopyDesk);
        public static Appliance DiscountDesk => Find<Appliance>(ApplianceReferences.BlueprintDiscountDesk);
        public static Appliance PrepStation => Find<Appliance>(ApplianceReferences.PrepStation);
        public static Appliance DiningTable => Find<Appliance>(ApplianceReferences.TableLarge);

        // Processes
        public static Process Cook => Find<Process>(ProcessReferences.Cook);
        public static Process Chop => Find<Process>(ProcessReferences.Chop);
        public static Process Knead => Find<Process>(ProcessReferences.Knead);
        public static Process Oven => Find<Process>(ProcessReferences.RequireOven);
        public static Process Research => Find<Process>(ProcessReferences.Upgrade);
        #endregion

        #region Modded References
        // Items
        public static Item ResearchingQuill => Find<Item, ResearchingQuill>();

        // Appliances
        public static Appliance ResearchingQuillProvider => Find<Appliance, ResearchingQuillProvider>();
        public static Appliance UniversalPrepStation => Find<Appliance, UniversalPrepStation>();
        public static Appliance ButcherBlock => Find<Appliance, ButcherBlock>();
        public static Appliance HibachiTable => Find<Appliance, HibachiTable>();

        //Processes
        //public static Process CookWaffle => Find<Process, CookWaffle>();
        #endregion





        private static Appliance.ApplianceProcesses FindApplianceProcess<C>() where C : CustomSubProcess
        {
            ((CustomApplianceProccess)CustomSubProcess.GetSubProcess<C>()).Convert(out var process);
            return process;
        }


    }
}
