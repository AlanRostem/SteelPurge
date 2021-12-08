extends "res://scenes/game/world/environment/Shop.gd"

		
func _purchase_condition(player):
	return player.stats.get_health() < player.stats.MAX_HEALTH
		
func _purchase_response(player):
	player.stats.add_one_health()
	print(player.stats.get_health())
