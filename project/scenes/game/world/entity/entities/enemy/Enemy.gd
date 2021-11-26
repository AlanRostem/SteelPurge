extends MovingEntity
class_name Enemy

signal player_detected(player)
signal player_visual_lost(player)

var scrap_scene = preload("res://scenes/game/world/entity/entities/item/collectible_items/Scrap.tscn")

onready var __health_component = $HealthComponent
onready var __damage_timer = $DamageAvailabilityTimer
onready var state_machine = $EnemyFSM

export(int) var scrap_drop_count_damaged = 1
export(int) var scrap_drop_count_eliminated = 10
export(float) var player_detection_range_in_tiles = 5 

var __can_deal_damage_to_player = true
var __is_player_seen = false
var __horizontal_player_detect_direction = -1
var horizontal_looking_direction = -1

func _physics_process(delta):
	var player = parent_world.player_node
	if player == null: return
	var diff = player.position.x - position.x
	__horizontal_player_detect_direction = sign(diff)
	if abs(diff) < player_detection_range_in_tiles * 8:
		if !__is_player_seen:
			__is_player_seen = true
			emit_signal("player_detected", player)
	elif __is_player_seen:
		__is_player_seen = false
		emit_signal("player_visual_lost", player)

func get_horizontal_player_detect_direction():
	return __horizontal_player_detect_direction

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
