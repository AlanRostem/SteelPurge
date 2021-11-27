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
		
	if __is_slamming:
		__is_slamming = false
		manually_end_attack_cycle()

func _on_OutHitBox_hit_dealt(hitbox):
	var damage = STANDARD_DAMAGE
	if __boost_damage:
		__boost_damage = false
		damage = CRITICAL_DAMAGE
	hitbox.take_hit(__hit_box, damage, {
		"knock_back": true
	})

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
	__hit_box_shape.disabled = false
	var player = get_owner_player()
	if player.is_on_ground():
		player.set_velocity_x(0)
	player.set_can_move_on_ground(false)
	player.set_velocity_y(SLAM_SPEED)
	__boost_damage = true
	__is_slamming = true
