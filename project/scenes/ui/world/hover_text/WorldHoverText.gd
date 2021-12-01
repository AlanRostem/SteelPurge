extends Label

signal display_off()

const HOVER_SPEED = 20

onready var __life_timer = $LifeTimer

func display(str_txt, location):
	rect_position = location - rect_size / 2
	text = str_txt
	visible = true
	__life_timer.start()

func _physics_process(delta):
	rect_position.y += -HOVER_SPEED * delta

func _on_LifeTimer_timeout():
	visible = false
	emit_signal("display_off")
