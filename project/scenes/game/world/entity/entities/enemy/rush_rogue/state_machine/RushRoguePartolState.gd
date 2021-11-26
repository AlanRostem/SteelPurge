extends "res://scenes/game/world/entity/entities/enemy/rush_rogue/state_machine/RushRogueState.gd"

func physics_update(delta):
	rush_rogue.set_velocity_x(-MAX_PATROL_SPEED)

func _on_RushRogue_player_visual_lost(player):
	parent_state_machine.transition_to(name)
