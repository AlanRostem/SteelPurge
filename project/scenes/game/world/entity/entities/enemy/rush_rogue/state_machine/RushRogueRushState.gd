extends "res://scenes/game/world/entity/entities/enemy/rush_rogue/state_machine/RushRogueState.gd"

var __rush = false
onready var __rush_timer = $RushTimer

func physics_update(delta):
	if __rush:
		rush_rogue.set_velocity_x(rush_rogue.get_horizontal_player_detect_direction() * MAX_RUSH_SPEED)

func _on_RushRogue_player_detected(player):
	parent_state_machine.transition_to(name)

func exit():
	__rush = false
	__rush_timer.stop()
	
func _on_RushTimer_timeout():
	__rush = true
