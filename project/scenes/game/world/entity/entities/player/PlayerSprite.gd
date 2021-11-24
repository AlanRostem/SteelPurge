extends AnimatedSprite

onready var player = get_parent()
onready var player_state_machine = get_parent().get_node("PlayerFSM")

func _physics_process(delta):
	flip_h = player.get_horizontal_looking_direction() < 0
	var f = frame
	match player_state_machine.get_current_state():
		"PlayerIdleState": 
			animation = "idle"
			if player.is_aiming_up():
				animation = "aim_up_idle"
		"PlayerWalkState": 
			animation = "walking"
			if player.is_aiming_up():
				animation = "aim_up_walking"
		"PlayerRunState":
			if player.is_aiming_up():
				animation = "aim_up_running"
			animation = "running"
			frame = f
		"PlayerAirBorneState": 
			animation = "jump"
			if player.is_aiming_up():
				animation = "aim_up_jump"
			elif player.get_looking_vector().y > 0:
				animation = "aim_down"
		"PlayerCrouchState": 
			animation = "slide"
			if player.is_aiming_up():
				animation = "aim_up_slide"
		"PlayerSlideState": 
			animation = "slide"
			if player.is_aiming_up():
				animation = "aim_up_slide"
