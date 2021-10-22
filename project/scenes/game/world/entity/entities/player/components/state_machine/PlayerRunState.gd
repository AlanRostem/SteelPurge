extends "res://scenes/game/world/entity/entities/player/components/state_machine/PlayerState.gd"

var __can_slide = true

func enter(message):
	if message.has("just_landed"):
		__can_slide = false

func movement_update(delta):
	if move_left or move_right:
		player.run(int(move_right) - int(move_left), delta)
	else:
		player.stop_running()
		if player.is_effectively_standing_still():
			parent_state_machine.transition_to("PlayerIdleState")
			player.set_velocity_x(0)
			
	if !crouch:
		__can_slide = true
			
	if crouch:
		if player.is_moving_too_fast(player.max_walk_speed / 2) and __can_slide:
			parent_state_machine.transition_to("PlayerSlideState", {
				"boost": true
			})
		else:
			parent_state_machine.transition_to("PlayerCrouchState")
