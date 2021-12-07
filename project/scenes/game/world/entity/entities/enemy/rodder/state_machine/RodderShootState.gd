extends "res://scenes/game/world/entity/entities/enemy/rodder/state_machine/RodderState.gd"

var __rod_scene = preload("res://scenes/game/world/entity/entities/projectile/projectiles/Rod.tscn")

onready var __shoot_timer = $ShootTimer

const MAX_SHOTS = 3

var __shots = 0

func physics_update(delta):
	rodder.horizontal_looking_direction = rodder.get_horizontal_player_detect_direction()
	rodder.set_velocity_x(0)
		
func enter(mesage: Dictionary):
	__shoot_timer.start()
	
func exit():
	__shots = 0
	__shoot_timer.stop()
	
func shoot():
	__shots += 1
	var rod = rodder.parent_world.spawn_entity_deferred(__rod_scene, rodder.position)
	rod.init_deferred(Vector2(rodder.horizontal_looking_direction, 0), HitBox.ENEMY_TEAM)

func _on_ShootTimer_timeout():
	if __shots == MAX_SHOTS:
		parent_state_machine.transition_to("RodderWalkState")
		return
	shoot()
