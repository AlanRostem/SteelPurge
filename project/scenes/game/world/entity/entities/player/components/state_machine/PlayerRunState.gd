extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

func physics_update(delta):
	var player = parent_state_machine.parent_entity
	if !move_left and !move_right:
		player.stop_running()
		if player.get_velocity().x == 0:
			parent_state_machine.transition_to("PlayerIdleState")
		return
	player.run(int(move_right) - int(move_left), delta)
