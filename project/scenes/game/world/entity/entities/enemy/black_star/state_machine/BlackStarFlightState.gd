extends "res://scenes/game/world/entity/entities/enemy/black_star/state_machine/BlackStarState.gd"


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
var __angle = 0

# Called when the node enters the scene tree for the first time.
func physics_update(delta):
	black_star.set_velocity(Vector2(cos(__angle), sin(__angle)) * FLIGHT_SPEED)
	__angle += ANGLE_ROTATION_SPEED * delta
