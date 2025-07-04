using PulsarModLoader;

namespace MaxPlayers
{
    public class Mod : PulsarMod
	{
        public override string Version => MyPluginInfo.PLUGIN_VERSION;

        public override string Author => MyPluginInfo.PLUGIN_AUTHORS;

        public override string ShortDescription => MyPluginInfo.PLUGIN_DESCRIPTION;

        public override string Name => MyPluginInfo.USERS_PLUGIN_NAME;

        public override string HarmonyIdentifier()
		{
			return MyPluginInfo.PLUGIN_GUID;
		}
	}
}
