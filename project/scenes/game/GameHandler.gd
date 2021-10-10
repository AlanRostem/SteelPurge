extends Node2D

"""
Handles everything related to playing the game itself. This node swaps out levels when
transitioning between them and handles the level selection world. This node also loads
all save data about the player and manages the player's temporary in-game stats. With
that in mind, all user interface, such as the pause menu and HUD, will be handled through
the playable nodes themselves (such as a level and the level select world)
"""

var __current_level = null


"""
Deletes the current level (if one is active) and instances a new one from the specified
scene.
"""
func set_current_level(level_scene):
	if __current_level != null:
		__current_level.queue_free()
	__current_level = level_scene.instance()
