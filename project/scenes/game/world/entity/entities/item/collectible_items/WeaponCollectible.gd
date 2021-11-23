extends "res://scenes/game/world/entity/entities/item/CollectibleItem.gd"

export(PackedScene) var __weapon_scene

func _player_collected(player):
	player.stats.instance_and_equip_weapon(__weapon_scene)
