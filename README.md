# MaxPlayers

## Information
Developers: Dragon, Kell.EngBot, and the ModdingTeam  
For Game Version 1.2.01  
For PML Version 0.11.0  
Mod Version 1.0.0  
Host/Client Requirements: Host | Clients get better experience when installed

Source: https://github.com/DragonFire47/MaxPlayers

Support the developer: https://www.patreon.com/DragonFire47


## Installation 
-have PulsarModLoader installed  
-go to \PULSARLostColony\Mods  
-add the .dll included with this package

## Features
- Increases the player capacity  
- modify player joined functionality  
  - Sets cap for classes. default: 1 captain, 63 all other roles.  
  - Send failed to select role to players attempting to join a filled role.  
- saves and loads player capacity info  
- sets role leader whose talents matter  
- adds kit command for multiple players in a class  
- adds GUI for managing player limits and role leaders.

## Usage
### Commands: (All commands and subcommands can be shortened to their capital letters.)  
/MaxPlayers - Controlls SubCommands. Usage: /MaxPlayers [SubCommand] [Value] [Value (If applicable)]

### SubCommands:  
SetSlotLimit - Sets limit on players joining the specified role. Run with no arguments to get current players and limits.  
Usage: /mp ssl [class letter] [player limit]  
SetPlayerLimit - Sets Overall Playerlimit. (You may run out of player slots while still gaining players)  
Usage: /mp spl [playerlimit]  
SetRoleLead - Sets player whose talents matter for their class. (When using this, players without the mod may lose synchronization of aspects related to talents.)  
Usage: /mp srl [class letter] [player id]  
kit - gives players a kit with an optional level  
Usage: /mp kit/kit[1-3] [playerID] [Optional item levels]  

### Kits:  
kit  - Phase Pistol, Repair gun/Multitool, fire extiguisher  
kit1 - Heavy Beam Pistol, Splitshot, Scanner, Repair gun/Multitool, fire extiguisher  
kit2 - Burst Rifle, Beam Pistol, Healing beam rifle, Repair gun/Multitool, fire extiguisher  
kit3 - Phase Pistol, Pulse Grenade, Revitalizing Syringe, Repair gun/Multitool, fire extiguisher  

## Common issues
- Network stability may be lost with high player counts
