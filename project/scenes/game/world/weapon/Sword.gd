extends "res://scenes/game/world/weapon/Weapon.gd"

const CRITICAL_DAMAGE = 24
const STANDARD_DAMAGE = 8

onready var __hit_box = $OutHitBox
onready var __hit_box_shape = $OutHitBox/CollisionShape2D

func _physics_process(delta):
	var player = get_owner_player()
	__hit_box.scale.x = player.get_horizontal_looking_dir()

func _on_OutHitBox_hit_dealt(hitbox):
	hitbox.take_hit(__hit_box, STANDARD_DAMAGE, {
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
