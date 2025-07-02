using PulsarModLoader;
using PulsarModLoader.Chat.Commands.CommandRouter;
using PulsarModLoader.MPModChecks;
using PulsarModLoader.Utilities;

namespace Max_Players
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
                                Messaging.Notification($"current count : Capacity: \nCap {Global.playercount[1]} : {Global.GetRoleLimit(0)} Pil {Global.playercount[2]} : {Global.GetRoleLimit(1)} \nSci {Global.playercount[3]} : {Global.GetRoleLimit(2)} Wep {Global.playercount[4]} : {Global.GetRoleLimit(3)} Eng {Global.playercount[5]} : {Global.GetRoleLimit(4)}");
                            }
                            break;
                        case "kit":
                            {
                                if (args.Length <= 2)
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
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 2, 0, kitLevel, 1);
                                    if (PLServer.Instance.CrewFactionID == 4)
                                    {
                                        player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 24, 0, kitLevel, 2);
                                    }
                                    else
                                    {
                                        player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 3, 0, kitLevel, 2);
                                    }
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 4, 0, kitLevel, 3);
                                }
                                break;
                            }
                        case "kit1":
                            {
                                if (args.Length <= 2)
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
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 10, 0, kitLevel, 1);
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 12, 0, kitLevel, 2);
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 16, 0, kitLevel, 3);
                                    if (PLServer.Instance.CrewFactionID == 4)
                                    {
                                        player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 24, 0, kitLevel, 4);
                                    }
                                    else
                                    {
                                        player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 3, 0, kitLevel, 4);
                                    }
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 4, 0, kitLevel, 5);
                                }
                                break;
                            }
                        case "kit2":
                            if (args.Length <= 2)
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
                                player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 9, 0, kitLevel, 1);
                                player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 7, 0, kitLevel, 2);
                                player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 26, 0, kitLevel, 3);
                                if (PLServer.Instance.CrewFactionID == 4)
                                {
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 24, 0, kitLevel, 4);
                                }
                                else
                                {
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 3, 0, kitLevel, 4);
                                }
                                player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 4, 0, kitLevel, 5);
                            }
                            break;
                        case "kit3":
                            if (args.Length <= 2)
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
                                player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 2, 0, kitLevel, 1);
                                player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 19, 0, kitLevel, 2);
                                player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 33, 0, kitLevel, 3);
                                if (PLServer.Instance.CrewFactionID == 4)
                                {
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 24, 0, kitLevel, 4);
                                }
                                else
                                {
                                    player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 3, 0, kitLevel, 4);
                                }
                                player.MyInventory.UpdateItem(PLServer.Instance.PawnInvItemIDCounter++, 4, 0, kitLevel, 5);
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
                                foreach (PhotonPlayer photonPlayer in MPModCheckManager.Instance.NetworkedPeersWithMod("Dragon.Max_Players"))
                                {
                                    ModMessage.SendRPC("Dragon.Max_Players", "Max_Players.SendRoleLeads", photonPlayer, new object[] { Global.roleleads });
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
            return new string[][] { new string[] { "spl", "setplayerlimit", "ssl", "setslotlimit", "kit", "kit1", "kit2", "kit3", "srl", "setrolelead" }, new string[] { "%player", "%class" } };
        }
    }
}