extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

var __is_jumping = false

func movement_update(delta):
	player.air_move(int(move_right) - int(move_left), delta)
	if player.is_on_floor():
		if !player.is_moving_too_fast(player.max_walk_speed):
			if player.get_velocity().x != 0:
				parent_state_machine.transition_to("PlayerRunState")
			else:
				parent_state_machine.transition_to("PlayerIdleState")
		else:
			pass # TODO: Implement this case

func physics_process_input_update(delta):
	if __is_jumping:
		if cancel_jump and player.get_velocity().y < -player.min_jump_speed:
			__is_jumping = false
			player.set_velocity_y(-player.min_jump_speed)

func enter(message: Dictionary):
	if message.has("jumping"):
		__is_jumping = true
		player.jump()

func exit():
	__is_jumping = false
