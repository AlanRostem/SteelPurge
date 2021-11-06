extends MovingEntity
class_name Enemy

onready var __health_component = $HealthComponent

func die():
	# TODO: Implement further
	queue_free()

func _on_HealthComponent_health_depleted(health_left):
	die()

func _on_InHitBox_hit_received(hitbox, damage):
	__health_component.take_damage(damage)
