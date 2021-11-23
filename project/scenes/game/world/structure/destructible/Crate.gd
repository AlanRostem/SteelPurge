extends "res://scenes/game/world/structure/DestructibleStructure.gd"

export(PackedScene) var __containment_scene

func _destroyed():
	parent_world.spawn_entity_deferred(__containment_scene, position)
