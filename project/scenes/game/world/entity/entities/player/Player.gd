extends "res://scenes/game/world/entity/MovingEntity.gd"

export var air_acceleration: float

export var walk_transition_weight: float
export var max_walk_speed: float
export var walk_friction: float

export var jump_speed: float
export var min_jump_speed: float

export var max_crouch_speed: float
export var crouch_transition_weight: float

export var slide_speed: float
export var slide_friction: float
export var slide_mitigation_acceleration: float


var moving_direction = 1
var can_swap_looking_direction = true

var __is_on_ground = false
var __is_crouched = false

onready var __upper_body_shape: CollisionShape2D = $UpperBodyShape
onready var __ground_detector = $GroundDetector

func run(direction: int, delta: float):
	set_velocity_x(lerp(get_velocity().x, direction * max_walk_speed, walk_transition_weight))
	moving_direction = direction
	
func sneak(direction: int, delta):
	set_velocity_x(lerp(get_velocity().x, direction * max_crouch_speed, crouch_transition_weight))
	moving_direction = direction
	
func air_move(direction: int, delta: float):
	accelerate_x(air_acceleration * direction, max_walk_speed, delta)
	moving_direction = direction
	
func negate_slide(direction: int, delta: float):
	accelerate_x(slide_mitigation_acceleration * direction, max_walk_speed, delta)
	moving_direction = direction
	
func apply_slide_friction(delta):
	set_velocity_x(lerp(get_velocity().x, 0, slide_friction))
	
func stop_running():
	set_velocity_x(lerp(get_velocity().x, 0, walk_friction))

func crouch_walk(direction: int, delta: float):
	pass

func slide(direction: int):
	set_velocity_x(direction * slide_speed)

func crouch():
	if __is_crouched: return
	__upper_body_shape.disabled = true
	__is_crouched = true
	
func stand_up():
	if !__is_crouched: return
	__upper_body_shape.disabled = false
	__is_crouched = false
	
func is_crouched():
	return __is_crouched

func jump():
	set_velocity_y(-jump_speed)
	
func is_effectively_standing_still():
	return int(round(get_velocity().x)) == 0

func is_on_ground():
	return __is_on_ground or is_on_floor()

func _on_GroundDetector_body_entered(area):
	__is_on_ground = true

func _on_GroundDetector_body_exited(area):
	__is_on_ground = false
