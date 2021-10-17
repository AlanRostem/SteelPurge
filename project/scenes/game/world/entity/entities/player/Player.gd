extends "res://scenes/game/world/entity/MovingEntity.gd"

export var walk_acceleration: float
export var max_walk_speed: float

var moving_direction = 1
var can_swap_looking_direction = true

func walk(direction: int, delta: float):
	accelerate_x(direction * walk_acceleration, max_walk_speed, delta)
	moving_direction = direction
