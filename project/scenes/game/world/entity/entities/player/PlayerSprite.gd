extends AnimatedSprite

onready var player = get_parent()
onready var player_state_machine = get_parent().get_node("PlayerFSM")

func _physics_process(delta):
	flip_h = player.looking_vector.x < 0
	var f = frame
	match player_state_machine.get_current_state():
		"PlayerIdleState": animation = "idle"
		"PlayerWalkState": animation = "walking"
		"PlayerRunState": 
			animation = "running"
			frame = f
		"PlayerAirBorneState": 
			animation = "jump"
			if player.looking_vector.y > 0:
				animation = "aim_down"
		"PlayerCrouchState": animation = "slide"
		"PlayerSlideState": animation = "slide"
