extends Control


onready var __first = $Health/First
onready var __second = $Health/Second
onready var __third = $Health/Third

onready var __healing_scrap = $HealingScrap

func _ready():
	__healing_scrap.max_value = PlayerStats.MAX_HEALING_SCRAP

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

func set_healing_scrap(count):
	__healing_scrap.value = count
