extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

func movement_update(delta):
	var player = parent_state_machine.parent_entity
	if move_left or move_right:
		parent_state_machine.transition_to("PlayerRunState")
