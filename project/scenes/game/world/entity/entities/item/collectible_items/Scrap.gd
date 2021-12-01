extends "res://scenes/game/world/entity/entities/item/CollectibleItem.gd"

export(int) var amount_per_collect = 5

func _player_collected(player):
	player.stats.add_scrap(amount_per_collect)
	parent_world.show_hover_scrap_collected(amount_per_collect, position + Vector2.UP * 16)
