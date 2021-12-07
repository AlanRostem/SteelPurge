extends Control

const MAX_RUSH_ENERGY = 6

onready var __first = $First
onready var __second = $Second
onready var __third = $Third

onready var __flash_timer = $FlashTimer

func set_rush_energy(count):
	if count > 0:
		__flash_timer.stop()
		visible = true
	else:
		__flash_timer.start()
	match count:
		6:
			__first.animation = "full"
			__second.animation = "full"
			__third.animation = "full"
		5:
			__first.animation = "half"
			__second.animation = "full"
			__third.animation = "full"
		4:
			__first.animation = "empty"
			__second.animation = "full"
			__third.animation = "full"
		3:
			__first.animation = "empty"
			__second.animation = "half"
			__third.animation = "full"
		2:
			__first.animation = "empty"
			__second.animation = "empty"
			__third.animation = "full"
		1:
			__first.animation = "empty"
			__second.animation = "empty"
			__third.animation = "half"
		0:
			__first.animation = "empty"
			__second.animation = "empty"
			__third.animation = "empty"


func _on_FlashTimer_timeout():
	visible = !visible
