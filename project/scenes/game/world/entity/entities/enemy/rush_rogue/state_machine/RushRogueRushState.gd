extends "res://scenes/game/world/entity/entities/enemy/rush_rogue/state_machine/RushRogueState.gd"

onready var __rush_timer = $RushTimer

func physics_update(delta):
	if rush_rogue.is_charging_up():
		rush_rogue.horizontal_looking_direction = rush_rogue.get_horizontal_player_detect_direction()
	elif rush_rogue.is_on_wall():
		rush_rogue.set_charging_up(true)
		__rush_timer.start()
	
func _on_RushRogue_player_detected(player):
	parent_state_machine.transition_to(name)

func enter(message: Dictionary):
	rush_rogue.set_charging_up(true)
	rush_rogue.set_velocity_x(0)
	__rush_timer.start()

func exit():
	__rush_timer.stop()
	rush_rogue.set_charging_up(false)
	
func _on_RushTimer_timeout():
	rush_rogue.set_velocity_x(rush_rogue.get_horizontal_player_detect_direction() * MAX_RUSH_SPEED)
	rush_rogue.set_charging_up(false)
