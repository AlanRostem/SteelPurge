extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

func movement_update(delta):
	player.apply_slide_friction(delta)
	if player.is_effectively_standing_still():
		player.set_velocity_x(0)
			
	var dir = int(move_right) - int(move_left)
	if dir != 0:
		player.horizontal_looking_direction = dir
			
	if jump:
		parent_state_machine.transition_to("PlayerAirborneState", {
			"jumping": true
		})
		return
			
	if !crouch:
		if player.is_effectively_standing_still():
			parent_state_machine.transition_to("PlayerIdleState")
		else:
			parent_state_machine.transition_to("PlayerRunState")

func enter(message):
	player.crouch()
	
func exit():
	player.stand_up()
