namespace Max_Players
{
    class Global
    {
        public static int[] rolelimits = new int[5] { 1, 63, 63, 63, 63 };
        public static int MaxPlayers = byte.MaxValue;
        public static int[] playercount = new int[6];
        public static int[] roleleads = new int[5];
        public static void Generateplayercount()
        {
            playercount = new int[6];
            foreach (PLPlayer ScannedPlayer in PLServer.Instance.AllPlayers)
            {
                if (ScannedPlayer != null && ScannedPlayer.TeamID == 0)
                {
                    playercount[ScannedPlayer.GetClassID() + 1]++;
                }
            }
        }
        public static bool CanJoinClass(int classID)
        {
            if (classID == -1 || Global.playercount[classID + 1] < Global.rolelimits[classID])
            {
                return true;
            }
            else return false;
        }
    }
}
