# Finite state machine for game objects such as the player, enemies and more.
# This uses a node path for the initial state which must be specified for 
# everything to work. The state objects are added simply by adding them to this
# node.

extends Node

signal transitioned(state_name)

export(NodePath) var __inital_state

export var init_state_on_ready = true

var __current_state
var __previous_state

func _ready():
	if init_state_on_ready and __inital_state != null and __inital_state != "":
		__current_state = get_node(__inital_state)

onready var parent_entity = get_parent()

func get_current_state():
	if __current_state == null: return ""
	return __current_state.name

func _unhandled_input(event):
	if __current_state == null: return
	__current_state.input_update(event)

func _physics_process(delta):
	if __current_state == null: return
	__current_state.physics_process_input_update(delta)
	__current_state.physics_update(delta)

func transition_to(state_name: String, message: Dictionary = {}):
	if not has_node(state_name):
		return false

	if __current_state != null:
		__current_state.exit()
	__current_state = get_node(state_name)
	__current_state.enter(message)
	emit_signal("transitioned", __current_state.name)
	return true

func get_initial_state():
	return __inital_state
