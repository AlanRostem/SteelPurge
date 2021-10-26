extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

func movement_update(delta):
	if crouch:
		parent_state_machine.transition_to("PlayerCrouchState")
		return
		
	if jump:
		parent_state_machine.transition_to("PlayerAirborneState", {
			"jumping": true
		})
		return
		
	if move_left or move_right:
		parent_state_machine.transition_to("PlayerWalkState")
