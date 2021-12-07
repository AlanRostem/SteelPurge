extends "res://scenes/game/world/entity/components/state_machine/StateMachine.gd"

func _on_VisibilityEnabler2D_screen_entered():
	transition_to(get_initial_state())


func _on_VisibilityEnabler2D_screen_exited():
	transition_to("EnemyIdleState")
