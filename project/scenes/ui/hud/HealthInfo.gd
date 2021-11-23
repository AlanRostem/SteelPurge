extends Control


onready var __first = $Health/First
onready var __second = $Health/Second
onready var __third = $Health/Third

func set_health(count):
	match count:
		3:
			__first.animation = "full"
			__second.animation = "full"
			__third.animation = "full"
		2:
			__first.animation = "full"
			__second.animation = "full"
			__third.animation = "empty"
		1:
			__first.animation = "full"
			__second.animation = "empty"
			__third.animation = "empty"
			
		0:
			__first.animation = "empty"
			__second.animation = "empty"
			__third.animation = "empty"
