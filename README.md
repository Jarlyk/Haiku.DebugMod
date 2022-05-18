
[//]: # ( Haiku Debug Mod )

# Introduction
This is a Debug tooling mod for the game Haiku, the Robot. 

### Installation
This requires that you already have BepInEx and the Haiku.CoreModdingApi installed.  To install, simply place the Haiku.DebugMod.dll into your BepInEx/plugins folder.

### Usage
Currently the following are supported:
F1: Open ConfigManager
F2: Toggle invulnerability
F3: Toggle no-heat
F4: Toggle hitboxes
F5: Toggle stat display (mostly for % tracking)
F6: Save SaveState
F7: Load SaveState
The MapWarp Button toggles MapWarp, enabling you to teleport anywhere you click on the Map

### Building
This Git repo includes the Haiku API dependency, but does not include the Unity or Haiku Assemblies.  Prior to building, you'll need to copy the necessary files from the Managed folder in your Haiku installation to the lib/Game folder.  This particular mod also relies on the publicizer, so you will then need to copy the publicized Assembly-CSharp.dll into the lib/Game folder as well.

### Contact
You can reach me via Github.  As with most mods, this is a hobby project, so please understand that response times to questions and time to update for new game releases may vary.

### License
All mods contained herein are released under the standard MIT license, which is a permissive license that allows for free use.  The text of this is included in the release archive in License.txt under each mod.
