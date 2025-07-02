using System;
using HarmonyLib;

namespace Max_Players
{
	// Token: 0x02000005 RID: 5
	[HarmonyPatch(typeof(PhotonNetwork), "CreateRoom", new Type[] { typeof(string), typeof(RoomOptions), typeof(TypedLobby)})]
	internal class PlayerCountPatch
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000023A5 File Offset: 0x000005A5
		private static void Prefix(ref RoomOptions roomOptions) //Load saved settings and set room limit to specified params
		{
			roomOptions.MaxPlayers = Global.MaxPlayers;
		}
	}
}
