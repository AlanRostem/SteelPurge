extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

func movement_update(delta):
	# player.slide_move(int(move_right) - int(move_left), delta)
	player.apply_slide_friction(delta)
	if !player.is_moving_too_fast(player.max_walk_speed):
		if !player.is_effectively_standing_still():
			if crouch:
				parent_state_machine.transition_to("PlayerCrouchState")
			else:
				parent_state_machine.transition_to("PlayerRunState")
		else:
			parent_state_machine.transition_to("PlayerIdleState")
			
	if jump:
		parent_state_machine.transition_to("PlayerAirBorneState", {
			"jumping": true
		})

func enter(message):
	if !player.is_moving_too_fast(player.max_walk_speed) and message.has("boost"):
		player.slide(player.moving_direction)
	player.crouch()
	player.collision_mode = MovingEntity.CollisionModes.SLIDE
	
func exit():
	player.stand_up()
	player.collision_mode = MovingEntity.CollisionModes.SNAP
