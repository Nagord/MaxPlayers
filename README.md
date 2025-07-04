[![](https://img.shields.io/badge/-Nagord-111111?style=just-the-label&logo=github&labelColor=24292f)](https://github.com/Nagord)
![](https://img.shields.io/badge/Game%20Version-1.2.07-111111?style=flat&labelColor=24292f&color=111111)
[![](https://img.shields.io/discord/458244416562397184.svg?&logo=discord&logoColor=ffffff&style=flat&label=Discord&labelColor=24292f&color=111111)](https://discord.gg/j3Pydn6 "Pulsar Crew Matchup Server")

# Max Players

Version 1.1.0  
For Game Version 1.2.07  
Developed by Dragon, Kell.EngBot, OnHyex, Pulsar Modding Team  
Requires: PulsarModLoader

Support the developer: https://www.patreon.com/DragonFire47

Source: https://github.com/DragonFire47/MaxPlayers

---------------------

### 💡 Function(s)

- Increases the player capacity
- Modify player joined functionality
  - Sets cap for classes. default: 1 captain, 63 all other roles.
  - Send failed to select role to players attempting to join a filled role.
- Saves and loads player capacity info
- Sets role leader whose talents matter
- Adds kit command for multiple players in a class
- Adds GUI for managing player limits and role leaders.
- 

### 🎮 Usage

### Commands: (All commands and subcommands can be shortened to their capital letters.)  
- /MaxPlayers - Controlls SubCommands.  
  - Usage: /MaxPlayers [SubCommand] [Value] [Value (If applicable)]

### SubCommands:  
- SetSlotLimit - Sets limit on players joining the specified role. Run with no arguments to get current players and limits.  
  - Usage: /mp ssl [class letter] [player limit]  
- SetPlayerLimit - Sets Overall Playerlimit. (You may run out of player slots while still gaining players)  
  - Usage: /mp spl [playerlimit]  
- SetRoleLead - Sets player whose talents matter for their class. (When using this, players without the mod may lose synchronization of aspects related to talents.)  
  - Usage: /mp srl [class letter] [player id]  
- kit - Gives players a kit with an optional level.  
  - Usage: /mp kit/kit[1-3] [playerID] [Optional item levels]  

### Kits:  
- kit  - Phase Pistol, Repair gun/Multitool, fire extiguisher  
- kit1 - Heavy Beam Pistol, Splitshot, Scanner, Repair gun/Multitool, fire extiguisher  
- kit2 - Burst Rifle, Beam Pistol, Healing beam rifle, Repair gun/Multitool, fire extiguisher  
- kit3 - Phase Pistol, Pulse Grenade, Revitalizing Syringe, Repair gun/Multitool, fire extiguisher  

### 👥 Multiplayer Functionality

- ✅ Client
  - Clients get better experience when installed
- ✅ Host
  - Only the host needs this mod.

---------------------

## 🔧 Install Instructions

- Have PulsarModLoader installed
- Go to \PULSARLostColony\Mods
- Add the .dll included with this package

---------------------

## Common issues
- Network stability may be lost with high player counts
- Max player limit may not update after configuration when current player count matches the limit.
