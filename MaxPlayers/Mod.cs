using PulsarModLoader;

namespace MaxPlayers
{
    public class Mod : PulsarMod
	{
        public override string Version => "1.1.0";

        public override string Author => "Dragon, Kell.EngBot, OnHyex, Pulsar Modding Team";

        public override string ShortDescription => "Increases player limit and allows multiple players of one class.";

        public override string Name => "Max_Players";

        public override string HarmonyIdentifier()
		{
			return "Dragon.Max_Players";
		}
	}
}
