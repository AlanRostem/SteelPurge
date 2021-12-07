extends "res://scenes/game/world/entity/entities/enemy/brute/state_machine/BruteState.gd"

func enter(message: Dictionary):
	if brute.can_see_player():
		parent_state_machine.transition_to("BruteSlamState")

func physics_update(delta):
	brute.set_velocity_x(brute.get_horizontal_player_detect_direction() * WALK_SPEED)

func _on_Brute_player_visual_lost(player):
	parent_state_machine.transition_to(name)
