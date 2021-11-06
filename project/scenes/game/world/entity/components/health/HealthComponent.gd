extends Node

const STANDARD_HEALTH = 10

signal damage_taken(damage)
signal health_depleted(health_left)

export(int) var max_health = STANDARD_HEALTH

onready var __health = max_health

func get_health():
	return __health

func take_damage(damage):
	if __health > damage:
		__health -= damage
		emit_signal("damage_taken", damage)
	else:
		emit_signal("health_depleted", __health)
		__health = 0
