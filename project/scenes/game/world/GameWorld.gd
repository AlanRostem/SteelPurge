# This node contains all world collision and an entity pool. Spawning/despawning of
# entities is managed by this node, but are attached to the entity pool node

extends Node2D

onready var __entity_pool = $EntityPool

onready var __tile_map = $CustomTileMap


# Retrieve the pixel cell size for the tile map
func get_tile_size():
	return __tile_map.get_tile_size()

# Instance a node that inherits the base entity scene through a specified scene and
# specify a relative location for the entity to be present. The node is then added as 
# a child of the EntityPool node.
func spawn_entity(entity_scene, location):
	var entity = entity_scene.instance()
	entity.position = location
	entity.parent_world = self
	__entity_pool.add_child(entity)
	return entity
	
# Instance a node that inherits the base entity scene through a specified scene and
# specify a relative location for the entity to be present. The node is then added as 
# a child of the EntityPool node. The adding is deferred. 
func spawn_entity_deferred(entity_scene, location):
	var entity = entity_scene.instance()
	entity.position = location
	entity.parent_world = self
	__entity_pool.call_deferred("add_child", entity)
	return entity

# func move_entity_to_world(entity, world):
#	pass
