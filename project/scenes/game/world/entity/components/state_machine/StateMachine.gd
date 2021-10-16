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
	__current_state.physics_update(delta)

func transition_to(state_name: String, message: Dictionary = {}):
	if not has_node(state_name):
		return

	__current_state.exit()
	__current_state = get_node(__current_state)
	__current_state.enter(message)
	emit_signal("transitioned", __current_state.name)
