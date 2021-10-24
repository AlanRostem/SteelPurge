extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

const MAX_POWER_SLIDE_ENERGY = 100

export(float) var __power_slide_energy_fill_per_second 
export(float) var __power_slide_energy_loss_per_second 

var __power_slide_speed_gauge = 0

func exit():
	__power_slide_speed_gauge = 0

func movement_update(delta):
	if player.is_moving_too_fast(player.max_walk_speed):
		parent_state_machine.transition_to("PlayerSlideState")
		return
	
	if move_left or move_right:
		player.run(int(move_right) - int(move_left), delta)
		__power_slide_speed_gauge = clamp(__power_slide_speed_gauge + __power_slide_energy_fill_per_second * delta, 0, MAX_POWER_SLIDE_ENERGY)
	else:
		__power_slide_speed_gauge = clamp(__power_slide_speed_gauge - __power_slide_energy_loss_per_second * delta, 0, MAX_POWER_SLIDE_ENERGY)
		player.stop_running()
		if player.is_effectively_standing_still():
			parent_state_machine.transition_to("PlayerIdleState")
			player.set_velocity_x(0)
			
	if crouch:
		if player.is_moving_too_fast(player.max_walk_speed / 2) and __power_slide_speed_gauge == MAX_POWER_SLIDE_ENERGY:
			parent_state_machine.transition_to("PlayerSlideState")
		else:
			parent_state_machine.transition_to("PlayerCrouchState")
