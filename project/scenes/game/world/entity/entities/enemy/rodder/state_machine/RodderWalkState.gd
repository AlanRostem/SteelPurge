extends "res://scenes/game/world/entity/entities/enemy/rodder/state_machine/RodderState.gd"

onready var __walk_timer = $WalkTimer

func physics_update(delta):
	rodder.horizontal_looking_direction = rodder.get_horizontal_player_detect_direction()
	rodder.set_velocity_x(rodder.horizontal_looking_direction * WALK_SPEED)

func enter(message: Dictionary):
	__walk_timer.start()
	
func exit():
	__walk_timer.stop()
	
func _on_WalkTimer_timeout():
	parent_state_machine.transition_to("RodderShootState")
