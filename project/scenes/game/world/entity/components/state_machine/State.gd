# State object as used by the StateMachine node. This can be extended and it 
# contains virtual functions for entering/exiting the state, input and a physics
# update that is called inside the parent state machine's _physics_process method.

extends Node

onready var parent_state_machine = get_parent()

func enter(message: Dictionary):
	pass
	
func exit():
	pass
	
func input_update(event: InputEvent):
	pass
	
func physics_process_input_update(delta):
	pass
	
func physics_update(delta):
	pass
