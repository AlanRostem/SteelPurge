extends MovingEntity
class_name Projectile

enum RotationMode {
	WHOLE,
	SPRITE,
	HIT_BOX,
	NONE
}

var __hit_effect = preload("res://scenes/game/world/other/BlastHitEffect.tscn")

export(float) var max_velocity = 200
export(String) var damage_type = HealthComponent.DAMAGE_TYPE_STANDARD

var owner_weapon

onready var __hit_box = $HitBox
onready var __in_hit_box = $CounterHitBox
onready var __sprite = $Sprite

export(int) var damage

export(RotationMode) var __rotation_mode = RotationMode.WHOLE

var direction: Vector2

func init_from_player_weapon(dir_vec, weapon, offset = Vector2.ZERO):
	owner_weapon = weapon
	init(dir_vec, HitBox.PLAYER_TEAM, offset)
	
func init(dir_vec, team, offset = Vector2.ZERO):
	set_velocity(dir_vec * max_velocity)
	position += offset
	__hit_box.change_team(team)
	__in_hit_box.change_team(team)
	direction = dir_vec
	
	var angle = direction.angle()
	match __rotation_mode:
		RotationMode.WHOLE:
			rotation = angle
		RotationMode.SPRITE:
			__sprite.rotation = angle
		RotationMode.HIT_BOX:
			__hit_box.rotation = angle
			__in_hit_box.rotation = angle
			
func init_deferred(dir_vec, team, offset = Vector2.ZERO):
	call_deferred("init", dir_vec, team, offset)
	
func deal_hit(hit_box):
	hit_box.take_hit(__hit_box, damage, {}, damage_type)
	destroy()

func deflect(new_team, new_damage):
	__hit_box.change_team(new_team)
	__in_hit_box.change_team(new_team)
	damage = new_damage
	set_velocity(-get_velocity())

func destroy():
	parent_world.show_effect_deferred(__hit_effect, position)
	queue_free()

func get_hit_box():
	return __hit_box

func _on_HitBox_body_entered(body):
	if body is TileMap:
		destroy()

func _on_HitBox_hit_dealt(hitbox):
	deal_hit(hitbox)
