using PulsarModLoader;

namespace Max_Players
{
    public class Mod : PulsarMod
	{
        public override string Version => "1.0.0";

        public override string Author => "Dragon, Kell.EngBot, the ModdingTeam";

        public override string ShortDescription => "Increases player limit and allows multiple players of one class.";

        public override string Name => "Max_Players";

        public override string HarmonyIdentifier()
		{
			return "Dragon.Max_Players";
		}
	}
}
