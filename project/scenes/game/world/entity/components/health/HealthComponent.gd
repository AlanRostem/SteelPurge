extends Node

const STANDARD_HEALTH = 10

signal damage_taken(damage)
signal health_depleted(health_left)

export(int) var max_health = STANDARD_HEALTH

onready var __health = max_health

func _ready():
	var parent = get_parent()
	assert(parent is HitBox)
	assert(parent.hit_box_action_type == HitBox.HitBoxActionType.HIT_RECEIVER)
	parent.connect("hit_received", self, "_on_parent_hit_box_hit_received")

func get_health():
	return __health

func take_damage(damage):
	if __health > damage:
		__health -= damage
		emit_signal("damage_taken", damage)
	else:
		emit_signal("health_depleted", __health)
		__health = 0

func _on_parent_hit_box_hit_received(hitbox, damage):
	take_damage(damage)
