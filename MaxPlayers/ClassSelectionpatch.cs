using HarmonyLib;
using PulsarModLoader.Utilities;

namespace MaxPlayers
{
    [HarmonyPatch(typeof(PLServer), "SetPlayerAsClassID")]
    internal class Classes
    {
        static bool CanJoinClass(int classID)
        {
            if (classID == -1 || Global.playercount[classID] < Global.GetRoleLimit(classID))
            {
                return true;
            }
            else return false;
        }

        static bool Prefix(PLServer __instance, ref int playerID, ref int classID, PhotonMessageInfo pmi)
        {
            //runs vanilla if client isn't hosting
            if (!PhotonNetwork.isMasterClient)
            {
                return true;
            }

            //fails if client not trying to be class -1 through 4
            if (classID < -1 || classID > 4)
            {
                return false;
            }

            //Protect Players from bad actors changing other player's classes.
            PLPlayer playerForPhotonPlayer = PLServer.GetPlayerForPhotonPlayer(pmi.sender);
            if (playerForPhotonPlayer != null && playerForPhotonPlayer.GetPlayerID() != playerID)
            {
                return false;
            }

            PLPlayer PlayerFromID = __instance.GetPlayerFromPlayerID(playerID);
            if (PlayerFromID != null)
            {
                //stop if player is already in the specified class
                if (PlayerFromID.GetClassID() == classID)
                {
                    return false;
                }
                Global.Generateplayercount();
                if (CanJoinClass(classID))
                {
                    //sends the classchangemessage, sets the player to the class id
                    PlayerFromID.SetClassID(classID);
                    AccessTools.Method(__instance.GetType(), "ClassChangeMessage", null, null).Invoke(__instance, new object[] { PlayerFromID.GetPlayerName(false), classID });
                }
                else //Couldn't become role, send available options.
                {
                    string options = "";
                    for (int classid = 0; classid < 5; classid++)
                    {
                        if (CanJoinClass(classid))
                        {
                            options += $"{PLPlayer.GetClassNameFromID(classid)}\n";
                        }
                    }
                    if (string.IsNullOrEmpty(options))
                    {
                        Messaging.Centerprint("There are no slots available. Ask the host to change this or leave.", PlayerFromID, "ROL", PLPlayer.GetClassColorFromID(classID), EWarningType.E_NORMAL);
                        Messaging.Notification($"Player {PlayerFromID.GetPlayerName()} Is trying to join as {PLPlayer.GetClassNameFromID(classID)}. There are no Roles available.");
                    }
                    else
                    {
                        Messaging.Centerprint("That slot is full, choose another one. Options on the left", PlayerFromID, "ROL", PLPlayer.GetClassColorFromID(classID), EWarningType.E_NORMAL);
                        Messaging.Notification(options, PlayerFromID, playerID, 10000 + PLServer.Instance.GetEstimatedServerMs());
                        Messaging.Notification($"Player {PlayerFromID.GetPlayerName()} Is trying to join as {PLPlayer.GetClassNameFromID(classID)}");
                    }
                }
            }
            return false;
        }
    }
}
