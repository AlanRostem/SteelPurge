extends "res://scenes/game/world/entity/components/state_machine/State.gd"

onready var player = parent_state_machine.get_parent()

# Input variables

var move_left = false
var move_right = false

var aim_up = false
var aim_down = false

var jump = false
var crouch = false

func physics_process_input_update(delta):
	move_left = Input.is_action_pressed("move_left")
	move_right = Input.is_action_pressed("move_right")
