extends AnimatedSprite

onready var __rodder = get_parent()

func _process(delta):
	flip_h = __rodder.get_horizontal_player_detect_direction() > 0
	match __rodder.state_machine.get_current_state():
		"RodderWalkState": animation = "walk"
		"RodderShootState": animation = "shoot"
