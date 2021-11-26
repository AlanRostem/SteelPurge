extends AnimatedSprite

onready var rush_rogue = get_parent()

func _process(delta):
	flip_h = rush_rogue.horizontal_looking_direction > 0
	
