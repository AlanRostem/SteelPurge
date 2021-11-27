extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

func movement_update(delta):
	if player.is_moving_too_fast(player.max_dash_speed):
		parent_state_machine.transition_to("PlayerSlideState")
		return
	
	if (move_left or move_right) and player.can_move_on_ground():
		var dir = int(move_right) - int(move_left)
		if dir != 0:
			player.look_horizontally(dir)
		player.run(dir, delta)
		
		var vel_x = player.get_velocity().x
		if sign(vel_x) != dir:
			player.reduce_dash_charge(delta)
		else:
			player.increase_dash_charge(delta)
	else:
		player.reduce_dash_charge(delta)
		player.stop_running()
		
	if !player.is_moving_too_fast(player.max_walk_speed):
		parent_state_machine.transition_to("PlayerWalkState")
		player.clear_dash_charge()
		return
			
	if crouch:
		if player.has_max_dash_charge():
			parent_state_machine.transition_to("PlayerSlideState", {
				"boost": true
			})
		else:
			parent_state_machine.transition_to("PlayerSlideState")
		player.clear_dash_charge()
		
