extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

func movement_update(delta):
	if move_left or move_right:
		player.sneak(int(move_right) - int(move_left), delta)
	else:
		player.stop_running()
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
