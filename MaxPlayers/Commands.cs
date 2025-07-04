using PulsarModLoader;
using PulsarModLoader.Chat.Commands.CommandRouter;
using PulsarModLoader.MPModChecks;
using PulsarModLoader.Utilities;
using System.Collections.Generic;

namespace MaxPlayers
{
    class Commands : ChatCommand
    {
        public override string[] CommandAliases()
        {
            return new string[] { "maxplayers", "mp" };
        }

        public override string Description()
        {
            return "Runs sub-commands.";
        }

        public override void Execute(string arguments)
        {
            string[] args = arguments.Split(' ');
            string LowerCaseArg = args[0].ToLower();
            if (PhotonNetwork.isMasterClient)
            {
                if (args.Length >= 1 && !string.IsNullOrEmpty(LowerCaseArg))
                {
                    PLPlayer player = null;
                    int kitLevel;

                    switch (LowerCaseArg)
                    {
                        default:
                            Messaging.Notification("Not a valid subcommand. Subcommands: SetPlayerLimit, SetSlotLimit, kit, SetRoleLead ; The short versions of the commands are their capital letters, ie: SetPlayerLimit = spl");
                            break;

                        case "spl":
                        case "setplayerlimit":
                            if (args.Length >= 2 && int.TryParse(args[1], out int playerLimit))
                            {
                                if (playerLimit > byte.MaxValue)
                                {
                                    Messaging.Notification("Cannot input a value higher than 255");
                                    break;
                                }
                                Global.MaxPlayers.Value = (byte)playerLimit;
                                PhotonNetwork.room.MaxPlayers = playerLimit;
                                Messaging.Notification($"set room player limit to {playerLimit}");
                            }
                            else
                            {
                                Messaging.Notification("Use a number. example: /setplayerlimit 2");
                            }
                            break;
                        case "ssl":
                        case "setslotlimit":
                            if (args.Length >= 3 && int.TryParse(args[2], out int roleLimit))
                            {
                                int SSLClassID = Global.GetClassIDFromString(args[1]);

                                if (SSLClassID == -1)
                                {
                                    Messaging.Notification("Could not find class. Provide the class name or ID (class names in displayed order 0-4)");
                                }

                                Global.SetRoleLimit(SSLClassID, roleLimit);

                                //Protect against RoleLimits.Value.sum being greater than 255
                                int num = 0;
                                for (int i = 0; i < 5; i++)
                                {
                                    num += Global.GetRoleLimit(i);
                                }
                                if (num > 255)
                                {
                                    Global.MaxPlayers.Value = 255;
                                }
                                else
                                {
                                    Global.MaxPlayers.Value = (byte)num;
                                }

                                PhotonNetwork.room.MaxPlayers = Global.MaxPlayers;
                                Messaging.Notification($"{PLPlayer.GetClassNameFromID(SSLClassID)} player limit set to {roleLimit}. Player limit is now {Global.MaxPlayers}");
                            }
                            else
                            {
                                Global.Generateplayercount();
                                Messaging.Notification("Usage: /mp ssl [class] [limit]");
                                Messaging.Notification($"current count : Capacity: \nCap {Global.playercount[0]} : {Global.GetRoleLimit(0)} Pil {Global.playercount[1]} : {Global.GetRoleLimit(1)} \nSci {Global.playercount[2]} : {Global.GetRoleLimit(2)} Wep {Global.playercount[3]} : {Global.GetRoleLimit(3)} Eng {Global.playercount[4]} : {Global.GetRoleLimit(4)}");
                            }
                            break;
                        case "kit":
                            {
                                if (args.Length < 2)
                                {
                                    Messaging.Notification("Please provide a player name, ID, or class. Usage: /mp kit [player] [kit level (optional)]");
                                    break;
                                }

                                player = HelperMethods.GetPlayer(args[1]);

                                if (player == null)
                                {
                                    Messaging.Notification("Cannot find specified player, provide the player name, ID, or class name. Set item levels with a 2nd number. usage: /mp kit p 0");
                                    break;
                                }

                                //Get level
                                if (args.Length < 3 || !int.TryParse(args[2], out kitLevel))
                                {
                                    kitLevel = 0;
                                }

                                if (player != null)
                                {
                                    List<PLPawnItem> kit = new List<PLPawnItem>
                                    {
                                        PLPawnItem.CreateFromInfo(EPawnItemType.E_PHASEPISTOL, 0, kitLevel),
                                        PLPawnItem.CreateFromInfo(EPawnItemType.E_REPAIRGUN, 0, kitLevel),
                                        PLPawnItem.CreateFromInfo(EPawnItemType.E_FIREGUN, 0, kitLevel)
                                    };
                                    SpawnItems(player, kit);
                                }
                                break;
                            }
                        case "kit1":
                            {
                                if (args.Length < 2)
                                {
                                    Messaging.Notification("Please provide a player name, ID, or class. Usage: /mp kit [player] [kit level (optional)]");
                                    break;
                                }

                                player = HelperMethods.GetPlayer(args[1]);

                                if (player == null)
                                {
                                    Messaging.Notification("Cannot find specified player, provide the player name, ID, or class name. Set item levels with a 2nd number. usage: /mp kit p 0");
                                    break;
                                }

                                //Get level
                                if (args.Length < 3 || !int.TryParse(args[2], out kitLevel))
                                {
                                    kitLevel = 0;
                                }

                                if (player != null)
                                {
                                    List<PLPawnItem> kit2 = new List<PLPawnItem>
                                    {
                                        PLPawnItem.CreateFromInfo(EPawnItemType.E_RANGER, 0, kitLevel),
                                        PLPawnItem.CreateFromInfo(EPawnItemType.E_HAND_SHOTGUN, 0, kitLevel),
                                        PLPawnItem.CreateFromInfo(EPawnItemType.E_SCANNER, 0, kitLevel),
                                        PLPawnItem.CreateFromInfo(EPawnItemType.E_REPAIRGUN, 0, kitLevel),
                                        PLPawnItem.CreateFromInfo(EPawnItemType.E_FIREGUN, 0, kitLevel)
                                    };
                                    SpawnItems(player, kit2);
                                }
                                break;
                            }
                        case "kit2":
                            if (args.Length < 2)
                            {
                                Messaging.Notification("Please provide a player name, ID, or class. Usage: /mp kit [player] [kit level (optional)]");
                                break;
                            }

                            player = HelperMethods.GetPlayer(args[1]);

                            if (player == null)
                            {
                                Messaging.Notification("Cannot find specified player, provide the player name, ID, or class name. Set item levels with a 2nd number. usage: /mp kit p 0");
                                break;
                            }

                            //Get level
                            if (args.Length < 3 || !int.TryParse(args[2], out kitLevel))
                            {
                                kitLevel = 0;
                            }

                            if (player != null)
                            {
                                List<PLPawnItem> kit3 = new List<PLPawnItem>
                                {
                                    PLPawnItem.CreateFromInfo(EPawnItemType.E_BURST_PISTOL, 0, kitLevel),
                                    PLPawnItem.CreateFromInfo(EPawnItemType.E_PIERCING_BEAM_PISTOL, 0, kitLevel),
                                    PLPawnItem.CreateFromInfo(EPawnItemType.E_HELD_BEAM_PISTOL_W_HEALING, 0, kitLevel),
                                    PLPawnItem.CreateFromInfo(EPawnItemType.E_REPAIRGUN, 0, kitLevel),
                                    PLPawnItem.CreateFromInfo(EPawnItemType.E_FIREGUN, 0, kitLevel)
                                };
                                SpawnItems(player, kit3);
                            }
                            break;
                        case "kit3":
                            if (args.Length < 2)
                            {
                                Messaging.Notification("Please provide a player name, ID, or class. Usage: /mp kit [player] [kit level (optional)]");
                                break;
                            }

                            player = HelperMethods.GetPlayer(args[1]);

                            if (player == null)
                            {
                                Messaging.Notification("Cannot find specified player, provide the player name, ID, or class name. Set item levels with a 2nd number. usage: /mp kit p 0");
                                break;
                            }

                            //Get level
                            if (args.Length < 3 || !int.TryParse(args[2], out kitLevel))
                            {
                                kitLevel = 0;
                            }

                            if (player != null)
                            {
                                List<PLPawnItem> kit4 = new List<PLPawnItem>
                                {
                                    PLPawnItem.CreateFromInfo(EPawnItemType.E_PHASEPISTOL, 0, kitLevel),
                                    PLPawnItem.CreateFromInfo(EPawnItemType.E_PULSE_GRENADE, 0, kitLevel),
                                    PLPawnItem.CreateFromInfo(EPawnItemType.E_SYRINGE, 0, kitLevel),
                                    PLPawnItem.CreateFromInfo(EPawnItemType.E_REPAIRGUN, 0, kitLevel),
                                    PLPawnItem.CreateFromInfo(EPawnItemType.E_FIREGUN, 0, kitLevel)
                                };
                                SpawnItems(player, kit4);
                            }
                            break;
                        case "setrolelead":
                        case "srl":
                            if (args.Length < 3)
                            {
                                Messaging.Notification("Did not recieve 2 arguments. Usage: /mp srl [class] [player]");
                                break;
                            }

                            int SRLClassID = Global.GetClassIDFromString(args[1]);

                            if (SRLClassID == -1)
                            {
                                Messaging.Notification("Could not find classID, set it to the first letter of the player's class or their class ID (class names in displayed order 0-4)");
                                break;
                            }

                            player = HelperMethods.GetPlayer(args[2]);

                            if (player == null)
                            {
                                Messaging.Notification("Could not find the specified player. Provide the player name, class name, or player ID.");
                                break;
                            }

                            if (SRLClassID >= 0 && SRLClassID <= 4)
                            {
                                Global.roleleads[SRLClassID] = player.GetPlayerID();
                                foreach (PhotonPlayer photonPlayer in MPModCheckManager.Instance.NetworkedPeersWithMod(MyPluginInfo.PLUGIN_GUID))
                                {
                                    ModMessage.SendRPC(MyPluginInfo.PLUGIN_GUID, "MaxPlayers.SendRoleLeads", photonPlayer, new object[] { Global.roleleads });
                                }
                                Messaging.Notification($"Player of ID {player.GetPlayerName()} is now the role lead of {PLPlayer.GetClassNameFromID(SRLClassID)}");
                            }
                            else //classid is not in bounds
                            {
                                Messaging.Notification("Received a class ID not between 0 and 4");
                            }
                            break;
                    }
                }
                else
                {
                    Messaging.Notification("Use a Subcommand. Subcommands: SetPlayerLimit, SetSlotLimit, kit, SetRoleLead ; The short versions of the commands are their capital letters, ie: SetPlayerLimit = spl");
                }
            }
            else
            {
                Messaging.Notification("Must be host to perform this action.");
            }
        }

        /*public override string[] UsageExamples()
        {
            return new string[] { $"/{CommandAliases()[0]} [SubCommand] [value] [value(if applicable)]" };
        }*/

        public override string[][] Arguments()
        {
            return new string[][] { new string[] { "spl", "setplayerlimit", "ssl", "setslotlimit", "kit", "kit1", "kit2", "kit3", "srl", "setrolelead" }, new string[] { "%player_name", "%player_role" } };
        }
        private static void SpawnItems(PLPlayer player, List<PLPawnItem> items)
        {
            List<bool> filledEquipSlots = new List<bool>(6) { false, false, false, false, false, false };
            foreach (PLPawnItem item in player.MyInventory.AllItems)
            {
                if (item.EquipID != -1)
                {
                    filledEquipSlots[item.EquipID] = true;
                }
            }
            foreach (PLPawnItem item in items)
            {
                if (!item.CanBeEquipped)
                {
                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, (int)item.PawnItemType, item.SubType, item.Level, -1);
                }
                else
                {
                    for (int i = 1; i < filledEquipSlots.Count; i++)
                    {
                        if (!filledEquipSlots[i])
                        {
                            if (item.PawnItemType == EPawnItemType.E_REPAIRGUN && PLServer.Instance.CrewFactionID == 4)
                            {
                                item.PawnItemType = EPawnItemType.E_FB_SELL_TOOL;
                            }
                            player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, (int)item.PawnItemType, item.SubType, item.Level, i);
                            filledEquipSlots[i] = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}
