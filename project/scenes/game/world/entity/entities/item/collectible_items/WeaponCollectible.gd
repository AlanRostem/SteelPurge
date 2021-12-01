extends "res://scenes/game/world/entity/entities/item/CollectibleItem.gd"

export(PackedScene) var __weapon_scene

onready var __sprite = $Sprite

onready var __pick_up_timer = $PickUpAvailabilityTimer

onready var __detection_shape = $PlayerDetectionArea/BodyShape

var weapon

func _player_collected(player):
	if weapon != null:
		player.stats.equip_weapon(weapon)
		parent_world.show_hover_weapon_collected(weapon.name, position)
		return
	player.stats.instance_and_equip_weapon(__weapon_scene)
	parent_world.show_hover_weapon_collected(player.stats.get_equipped_weapon().name, position)
	
func set_sprite(sprite):
	__sprite.texture = sprite

func set_recently_dropped(value):
	if value:
		call_deferred("__set_life_timer_wait_time_to_low")
		
func __set_life_timer_wait_time_to_low():
	_life_timer.start(3.5)
	__pick_up_timer.start()
	__detection_shape.disabled = true

func _on_PickUpAvailabilityTimer_timeout():
	__detection_shape.disabled = false
