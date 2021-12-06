extends Node
class_name HealthComponent

const STANDARD_HEALTH = 10

const DAMAGE_TYPE_STANDARD = "standard"
const DAMAGE_TYPE_ENERGY = "energy"
const DAMAGE_TYPE_MELEE = "melee"
const DAMAGE_TYPE_CRITICAL = "critical"

signal damage_taken(damage)
signal health_depleted(health_left)

export(int) var max_health = STANDARD_HEALTH

export(Array, String) var __immunities

onready var __health = max_health

func get_health():
	return __health

func take_damage(damage, type = DAMAGE_TYPE_STANDARD):
	if type != DAMAGE_TYPE_STANDARD and __immunities.has(type): return
	
	if __health > damage:
		__health -= damage
		emit_signal("damage_taken", damage)
	else:
		emit_signal("health_depleted", __health)
		__health = 0
