extends "res://scenes/game/world/environment/Shop.gd"

export(PackedScene) var __weapon_scene

func _purchase_response(player):
	player.stats.instance_and_equip_weapon(__weapon_scene)

