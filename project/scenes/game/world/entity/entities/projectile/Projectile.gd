extends MovingEntity

export(float) var max_velocity = 200

var owner_weapon

onready var __hit_box = $HitBox

export(int) var __damage

func init_from_player_weapon(dir_vec, weapon, offset = Vector2.ZERO):
	owner_weapon = weapon
	__damage = weapon.damage_per_shot
	init(dir_vec, HitBox.PLAYER_TEAM, offset)
	
func init(dir_vec, team, offset = Vector2.ZERO):
	set_velocity(dir_vec * max_velocity)
	position += offset
	__hit_box.change_team(team)
	
func deal_hit(hit_box):
	hit_box.take_hit(self, __damage)
	destroy()

func destroy():
	queue_free()

func _on_HitBox_body_entered(body):
	if body is TileMap:
		destroy()

func _on_HitBox_hit_dealt(hitbox):
	deal_hit(hitbox)
