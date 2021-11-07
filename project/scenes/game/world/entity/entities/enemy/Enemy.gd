extends MovingEntity
class_name Enemy

var scrap_scene = preload("res://scenes/game/world/entity/entities/item/collectible_items/Scrap.tscn")
var healing_scrap_scene = preload("res://scenes/game/world/entity/entities/item/collectible_items/HealingScrap.tscn")

onready var __health_component = $HealthComponent

export(int) var scrap_drop_count_damaged = 1
export(int) var scrap_drop_count_eliminated = 10
export(float) var healing_scrap_drop_after_health_percentage = .5

func drop_scrap(count):
	randomize()
	var drop_speed = 50
	var scrap = parent_world.spawn_entity_deferred(scrap_scene, position)
	scrap.amount_per_collect = count
	scrap.set_velocity(Vector2(
		rand_range(-drop_speed, drop_speed), -drop_speed
	))
	
func drop_healing_scrap(count):
	randomize()
	var drop_speed = 50
	var scrap = parent_world.spawn_entity_deferred(healing_scrap_scene, position)
	scrap.amount_per_collect = count
	scrap.set_velocity(Vector2(
		rand_range(-drop_speed, drop_speed), -drop_speed
	))

func die():
	# TODO: Implement further
	queue_free()
	drop_scrap(scrap_drop_count_eliminated)

func _on_HealthComponent_health_depleted(health_left):
	die()

func _on_InHitBox_hit_received(hitbox, damage):
	__health_component.take_damage(damage)

func _on_HealthComponent_damage_taken(damage):
	if __health_component.get_health() > __health_component.max_health * healing_scrap_drop_after_health_percentage:
		drop_scrap(scrap_drop_count_damaged)
	else:
		drop_healing_scrap(scrap_drop_count_damaged)


func _on_OutHitBox_hit_dealt(hitbox):
	var player = hitbox.get_parent()
	if player is Player:
		player.stats.take_one_damage()
