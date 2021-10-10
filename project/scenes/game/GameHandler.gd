extends Node2D

"""
Handles everything related to playing the game itself. This node swaps out levels when
transitioning between them and handles the level selection world. This node also loads
all save data about the player and manages the player's temporary in-game stats. With
that in mind, all user interface, such as the pause menu and HUD, will be handled through
the playable nodes themselves (such as a level and the level select world)
"""

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
