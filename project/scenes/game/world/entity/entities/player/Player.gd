extends "res://scenes/game/world/entity/MovingEntity.gd"

export var air_acceleration: float
export var walk_transition_weight: float
export var max_walk_speed: float
export var walk_friction: float
export var jump_speed: float
export var min_jump_speed: float

var moving_direction = 1
var can_swap_looking_direction = true

func run(direction: int, delta: float):
	set_velocity_x(lerp(get_velocity().x, direction * max_walk_speed, walk_transition_weight))
	moving_direction = direction
	
func air_move(direction: int, delta: float):
	accelerate_x(air_acceleration * direction, max_walk_speed, delta)
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
	set_velocity_y(-jump_speed)
	
func is_effectively_standing_still():
	return int(round(get_velocity().x)) == 0
