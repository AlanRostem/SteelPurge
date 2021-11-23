extends "res://scenes/game/world/weapon/Weapon.gd"

var __projectile_scene = preload("res://scenes/game/world/entity/entities/projectile/projectiles/EnergyBlast.tscn")

func _on_Blaster_attacked():
	fire_projectile(__projectile_scene)

func fire_projectile(scene):
	var player = get_owner_player()
	var world = player.parent_world
	var projectile = world.spawn_entity_deferred(scene, player.position)
	
	var dir = player.get_looking_vector()
	var offset = Vector2(6, -3)
	
	if dir.x != 0:
		offset.x *= dir.x
		
	if dir.y != 0:
		offset.x = 0
		offset.y = 6
		
	projectile.call_deferred("init_from_player_weapon", dir, self, offset)


func _on_Blaster_downwards_attack():
	fire_projectile(__projectile_scene)
	
	var player = get_owner_player()
	if player.stats.get_rush_energy() > 0:
		player.recoil_boost(35)
		player.stats.use_rush_energy(1)
