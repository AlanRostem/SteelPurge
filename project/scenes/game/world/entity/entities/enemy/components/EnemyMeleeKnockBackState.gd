extends "res://scenes/game/world/entity/entities/enemy/components/EnemyState.gd"

const KNOCK_BACK_SPEED = 100

onready var __knock_back_timer = $KnockBackTimer

func enter(message: Dictionary):
	if message.has("direction"):
		var dir = message["direction"]
		enemy.set_velocity_x(dir * KNOCK_BACK_SPEED)
	__knock_back_timer.start()


func _on_KnockBackTimer_timeout():
	parent_state_machine.transition_to(parent_state_machine.get_initial_state())
