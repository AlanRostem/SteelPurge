# Steel Purge

Steel Purge is a passion project I'm working on for both learning Godot Engine and fun.
All ideas are my own, but the game mechanics are heavily inspired by the Call of Duty: 
Zombies series.

## Ideas

### Background Story

It is the year 2034 and the world is undergoing a global industrial revolution. This time,
military technology is the hope of every land's survival. Countries are putting
resources, including their people, into weapon production in order to dominate any war 
that inevitably arises. You are working for district Goliath-A1, the primary weapons 
factory of your government that spans two thirds of the capital. Every employee must wear 
a specialized industrial body armor which protects from any accidental friendly fire at 
the workplace. This factory only produces ballistic infantry weapons, but your curiosity
doesn't fool you. Seeking around the deeper structures of the factory, you find a hatch
which seemingly would lead to a sewer. Being your curious you, you open the hatch and for
some reason strong chiming winds throw you into the hole which leads to a strangely large
sewer. It is dark, but some lights work. Through the dark alleys of the sewer, strange
metalized humanoid beings emerge with malicious intent. An industrial revolver that came 
along with the winds rests on the ground. Your options become clearer by the second. 

### Game Goal

Survive by making the weapons you can to make it out of the apocalypse you found yourself in. 
The game is a pretty simple platformer with level completion and the strive for the progression
system. 

### Progression system

The progression system in Steel Purge will feature RPG style features as well as simple platformer
traits like completing levels.

#### Gunsmith levels

The player can gain experience and level up. Leveling up to a certain point gives the player the ability
to craft new weapons and/or weapon parts. The parts are going to be harder to level up for than the weapons.

#### Weapon Drops

Weapons can be dropped by enemies when playing in *Purge Mode*, even ones the player does not know how to craft (he just learns how to
make them when picking it up for the first time).

### Items

There are certain items the player can keep on them.

#### Weapon Parts

Player can find weapon parts in secret areas or get them by defeating bosses. The parts are used to upgrade
the respective weapons to their respective stage. 

#### Scrap

*Scrap* is a progression unit that is essentially the currency of the game. *Scrap* can be used
to make weapons and ammo and also upgrade the weapons. 

### Movement

The player movement in Steel Purge is a standard platformer movement with walking and jumping. Due to the 
limited input the player will have, there are some twists on how he can move.

#### Auto-sprint

After the player has walked for a few seconds without interruptions he starts sprinting.

#### Double-tap sprint 

Player must double tap a movement key to engage in sprinting mode

### Weapons

Weapons in Steel Purge are guns that use up ammo and need to be reloaded. They can also be upgraded 
to 3 different stages that affect their stats and add additional features to them. Each weapon has unique
features when it is upgraded, and its purpose is to provide special mechanics unique to certain level designs.
The level designs are not meant to force the player to use a weapon (not always at least), but to provide a way
to choose a strategy. The harder the levels get the more the player has to think about strategies. 

#### Weapon Stages

- **Stage 0**: Default stage which the weapon comes in. No additional upgrades added
- **Stage 1**: First upgrade stage which gives the weapon more ammo, damage, or fire rate depending on what
the weapon is. In addition, the weapon gets a passive ability that comes along with firing.
- **Stage 2**: This stage grants the weapon a new ability that is on a cooldown whenever it is used. In addition,
the passive ability gets an upgrade related to this new ability.
- **Stage 3**: The weapon is given a new abiltiy which needs to be charged up the more damage the player does using
the weapon. The ability is supposed to be very powerful and do something unique to the weapon. 

#### Shooting

Player can shoot with his equipped weapon in 4 different directions. 

#### Recoil-hovering

By firing downwards the player can fall slower. The slower the weapon makes you walk while firing the slower it makes you fall

### Weapon Ideas

#### Judger .45

#### P53 Viper

#### Marshal D12

#### M11 Warden

#### Hamilton 700

#### MG27 Jackhammer

#### Bellum AT-98

#### WASP AA

### Levels

The levels in Steel Purge have unique platforming layouts and ways to be completed.

#### Anvils

The checkpoints on each level is an anvil. These can be used to craft certain items (ammo, weapons, etc.). When a level is complete,
before the player is about to leave the level there is an anvil for the player to use. Anvils have a quick repair-and-refill option
for players who choose not to upgrade their weapons. There will be a notification on the anvil saying that the player can upgrade
or make new weapons and the amount of new things he can explore.

#### Sections

Levels are divided into sections and the camera does not move further than the section. When the player moves out of the section the camera 
pans over to the next section. This mechanic does not apply to *Purge Mode*.

#### Purge Mode

Upon clearing a level the player is given the ability to enter *Purge Mode* on that particular level. This mode is essentially an event
that occurs every now and again on cleared levels. The story aspect of this is that the X-Warbs have suddenly appeared in one particular
part of the world and you need to destroy them all.

*Purge Mode* is a round-based mode where you survive as many waves of enemies as possible. Every X rounds a mini-boss appears and defeating
it rewards you with a random *Stage Y* weapon. Dying in this mode has no penalty on the user's progress, but stops the mode and kicks the 
player out of the level. The player can also choose to exit the level, but this also cancels the mode and the player must wait for another
one to appear next time.
