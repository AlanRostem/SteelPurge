extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

var __is_jumping = false

func movement_update(delta):
	if __is_jumping:
		if cancel_jump and player.get_velocity().y < -player.min_jump_speed:
			__is_jumping = false
			player.set_velocity_y(-player.min_jump_speed)

	var dir = int(move_right) - int(move_left)
	if dir != 0:
		player.look_horizontally(dir)
		
	if !player.is_moving_too_fast(player.max_walk_speed):
		player.air_move(dir, delta)
	else:
		var intended_dir = int(move_right) - int(move_left)
		var move_dir = sign(player.get_velocity().x)
		if intended_dir != move_dir:
			player.air_move(intended_dir, delta)
	
	if player.is_on_ground():
		if !player.is_moving_too_fast(player.max_dash_speed):
			if !player.is_effectively_standing_still():
				if crouch:
					if player.has_max_dash_charge():
						parent_state_machine.transition_to("PlayerSlideState", {
							"boost": true
						})
					else:
						parent_state_machine.transition_to("PlayerSlideState")
				else:
					if player.is_moving_too_fast(player.max_walk_speed):
						parent_state_machine.transition_to("PlayerRunState")
					else:
						parent_state_machine.transition_to("PlayerWalkState")
			else:
				parent_state_machine.transition_to("PlayerIdleState")
		else:
			parent_state_machine.transition_to("PlayerSlideState")
	

func enter(message: Dictionary):
	if message.has("jumping"):
		__is_jumping = true
		player.jump()

func exit():
	__is_jumping = false
