extends AnimatedSprite

onready var player = get_parent()
onready var player_state_machine = get_parent().get_node("PlayerFSM")

func _physics_process(delta):
	flip_h = player.horizontal_looking_direction < 0
	match player_state_machine.get_current_state():
		"PlayerIdleState": animation = "idle"
		"PlayerRunState": animation = "idle"
		"PlayerAirborneState": animation = "idle"
		"PlayerCrouchState": animation = "slide"
		"PlayerSlideState": animation = "slide"
