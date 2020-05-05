using HarmonyLib;

namespace Max_Players
{
    //[HarmonyPatch(typeof(PLPawnInventoryBase))]
    //[HarmonyPatch("ServerItemSwap")]
    //class lockers
    //{
    //    private static bool Prefix(PLPawnInventoryBase __instance, ref int targetInvID, ref int itemNetID)
    //    {
    //        PLServer.Instance.AddNotification($"Attempting to swap item {__instance.GetItemAtNetID(itemNetID).Name} from {__instance.Name} to inv {PLNetworkManager.Instance.GetInvAtID(targetInvID).Name}",
    //        PLNetworkManager.Instance.LocalPlayerID, PLServer.Instance.GetEstimatedServerMs() + 6000, false);
    //        return true;
    //    }
    //}
    // Token: 0x02000004 RID: 4
    [HarmonyPatch(typeof(PLTabMenu), "OnClickPID")]
    internal class Lockers
    {
    	// Token: 0x06000005 RID: 5 RVA: 0x000021B0 File Offset: 0x000003B0
    	private static bool Prefix(PLTabMenu __instance, ref PLTabMenu.PawnItemDisplay inPid)
    	{
    		bool flag = __instance.TargetContainer != null && PLNetworkManager.Instance.LocalPlayer != null && inPid != null && PLServer.Instance != null;
    		checked
    		{
    			if (flag)
    			{
    				for (int i = 0; i < __instance.DisplayedPIDS_MyInventory.Count; i++)
    				{
    					PLTabMenu.PawnItemDisplay pawnItemDisplay = __instance.DisplayedPIDS_MyInventory[i];
    					bool flag2 = pawnItemDisplay == inPid && pawnItemDisplay.Item.PawnItemType > EPawnItemType.E_HANDS;
    					if (flag2)
    					{
    						PLNetworkManager.Instance.LocalPlayer.MyInventory.photonView.RPC("ServerItemSwap", PhotonTargets.All, new object[]
    						{
    							__instance.TargetContainer.InventoryID,
    							inPid.Item.NetID
    						});
    					}
    				}
    				for (int j = 0; j < __instance.DisplayedPIDS_Container.Count; j++)
    				{
    					PLTabMenu.PawnItemDisplay pawnItemDisplay2 = __instance.DisplayedPIDS_Container[j];
    					bool flag3 = pawnItemDisplay2 == inPid;
    					if (flag3)
    					{
    						PLPlayer playerOwner = __instance.TargetContainer.PlayerOwner;
                            bool flag4 = playerOwner == PLNetworkManager.Instance.LocalPlayer || PLGlobal.Instance.ClassColors[PLNetworkManager.Instance.LocalPlayer.GetClassID()] == __instance.TargetContainer.Color || PLNetworkManager.Instance.LocalPlayer.GetClassID() == 0 || PLServer.Instance.ResearchLockerInventory == __instance.TargetContainer;
                            //Vanilla:   playerOwner == PLNetworkManager.Instance.LocalPlayer || PLNetworkManager.Instance.LocalPlayer.GetClassID() == 0 || PLServer.Instance.ResearchLockerInventory == __instance.TargetContainer
                            if (flag4)
    						{
    							__instance.TargetContainer.photonView.RPC("ServerItemSwap", PhotonTargets.All, new object[]
    							{
    								PLNetworkManager.Instance.LocalPlayer.MyInventory.InventoryID,
    								inPid.Item.NetID
    							});
    						}
    						else
    						{
                                __instance.TimedErrorMsg = "You don't have permission to take that item!";
    						}
    					}
    				}
    			}
    			return false;
    		}
    	}
    }
}
