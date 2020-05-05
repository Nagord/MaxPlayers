using HarmonyLib;

namespace Max_Players
{
    class CompShopFix
    {
        [HarmonyPatch(typeof(PLShop_Exotic1), "CreateInitialWares")]
        class AddOxygenGen
        {
            static void Prefix(TraderPersistantDataEntry inPDE)
            {
                inPDE.ServerAddWare(new PLOxygenGenerator(EO2GeneratorType.E_O2_LARGE, 0));
            }
        }

        [HarmonyPatch(typeof(PLShop_Exotic2), "CreateInitialWares")]
        class AddExotic2
        {
            static void Prefix(TraderPersistantDataEntry inPDE)
            {
                inPDE.ServerAddWare(new PLOxygenGenerator(EO2GeneratorType.E_O2_LARGE, 1));
            }
        }

        [HarmonyPatch(typeof(PLShop_Exotic3), "CreateInitialWares")]
        class AddExotic3
        {
            static void Prefix(TraderPersistantDataEntry inPDE)
            {
                inPDE.ServerAddWare(new PLOxygenGenerator(EO2GeneratorType.E_O2_LARGE, 0));
            }
        }
    }
}
