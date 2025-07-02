using HarmonyLib;
using PulsarModLoader.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Mono.Security.X509.X520;

namespace Max_Players
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
        private static bool Prefix(PLServer __instance, ref int playerID, ref int classID, PhotonMessageInfo pmi)
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
    [HarmonyPatch(typeof(PLServer), "UpdateCachedValues")]
    class CachedValues
    {
        private static void Postfix(ref List<PLPlayer> ___LocalCachedPlayerByClass)
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
    //Patching in the creation of new buttons as needed and handling the updates of the new buttons
    [HarmonyPatch(typeof(PLTabMenu), "UpdatePIDs")]
    class SendItemMenu
    {
        private static GameObject storedSendButtonGameObject;
        private static Task<UnityEngine.UI.Text[]> storedTask;
        private static bool StartUp = false;
        private static int SendItemPageNumber = 0;
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list = new List<CodeInstruction>() 
            { 
                new CodeInstruction(OpCodes.Ldc_I4_M1)
            };
            List<CodeInstruction> list2 = new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldc_I4_5)
            };
            instructions = PulsarModLoader.Patches.HarmonyHelpers.PatchBySequence(instructions, list, list2, PulsarModLoader.Patches.HarmonyHelpers.PatchMode.REPLACE, PulsarModLoader.Patches.HarmonyHelpers.CheckMode.ALWAYS, false);
            List<CodeInstruction> InstructionsList = instructions.ToList();
            list = new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldloc_S),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Add),
                new CodeInstruction(OpCodes.Stloc_S),
                new CodeInstruction(OpCodes.Ldloc_S),
                new CodeInstruction(OpCodes.Ldc_I4_4),
                new CodeInstruction(OpCodes.Blt),
                new CodeInstruction(OpCodes.Ldloc_S),
                new CodeInstruction(OpCodes.Brtrue_S),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call),
                new CodeInstruction(OpCodes.Brfalse_S),
            };
            list2 = new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SendItemMenu), "SendItemListCountAdjustment"))
            };
            for (int i = 0; i < InstructionsList.Count - list.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (InstructionsList[i + j].opcode == list[j].opcode)
                    {
                        if (j == list.Count - 1)
                        {
                            //Debug.Log("Doing the call transpile");
                            InstructionsList.Insert(i + 5, list2[0]);
                            InstructionsList.RemoveRange(i + 6, 1);
                        }
                    } 
                    else
                    {
                        break;
                    }
                }
            }

            return InstructionsList;
        }
        //private static int SendItemListCountAdjustment(List<PLPlayer> SendPlayerTargets)
        private static int SendItemListCountAdjustment()
        {
            if (!StartUp)
            {
                //Remove incorrect Send buttons that have certain references messed up internally causing issues with the highlighting and selection animation
                for (int i = 1; i < PLTabMenu.Instance.SendItem_Targets.Length; i++)
                {
                    UnityEngine.GameObject.Destroy(PLTabMenu.Instance.SendItem_Targets[i].gameObject);
                }
                PLTabMenu.Instance.SendItem_Targets = new UnityEngine.UI.Text[1] { PLTabMenu.Instance.SendItem_Targets[0] };
                //Add Buttons that allow the menu to scroll
                CreateScrollButtons();
                //Update the Layout of the Send Item Buttons
                UpdateDisplayedSendItemButtons();
                //Hide 'selector' as it doesn't seem to do anything
                PLTabMenu.Instance.SendItem_Selector.gameObject.SetActive(false);
                StartUp = true;
            }
            if (PLTabMenu.Instance.SendItem_Targets.Length > 0 && PLTabMenu.Instance.SendItem_Targets.Length < PLTabMenu.Instance.SendPlayerTargets.Count)
            {
                if (storedTask == null)
                {
                    //run async task to create new send buttons
                    storedTask = Task.Run<UnityEngine.UI.Text[]>(() => CreateNewSendButtons(PLTabMenu.Instance.SendItem_Targets, new UnityEngine.UI.Text[PLTabMenu.Instance.SendPlayerTargets.Count]));
                    //storedTask = Task.Run<UnityEngine.UI.Text[]>(CreateNewSendButtons);
                } 
                else if (storedTask.IsCompleted)
                {
                    //takes result of task and merges it back to main thread
                    PLTabMenu.Instance.SendItem_Targets = storedTask.Result;
                    UpdateDisplayedSendItemButtons();
                    //free resources of task
                    storedTask.Dispose();
                    storedTask = null;
                }
            }
            return PLTabMenu.Instance.SendItem_Targets.Length;
        }
        private static async Task<UnityEngine.UI.Text[]> CreateNewSendButtons(UnityEngine.UI.Text[] StoredSendTargets, UnityEngine.UI.Text[] tempSendTargets)
        {
            //forces async from the start
            await Task.Yield();
            for (int i = 0; i < StoredSendTargets.Length; i++)
            {
                tempSendTargets[i] = StoredSendTargets[i];
            }
            if (storedSendButtonGameObject == null)
            {
                storedSendButtonGameObject = global::UnityEngine.GameObject.Instantiate(PLTabMenu.Instance.SendItem_Targets[0].gameObject);
                await Task.Delay(100);
            }
            for (int i = StoredSendTargets.Length; i < tempSendTargets.Length; i++)
            {
                GameObject tempGameObject = global::UnityEngine.GameObject.Instantiate(storedSendButtonGameObject);
                tempGameObject.name = $"Slot{i + 1}";
                UnityEngine.UI.Text tempText = tempGameObject.GetComponent<UnityEngine.UI.Text>();
                tempSendTargets[i] = tempText;
                UnityEngine.UI.Button tempButton = tempGameObject.GetComponent<UnityEngine.UI.Button>();
                tempButton.onClick.SetPersistentListenerState(0, UnityEventCallState.Off);
                //Has to be its own int sent to it, setting the listener as i causes issues with the lambda capture
                //All calls of the listener would call the final result of i after this entire function ends instead it calls
                //the current value of i as the for loop sets it
                int SendPlayerTargetID = i;
                tempButton.onClick.AddListener(new UnityAction(() => PLTabMenu.Instance.ClickSendItemOfIndex(SendPlayerTargetID)));
                UnityEngine.Transform transform = tempGameObject.transform;
                transform.SetParent(PLTabMenu.Instance.SendItemMenu.transform);
                transform.localRotation = new Quaternion(0, 0, 0, 0);
                transform.localScale = new Vector3(1, 1, 1);
                transform.localPosition = new Vector3(0, 160 - 55 * i + SendItemPageNumber * 9 * 55, 0);
                global::UnityEngine.Object.DontDestroyOnLoad(tempGameObject);
                tempGameObject.SetActive(false);
            }
            return tempSendTargets;
        }
        private static void CreateScrollButtons()
        {
            GameObject UpArrow = new GameObject("SendButtonScrollUp", new Type[]
            {
                typeof(Image),
                typeof(Button)
            })
            {
                layer = 5,
                hideFlags = HideFlags.HideAndDontSave,
            };
            UpArrow.GetComponent<Image>().sprite = PLGlobal.Instance.LeftArrowSprite;
            UpArrow.transform.SetParent(PLTabMenu.Instance.SendItemMenu.transform);
            UpArrow.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 32f);
            UpArrow.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 32f);
            UpArrow.transform.localPosition = new Vector3(-355, 160, 0);
            UpArrow.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            UpArrow.transform.localRotation = new Quaternion(0, 0, 1, -1);
            UpArrow.GetComponent<Button>().onClick.AddListener(() => ChangeSendItemPageNumber(false));
            GameObject DownArrow = new GameObject("SendButtonScrollDown", new Type[]
            {
                typeof(Image),
                typeof(Button)
            })
            {
                layer = 5,
                hideFlags = HideFlags.HideAndDontSave,
            };
            DownArrow.GetComponent<Image>().sprite = PLGlobal.Instance.LeftArrowSprite;
            DownArrow.transform.SetParent(PLTabMenu.Instance.SendItemMenu.transform);
            DownArrow.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 32f);
            DownArrow.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 32f);
            DownArrow.transform.localPosition = new Vector3(-355, -280, 0);
            DownArrow.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            DownArrow.transform.localRotation = new Quaternion(0, 0, 1, 1);
            DownArrow.GetComponent<Button>().onClick.AddListener(() => ChangeSendItemPageNumber(true));
        }
        private static void ChangeSendItemPageNumber(bool Down)
        {
            if (Down)
            {
                if (PLTabMenu.Instance.SendItem_Targets.Length > (SendItemPageNumber + 1) * 9)
                {
                    SendItemPageNumber++;
                }
                else
                {
                    SendItemPageNumber = 0;
                }
            }
            else
            {
                if (SendItemPageNumber == 0)
                {
                    SendItemPageNumber = PLTabMenu.Instance.SendItem_Targets.Length / 9;
                }
                else
                {
                    SendItemPageNumber--;
                }
            }
            UpdateDisplayedSendItemButtons();
        }
        private static void UpdateDisplayedSendItemButtons()
        {
            for (int i = 0; i < PLTabMenu.Instance.SendItem_Targets.Length; i++)
            {
                PLTabMenu.Instance.SendItem_Targets[i].gameObject.SetActive((i >= SendItemPageNumber * 9 && i < (SendItemPageNumber + 1) * 9));
                PLTabMenu.Instance.SendItem_Targets[i].transform.localPosition = new Vector3(0, 160 - 55 * i + SendItemPageNumber * 9 * 55, 0);
            }
        }
    }
}
