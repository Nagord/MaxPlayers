using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Max_Players
{
    // Token: 0x02000003 RID: 3
    [HarmonyPatch(typeof(PLServer))]
    [HarmonyPatch("SetPlayerAsClassID")]
    internal class Classes
    {
        // Token: 0x06000003 RID: 3 RVA: 0x00002070 File Offset: 0x00000270
        private static bool Prefix(PLServer __instance, ref int playerID, ref int classID, ref List<PLPlayer> ___LocalCachedPlayerByClass)
        {
            if(!PhotonNetwork.isMasterClient)
            {
                return true;
            }
            if (classID > 4 || classID < -1)
            {
                return false;
            }

            //Gets Player from Method PlayerID
            //format: classid+1; playercount
            PLPlayer PlayerFromID = PLServer.Instance.GetPlayerFromPlayerID(playerID);

            if (PlayerFromID != null)
            {
                Global.Generateplayercount();
                if (classID == -1 || Global.playercount[classID + 1] < Global.rolelimits[classID] && PlayerFromID.GetClassID() != classID)
                {
                    //sends the classchangemessage, sets the player to the class id
                    PlayerFromID.SetClassID(classID);
                    MethodInfo classchangemessage = AccessTools.Method(__instance.GetType(), "ClassChangeMessage", null, null);
                    classchangemessage.Invoke(__instance, new object[] { PlayerFromID.GetPlayerName(false), classID });
                    //PLServer.Instance.AddNotification($"current count : Capacity: \nCap {playercount[1]} : {Global.rolelimits[0]} Pil {playercount[2]} : {Global.rolelimits[1]} \nSci {playercount[3]} : {Global.rolelimits[2]} Wep {playercount[4]} : {Global.rolelimits[3]} Eng {playercount[5]} : {Global.rolelimits[4]}",
                    //PLNetworkManager.Instance.LocalPlayerID, PLServer.Instance.GetEstimatedServerMs() + 6000, false);
                }
                else
                {

                    PLServer.Instance.photonView.RPC("AddCrewWarning", PlayerFromID.GetPhotonPlayer(), new object[]
                    {
                        "That slot is full, choose another one.",
                        new Color(1f, 1f, 1f),
                        0,
                        "ROL"
                    });
                }
            }
            return false;
        }
    }
    //[HarmonyPatch(typeof(PLServer))]
    //[HarmonyPatch("UpdateCachedValues")]
    //internal class CachedValues
    //{
    //    private static void Postfix(ref List<PLPlayer> ___LocalCachedPlayerByClass)
    //    {
    //        for(int i = 0; i < 5; i++)
    //        {
    //            int playerid = Global.roleleads[i];
    //            PLPlayer RoleLead = PLServer.Instance.GetPlayerFromPlayerID(playerid);
    //            if (RoleLead != null && RoleLead.GetClassID() == i)
    //            {
    //                ___LocalCachedPlayerByClass[i] = RoleLead;
    //            }
    //        }
    //    }
    //}
}
