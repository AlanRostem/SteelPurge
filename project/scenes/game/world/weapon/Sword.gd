extends "res://scenes/game/world/weapon/Weapon.gd"

const CRITICAL_DAMAGE = 24
const STANDARD_DAMAGE = 8

onready var __hit_box = $OutHitBox

func _on_OutHitBox_hit_dealt(hitbox):
	hitbox.take_hit(__hit_box, STANDARD_DAMAGE, {
		"knock_back": true
	})


func _on_Sword_attacked():
	var player = get_owner_player()
	if player.is_on_ground():
		player.set_velocity_x(0)
