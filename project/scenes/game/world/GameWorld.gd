# This node contains all world collision and an entity pool. Spawning/despawning of
# entities is managed by this node, but are attached to the entity pool node

extends Node2D

class_name GameWorld

const SOLID_OBJECT_COLLISION_BIT = 0

var __hover_text_scene = preload("res://scenes/ui/world/hover_text/WorldHoverText.tscn")

onready var __entity_pool = $EntityPool

onready var __tile_map = $Geometry/CustomTileMap

onready var __parent_level = get_parent()

var player_node

onready var __scrap_hover_text = $ScrapHoverText
onready var __weapon_hover_text = $WeaponHoverText

var __scrap_recently_collected = 0

func _ready():
	var player = __entity_pool.get_node_or_null("Player")
	if player != null:
		assign_player_node(player)

func assign_player_node(player):
	player_node = player
	__parent_level.player_node = player

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
#	if entity is Player:
#		assign_player_node(entity)
	return entity
	
# Instance a node that inherits the base entity scene through a specified scene and
# specify a relative location for the entity to be present. The node is then added as 
# a child of the EntityPool node. The adding is deferred. 
func spawn_entity_deferred(entity_scene, location):
	var entity = entity_scene.instance()
	entity.position = location
	entity.parent_world = self
	__entity_pool.call_deferred("add_child", entity)
#	if entity is Player:
#		assign_player_node(entity)
	return entity

# func move_entity_to_world(entity, world):
#	pass

func show_effect_deferred(scene, location):
	var effect = scene.instance()
	effect.position = location
	__entity_pool.call_deferred("add_child", effect)
	
func show_hover_scrap_collected(amount, location):
	__scrap_recently_collected += amount
	__scrap_hover_text.display("+" + str(__scrap_recently_collected), location)

func show_hover_weapon_collected(weapon_name, location):
	__weapon_hover_text.display("+" + weapon_name, location)

func _on_ScrapHoverText_display_off():
	__scrap_recently_collected = 0
