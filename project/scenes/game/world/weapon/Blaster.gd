extends "res://scenes/game/world/weapon/Weapon.gd"

func _on_Blaster_attacked():
	pass # Replace with function body.

func fire_projectile(scene, dir_vec):
	var player = get_owner_player()
	var world = player.parent_world
	var projectile = world.spawn_entity_deferred(scene, player.position)
	projectile.init_from_player_weapon(dir_vec, self)
