namespace Max_Players
{
    class Global
    {
        public static int[] rolelimits = new int[5] { 1, 63, 63, 63, 63 };
        public static byte MaxPlayers = byte.MaxValue;
        public static int[] playercount = new int[5];
        public static int[] roleleads = new int[5];
        public static void Generateplayercount()
        {
            playercount = new int[5];
            foreach (PLPlayer ScannedPlayer in PLServer.Instance.AllPlayers)
            {
                if (ScannedPlayer != null && ScannedPlayer.TeamID == 0 && ScannedPlayer.GetClassID() != -1)
                {
                    playercount[ScannedPlayer.GetClassID()]++;
                }
            }
        }
        
    }
}
