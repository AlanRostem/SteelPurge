extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

func movement_update(delta):
	if player.is_moving_too_fast(player.max_dash_speed):
		parent_state_machine.transition_to("PlayerSlideState")
		return
	
	if move_left or move_right:
		var dir = int(move_right) - int(move_left)
		if dir != 0:
			player.horizontal_looking_direction = dir
		player.run(dir, delta)
		
		var vel_x = player.get_velocity().x
		if sign(vel_x) != dir:
			player.reduce_dash_charge(delta)
	else:
		player.reduce_dash_charge(delta)
		player.stop_running()
		
	if !player.is_moving_too_fast(player.max_walk_speed):
		parent_state_machine.transition_to("PlayerWalkState")
		return
			
	if crouch:
		player.clear_dash_charge()
		parent_state_machine.transition_to("PlayerSlideState", {
				"boost": true
			})
