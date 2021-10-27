extends "res://scenes/game/world/gun/components/ProjectileFiringOutputDevice.gd"

export(int) var pellets_per_shot = 5
export(float) var pellet_spread_angle = 15

func _on_Gun_fired():
	var player = parent_weapon.owner_player
	var world = parent_weapon.owner_player.parent_world
	fire_projectiles_in_shot_gun_pattern(projectile_scene, player.looking_vector, deg2rad(pellet_spread_angle), pellets_per_shot)

func fire_projectiles_in_shot_gun_pattern(scene, dir_vec, spread_angle, pellet_count):
	var player = parent_weapon.owner_player
	var world = parent_weapon.owner_player.parent_world
	
	var offsetAngle = dir_vec.angle() - spread_angle / 2
	for i in range(pellet_count):
		var projectile = world.spawn_entity_deferred(scene, player.position)
		projectile.call_deferred("init_from_player_weapon", Vector2(sin(offsetAngle), cos(offsetAngle)), parent_weapon)
		offsetAngle += spread_angle / float(pellet_count)
