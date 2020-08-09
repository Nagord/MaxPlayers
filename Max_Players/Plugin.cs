using PulsarPluginLoader;

namespace Max_Players
{
    public class Plugin : PulsarPlugin
	{
        public override string Version => "0.8.0";

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
