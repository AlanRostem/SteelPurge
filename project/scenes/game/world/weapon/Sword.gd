extends "res://scenes/game/world/weapon/Weapon.gd"

const CRITICAL_DAMAGE = 24
const STANDARD_DAMAGE = 8
const SLAM_SPEED = 230

onready var __hit_box = $OutHitBox
onready var __hit_box_shape = $OutHitBox/CollisionShape2D

var __boost_damage = false
var __is_slamming = false

func _physics_process(delta):
	var player = get_owner_player()
	__hit_box.scale.x = player.get_horizontal_looking_dir()
	var dir = player.get_looking_vector()
	if dir.y != 0:
		__hit_box.rotation = dir.angle() * player.get_horizontal_looking_dir()
	else:
		__hit_box.rotation = 0
		
	if player.stats.get_rush_energy() > 0:
		use_attacking_delay = !player.is_aiming_down()
	else:
		use_attacking_delay = true
		
	if __is_slamming and player.is_on_ground():
		__is_slamming = false
		manually_end_attack_cycle()

func _on_OutHitBox_hit_dealt(hitbox):
	var parent = hitbox.get_parent()
	var damage = STANDARD_DAMAGE
	if __boost_damage:
		__boost_damage = false
		damage = CRITICAL_DAMAGE
	elif parent is Projectile:
		parent.deflect(HitBox.PLAYER_TEAM, STANDARD_DAMAGE)
	hitbox.take_hit(__hit_box, damage, {
		"knock_back": get_owner_player().get_horizontal_looking_direction()
	}, HealthComponent.DAMAGE_TYPE_MELEE)

func _on_Sword_attacked():
	__hit_box_shape.disabled = false
	var player = get_owner_player()
	if player.is_on_ground():
		player.set_velocity_x(0)
	player.set_can_move_on_ground(false)

func _on_Sword_attack_cycle_end():
	var player = get_owner_player()
	player.set_can_move_on_ground(true)
	__hit_box_shape.disabled = true

func _on_Sword_downwards_attack():
	var player = get_owner_player()
	if player.stats.get_rush_energy() > 0:
		player.stats.use_rush_energy(2)
		__hit_box_shape.disabled = false
		if player.is_on_ground():
			player.set_velocity_x(0)
		player.set_velocity_y(SLAM_SPEED)
		__boost_damage = true
		__is_slamming = true

func _on_Sword_dropped():
	__is_slamming = false
	__boost_damage = false
	__hit_box_shape.disabled = true
	var player = get_owner_player()
	player.set_can_move_on_ground(true)
