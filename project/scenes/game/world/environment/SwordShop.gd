extends "res://scenes/game/world/environment/WeaponShop.gd"

func _purchase_condition(player):
	return player.stats.get_weapon().name != "Sword"
