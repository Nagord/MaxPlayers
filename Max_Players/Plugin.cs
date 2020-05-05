using PulsarPluginLoader;

namespace Max_Players
{
    public class Plugin : PulsarPlugin
	{
        public override string Version => "0.7.0";

        public override string Author => "Kell.EngBot, Dragon, the ModdingTeam";

        public override string ShortDescription => "Increases player limit and allows multiple players of one class.";

        public override string Name => "Max_Players";

        public override string HarmonyIdentifier()
		{
			return "Kell.EngBot.Pulsar.Max_Players";
		}
	}
}
