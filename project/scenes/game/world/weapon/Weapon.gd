extends Node2D

signal attacked()
signal downwards_attack()

export(SpriteFrames) var __player_sprite_frames

export(float) var __attack_delay = 0.8

onready var __player_owner = get_parent().get_parent()

onready var __attack_delay_timer = $AttackDelayTimer

var __can_attack = true

func attack():
	if !__can_attack: return
	__attack_delay_timer.start(__attack_delay)
	__can_attack = false
	
	if __player_owner.get_looking_vector().y > 0:
		emit_signal("downwards_attack")
	else:
		emit_signal("attacked")
	
func equip():
	__player_owner.set_sprite_frames(__player_sprite_frames)

func _on_AttackDelayTimer_timeout():
	__can_attack = true
