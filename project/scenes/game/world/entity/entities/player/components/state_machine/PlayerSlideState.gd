extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

func movement_update(delta):
	if player.is_roof_above(): return

	var intended_dir = int(move_right) - int(move_left)
	var move_dir = sign(player.get_velocity().x)
	if intended_dir != move_dir:
		player.negate_slide(intended_dir, delta)
		
	player.apply_slide_friction(delta)
	
	if !crouch:
		if player.is_effectively_standing_still():
			parent_state_machine.transition_to("PlayerIdleState")
			return
		elif !player.is_moving_too_fast(player.max_dash_speed):
			parent_state_machine.transition_to("PlayerRunState")
			return
		elif !player.is_moving_too_fast(player.max_walk_speed):
			parent_state_machine.transition_to("PlayerWalkState")
			return
	elif !player.is_moving_too_fast(player.max_walk_speed):
		parent_state_machine.transition_to("PlayerCrouchState")
		return
		
	if jump and player.is_on_ground():
		parent_state_machine.transition_to("PlayerAirBorneState", {
			"jumping": true
		})
		
	if !player.is_on_ground():
		parent_state_machine.transition_to("PlayerAirBorneState")

func enter(message):
	if message.has("boost"):
		if player.stats.get_rush_energy() >= 2:
			player.stats.use_rush_energy(2)
			player.slide(player.moving_direction)
			player.set_ram_slide_hit_box_enabled(true)
	player.clear_dash_charge()
	player.crouch()
	player.collision_mode = MovingEntity.CollisionModes.SLIDE
	
func exit():
	player.stand_up()
	player.collision_mode = MovingEntity.CollisionModes.SNAP
	player.set_ram_slide_hit_box_enabled(false)
