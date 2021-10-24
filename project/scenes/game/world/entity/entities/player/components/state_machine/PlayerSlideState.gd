extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

func movement_update(delta):
	var intended_dir = int(move_right) - int(move_left)
	var move_dir = sign(player.get_velocity().x)
	if intended_dir != move_dir:
		player.negate_slide(intended_dir, delta)
	player.apply_slide_friction(delta)
	if !player.is_moving_too_fast(player.max_walk_speed):
		if player.is_effectively_standing_still():
			if crouch:
				parent_state_machine.transition_to("PlayerCrouchState")
			else:
				parent_state_machine.transition_to("PlayerIdleState")
		else:
			parent_state_machine.transition_to("PlayerRunState")
		
	if jump and player.is_on_ground():
		parent_state_machine.transition_to("PlayerAirBorneState", {
			"jumping": true
		})
		
	if !player.is_on_ground():
		parent_state_machine.transition_to("PlayerAirBorneState")

func enter(message):
	if !player.is_moving_too_fast(player.max_walk_speed):
		player.slide(player.moving_direction)
	player.crouch()
	player.collision_mode = MovingEntity.CollisionModes.SLIDE
	
func exit():
	if !crouch:
		player.stand_up()
	player.collision_mode = MovingEntity.CollisionModes.SNAP
