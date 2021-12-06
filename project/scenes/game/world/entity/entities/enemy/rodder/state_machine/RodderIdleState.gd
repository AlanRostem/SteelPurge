extends "res://scenes/game/world/entity/entities/enemy/rodder/state_machine/RodderState.gd"

func physics_update(delta):
	rodder.set_velocity_x(0)

func _on_Rodder_player_visual_lost(player):
	parent_state_machine.transition_to(name)
