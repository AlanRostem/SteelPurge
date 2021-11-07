extends "res://scenes/game/world/entity/entities/item/collectible_items/Scrap.gd"


func _player_collected(player):
	player.stats.add_healing_scrap(amount_per_collect)
