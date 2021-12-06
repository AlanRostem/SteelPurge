extends "res://scenes/game/world/entity/entities/enemy/brute/state_machine/BruteState.gd"


onready var __slam_timer = $SlamTimer
onready var __slam_delay_timer = $SlamDelayTimer

var __blast_scene = preload("res://scenes/game/world/entity/entities/enemy/brute/SeismicBlast.tscn")


func physics_update(delta):
	brute.set_velocity_x(0)

func enter(message: Dictionary):
	__slam_timer.start()
	
func exit():
	__slam_timer.stop()
	__slam_delay_timer.stop()
	brute.set_recovering_from_slam(false)

func _on_Brute_player_detected(player):
	parent_state_machine.transition_to(name)

func _on_SlamTimer_timeout():
	__slam_delay_timer.start()
	brute.set_recovering_from_slam(true)
	var dir = brute.get_horizontal_player_detect_direction()
	var location = brute.position + Vector2(dir * 12, -2)
	brute.parent_world.spawn_entity_deferred(__blast_scene, location).dir = dir

func _on_SlamDelayTimer_timeout():
	__slam_timer.start()
	brute.set_recovering_from_slam(false)
