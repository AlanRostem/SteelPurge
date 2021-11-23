extends "res://scenes/game/world/entity/entities/item/CollectibleItem.gd"

export(PackedScene) var __weapon_scene

onready var __sprite = $Sprite

var weapon

func _player_collected(player):
	if weapon != null:
		player.stats.equip_weapon(weapon)
		return
	player.stats.instance_and_equip_weapon(__weapon_scene)
	
func set_sprite(sprite):
	__sprite.texture = sprite
