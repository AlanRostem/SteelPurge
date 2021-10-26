extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

var __is_jumping = false

func movement_update(delta):
	if __is_jumping:
		if cancel_jump and player.get_velocity().y < -player.min_jump_speed:
			__is_jumping = false
			player.set_velocity_y(-player.min_jump_speed)
			
	if !player.is_moving_too_fast(player.max_walk_speed):
		var dir = int(move_right) - int(move_left)
		if dir != 0:
			player.horizontal_looking_direction = dir
		player.air_move(dir, delta)
	else:
		var intended_dir = int(move_right) - int(move_left)
		var move_dir = sign(player.get_velocity().x)
		if intended_dir != move_dir:
			player.air_move(intended_dir, delta)
	
	if player.is_on_ground():
		if !player.is_moving_too_fast(player.max_walk_speed):
			if player.get_velocity().x != 0:
				if crouch:
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
