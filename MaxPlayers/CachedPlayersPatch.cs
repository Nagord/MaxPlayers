using HarmonyLib;
using System.Collections.Generic;

namespace MaxPlayers
{
    [HarmonyPatch(typeof(PLServer), "UpdateCachedValues")]
    class CachedPlayersPatch
    {
        static void Postfix(ref List<PLPlayer> ___LocalCachedPlayerByClass)
        {
            for(int i = 0; i < 5; i++)
            {
                int playerid = Global.roleleads[i];
                PLPlayer RoleLead = PLServer.Instance.GetPlayerFromPlayerID(playerid);
                if (RoleLead != null && RoleLead.GetClassID() == i)
                {
                    ___LocalCachedPlayerByClass[i] = RoleLead;
                }
            }
        }
    }
}
