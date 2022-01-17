using PulsarModLoader.CustomGUI;
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
            CaptainStyle.normal.textColor = PLPlayer.GetClassColorFromID(0); ;

            PilotStyle.alignment = TextAnchor.MiddleCenter;
            PilotStyle.normal.textColor = PLPlayer.GetClassColorFromID(1); ; 

            ScienceStyle.alignment = TextAnchor.MiddleCenter;
            ScienceStyle.normal.textColor = PLPlayer.GetClassColorFromID(2); ;

            WeaponsStyle.alignment = TextAnchor.MiddleCenter;
            WeaponsStyle.normal.textColor = PLPlayer.GetClassColorFromID(3); ;

            EngineerStyle.alignment = TextAnchor.MiddleCenter;
            EngineerStyle.normal.textColor = PLPlayer.GetClassColorFromID(4); ;
        }

        public override void OnOpen()
        {
            MaxPlayerCount = Global.MaxPlayers.ToString();
            RoleLimits = new string[] { Global.rolelimits[0].ToString(), Global.rolelimits[1].ToString(), Global.rolelimits[2].ToString(), Global.rolelimits[3].ToString(), Global.rolelimits[4].ToString() };
            CandidateRoleLeads = Global.roleleads;
            errorMessage = null;
            
        }


        public override string Name()
        {
            return "Max Players";
        }

        string[] RoleLimits;
        string MaxPlayerCount;
        int[] CandidateRoleLeads;
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
            List<PLPlayer> allPlayers = null;
            int playercount;
            if (PLServer.Instance == null || !PhotonNetwork.isMasterClient)
            {
                Label("Host a game to access this menu");
                return;
            }
            else
            {
                allPlayers = PLServer.Instance.AllPlayers;
                playercount = allPlayers.Count;
                if (playercount < 1)
                {
                    return;
                }
            }
            Label("Max Player Count");
            MaxPlayerCount = TextField(MaxPlayerCount, normalTextParams);



            Label("Player Limits");

            BeginHorizontal();
            Label("Captain", CaptainStyle);
            Label("Pilot", PilotStyle);
            Label("Science", ScienceStyle);
            Label("Weapons", WeaponsStyle);
            Label("Engineer", EngineerStyle);
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
            Label("Captain", CaptainStyle);
            Label("Pilot", PilotStyle);
            Label("Science", ScienceStyle);
            Label("Weapons", WeaponsStyle);
            Label("Engineer", EngineerStyle);
            EndHorizontal();

            BeginHorizontal();
            
            if(allPlayers.Count < 1)
            {
                return;
            }
            for (int i = 0; i < 5; i++)
            {
                if (allPlayers[CandidateRoleLeads[i]] == null)
                {
                    CandidateRoleLeads[i] = 0;
                }
            }
            if (allPlayers[CandidateRoleLeads[0]].GetClassID() == 0)
                Label(allPlayers[CandidateRoleLeads[0]].GetPlayerName(), CaptainStyle);
            else
                Label("Unassigned", CaptainStyle);
            if (allPlayers[CandidateRoleLeads[1]].GetClassID() == 1)
                Label(allPlayers[CandidateRoleLeads[1]].GetPlayerName(), PilotStyle);
            else
                Label("Unassigned", PilotStyle);
            if (allPlayers[CandidateRoleLeads[2]].GetClassID() == 2)
                Label(allPlayers[CandidateRoleLeads[2]].GetPlayerName(), ScienceStyle);
            else
                Label("Unassigned", ScienceStyle);
            if (allPlayers[CandidateRoleLeads[3]].GetClassID() == 3)
                Label(allPlayers[CandidateRoleLeads[3]].GetPlayerName(), WeaponsStyle);
            else
                Label("Unassigned", WeaponsStyle);
            if (allPlayers[CandidateRoleLeads[4]].GetClassID() == 4)
                Label(allPlayers[CandidateRoleLeads[4]].GetPlayerName(), EngineerStyle);
            else
                Label("Unassigned", EngineerStyle);
            EndHorizontal();

            BeginHorizontal();

            //Begin Cap
            if (Button("<"))
            {
                int earlierPlayerID = -1;
                for (int i = 0; i < CandidateRoleLeads[0]; i++)
                {
                    if (allPlayers[i].GetClassID() == 0 && allPlayers[i].TeamID == 0)
                    {
                        earlierPlayerID = i;
                    }
                }
                if (earlierPlayerID != -1 && earlierPlayerID < Global.roleleads[0])
                {
                    CandidateRoleLeads[0] = earlierPlayerID;
                }
            }
            if (Button(">"))
            {
                for (int i = CandidateRoleLeads[0]; i < playercount; i++)
                {
                    if (allPlayers[i].GetClassID() == 0 && allPlayers[i].TeamID == 0)
                    {
                        CandidateRoleLeads[0] = i;
                        break;
                    }
                }
            }
            //Begin Pilot
            if (Button("<"))
            {
                int earlierPlayerID = -1;
                for (int i = 0; i < CandidateRoleLeads[1]; i++)
                {
                    if (allPlayers[i].GetClassID() == 1 && allPlayers[i].TeamID == 0)
                    {
                        earlierPlayerID = i;
                    }
                }
                if (earlierPlayerID != -1 && earlierPlayerID < Global.roleleads[1])
                {
                    CandidateRoleLeads[1] = earlierPlayerID;
                }
            }
            if (Button(">"))
            {
                for (int i = CandidateRoleLeads[1]; i < playercount; i++)
                {
                    if (allPlayers[i].GetClassID() == 1 && allPlayers[i].TeamID == 0)
                    {
                        CandidateRoleLeads[1] = i;
                        break;
                    }
                }
            }
            //Begin Sci
            if (Button("<"))
            {
                int earlierPlayerID = -1;
                for (int i = 0; i < CandidateRoleLeads[2]; i++)
                {
                    if (allPlayers[i].GetClassID() == 2 && allPlayers[i].TeamID == 0)
                    {
                        earlierPlayerID = i;
                    }
                }
                if (earlierPlayerID != -1 && earlierPlayerID < Global.roleleads[2])
                {
                    CandidateRoleLeads[2] = earlierPlayerID;
                }
            }
            if (Button(">"))
            {
                for (int i = CandidateRoleLeads[2]; i < playercount; i++)
                {
                    if (allPlayers[i].GetClassID() == 2 && allPlayers[i].TeamID == 0)
                    {
                        CandidateRoleLeads[2] = i;
                        break;
                    }
                }
            }
            //Begin Wep
            if (Button("<"))
            {
                int earlierPlayerID = -1;
                for (int i = 0; i < CandidateRoleLeads[3]; i++)
                {
                    if (allPlayers[i].GetClassID() == 3 && allPlayers[i].TeamID == 0)
                    {
                        earlierPlayerID = i;
                    }
                }
                if (earlierPlayerID != -1 && earlierPlayerID < Global.roleleads[3])
                {
                    CandidateRoleLeads[3] = earlierPlayerID;
                }
            }
            if (Button(">"))
            {
                for (int i = CandidateRoleLeads[3]; i < playercount; i++)
                {
                    if (allPlayers[i].GetClassID() == 3 && allPlayers[i].TeamID == 0)
                    {
                        CandidateRoleLeads[3] = i;
                        break;
                    }
                }
            }
            //Begin Eng
            if (Button("<"))
            {
                int earlierPlayerID = -1;
                for (int i = 0; i < CandidateRoleLeads[4]; i++)
                {
                    if (allPlayers[i].GetClassID() == 4 && allPlayers[i].TeamID == 0)
                    {
                        earlierPlayerID = i;
                    }
                }
                if (earlierPlayerID != -1 && earlierPlayerID < Global.roleleads[4])
                {
                    CandidateRoleLeads[4] = earlierPlayerID;
                }
            }
            if (Button(">"))
            {
                for (int i = CandidateRoleLeads[4]; i < playercount; i++)
                {
                    if (allPlayers[i].GetClassID() == 4 && allPlayers[i].TeamID == 0)
                    {
                        CandidateRoleLeads[4] = i;
                        break;
                    }
                }
            }
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
                Global.MaxPlayers = result;
                Global.rolelimits = roleLimitsResults;
                Global.roleleads = CandidateRoleLeads;
            }
        }
    }
}
