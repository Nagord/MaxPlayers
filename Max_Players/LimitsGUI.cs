using PulsarModLoader;
using PulsarModLoader.CustomGUI;
using PulsarModLoader.MPModChecks;
using PulsarModLoader.Utilities;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace Max_Players
{
    class LimitsGUI : ModSettingsMenu
    {
        public LimitsGUI()
        {
            ErrorGUIStyle.normal.textColor = new Color(255f, 0f, 0f);


            CaptainStyle.alignment = TextAnchor.MiddleCenter;
            CaptainStyle.normal.textColor = PLPlayer.GetClassColorFromID(0);
            CaptainStyle.fixedWidth = 105f;
            CaptainStyle.clipping = TextClipping.Clip;

            PilotStyle.alignment = TextAnchor.MiddleCenter;
            PilotStyle.normal.textColor = PLPlayer.GetClassColorFromID(1);
            PilotStyle.fixedWidth = 105f;
            PilotStyle.clipping = TextClipping.Clip;

            ScienceStyle.alignment = TextAnchor.MiddleCenter;
            ScienceStyle.normal.textColor = PLPlayer.GetClassColorFromID(2);
            ScienceStyle.fixedWidth = 105f;
            ScienceStyle.clipping = TextClipping.Clip;

            WeaponsStyle.alignment = TextAnchor.MiddleCenter;
            WeaponsStyle.normal.textColor = PLPlayer.GetClassColorFromID(3);
            WeaponsStyle.fixedWidth = 105f;
            WeaponsStyle.clipping = TextClipping.Clip;

            EngineerStyle.alignment = TextAnchor.MiddleCenter;
            EngineerStyle.normal.textColor = PLPlayer.GetClassColorFromID(4);
            EngineerStyle.fixedWidth = 105f;
            EngineerStyle.clipping = TextClipping.Clip;
        }

        public override void OnOpen()
        {
            MaxPlayerCount = Global.MaxPlayers.ToString();
            RoleLimits = new string[] { Global.GetRoleLimit(0).ToString(), Global.GetRoleLimit(1).ToString(), Global.GetRoleLimit(2).ToString(), Global.GetRoleLimit(3).ToString(), Global.GetRoleLimit(4).ToString() };
            Global.roleleads.CopyTo(CandidateRoleLeads, 0);
            errorMessage = null;
        }


        public override string Name()
        {
            return "Max Players";
        }

        string[] RoleLimits;
        string MaxPlayerCount;
        int[] CandidateRoleLeads = new int[5];
        string errorMessage = null;

        GUILayoutOption[] normalTextParams = { MaxWidth(40f) };
        GUIStyle ErrorGUIStyle = new GUIStyle();
        GUIStyle CaptainStyle = new GUIStyle();
        GUIStyle PilotStyle = new GUIStyle();
        GUIStyle ScienceStyle = new GUIStyle();
        GUIStyle WeaponsStyle = new GUIStyle();
        GUIStyle EngineerStyle = new GUIStyle();


        public override void Draw()
        {
            if (PLServer.Instance == null || !PhotonNetwork.isMasterClient)
            {
                Label("Host a game to access this menu");
                return;
            }
            ref List<PLPlayer> allPlayers = ref PLServer.Instance.AllPlayers;
            if (allPlayers.Count < 1)
            {
                return;
            }
            Label("Max Player Count");
            MaxPlayerCount = TextField(MaxPlayerCount, normalTextParams);



            Label("Player Limits");

            BeginHorizontal();
            FlexibleSpace();
            Label("Captain", CaptainStyle);
            FlexibleSpace();
            Label("Pilot", PilotStyle);
            FlexibleSpace();
            Label("Science", ScienceStyle);
            FlexibleSpace();
            Label("Weapons", WeaponsStyle);
            FlexibleSpace();
            Label("Engineer", EngineerStyle);
            FlexibleSpace();
            EndHorizontal();

            BeginHorizontal();
            FlexibleSpace();
            RoleLimits[0] = TextField(RoleLimits[0], normalTextParams);
            FlexibleSpace();
            RoleLimits[1] = TextField(RoleLimits[1], normalTextParams);
            FlexibleSpace();
            RoleLimits[2] = TextField(RoleLimits[2], normalTextParams);
            FlexibleSpace();
            RoleLimits[3] = TextField(RoleLimits[3], normalTextParams);
            FlexibleSpace();
            RoleLimits[4] = TextField(RoleLimits[4], normalTextParams);
            FlexibleSpace();
            EndHorizontal();



            Label("Role Leaders");

            BeginHorizontal();
            FlexibleSpace();
            Label("Captain", CaptainStyle);
            FlexibleSpace();
            Label("Pilot", PilotStyle);
            FlexibleSpace();
            Label("Science", ScienceStyle);
            FlexibleSpace();
            Label("Weapons", WeaponsStyle);
            FlexibleSpace();
            Label("Engineer", EngineerStyle);
            FlexibleSpace();
            EndHorizontal();

            BeginHorizontal();
            
            for (int i = 0; i < 5; i++)
            {
                if (PLServer.Instance.GetPlayerFromPlayerID(CandidateRoleLeads[i]) == null)
                {
                    PulsarModLoader.Utilities.Logger.Info("resetting role lead");
                    CandidateRoleLeads[i] = 0;
                }
            }
            FlexibleSpace();
            if (PLServer.Instance.GetPlayerFromPlayerID(CandidateRoleLeads[0]).GetClassID() == 0)
                Label(PLServer.Instance.GetPlayerFromPlayerID(CandidateRoleLeads[0]).GetPlayerName(), CaptainStyle);
            else
                Label("Unassigned", CaptainStyle);
            FlexibleSpace();
            if (PLServer.Instance.GetPlayerFromPlayerID(CandidateRoleLeads[1]).GetClassID() == 1)
                Label(PLServer.Instance.GetPlayerFromPlayerID(CandidateRoleLeads[1]).GetPlayerName(), PilotStyle);
            else
                Label("Unassigned", PilotStyle);
            FlexibleSpace();
            if (PLServer.Instance.GetPlayerFromPlayerID(CandidateRoleLeads[2]).GetClassID() == 2)
                Label(PLServer.Instance.GetPlayerFromPlayerID(CandidateRoleLeads[2]).GetPlayerName(), ScienceStyle);
            else
                Label("Unassigned", ScienceStyle);
            FlexibleSpace();
            if (PLServer.Instance.GetPlayerFromPlayerID(CandidateRoleLeads[3]).GetClassID() == 3)
                Label(PLServer.Instance.GetPlayerFromPlayerID(CandidateRoleLeads[3]).GetPlayerName(), WeaponsStyle);
            else
                Label("Unassigned", WeaponsStyle);
            FlexibleSpace();
            if (PLServer.Instance.GetPlayerFromPlayerID(CandidateRoleLeads[4]).GetClassID() == 4)
                Label(PLServer.Instance.GetPlayerFromPlayerID(CandidateRoleLeads[4]).GetPlayerName(), EngineerStyle);
            else
                Label("Unassigned", EngineerStyle);
            FlexibleSpace();
            EndHorizontal();


            BeginHorizontal();
            FlexibleSpace();
            //Begin Cap
            if (Button("<"))
            {
                CandidateRoleLeads[0] = GetLowerPlayerIDOfClass(0, CandidateRoleLeads[0]);
            }
            if (Button(">"))
            {
                CandidateRoleLeads[0] = GetHigherPlayerIDOfClass(0, CandidateRoleLeads[0]);
            }
            FlexibleSpace();
            //Begin Pilot
            if (Button("<"))
            {
                CandidateRoleLeads[1] = GetLowerPlayerIDOfClass(1, CandidateRoleLeads[1]);
            }
            if (Button(">"))
            {
                CandidateRoleLeads[1] = GetHigherPlayerIDOfClass(1, CandidateRoleLeads[1]);
            }
            FlexibleSpace();
            //Begin Sci
            if (Button("<"))
            {
                CandidateRoleLeads[2] = GetLowerPlayerIDOfClass(2, CandidateRoleLeads[2]);
            }
            if (Button(">"))
            {
                CandidateRoleLeads[2] = GetHigherPlayerIDOfClass(2, CandidateRoleLeads[2]);
            }
            FlexibleSpace();
            //Begin Wep
            if (Button("<"))
            {
                CandidateRoleLeads[3] = GetLowerPlayerIDOfClass(3, CandidateRoleLeads[3]);
            }
            if (Button(">"))
            {
                CandidateRoleLeads[3] = GetHigherPlayerIDOfClass(3, CandidateRoleLeads[3]);
            }
            FlexibleSpace();
            //Begin Eng
            if (Button("<"))
            {
                CandidateRoleLeads[4] = GetLowerPlayerIDOfClass(4, CandidateRoleLeads[4]);
            }
            if (Button(">"))
            {
                CandidateRoleLeads[4] = GetHigherPlayerIDOfClass(4, CandidateRoleLeads[4]);
            }
            FlexibleSpace();
            EndHorizontal();


            //Error Message, Only displays if an error was generated by the last save button press
            if (errorMessage != null)
            {
                Label(errorMessage, ErrorGUIStyle);
            }
            if (Button("Save and Apply Settings"))
            {
                //Validate MaxPlayers
                if (!byte.TryParse(MaxPlayerCount, out byte result))
                {
                    errorMessage = "Max Player Count value is not a byte (between 0-255)";
                    return;
                }
                //Validate RoleLimits Strings as numbers
                int[] roleLimitsResults = new int[5];
                for (int i = 0; i < 5; i++)
                {
                    if (int.TryParse(RoleLimits[i], out roleLimitsResults[i]))
                    {
                        if (!(roleLimitsResults[i] > -1 && roleLimitsResults[i] < 255))
                        {
                            errorMessage = "Player Limits Values must be between 0 and 255";
                            return;
                        }
                    }
                    else
                    {
                        errorMessage = "A Player Limits value is not a number";
                        return;
                    }
                }

                errorMessage = null;
                Global.MaxPlayers.Value = result;
                PhotonNetwork.room.MaxPlayers = result;
                for (byte i = 0; i < 5; i++)
                {
                    Global.SetRoleLimit(i, roleLimitsResults[i]);
                }
                CandidateRoleLeads.CopyTo(Global.roleleads, 0);
                foreach(PhotonPlayer player in MPModCheckManager.Instance.NetworkedPeersWithMod("Dragon.Max_Players"))
                {
                    ModMessage.SendRPC("Dragon.Max_Players", "Max_Players.SendRoleLeads", player, new object[] { Global.roleleads });
                }
                Messaging.Notification("<color=green>Success!</color>");
            }
        }
        public static int GetLowerPlayerIDOfClass(int classID, int playerID)
        {
            int earlierPlayerID = 0;
            foreach (PLPlayer player in PLServer.Instance.AllPlayers)
            {
                if(player != null && player.GetClassID() == classID && player.TeamID == 0)
                {
                    if (player.GetPlayerID() < playerID)
                    {
                        earlierPlayerID = player.GetPlayerID();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return earlierPlayerID;
        }
        public static int GetHigherPlayerIDOfClass(int classID, int playerID)
        {
            int higherPlayerID = playerID;
            foreach (PLPlayer player in PLServer.Instance.AllPlayers)
            {
                if (player != null && player.GetClassID() == classID && player.TeamID == 0)
                {
                    if (player.GetPlayerID() > playerID)
                    {
                        higherPlayerID = player.GetPlayerID();
                        break;
                    }
                }
            }
            return higherPlayerID;
        }
    }
}
