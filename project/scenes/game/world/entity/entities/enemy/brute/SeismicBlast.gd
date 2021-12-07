extends MovingEntity

const MAX_IDX = 3

var __scene = load("res://scenes/game/world/entity/entities/enemy/brute/SeismicBlast.tscn")

var dir = -1
var idx = 0

onready var __hit_box = $HitBox
	
func _on_LifeTimer_timeout():
	queue_free()
	if idx == MAX_IDX: return
	var location = position + Vector2(dir * 16, 0)
	var blast = parent_world.spawn_entity_deferred(__scene, location)
	blast.idx = idx + 1
	blast.dir = dir

func _on_HitBox_hit_dealt(hitbox):
	hitbox.take_hit(__hit_box, 1)

func _on_GeometryDetector_body_entered(body):
	if body is TileMap:
		queue_free()
