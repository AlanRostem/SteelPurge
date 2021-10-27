extends Node

onready var parent_weapon = get_parent()

onready var _fire_timer = $FireTimer

func _on_Gun_start_firing_cycle():
	_fire_timer.start(60.0 / parent_weapon.shots_per_minute)

func _on_FireTimer_timeout():
	parent_weapon.fire()

func _on_Gun_end_firing_cycle():
	_fire_timer.stop()
