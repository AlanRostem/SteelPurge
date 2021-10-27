extends "res://scenes/game/world/gun/components/ProjectileFiringOutputDevice.gd"

func fire_projectiles_in_shot_gun_pattern(scene, dir_vec, spread_angle, pellet_count):
	var player = parent_weapon.owner_player
	var world = parent_weapon.owner_player.parent_world
	
	var offsetAngle = dir_vec.angle() - spread_angle / 2
	for i in range(pellet_count):
		var projectile = world.spawn_entity_deferred(scene, player.position)
		projectile.init_from_player_weapon(Vector2(sin(offsetAngle), cos(offsetAngle)), self)
		offsetAngle += spread_angle / float(pellet_count)
