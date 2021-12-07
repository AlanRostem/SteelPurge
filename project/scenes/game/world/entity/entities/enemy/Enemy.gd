extends MovingEntity
class_name Enemy

signal player_detected(player)
signal player_visual_lost(player)

var scrap_scene = preload("res://scenes/game/world/entity/entities/item/collectible_items/Scrap.tscn")

onready var __health_component = $HealthComponent
onready var state_machine = $EnemyFSM
onready var __damage_taken_timer = $DamageTakenTimer
onready var __sprite = $EnemySprite

export(int) var scrap_drop_count_damaged = 1
export(int) var scrap_drop_count_eliminated = 10
export(float) var player_detection_range_in_tiles = 5 
export(bool) var can_be_knocked_back = true 
export(bool) var detect_player_on_visible = false 

var __can_deal_damage_to_player = true
var __is_player_seen = false
var __horizontal_player_detect_direction = -1
var horizontal_looking_direction = -1

func _physics_process(delta):
	var player = parent_world.player_node
	if player == null: return
	var diff = player.position.x - position.x
	__horizontal_player_detect_direction = sign(diff)
	if detect_player_on_visible: return
	if abs(diff) < player_detection_range_in_tiles * 8:
		if !__is_player_seen:
			__is_player_seen = true
			emit_signal("player_detected", player)
	elif __is_player_seen:
		__is_player_seen = false
		emit_signal("player_visual_lost", player)
		
func can_see_player():
	return __is_player_seen

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

func _on_InHitBox_hit_received(hitbox, damage, damage_type):
	__health_component.take_damage(damage, damage_type)

func _on_HealthComponent_damage_taken(damage):
	drop_scrap(scrap_drop_count_damaged)
	__damage_taken_timer.start()
	__sprite.use_parent_material = false

func _on_OutHitBox_hit_dealt(hitbox):
	if !__can_deal_damage_to_player: return
	var player = hitbox.get_parent()
	if player is Player:
		player.stats.take_one_damage()

func set_can_deal_damage(value):
	__can_deal_damage_to_player = value

func _on_InHitBox_received_additional_message(message: Dictionary):
	if !can_be_knocked_back: return
	if message.has("ram_slide"):
		state_machine.transition_to("EnemyRamIntoAirState")
	elif message.has("knock_back"):
		state_machine.transition_to("EnemyMeleeKnockBackState", {
			"direction": message["knock_back"]
		})

func _on_DamageTakenTimer_timeout():
	__sprite.use_parent_material = true


func _on_VisibilityEnabler2D_screen_entered():
	if !detect_player_on_visible: return
	var player = parent_world.player_node
	if player == null: return
	__is_player_seen = true
	emit_signal("player_detected", player)

func _on_VisibilityEnabler2D_screen_exited():
	if !detect_player_on_visible: return
	var player = parent_world.player_node
	if player == null: return
	__is_player_seen = false
	emit_signal("player_visual_lost", player)
