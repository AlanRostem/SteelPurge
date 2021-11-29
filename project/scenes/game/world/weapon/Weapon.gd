extends Node2D

signal attacked()
signal attack_cycle_end()
signal downwards_attack()
signal dropped()

export(SpriteFrames) var __player_sprite_frames
export(Texture) var __collectible_sprite

export(bool) var use_attacking_delay = true

var __collectible_scene = preload("res://scenes/game/world/entity/entities/item/collectible_items/WeaponCollectible.tscn")

export(float) var __attack_delay = 0.8

onready var __player_owner = get_parent().get_parent()

onready var __attack_delay_timer = $AttackDelayTimer

var __can_attack = true

func is_attacking():
	return !__can_attack
	
func set_can_attack(value):
	__can_attack = value

func attack():
	if !__can_attack: return
	
	if use_attacking_delay:
		__attack_delay_timer.start(__attack_delay)
	__can_attack = false
	
	if __player_owner.get_looking_vector().y > 0 and __player_owner.state_machine.get_current_state() == "PlayerAirBorneState":
		emit_signal("downwards_attack")
	else:
		emit_signal("attacked")
		
func manually_end_attack_cycle():
	__can_attack = true
	__attack_delay_timer.stop()
	emit_signal("attack_cycle_end")
	
func get_owner_player():
	return __player_owner
	
func equip():
	__player_owner.set_sprite_frames(__player_sprite_frames)
	
func drop():
	var collectible = __player_owner.parent_world.spawn_entity_deferred(__collectible_scene, __player_owner.position)
	collectible.weapon = self
	collectible.set_velocity(Vector2(-__player_owner.get_horizontal_looking_dir() * 50, -100))
	collectible.call_deferred("set_sprite", __collectible_sprite)
	__player_owner.stats.remove_child(self)
	collectible.set_recently_dropped(true)
	emit_signal("dropped")
	__attack_delay_timer.stop()
	__can_attack = true

func _on_AttackDelayTimer_timeout():
	__can_attack = true
	emit_signal("attack_cycle_end")
