extends "res://scenes/game/world/gun/components/FiringOutputDevice.gd"

export(float) var scan_length = 100

var __ray_cast = RayCast2D.new()

func _ready():
	__ray_cast.collide_with_areas = true
	__ray_cast.set_collision_mask_bit(HitBox.HIT_BOX_IN_COLLISION_BIT, true)
	__ray_cast.set_collision_mask_bit(GameWorld.SOLID_OBJECT_COLLISION_BIT, true)
	__ray_cast.exclude_parent = true
	parent_weapon.call_deferred("add_child", __ray_cast)

func _on_Gun_fired():
	var player = parent_weapon.owner_player
	var world = parent_weapon.owner_player.parent_world
	__ray_cast.cast_to = player.get_looking_vector() * scan_length
	__ray_cast.force_raycast_update()
	if !__ray_cast.is_colliding(): return
	var collider = __ray_cast.get_collider()
	if collider is HitBox:
		if collider.get_team() != player.hit_box.get_team():
			# Using the player's hitbox since the raycast is not a hitbox.
			# This may cause future bugs
			collider.take_hit(player.hit_box, parent_weapon.damage_per_shot)
	#elif collider is TileMap:
	#	pass
