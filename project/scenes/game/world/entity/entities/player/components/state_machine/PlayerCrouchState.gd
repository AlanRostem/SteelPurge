extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

func movement_update(delta):
	player.apply_slide_friction(delta)
	if player.is_effectively_standing_still():
		player.set_velocity_x(0)
			
	if !crouch:
		if player.is_effectively_standing_still():
			parent_state_machine.transition_to("PlayerIdleState")
		else:
			parent_state_machine.transition_to("PlayerRunState")

func enter(message):
	player.crouch()
	
func exit():
	player.stand_up()
