extends "res://scenes/game/world/entity/components/state_machine/State.gd"

onready var dummy = get_parent().get_parent()

func physics_update(delta):
	if dummy.is_on_floor():
		dummy.set_velocity_x(0)
