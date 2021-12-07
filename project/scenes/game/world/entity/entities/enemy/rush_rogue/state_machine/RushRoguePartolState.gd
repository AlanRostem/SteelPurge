extends "res://scenes/game/world/entity/entities/enemy/rush_rogue/state_machine/RushRogueState.gd"

func enter(message: Dictionary):
	if rush_rogue.can_see_player():
		parent_state_machine.transition_to("RushRogueRushState")

func physics_update(delta):
	if rush_rogue.is_on_wall():
		rush_rogue.horizontal_looking_direction *= -1
	rush_rogue.set_velocity_x(MAX_PATROL_SPEED * rush_rogue.horizontal_looking_direction)

func _on_RushRogue_player_visual_lost(player):
	parent_state_machine.transition_to(name)
