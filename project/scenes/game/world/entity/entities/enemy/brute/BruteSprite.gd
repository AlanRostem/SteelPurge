extends AnimatedSprite

onready var __brute = get_parent()

func _physics_process(delta):
	flip_h = __brute.get_horizontal_player_detect_direction() > 0
	match __brute.state_machine.get_current_state():
		"BruteWalkState": animation = "walk"
		"BruteSlamState":
			if __brute.is_recovering_from_slam():
				animation = "idle_slam"
			else:
				animation = "slam"
