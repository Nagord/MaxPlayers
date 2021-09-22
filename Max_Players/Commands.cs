using PulsarModLoader;
using PulsarModLoader.Chat.Commands.CommandRouter;
using PulsarModLoader.Utilities;
using System.Linq;

namespace Max_Players
{
    class Commands : ChatCommand
    {
        void SetSavedPlayerLimits()
        {
            PLXMLOptionsIO.Instance.CurrentOptions.SetStringValue("ClassSlotLimits", $"{Global.rolelimits[0]},{Global.rolelimits[1]},{Global.rolelimits[2]},{Global.rolelimits[3]},{Global.rolelimits[4]}");
            PLXMLOptionsIO.Instance.CurrentOptions.SetStringValue("MaxPlayerLimit", $"{Global.MaxPlayers}");
        }
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
            string[] args = new string[2];
            args = arguments.Split(' ');
            int[] CommandArg = new int[2];
            bool[] ArgConvertSuccess = new bool[2];
            string LowerCaseArg = args[0].ToLower();
            if (PhotonNetwork.isMasterClient)
            {
                if (args.Length >= 1 && !string.IsNullOrEmpty(LowerCaseArg))
                {
                    if (args.Length >= 2)
                    {
                        ArgConvertSuccess[0] = int.TryParse(args[1], out CommandArg[0]);
                        if (args.Length >= 3)
                        {
                            ArgConvertSuccess[1] = int.TryParse(args[2], out CommandArg[1]);
                            //if (args.Length >= 4)
                            //{
                            //    ArgConvertSuccess[2] = int.TryParse(args[3], out CommandArg[2]);
                            //}
                        }
                    }
                    PLPlayer player = null;
                    switch (LowerCaseArg)
                    {
                        default:
                            Messaging.Notification("Not a valid subcommand. Subcommands: SetPlayerLimit, SetSlotLimit, kit, SetRoleLead ; The short versions of the commands are their capital letters, ie: SetPlayerLimit = spl");
                            break;

                        case "spl":
                        case "setplayerlimit":
                            if (ArgConvertSuccess[0])
                            {
                                if (CommandArg[0] > byte.MaxValue)
                                {
                                    Messaging.Notification("Cannot input a value higher than 255");
                                    break;
                                }
                                Global.MaxPlayers = CommandArg[0];
                                PLXMLOptionsIO.Instance.CurrentOptions.SetStringValue("MaxPlayerLimit", $"{Global.MaxPlayers}");
                                PhotonNetwork.room.MaxPlayers = CommandArg[0];
                                Messaging.Notification($"set room player limit to {CommandArg[0]}");
                            }
                            else
                            {
                                Messaging.Notification("Use a number. example: /setplayerlimit 2");
                            }
                            break;
                        case "ssl":
                        case "setslotlimit":
                            if (ArgConvertSuccess[1])
                            {
                                int classid = CommandArg[0];
                                if (!ArgConvertSuccess[0])
                                {
                                    classid = HelperMethods.GetClassIDFromClassName(args[1], out ArgConvertSuccess[0]);
                                }
                                if (ArgConvertSuccess[0])
                                {
                                    Global.rolelimits[classid] = CommandArg[1];
                                    Global.MaxPlayers = Global.rolelimits.Sum();
                                    if (Global.MaxPlayers > byte.MaxValue)
                                    {
                                        Global.MaxPlayers = byte.MaxValue;
                                    }
                                    PhotonNetwork.room.MaxPlayers = Global.MaxPlayers;
                                    Messaging.Notification($"{PLPlayer.GetClassNameFromID(classid)} player limit set to {CommandArg[1]}. Player limit is now {Global.MaxPlayers}");
                                    SetSavedPlayerLimits();
                                }
                            }
                            else
                            {
                                Global.Generateplayercount();
                                Messaging.Notification($"current count : Capacity: \nCap {Global.playercount[1]} : {Global.rolelimits[0]} Pil {Global.playercount[2]} : {Global.rolelimits[1]} \nSci {Global.playercount[3]} : {Global.rolelimits[2]} Wep {Global.playercount[4]} : {Global.rolelimits[3]} Eng {Global.playercount[5]} : {Global.rolelimits[4]}");
                            }
                            break;
                        case "kit":
                            {
                                player = PLServer.Instance.GetPlayerFromPlayerID(CommandArg[0]);
                                if (!ArgConvertSuccess[0] && args.Length >= 2)
                                {
                                    player = PLServer.Instance.GetPlayerFromPlayerID(HelperMethods.GetClassIDFromClassName(args[1], out ArgConvertSuccess[0]));
                                }
                                if (ArgConvertSuccess[0])
                                {
                                    int level = 0;
                                    if (ArgConvertSuccess[1])
                                    {
                                        level = CommandArg[1];
                                    }

                                    if (player != null)
                                    {
                                        player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 2, 0, level, 1);
                                        if (PLServer.Instance.CrewFactionID == 4)
                                        {
                                            player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 3, 0, level, 2);
                                        }
                                        else
                                        {
                                            player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 3, 0, level, 2);
                                        }
                                        player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 4, 0, level, 3);
                                    }
                                }

                                else
                                {
                                    Messaging.Notification("Cannot find specified player. Try using a player id or class letter. Set item levels with a 2nd number. usage: /mp kit p 0");
                                }
                                break;
                            }
                        case "kit1":
                            {
                                player = PLServer.Instance.GetPlayerFromPlayerID(CommandArg[0]);
                                if (!ArgConvertSuccess[0] && args.Length >= 2)
                                {
                                    player = PLServer.Instance.GetPlayerFromPlayerID(HelperMethods.GetClassIDFromClassName(args[1], out ArgConvertSuccess[0]));
                                }
                                if (ArgConvertSuccess[0])
                                {
                                    int level = 0;
                                    if (ArgConvertSuccess[1])
                                    {
                                        level = CommandArg[1];
                                    }

                                    if (player != null)
                                    {
                                        player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 10, 0, level, 1);
                                        player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 12, 0, level, 2);
                                        player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 16, 0, level, 3);
                                        if (PLServer.Instance.CrewFactionID == 4)
                                        {
                                            player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 3, 0, level, 4);
                                        }
                                        else
                                        {
                                            player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 3, 0, level, 4);
                                        }
                                        player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 4, 0, level, 5);
                                    }
                                }
                                else
                                {
                                    Messaging.Notification("Cannot find specified player. Try using a player id or class letter. Set item levels with a 2nd number. usage: /mp kit p 0");
                                }
                                break;
                            }
                        case "kit2":
                            player = PLServer.Instance.GetPlayerFromPlayerID(CommandArg[0]);
                            if (!ArgConvertSuccess[0] && args.Length >= 2)
                            {
                                player = PLServer.Instance.GetPlayerFromPlayerID(HelperMethods.GetClassIDFromClassName(args[1], out ArgConvertSuccess[0]));
                            }
                            if (ArgConvertSuccess[0])
                            {
                                int level = 0;
                                if (ArgConvertSuccess[1])
                                {
                                    level = CommandArg[1];
                                }

                                if (player != null)
                                {
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 9, 0, level, 1);
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 7, 0, level, 2);
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 26, 0, level, 3);
                                    if (PLServer.Instance.CrewFactionID == 4)
                                    {
                                        player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 3, 0, level, 4);
                                    }
                                    else
                                    {
                                        player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 3, 0, level, 4);
                                    }
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 4, 0, level, 5);
                                }
                            }
                            else
                            {
                                Messaging.Notification("Cannot find specified player. Try using a player id or class letter. Set item levels with a 2nd number. usage: /mp kit p 0");
                            }
                            break;
                        case "kit3":
                            player = PLServer.Instance.GetPlayerFromPlayerID(CommandArg[0]);
                            if (!ArgConvertSuccess[0] && args.Length >= 2)
                            {
                                player = PLServer.Instance.GetPlayerFromPlayerID(HelperMethods.GetClassIDFromClassName(args[1], out ArgConvertSuccess[0]));
                            }
                            if (ArgConvertSuccess[0])
                            {
                                int level = 0;
                                if (ArgConvertSuccess[1])
                                {
                                    level = CommandArg[1];
                                }

                                if (player != null)
                                {
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 2, 0, level, 1);
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 19, 0, level, 2);
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 33, 0, level, 3);
                                    if (PLServer.Instance.CrewFactionID == 4)
                                    {
                                        player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 3, 0, level, 4);
                                    }
                                    else
                                    {
                                        player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 3, 0, level, 4);
                                    }
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 4, 0, level, 5);
                                }
                            }
                            else
                            {
                                Messaging.Notification("Cannot find specified player. Try using a player id or class letter. Set item levels with a 2nd number. usage: /mp kit p 0");
                            }
                            break;
                        case "setrolelead":
                        case "srl":
                            if (ArgConvertSuccess[1])
                            {
                                int classid = CommandArg[0];
                                if (!ArgConvertSuccess[0])
                                {
                                    classid = HelperMethods.GetClassIDFromClassName(args[1], out ArgConvertSuccess[0]);
                                }
                                if (ArgConvertSuccess[0])
                                {
                                    if (classid > -1 && classid < 5)
                                    {
                                        Global.roleleads[classid] = CommandArg[1];
                                        foreach (PhotonPlayer photonPlayer in ModMessageHelper.Instance.PlayersWithMods.Keys)
                                        {
                                            if (ModMessageHelper.Instance.GetPlayerMods(photonPlayer).Contains(ModMessageHelper.Instance.GetModName("Max_Players")))
                                            {
                                                ModMessage.SendRPC("Dragon.Max_Players", "Max_Players.SendRoleLeads", photonPlayer, new object[] { Global.roleleads });
                                            }
                                        }
                                        Messaging.Notification($"Player of ID {CommandArg[1]} is now the role lead of {PLPlayer.GetClassNameFromID(classid)}");
                                    }
                                    else //classid is not in bounds
                                    {
                                        Messaging.Notification("Received a number not equal to a number between 0-4");
                                    }
                                }
                                else //Second arg conversion was successfull, first was not.
                                {
                                    Messaging.Notification("Could not find classID, set it to the first letter of the player's class or their class ID (class names in displayed order 0-4)");
                                }
                            }
                            else //Second arg conversion was unsuccessfull
                            {
                                Messaging.Notification("Did not detect a second argument! Try using a number");
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
            return new string[][] { new string[] { "spl", "setplayerlimit", "ssl", "setslotlimit", "kit", "kit1", "kit2", "kit3", "srl", "setrolelead" }, new string[] { "%player", "%class" } };
        }
    }
}