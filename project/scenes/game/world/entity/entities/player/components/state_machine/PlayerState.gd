extends "res://scenes/game/world/entity/components/state_machine/State.gd"

onready var player = parent_state_machine.get_parent()

# Input variables

var move_left = false
var move_right = false

var aim_up = false
var aim_down = false

var jump = false
var crouch = false

func input_update(event: InputEvent):
	pass
