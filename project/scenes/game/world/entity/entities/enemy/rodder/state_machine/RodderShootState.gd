extends "res://scenes/game/world/entity/entities/enemy/rodder/state_machine/RodderState.gd"

onready var __shoot_timer = $ShootTimer

const MAX_SHOTS = 3

var __shots = 0

func physics_update(delta):
	rodder.horizontal_looking_direction = rodder.get_horizontal_player_detect_direction()
		
func enter(mesage: Dictionary):
	__shoot_timer.start()

func shoot():
	__shots += 1
	print("bang")

func _on_ShootTimer_timeout():
	if __shots == MAX_SHOTS:
		__shots = 0
		parent_state_machine.transition_to("RodderWalkState")
		__shoot_timer.stop()
		return
	shoot()
