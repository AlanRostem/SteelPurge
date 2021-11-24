extends AnimatedSprite

onready var player = get_parent()
onready var player_state_machine = get_parent().get_node("PlayerFSM")

func _physics_process(delta):
	flip_h = player.get_horizontal_looking_direction() < 0
	var f = frame
	match player_state_machine.get_current_state():
		"PlayerIdleState": 
			if player.is_aiming_up():
				animation = "aim_up_idle"
			else: 
				animation = "idle"
		"PlayerWalkState": 
			if player.is_aiming_up():
				animation = "aim_up_walking"
			else: 
				animation = "walking"
		"PlayerRunState":
			if player.is_aiming_up():
				animation = "aim_up_running"
			else: 
				animation = "running"
			frame = f
		"PlayerAirBorneState": 
			if player.is_aiming_up():
				animation = "aim_up_jump"
			elif player.is_aiming_down():
				animation = "aim_down"
			else:
				animation = "jump"
		"PlayerCrouchState": 
			animation = "slide"
		"PlayerSlideState": 
			animation = "slide"
