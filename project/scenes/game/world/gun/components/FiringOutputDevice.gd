extends Node

onready var parent_weapon = get_parent()

func _on_Gun_fired():
	pass

func fire_projectile(scene, dir_vec):
	var player = parent_weapon.owner_player
	var world = parent_weapon.owner_player.parent_world
	var projectile = world.spawn_entity_deferred(scene, player.position)
	projectile.init_from_player_weapon(dir_vec, self)
	
func fire_projectiles_in_shot_gun_pattern(scene, dir_vec, pellet_count):
	pass
