using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using PulsarPluginLoader.Utilities;
using UnityEngine;

namespace Max_Players
{
    // Token: 0x02000003 RID: 3
    [HarmonyPatch(typeof(PLServer), "SetPlayerAsClassID")]
    internal class Classes
    {
        // Token: 0x06000003 RID: 3 RVA: 0x00002070 File Offset: 0x00000270
        private static bool Prefix(PLServer __instance, ref int playerID, ref int classID, ref List<PLPlayer> ___LocalCachedPlayerByClass)
        {
            //runs vanilla if client isn't hosting
            if(!PhotonNetwork.isMasterClient)
            {
                return false;
            }
            //fails if client trying to be class -1 through 4
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
                if (Global.CanJoinClass(classID) && PlayerFromID.GetClassID() != classID)
                {
                    //sends the classchangemessage, sets the player to the class id
                    PlayerFromID.SetClassID(classID);
                    MethodInfo classchangemessage = AccessTools.Method(__instance.GetType(), "ClassChangeMessage", null, null);
                    classchangemessage.Invoke(__instance, new object[] { PlayerFromID.GetPlayerName(false), classID });
                }
                else
                {
                    string options = "";
                    for (int classid = 0; classid <5; classid++)
                    {
                        if(Global.CanJoinClass(classid))
                        {
                            options += $"{PLPlayer.GetClassNameFromID(classid)}\n";
                        }
                    }
                    if (string.IsNullOrEmpty(options))
                    {
                        options = "none";
                    }
                    Messaging.Centerprint("That slot is full, choose another one. options on the left", PlayerFromID, "ROL", PLPlayer.GetClassColorFromID(classID), EWarningType.E_RACE_WIN);
                    Messaging.Notification(options, PlayerFromID, playerID, 10000);
                    Messaging.Notification($"Player {PlayerFromID.GetPlayerName()} Is trying to join as {PLPlayer.GetClassNameFromID(classID)}");
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
