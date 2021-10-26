extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

func enter(message):
	if message.has("landed"):
		player.maximize_dash_charge()

func exit():
	player.clear_dash_charge()

func movement_update(delta):
	if player.is_moving_too_fast(player.max_walk_speed):
		parent_state_machine.transition_to("PlayerSlideState")
		return
	
	if move_left or move_right:
		var dir = int(move_right) - int(move_left)
		if dir != 0:
			player.horizontal_looking_direction = dir
		player.run(dir, delta)
		if player.is_moving_too_fast(player.max_walk_speed / 2):
			player.increase_dash_charge(delta)
		else:
			player.reduce_dash_charge(delta)
	else:
		player.reduce_dash_charge(delta)
		player.stop_running()
		if player.is_effectively_standing_still():
			parent_state_machine.transition_to("PlayerIdleState")
			player.set_velocity_x(0)
		
	if crouch:
		if player.is_moving_too_fast(player.max_walk_speed / 2) and player.has_max_dash_charge():
			parent_state_machine.transition_to("PlayerSlideState")
		else:
			parent_state_machine.transition_to("PlayerCrouchState")
