extends "res://scenes/game/world/structure/Structure.gd"

var scrap_scene = preload("res://scenes/game/world/entity/entities/item/collectible_items/Scrap.tscn")

export(bool) var drop_scrap_on_damaged = true

export(int) var scrap_drop_count_damaged = 1

onready var __health_component = $HealthComponent
onready var __scrap_drop_location = $ScrapDropLocation

func drop_scrap(count):
	randomize()
	var drop_speed = 50
	var scrap = parent_world.spawn_entity_deferred(scrap_scene, position + __scrap_drop_location.position)
	scrap.amount_per_collect = count
	scrap.set_velocity(Vector2(
		rand_range(-drop_speed, drop_speed), -drop_speed
	))

func _on_InHitBox_hit_received(hitbox, damage, damage_type):
	__health_component.take_damage(damage, damage_type)
	if drop_scrap_on_damaged:
		drop_scrap(scrap_drop_count_damaged)

func destroy_self():
	_destroyed()
	queue_free()

func _on_HealthComponent_health_depleted(health_left):
	destroy_self()

func _destroyed():
	pass
