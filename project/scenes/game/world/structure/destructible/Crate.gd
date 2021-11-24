extends "res://scenes/game/world/structure/DestructibleStructure.gd"

export(PackedScene) var __containment_scene

var __collectible

func _destroyed():
	__collectible = parent_world.spawn_entity_deferred(__containment_scene, position)

func _on_InHitBox_received_additional_message(message):
	if message.has("ram_slide"):
		call_deferred("__set_collectible_y_speed")
		
func __set_collectible_y_speed():
	__collectible.set_velocity_y(-Player.RAM_SLIDE_SPEED)
