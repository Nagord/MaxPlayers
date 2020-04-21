using PulsarPluginLoader.Chat.Commands;
using System.Linq;

namespace Max_Players
{
    class Commands : IChatCommand
    {
        void GetSavedPlayerLimits()
        {
            string[] limits = PLXMLOptionsIO.Instance.CurrentOptions.GetStringValue("ClassSlotLimits").Split(',');
            for (int i = 0; i < 5; i++)
            {
                Global.rolelimits[i] = int.Parse(limits[i]);
            }
            Global.MaxPlayers = PLXMLOptionsIO.Instance.CurrentOptions.GetStringValueAsInt("MaxPlayerLimit");
        }
        void SetSavedPlayerLimits()
        {
            PLXMLOptionsIO.Instance.CurrentOptions.SetStringValue("ClassSlotLimits", $"{Global.rolelimits[0]},{Global.rolelimits[1]},{Global.rolelimits[2]},{Global.rolelimits[3]},{Global.rolelimits[4]}");
            PLXMLOptionsIO.Instance.CurrentOptions.SetStringValue("MaxPlayerLimit", $"{Global.MaxPlayers}");
        }
        public string[] CommandAliases()
        {
            if (PLXMLOptionsIO.Instance.CurrentOptions.GetStringValue("ClassSlotLimits") != string.Empty)
            {
                GetSavedPlayerLimits();
            }
            return new string[] { "maxplayers", "mp" };
        }

        public string Description()
        {
            return "Runs sub-commands.";
        }

        public bool Execute(string arguments)
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
                    switch (LowerCaseArg)
                    {
                        default:
                            PLServer.Instance.AddNotification("Not a valid subcommand. Subcommands: SetPlayerLimit, SetSlotLimit, SetRoleLead ; The short versions of the commands are their capital letters, ie: SetPlayerLimit = spl",
                            PLNetworkManager.Instance.LocalPlayerID, PLServer.Instance.GetEstimatedServerMs() + 6000, false);
                            break;

                        case "spl":
                        case "setplayerlimit":
                            if (ArgConvertSuccess[0])
                            {
                                if(CommandArg[0] > byte.MaxValue)
                                {
                                    PLServer.Instance.AddNotification("Cannot input a value higher than 256",
                                    PLNetworkManager.Instance.LocalPlayerID, PLServer.Instance.GetEstimatedServerMs() + 6000, false);
                                    break;
                                }
                                Global.MaxPlayers = CommandArg[0];
                                PLXMLOptionsIO.Instance.CurrentOptions.SetStringValue("MaxPlayerLimit", $"{Global.MaxPlayers}");
                                PhotonNetwork.room.maxPlayers = CommandArg[0];
                                PLServer.Instance.AddNotification($"set room player limit to {CommandArg[0]}",
                                PLNetworkManager.Instance.LocalPlayerID, PLServer.Instance.GetEstimatedServerMs() + 6000, false);
                            }
                            else
                            {
                                PLServer.Instance.AddNotification("Use a number. example: /setplayerlimit 2",
                                PLNetworkManager.Instance.LocalPlayerID, PLServer.Instance.GetEstimatedServerMs() + 6000, false);
                            }
                            break;
                        case "ssl":
                        case "setslotlimit":
                            if (ArgConvertSuccess[1])
                            {
                                int classid = CommandArg[0];
                                if (!ArgConvertSuccess[0])
                                {
                                    classid = Global.GetClassTypeFromString(args[1], out ArgConvertSuccess[0]);
                                }
                                if (ArgConvertSuccess[0])
                                {
                                    Global.rolelimits[classid] = CommandArg[1];
                                    Global.MaxPlayers = Global.rolelimits.Sum();
                                    if (Global.MaxPlayers > byte.MaxValue)
                                    {
                                        Global.MaxPlayers = byte.MaxValue;
                                    }
                                    PhotonNetwork.room.maxPlayers = Global.MaxPlayers;
                                    PLServer.Instance.AddNotification($"{PLPlayer.GetClassNameFromID(classid)} player limit set to {CommandArg[1]}. Player limit is now {Global.MaxPlayers}",
                                    PLNetworkManager.Instance.LocalPlayerID, PLServer.Instance.GetEstimatedServerMs() + 6000, false);
                                    SetSavedPlayerLimits();
                                }
                            }
                            else
                            {
                                Global.Generateplayercount();
                                PLServer.Instance.AddNotification($"current count : Capacity: \nCap {Global.playercount[1]} : {Global.rolelimits[0]} Pil {Global.playercount[2]} : {Global.rolelimits[1]} \nSci {Global.playercount[3]} : {Global.rolelimits[2]} Wep {Global.playercount[4]} : {Global.rolelimits[3]} Eng {Global.playercount[5]} : {Global.rolelimits[4]}",
                                PLNetworkManager.Instance.LocalPlayerID, PLServer.Instance.GetEstimatedServerMs() + 6000, false);
                            }
                            break;
                        //case "setrolelead":
                        //case "srl":
                        //    if (ArgConvertSuccess[1])
                        //    {
                        //        int classid = CommandArg[0];
                        //        if (!ArgConvertSuccess[0])
                        //        {
                        //            classid = Global.GetClassTypeFromString(args[1], out ArgConvertSuccess[0]);
                        //        }
                        //        if (ArgConvertSuccess[0])
                        //        {
                        //            Global.roleleads[classid] = CommandArg[1];
                        //            PLServer.Instance.AddNotification($"Player of ID {CommandArg[1]} is now the role lead of {PLPlayer.GetClassNameFromID(classid)}",
                        //            PLNetworkManager.Instance.LocalPlayerID, PLServer.Instance.GetEstimatedServerMs() + 6000, false);
                        //        }
                        //    }
                        //    break;
                    }
                }
            }
            else
            {
                PLServer.Instance.AddNotification("Must be host to perform this action.",
                PLNetworkManager.Instance.LocalPlayerID, PLServer.Instance.GetEstimatedServerMs() + 6000, false);
            }
            return false;
        }

        public string UsageExample()
        {
            return $"/{CommandAliases()[0]} [SubCommand] [value] [value(if applicable)]";
        }
    }
}