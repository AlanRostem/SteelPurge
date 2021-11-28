extends "res://scenes/game/world/structure/DestructibleStructure.gd"

var __break_effect_scene = preload("res://scenes/game/world/other/CrateBreakEffect.tscn")

export(PackedScene) var __containment_scene

var __collectible
var __player_nearby

func _process(delta):
	if __player_nearby != null and !__player_nearby.stats.has_weapon() and  Input.is_action_just_pressed("fire"):
		destroy_self()
		__player_nearby.set_opening_crate(true)

func _destroyed():
	__collectible = parent_world.spawn_entity_deferred(__containment_scene, position)
	parent_world.show_effect_deferred(__break_effect_scene, position)

func _on_InHitBox_received_additional_message(message):
	if message.has("ram_slide"):
		call_deferred("__set_collectible_y_speed")
		
func __set_collectible_y_speed():
	__collectible.set_velocity_y(-Player.RAM_SLIDE_SPEED)


func _on_PlayerDetector_body_entered(body):
	__player_nearby = body


func _on_PlayerDetector_body_exited(body):
	__player_nearby = null
