extends "res://scenes/game/world/entity/components/state_machine/State.gd"

onready var player = parent_state_machine.get_parent()

export var is_grounded_state = false

# Input variables

var move_left = false
var move_right = false

var aim_up = false
var aim_down = false

var jump = false
var cancel_jump = false
var crouch = false

func physics_process_input_update(delta):
	move_left = Input.is_action_pressed("move_left")
	move_right = Input.is_action_pressed("move_right")
	jump = Input.is_action_just_pressed("jump")
	cancel_jump = Input.is_action_just_released("jump")
	crouch = Input.is_action_pressed("crouch")

func physics_update(delta):
	movement_update(delta)
#	print(name)
	
	if player.is_on_wall():
		player.set_velocity_x(0)
	
	if is_grounded_state:
		if player.is_on_ground():
			if jump:
				parent_state_machine.transition_to("PlayerAirBorneState", {
					"jumping": true
				})
		else:
			parent_state_machine.transition_to("PlayerAirBorneState")
		
	
func movement_update(delta):
	pass
