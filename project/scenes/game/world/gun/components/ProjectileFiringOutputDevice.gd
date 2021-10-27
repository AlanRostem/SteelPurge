extends "res://scenes/game/world/gun/components/FiringOutputDevice.gd"

export(PackedScene) var projectile_scene

func _on_Gun_fired():
	var player = parent_weapon.owner_player
	fire_projectile(projectile_scene, player.looking_vector)

func fire_projectile(scene, dir_vec):
	var player = parent_weapon.owner_player
	var world = parent_weapon.owner_player.parent_world
	var projectile = world.spawn_entity_deferred(scene, player.position)
	projectile.init_from_player_weapon(dir_vec, self)
