extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

func movement_update(delta):
	if (move_left or move_right) and player.can_move_on_ground():
		var dir = int(move_right) - int(move_left)
		if dir != 0:
			player.look_horizontally(dir)
		player.walk(dir, delta)

		if player.is_moving_approximately_at_speed(player.max_walk_speed):
			parent_state_machine.transition_to("PlayerRunState")
			return
	else:
		player.reduce_dash_charge(delta)
		player.stop_running()
		if player.is_effectively_standing_still():
			parent_state_machine.transition_to("PlayerIdleState")
			player.set_velocity_x(0)
			player.clear_dash_charge()
		
	if crouch:
		if !player.is_effectively_standing_still():
			parent_state_machine.transition_to("PlayerSlideState")
			player.clear_dash_charge()
		else:
			parent_state_machine.transition_to("PlayerCrouchState")
