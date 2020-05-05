using CodeStage.AntiCheat.ObscuredTypes;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Max_Players
{
    [HarmonyPatch(typeof(PLSaveGameIO), "SaveToFile")]
    class SaveFix
    {
        static bool Prefix(PLSaveGameIO __instance, ref string inFileName, ref ObscuredInt ___CurrentVersionSaveID, ref ObscuredInt ___CurrentVersionGalaxySaveID)
        {
			if (PLNetworkManager.Instance.VersionString == "28.1")
			{
				return true;
			}
				if (PLServer.Instance != null && PLEncounterManager.Instance.PlayerShip != null)
				{
					string text = inFileName + "_temp";
					if (File.Exists(text))
					{
						text += "0";
					}
					if (File.Exists(text))
					{
						text += "0";
					}
					FileStream fileStream = File.Create(text);
					BinaryWriter binaryWriter = new BinaryWriter(fileStream);
					binaryWriter.Write(___CurrentVersionSaveID);
					binaryWriter.Write(___CurrentVersionGalaxySaveID);
					binaryWriter.Write(DateTime.Now.ToBinary());
					binaryWriter.Write(PLServer.Instance.GalaxySeed);
					binaryWriter.Write(PLNetworkManager.Instance.Stats_GameEndTime);
					binaryWriter.Write(PLServer.Instance.CurrentCrewCredits);
					binaryWriter.Write(PLServer.Instance.CurrentCrewLevel);
					binaryWriter.Write(PLServer.Instance.CurrentCrewXP);
					binaryWriter.Write(PLServer.Instance.GetCurrentHubID());
					binaryWriter.Write(PLServer.Instance.IronmanModeIsActive);
					binaryWriter.Write(PLServer.Instance.PlayerCrew_BiscuitsSold);
					binaryWriter.Write(PLServer.Instance.PlayerCrew_WonFBContest);
					binaryWriter.Write(PLServer.Instance.PlayerCrew_BiscuitsSold_WhenContestEnded);
					binaryWriter.Write(PLServer.Instance.BiscuitBombAvailable);
					binaryWriter.Write(PLServer.Instance.CurrentUpgradeMats);
					binaryWriter.Write(PLEncounterManager.Instance.PlayerShip.WeapUpgrade_PawnItemInLockerHash);
					binaryWriter.Write(PLServer.Instance.RepLevels[0]);
					binaryWriter.Write(PLServer.Instance.RepLevels[1]);
					binaryWriter.Write(PLServer.Instance.RepLevels[2]);
					binaryWriter.Write(PLServer.Instance.RepLevels[3]);
					binaryWriter.Write(PLServer.Instance.RepLevels[4]);
					binaryWriter.Write(PLServer.Instance.BiscuitContestIsOver);
					binaryWriter.Write(PLServer.Instance.ActiveBountyHunter_SectorID);
					binaryWriter.Write(PLServer.Instance.ActiveBountyHunter_SecondsSinceWarp);
					binaryWriter.Write(PLServer.Instance.ActiveBountyHunter_TypeID);
					binaryWriter.Write(PLServer.Instance.ActiveBountyHunter_ProcessedChaosLevel);
					binaryWriter.Write(PLServer.Instance.PacifistRun);
					binaryWriter.Write(PLServer.Instance.CreditsSpent_InRun);
					binaryWriter.Write(PLServer.Instance.PerfectBiscuitStreak);
					binaryWriter.Write(PLServer.Instance.BlindJumpCount);
					binaryWriter.Write(PLServer.Instance.CrewPurchaseLimitsEnabled);
					binaryWriter.Write(PLGlobal.Instance.Galaxy.StormPosition.x);
					binaryWriter.Write(PLGlobal.Instance.Galaxy.StormPosition.y);
					binaryWriter.Write(PLGlobal.Instance.Galaxy.StormPosition.z);
					binaryWriter.Write(PLServer.Instance.RacesWonBitfield);
					binaryWriter.Write(PLServer.Instance.RacesLostBitfield);
					binaryWriter.Write(PLServer.Instance.RacesStartedBitfield);
					binaryWriter.Write(PhotonNetwork.room.name);
					binaryWriter.Write(PLEncounterManager.Instance.PlayerShip.ShipNameValue);
					binaryWriter.Write((int)PLEncounterManager.Instance.PlayerShip.ShipTypeID);
					binaryWriter.Write(PLEncounterManager.Instance.PlayerShip.NumberOfFuelCapsules);
					binaryWriter.Write(PLEncounterManager.Instance.PlayerShip.MyStats.HullCurrent);
					binaryWriter.Write(PLEncounterManager.Instance.PlayerShip.FBCrateSupply);
					binaryWriter.Write(PLEncounterManager.Instance.PlayerShip.ReactorCoolantLevelPercent);
					byte[] additionalDataForSaveIO = PLEncounterManager.Instance.PlayerShip.GetAdditionalDataForSaveIO();
					binaryWriter.Write(additionalDataForSaveIO.Length);
					binaryWriter.Write(additionalDataForSaveIO);
					binaryWriter.Write(PLEncounterManager.Instance.PlayerShip.MyStats.AllComponents.Count);
					for (int i = 0; i < PLEncounterManager.Instance.PlayerShip.MyStats.AllComponents.Count; i++)
					{
						PLShipComponent plshipComponent = PLEncounterManager.Instance.PlayerShip.MyStats.AllComponents[i];
						if (plshipComponent != null)
						{
							binaryWriter.Write(plshipComponent.getHash());
							binaryWriter.Write(plshipComponent.SortID);
							binaryWriter.Write(plshipComponent.SubTypeData);
						}
						else
						{
							binaryWriter.Write(0U);
							binaryWriter.Write(-1);
							binaryWriter.Write(0);
						}
					}
					binaryWriter.Write(PLEncounterManager.Instance.PlayerShip.AllPlayerDroppedItems.Count);
					for (int j = 0; j < PLEncounterManager.Instance.PlayerShip.AllPlayerDroppedItems.Count; j++)
					{
						binaryWriter.Write(PLEncounterManager.Instance.PlayerShip.AllPlayerDroppedItems[j].ItemHash);
						binaryWriter.Write(PLEncounterManager.Instance.PlayerShip.AllPlayerDroppedItems[j].Position.x);
						binaryWriter.Write(PLEncounterManager.Instance.PlayerShip.AllPlayerDroppedItems[j].Position.y);
						binaryWriter.Write(PLEncounterManager.Instance.PlayerShip.AllPlayerDroppedItems[j].Position.z);
					}
					for (int k = 0; k < 5; k++)
					{
						PLPlayer cachedFriendlyPlayerOfClass = PLServer.Instance.GetCachedFriendlyPlayerOfClass(k);
						PLPawnInventoryBase plpawnInventoryBase = null;
						PLPawnInventoryBase plpawnInventoryBase2 = null;
						if (cachedFriendlyPlayerOfClass != null)
						{
							plpawnInventoryBase = cachedFriendlyPlayerOfClass.MyInventory;
						}
						PLServerClassInfo plserverClassInfo = PLServer.Instance.ClassInfos[k];
						if (plserverClassInfo != null)
						{
							plpawnInventoryBase2 = plserverClassInfo.ClassLockerInventory;
						}
						int num = 0;
						if (plpawnInventoryBase != null)
						{
							foreach (List<PLPawnItem> list in plpawnInventoryBase.GetAllItems(true))
							{
								foreach (PLPawnItem plpawnItem in list)
								{
									if (plpawnItem != null)
									{
										num++;
									}
								}
							}
						}
						if (plpawnInventoryBase2 != null)
						{
							foreach (List<PLPawnItem> list2 in plpawnInventoryBase2.GetAllItems(false))
							{
								foreach (PLPawnItem plpawnItem2 in list2)
								{
									if (plpawnItem2 != null)
									{
										num++;
									}
								}
							}
						}
						binaryWriter.Write(num);
						if (plpawnInventoryBase != null)
						{
							foreach (List<PLPawnItem> list3 in plpawnInventoryBase.GetAllItems(true))
							{
								foreach (PLPawnItem plpawnItem3 in list3)
								{
									if (plpawnItem3 != null)
									{
										binaryWriter.Write((int)plpawnItem3.PawnItemType);
										binaryWriter.Write(plpawnItem3.SubType);
										binaryWriter.Write(plpawnItem3.Level);
									}
								}
							}
						}
						if (plpawnInventoryBase2 != null)
						{
							foreach (List<PLPawnItem> list4 in plpawnInventoryBase2.GetAllItems(false))
							{
								foreach (PLPawnItem plpawnItem4 in list4)
								{
									if (plpawnItem4 != null)
									{
										binaryWriter.Write((int)plpawnItem4.PawnItemType);
										binaryWriter.Write(plpawnItem4.SubType);
										binaryWriter.Write(plpawnItem4.Level);
									}
								}
							}
						}
					}
					int num2 = 0;
					foreach (PLFactionInfo plfactionInfo in PLGlobal.Instance.Galaxy.AllFactions)
					{
						if (plfactionInfo != null)
						{
							num2++;
						}
					}
					binaryWriter.Write(num2);
					foreach (PLFactionInfo plfactionInfo2 in PLGlobal.Instance.Galaxy.AllFactions)
					{
						if (plfactionInfo2 != null)
						{
							binaryWriter.Write(plfactionInfo2.FactionID);
							binaryWriter.Write(plfactionInfo2.FactionAI_Continuous_GalaxySpreadLimit);
							binaryWriter.Write(plfactionInfo2.FactionAI_Continuous_GalaxySpreadFactor);
						}
					}
					binaryWriter.Write(PLServer.Instance.ChaosLevel);
					binaryWriter.Write(PLServer.Instance.ActiveChaosEvents);
					binaryWriter.Write(PLServer.Instance.TalentLockedStatus);
					binaryWriter.Write((int)PLServer.Instance.TalentToResearch);
					binaryWriter.Write(PLServer.Instance.JumpsNeededToResearchTalent);
					binaryWriter.Write(PLServer.Instance.ResearchMaterials[0]);
					binaryWriter.Write(PLServer.Instance.ResearchMaterials[1]);
					binaryWriter.Write(PLServer.Instance.ResearchMaterials[2]);
					binaryWriter.Write(PLServer.Instance.ResearchMaterials[3]);
					binaryWriter.Write(PLServer.Instance.ResearchMaterials[4]);
					binaryWriter.Write(PLServer.Instance.ResearchMaterials[5]);
					int count = PLServer.Instance.ResearchLockerInventory.AllItems.Count;
					binaryWriter.Write(count);
					foreach (PLPawnItem plpawnItem5 in PLServer.Instance.ResearchLockerInventory.AllItems)
					{
						if (plpawnItem5 != null)
						{
							binaryWriter.Write((int)plpawnItem5.PawnItemType);
							binaryWriter.Write(plpawnItem5.SubType);
							binaryWriter.Write(plpawnItem5.Level);
						}
					}
					for (int l = 0; l < 5; l++)
					{
						PLPlayer cachedFriendlyPlayerOfClass2 = PLServer.Instance.GetCachedFriendlyPlayerOfClass(l);
						if (cachedFriendlyPlayerOfClass2 != null)
						{
							binaryWriter.Write(true);
							binaryWriter.Write(cachedFriendlyPlayerOfClass2.TalentPointsAvailable);
							PLServerClassInfo plserverClassInfo2 = PLServer.Instance.ClassInfos[l];
							if (plserverClassInfo2 != null)
							{
								binaryWriter.Write(plserverClassInfo2.SurvivalBonusCounter);
							}
							else
							{
								binaryWriter.Write(0);
							}
							int num3 = cachedFriendlyPlayerOfClass2.Talents.Length;
							binaryWriter.Write(num3);
							for (int m = 0; m < num3; m++)
							{
								binaryWriter.Write(cachedFriendlyPlayerOfClass2.Talents[m]);
							}
							int count2 = cachedFriendlyPlayerOfClass2.MyInventory.AllItems.Count;
							binaryWriter.Write(count2);
							for (int n = 0; n < count2; n++)
							{
								binaryWriter.Write((int)cachedFriendlyPlayerOfClass2.MyInventory.AllItems[n].PawnItemType);
								binaryWriter.Write(cachedFriendlyPlayerOfClass2.MyInventory.AllItems[n].SubType);
								binaryWriter.Write(cachedFriendlyPlayerOfClass2.MyInventory.AllItems[n].Level);
								binaryWriter.Write(cachedFriendlyPlayerOfClass2.MyInventory.AllItems[n].EquipID);
							}
						}
						else
						{
							binaryWriter.Write(false);
						}
					}
					binaryWriter.Write(PLServer.Instance.CrewFactionID);
					binaryWriter.Write(PLEncounterManager.Instance.PlayerShip.IsFlagged);
					binaryWriter.Write(PLServer.Instance.IsCrewRepRevealed);
					binaryWriter.Write(PLServer.Instance.LongRangeCommsDisabled);
					int count3 = PLServer.Instance.AlreadyAttemptedToStartPickupMissionID.Count;
					binaryWriter.Write(count3);
					foreach (ObscuredInt value in PLServer.Instance.AlreadyAttemptedToStartPickupMissionID)
					{
						int value2 = value;
						binaryWriter.Write(value2);
					}
					binaryWriter.Write(PLNetworkManager.Instance.SelectedShipTypeID);
					binaryWriter.Write(PLServer.ServerSpaceTargetIDCounter);
					int num4 = 0;
					foreach (PLPersistantDialogueActor plpersistantDialogueActor in PLServer.Instance.AllPDAs)
					{
						if (plpersistantDialogueActor != null)
						{
							num4++;
						}
					}
					binaryWriter.Write(num4);
					foreach (PLPersistantDialogueActor plpersistantDialogueActor2 in PLServer.Instance.AllPDAs)
					{
						if (plpersistantDialogueActor2 != null)
						{
							PLPersistantDialogueActorNPC plpersistantDialogueActorNPC = plpersistantDialogueActor2 as PLPersistantDialogueActorNPC;
							PLPersistantDialogueActorShip plpersistantDialogueActorShip = plpersistantDialogueActor2 as PLPersistantDialogueActorShip;
							if (plpersistantDialogueActorNPC != null)
							{
								binaryWriter.Write(0);
								binaryWriter.Write(plpersistantDialogueActorNPC.Hostile);
								binaryWriter.Write(plpersistantDialogueActorNPC.DialogueActorID);
								try
								{
									binaryWriter.Write(plpersistantDialogueActorNPC.NPCName);
								}
								catch (ArgumentNullException ex)
								{
									Debug.DebugBreak();
								}
							}
							else if (plpersistantDialogueActorShip != null)
							{
								binaryWriter.Write(1);
								binaryWriter.Write(plpersistantDialogueActorShip.Hostile);
								binaryWriter.Write(plpersistantDialogueActorShip.DialogueActorID);
								binaryWriter.Write(plpersistantDialogueActorShip.ShipName);
								binaryWriter.Write(plpersistantDialogueActorShip.SpecialActionCompleted);
								binaryWriter.Write(plpersistantDialogueActorShip.LinesToShowPercent.Count);
								foreach (ObscuredInt value3 in plpersistantDialogueActorShip.LinesToShowPercent)
								{
									int value4 = value3;
									binaryWriter.Write(value4);
								}
							}
							else
							{
								binaryWriter.Write(2);
							}
							int count4 = plpersistantDialogueActor2.LinesAleradyDisplayed.Count;
							binaryWriter.Write(count4);
							foreach (ObscuredInt value5 in plpersistantDialogueActor2.LinesAleradyDisplayed)
							{
								int value6 = value5;
								binaryWriter.Write(value6);
							}
						}
					}
					int num5 = 0;
					foreach (PLPersistantShipInfo plpersistantShipInfo in PLServer.Instance.AllPSIs)
					{
						if (plpersistantShipInfo != null)
						{
							num5++;
						}
					}
					binaryWriter.Write(num5);
					foreach (PLPersistantShipInfo plpersistantShipInfo2 in PLServer.Instance.AllPSIs)
					{
						if (plpersistantShipInfo2 != null)
						{
							binaryWriter.Write((int)plpersistantShipInfo2.Type);
							binaryWriter.Write(plpersistantShipInfo2.FactionID);
							binaryWriter.Write(plpersistantShipInfo2.IsShipDestroyed);
							if (plpersistantShipInfo2.MyCurrentSector != null)
							{
								binaryWriter.Write(plpersistantShipInfo2.MyCurrentSector.ID);
							}
							else
							{
								binaryWriter.Write(-1);
							}
							binaryWriter.Write(plpersistantShipInfo2.ShipName);
							binaryWriter.Write(plpersistantShipInfo2.CompOverrides.Count);
							foreach (ComponentOverrideData componentOverrideData in plpersistantShipInfo2.CompOverrides)
							{
								binaryWriter.Write(componentOverrideData.CompLevel);
								binaryWriter.Write(componentOverrideData.CompTypeToReplace);
								binaryWriter.Write(componentOverrideData.CompSubType);
								binaryWriter.Write(componentOverrideData.CompType);
								binaryWriter.Write(componentOverrideData.ReplaceExistingComp);
								binaryWriter.Write(componentOverrideData.CompSubTypeToReplace);
								binaryWriter.Write(componentOverrideData.SlotNumberToReplace);
								binaryWriter.Write(componentOverrideData.IsCargo);
							}
							binaryWriter.Write(plpersistantShipInfo2.HullPercent);
							binaryWriter.Write(plpersistantShipInfo2.ShldPercent);
							binaryWriter.Write(plpersistantShipInfo2.Modifiers);
							binaryWriter.Write(plpersistantShipInfo2.IsFlagged);
							binaryWriter.Write(plpersistantShipInfo2.ForcedHostile);
							binaryWriter.Write(plpersistantShipInfo2.ForcedHostileAll);
							binaryWriter.Write(plpersistantShipInfo2.ForcedHostileName);
							binaryWriter.Write(plpersistantShipInfo2.SelectedActorID);
							if (___CurrentVersionSaveID >= 29)
							{
								binaryWriter.Write(plpersistantShipInfo2.BiscuitsSold);
							}
							if (___CurrentVersionSaveID >= 34)
							{
								PLPersistantShipInfo_FBRival plpersistantShipInfo_FBRival = plpersistantShipInfo2 as PLPersistantShipInfo_FBRival;
								if (plpersistantShipInfo_FBRival != null)
								{
									binaryWriter.Write(plpersistantShipInfo_FBRival.WonFBContest);
								}
								else
								{
									binaryWriter.Write(false);
								}
							}
							if (___CurrentVersionSaveID >= 36)
							{
								binaryWriter.Write(plpersistantShipInfo2.EnsureNoCrew);
							}
						}
					}
					int num6 = 0;
					foreach (PLMissionBase x in PLServer.Instance.AllMissions)
					{
						if (x != null)
						{
							num6++;
						}
					}
					binaryWriter.Write(num6);
					foreach (PLMissionBase plmissionBase in PLServer.Instance.AllMissions)
					{
						if (plmissionBase != null)
						{
							binaryWriter.Write(plmissionBase.MissionTypeID);
							binaryWriter.Write(plmissionBase.Abandoned);
							binaryWriter.Write(plmissionBase.Ended);
							int num7 = 0;
							foreach (PLMissionObjective plmissionObjective in plmissionBase.Objectives)
							{
								if (plmissionObjective != null)
								{
									num7++;
								}
							}
							binaryWriter.Write(num7);
							foreach (PLMissionObjective plmissionObjective2 in plmissionBase.Objectives)
							{
								if (plmissionObjective2 != null)
								{
									binaryWriter.Write(plmissionObjective2.IsCompleted);
									binaryWriter.Write(plmissionObjective2.AmountCompleted);
									binaryWriter.Write(plmissionObjective2.AmountNeeded);
									binaryWriter.Write(plmissionObjective2.HasShownCompletedMessage);
								}
							}
							binaryWriter.Write(plmissionBase.IsPickupMission);
							PLPickupMissionBase plpickupMissionBase = plmissionBase as PLPickupMissionBase;
							if (plpickupMissionBase != null)
							{
								binaryWriter.Write(plpickupMissionBase.RanStartRewards);
							}
							else
							{
								binaryWriter.Write(false);
							}
						}
					}
					int num8 = 0;
					foreach (PLSectorInfo plsectorInfo in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
					{
						if (plsectorInfo != null)
						{
							num8++;
						}
					}
					binaryWriter.Write(num8);
					foreach (PLSectorInfo plsectorInfo2 in PLGlobal.Instance.Galaxy.AllSectorInfos.Values)
					{
						if (plsectorInfo2 != null)
						{
							binaryWriter.Write(plsectorInfo2.ID);
							binaryWriter.Write((int)plsectorInfo2.VisualIndication);
							binaryWriter.Write(plsectorInfo2.FactionStrength);
							binaryWriter.Write(plsectorInfo2.MySPI.Faction);
							binaryWriter.Write(plsectorInfo2.Position.x);
							binaryWriter.Write(plsectorInfo2.Position.y);
							binaryWriter.Write(plsectorInfo2.Position.z);
							binaryWriter.Write(plsectorInfo2.Name);
							binaryWriter.Write(plsectorInfo2.MissionSpecificID);
							binaryWriter.Write(plsectorInfo2.LockedToFaction);
							binaryWriter.Write(plsectorInfo2.LastCalculatedSectorStength);
							binaryWriter.Write(plsectorInfo2.IsPartOfLongRangeWarpNetwork);
							binaryWriter.Write(plsectorInfo2.Visited);
							binaryWriter.Write(plsectorInfo2.BiscuitsSoldCounter);
							PLPersistantEncounterInstance persistantEncounterInstanceAtID = PLEncounterManager.Instance.GetPersistantEncounterInstanceAtID(plsectorInfo2.ID);
							if (persistantEncounterInstanceAtID != null)
							{
								binaryWriter.Write(true);
								__instance.WriteDictionaryIntTPDE(persistantEncounterInstanceAtID.MyPersistantData.GetTraderInfoDictionary(), binaryWriter);
								__instance.WriteDictionaryIntSENE(persistantEncounterInstanceAtID.MyPersistantData.SpecialNetObjectPersistantData, binaryWriter);
								__instance.WriteDictionaryIntBool(persistantEncounterInstanceAtID.MyPersistantData.PickupObjectPersistantData, binaryWriter);
								__instance.WriteDictionaryIntBool(persistantEncounterInstanceAtID.MyPersistantData.PickupComponentPersistantData, binaryWriter);
								__instance.WriteDictionaryIntBool(persistantEncounterInstanceAtID.MyPersistantData.PickupRandomComponentPersistantData, binaryWriter);
								__instance.WriteDictionaryIntBool(persistantEncounterInstanceAtID.MyPersistantData.ProbePickupPersistantData, binaryWriter);
								__instance.WriteDictionaryStringString(persistantEncounterInstanceAtID.MyPersistantData.MiscPersistantData, binaryWriter);
								__instance.WriteDictionaryStringDPO(persistantEncounterInstanceAtID.MyPersistantData.DPOPersistantData, binaryWriter);
								__instance.WriteDictionaryStringDSO(persistantEncounterInstanceAtID.MyPersistantData.DSOPersistantData, binaryWriter);
								binaryWriter.Write(persistantEncounterInstanceAtID.AllPlayerDroppedItems.Count);
								for (int num9 = 0; num9 < persistantEncounterInstanceAtID.AllPlayerDroppedItems.Count; num9++)
								{
									binaryWriter.Write(persistantEncounterInstanceAtID.AllPlayerDroppedItems[num9].ItemHash);
									binaryWriter.Write(persistantEncounterInstanceAtID.AllPlayerDroppedItems[num9].Position.x);
									binaryWriter.Write(persistantEncounterInstanceAtID.AllPlayerDroppedItems[num9].Position.y);
									binaryWriter.Write(persistantEncounterInstanceAtID.AllPlayerDroppedItems[num9].Position.z);
									binaryWriter.Write(persistantEncounterInstanceAtID.AllPlayerDroppedItems[num9].SubHubID);
									binaryWriter.Write(persistantEncounterInstanceAtID.AllPlayerDroppedItems[num9].InteriorID);
								}
							}
							else
							{
								binaryWriter.Write(false);
								binaryWriter.Write(0);
								binaryWriter.Write(0);
								binaryWriter.Write(0);
								binaryWriter.Write(0);
								binaryWriter.Write(0);
								binaryWriter.Write(0);
								binaryWriter.Write(0);
								binaryWriter.Write(0);
								binaryWriter.Write(0);
								binaryWriter.Write(0);
							}
						}
					}
					AIData loadedAIData = PLGlobal.Instance.LoadedAIData;
					PLAIIO.WriteToBinary(loadedAIData, binaryWriter);
					binaryWriter.Close();
					fileStream.Close();
					if (File.Exists(inFileName))
					{
						File.Delete(inFileName);
					}
					File.Move(text, inFileName);
					string relativeSaveFileName = PLNetworkManager.Instance.FileNameToRelative(inFileName);
					string item = "Game has been saved to file: " + relativeSaveFileName;
					PLNetworkManager.Instance.ConsoleText.Insert(0, item);
					bool flag = false;
					if (relativeSaveFileName.StartsWith(__instance.LocalSaveDir))
					{
						flag = true;
					}
					if (!PLServer.Instance.IronmanModeIsActive && !flag)
					{
						PLNetworkManager.Instance.SteamCloud_WriteFileName(relativeSaveFileName, delegate (RemoteStorageFileWriteAsyncComplete_t pCallback, bool bIOFailure)
						{
							__instance.OnRemoteFileWriteAsyncComplete(pCallback, bIOFailure, relativeSaveFileName);
						});
					}
				}
			return false;
		}
    }
}
