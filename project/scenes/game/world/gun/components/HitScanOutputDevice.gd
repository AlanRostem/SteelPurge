extends "res://scenes/game/world/gun/components/FiringOutputDevice.gd"

export(float) var scan_length = 100

var __hit_box_scene = preload("res://scenes/game/world/entity/components/hit_box/HitBox.tscn")

var __hit_box: HitBox = __hit_box_scene.instance()
var __hit_box_shape

var __timer = Timer.new()

func _ready():
	__hit_box_shape = CollisionShape2D.new()
	__hit_box_shape.shape = RectangleShape2D.new()
	__hit_box_shape.shape.extents = Vector2(scan_length, 1)
	__hit_box_shape.disabled = true
	__hit_box_shape.position.x += __hit_box_shape.shape.extents.x
	__hit_box.call_deferred("add_child", __hit_box_shape)
	
	__hit_box.hit_box_action_type = HitBox.HitBoxActionType.HIT_DEALER
	__hit_box.change_team(HitBox.PLAYER_TEAM)
	
	__hit_box.connect("hit_dealt", self, "_on_hit_box_hit_dealt")
	
	parent_weapon.call_deferred("add_child", __hit_box)
	
	add_child(__timer)
	__timer.connect("timeout", self, "_on_Timer_timeout")

func _on_hit_box_hit_dealt(hitbox):
	hitbox.take_hit(__hit_box, parent_weapon.damage_per_shot)
	
func _on_Timer_timeout():
	__hit_box_shape.disabled = true

func _on_Gun_fired():
	var player = parent_weapon.owner_player
	var world = parent_weapon.owner_player.parent_world
	__hit_box.rotation = player.get_looking_vector().angle()
	__hit_box_shape.disabled = false
	__timer.start(0.1)
	
