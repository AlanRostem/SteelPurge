extends MovingEntity
class_name Enemy

var scrap_scene = preload("res://scenes/game/world/entity/entities/item/collectible_items/Scrap.tscn")

onready var __health_component = $HealthComponent
onready var __damage_timer = $DamageAvailabilityTimer

export(int) var scrap_drop_count_damaged = 1
export(int) var scrap_drop_count_eliminated = 10

var __can_deal_damage_to_player = true

func drop_scrap(count):
	randomize()
	var drop_speed = 50
	var scrap = parent_world.spawn_entity_deferred(scrap_scene, position)
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
	drop_scrap(scrap_drop_count_damaged)


func _on_OutHitBox_hit_dealt(hitbox):
	if !__can_deal_damage_to_player: return
	var player = hitbox.get_parent()
	if player is Player:
		player.stats.take_one_damage()

func set_can_deal_damage(value):
	__can_deal_damage_to_player = value

func _on_InHitBox_received_additional_message(message):
	if message.has("ram_slide"):
		set_velocity_x(0)
		set_velocity_y(-Player.RAM_SLIDE_SPEED)
		set_can_deal_damage(false)
		__damage_timer.start()


func _on_DamageAvailabilityTimer_timeout():
	set_can_deal_damage(true)
