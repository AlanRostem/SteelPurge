extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

func movement_update(delta):
	player.apply_slide_friction(delta)
	if player.is_effectively_standing_still():
		player.set_velocity_x(0)
			
	player.reduce_dash_charge(delta)
	
	var dir = int(move_right) - int(move_left)
	if dir != 0:
		player.looking_vector.x = dir
			
	if jump:
		parent_state_machine.transition_to("PlayerAirborneState", {
			"jumping": true
		})
		return
			
	if !crouch:
		if player.is_effectively_standing_still():
			parent_state_machine.transition_to("PlayerIdleState")
		else:
			parent_state_machine.transition_to("PlayerRunState")

func enter(message):
	player.crouch()
	player.collision_mode = MovingEntity.CollisionModes.SLIDE
	
func exit():
	player.stand_up()
	player.collision_mode = MovingEntity.CollisionModes.SNAP
