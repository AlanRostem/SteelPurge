extends "res://scenes/game/world/environment/WeaponShop.gd"

func _purchase_condition(player):
	if !player.stats.has_weapon(): return true
	return player.stats.get_weapon().name != "Blaster"
