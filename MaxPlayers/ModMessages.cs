﻿using HarmonyLib;
using PulsarModLoader;
using PulsarModLoader.MPModChecks;

namespace MaxPlayers
{
    class SendRoleLeads : ModMessage
    {
        public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
        {
            Global.roleleads = (int[])arguments[0];
        }
    }
    [HarmonyPatch(typeof(PLServer), "LoginMessage")]
    class PlayerConnectedPatch //used to sync the client with the host's settings
    {
        static void Postfix(ref PhotonPlayer newPhotonPlayer)
        {
            if (PhotonNetwork.isMasterClient && MPModCheckManager.Instance.NetworkedPeerHasMod(newPhotonPlayer, "Dragon.Max_Players"))
            {
                ModMessage.SendRPC("Dragon.Max_Players", "Max_Players.SendRoleLeads", newPhotonPlayer, new object[] { Global.roleleads });
            }
        }
    }
}
