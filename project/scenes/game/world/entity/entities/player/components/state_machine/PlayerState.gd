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
	# Testing how Godot input works
	if event is InputEventKey:
		if event.is_action_pressed("move_left"):
			move_left = true
		elif event.is_action_released("move_left"):
			move_left = false
			
		if event.is_action_pressed("move_right"):
			move_right = true
		elif event.is_action_released("move_right"):
			move_right = false
