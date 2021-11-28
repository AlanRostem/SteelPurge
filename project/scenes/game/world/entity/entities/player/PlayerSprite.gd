extends AnimatedSprite

onready var player = get_parent()
onready var player_state_machine = get_parent().get_node("PlayerFSM")
onready var __rush_energy_activity_timer = $RushEnergyVisualActivityTimer

func _physics_process(delta):
	scale.x = player.get_horizontal_looking_direction()
	
	if player.stats.has_weapon():
		var weapon = player.stats.get_weapon()
		if weapon.name == "Sword":
			if weapon.is_attacking():
				if player.is_aiming_up():
					animation = "melee_up"
				elif player.is_aiming_down():
					pass
				else:
					animation = "melee"
				return
	elif player.is_opening_crate():
		animation = "open_crate"
		return

	
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


func _on_RushEnergyVisualActivityTimer_timeout():
	use_parent_material = true


func _on_PlayerStats_rush_energy_consumed():
	__rush_energy_activity_timer.start()
	use_parent_material = false
