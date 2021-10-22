# Finite state machine for game objects such as the player, enemies and more.
# This uses a node path for the initial state which must be specified for 
# everything to work. The state objects are added simply by adding them to this
# node.

extends Node

signal transitioned(state_name)

export(NodePath) var __inital_state

onready var __current_state = get_node(__inital_state)
var __previous_state

onready var parent_entity = get_parent()

func get_current_state():
	return __current_state.name

func _unhandled_input(event):
	__current_state.input_update(event)

func _physics_process(delta):
	__current_state.physics_process_input_update(delta)
	__current_state.physics_update(delta)

func transition_to(state_name: String, message: Dictionary = {}):
	if not has_node(state_name):
		return

	__current_state.exit()
	__current_state = get_node(state_name)
	__current_state.enter(message)
	emit_signal("transitioned", __current_state.name)
