using System;
using HarmonyLib;

namespace Max_Players
{
	// Token: 0x02000005 RID: 5
	[HarmonyPatch(typeof(PhotonNetwork), "CreateRoom")]
	[HarmonyPatch(new Type[]
	{
		typeof(string),
		typeof(RoomOptions),
		typeof(TypedLobby)
	})]
	internal class Players
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000023A5 File Offset: 0x000005A5
		private static void Prefix(ref RoomOptions roomOptions) //Load saved settings and set room limit to specified params
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
		    Global.MaxPlayers = PLXMLOptionsIO.Instance.CurrentOptions.GetStringValueAsInt("MaxPlayerLimit");
			roomOptions.MaxPlayers = (byte)Global.MaxPlayers;
		}
	}
}
