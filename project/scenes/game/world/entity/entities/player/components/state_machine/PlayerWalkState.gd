extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

func movement_update(delta):
	if move_left or move_right:
		var dir = int(move_right) - int(move_left)
		if dir != 0:
			player.horizontal_looking_direction = dir
		player.walk(dir, delta)
		
		var vel_x = player.get_velocity().x
		if sign(vel_x) == dir:
			player.increase_dash_charge(delta)
			if player.has_max_dash_charge():
				parent_state_machine.transition_to("PlayerRunState")
		else:
			player.reduce_dash_charge(delta)
	else:
		player.reduce_dash_charge(delta)
		player.stop_running()
		if player.is_effectively_standing_still():
			parent_state_machine.transition_to("PlayerIdleState")
			player.set_velocity_x(0)
			player.clear_dash_charge()
		
	if crouch:
		if player.get_velocity().x != 0:
			parent_state_machine.transition_to("PlayerSlideState")
			player.clear_dash_charge()
		else:
			parent_state_machine.transition_to("PlayerCrouchState")
