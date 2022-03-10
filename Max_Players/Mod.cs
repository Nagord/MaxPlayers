using PulsarModLoader;

namespace Max_Players
{
    public class Mod : PulsarMod
	{
        public Mod()
        {
            string limitsraw = PLXMLOptionsIO.Instance.CurrentOptions.GetStringValue("ClassSlotLimits");
            if (limitsraw != string.Empty)
            {
                string[] limits = limitsraw.Split(',');
                for (int i = 0; i < 5; i++)
                {
                    Global.rolelimits[i] = int.Parse(limits[i]);
                }
            }
            Global.MaxPlayers = (byte)PLXMLOptionsIO.Instance.CurrentOptions.GetStringValueAsInt("MaxPlayerLimit");
        }

        public override string Version => "0.9.0";

        public override string Author => "Dragon, Kell.EngBot, the ModdingTeam";

        public override string ShortDescription => "Increases player limit and allows multiple players of one class.";

        public override string Name => "Max_Players";

        public override int MPFunctionality => (int)MPFunction.HostOnly;

        public override string HarmonyIdentifier()
		{
			return "Dragon.Max_Players";
		}
	}
}
