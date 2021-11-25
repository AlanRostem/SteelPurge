extends MovingEntity

onready var __flash_timer = $FlashTimer
onready var _life_timer = $LifeTimer

var __flashing = false

var __player_found = null

func _process(delta):
	if _life_timer.time_left < _life_timer.wait_time / 2 and !__flashing:
		__flash_timer.start()
		__flashing = true
		
	if __player_found != null:
		if _pick_up_condition(__player_found):
			_player_collected(__player_found)
			queue_free()
		
func _physics_process(delta):
	if is_on_floor():
		set_velocity_x(0)

func _player_collected(player):
	pass
	
func _pick_up_condition(player):
	return true

func _on_PlayerDetectionArea_body_entered(body):
	__player_found = body

func _on_LifeTimer_timeout():
	queue_free()

func _on_FlashTimer_timeout():
	visible = !visible
