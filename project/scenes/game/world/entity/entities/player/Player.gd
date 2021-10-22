extends "res://scenes/game/world/entity/MovingEntity.gd"

export var walk_acceleration: float
export var walk_transition_weight: float
export var max_walk_speed: float
export var walk_friction: float

var moving_direction = 1
var can_swap_looking_direction = true

func run(direction: int, delta: float):
	set_velocity_x(lerp(get_velocity().x, direction * max_walk_speed, walk_transition_weight))
	moving_direction = direction
	
func stop_running():
	set_velocity_x(lerp(get_velocity().x, 0, walk_friction))

func crouch_walk(direction: int, delta: float):
	pass

func slide(direction: int, delta: float):
	pass
	
func crouch():
	pass
	
func jump():
	pass
