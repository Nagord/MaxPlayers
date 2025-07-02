using PulsarModLoader;
using PulsarModLoader.Utilities;

namespace Max_Players
{
    class Global
    {
        public static void SetRoleLimit(int index, int value)
        {
            switch (index)
            {
                case 0:
                    CapLimit.Value = value;
                    break;
                case 1:
                    PilotLimit.Value = value;
                    break;
                case 2:
                    SciLimit.Value = value;
                    break;
                case 3:
                    WeapLimit.Value = value;
                    break;
                case 4:
                    EngLimit.Value = value;
                    break;
            }
        }
        public static int GetRoleLimit(int index)
        {
            switch (index)
            {
                default:
                    return CapLimit.Value;
                case 1:
                    return PilotLimit.Value;
                case 2:
                    return SciLimit.Value;
                case 3:
                    return WeapLimit.Value;
                case 4:
                    return EngLimit.Value;
            }
        }

        /// <summary>
        /// Get Class ID from ID string or class name.
        /// </summary>
        /// <param name="Class"></param>
        /// <returns>Class ID. -1 if failed to parse.</returns>
        public static int GetClassIDFromString(string Class)
        {
            if (int.TryParse(Class, out int ClassID))
            {
                if (ClassID >= -1 || ClassID <= 4)
                {
                    return ClassID;
                }
                else
                {
                    return -1;
                }
            }

            return HelperMethods.GetClassIDFromClassName(Class, out _);
        }

        public static SaveValue<int> CapLimit = new SaveValue<int>("CapLimit", 1);
        public static SaveValue<int> PilotLimit = new SaveValue<int>("PilotLimit", 63);
        public static SaveValue<int> SciLimit = new SaveValue<int>("SciLimit", 63);
        public static SaveValue<int> WeapLimit = new SaveValue<int>("WeapLimit", 63);
        public static SaveValue<int> EngLimit = new SaveValue<int>("EngLimit", 63);
        public static SaveValue<byte> MaxPlayers = new SaveValue<byte>("MaxPlayers", byte.MaxValue);
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
