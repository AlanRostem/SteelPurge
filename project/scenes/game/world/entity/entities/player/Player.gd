extends "res://scenes/game/world/entity/MovingEntity.gd"
class_name Player

const MAX_DASH_CHARGE = 100
const PLAYER_TEAM = "player_team"
const RAM_SLIDE_SPEED = 200
const RAM_SLIDE_DAMAGE = 5

export var air_acceleration: float

export var walk_transition_weight: float
export var max_walk_speed: float
export var walk_friction: float

export var dash_transition_weight: float
export var max_dash_speed: float
export var dash_friction: float

export var jump_speed: float
export var min_jump_speed: float

export var max_crouch_speed: float
export var crouch_transition_weight: float

export var slide_speed: float
export var slide_friction: float
export var slide_mitigation_acceleration: float

var __dash_charge_fill_per_second = 200
var __dash_charge_loss_per_second = 120

var __dash_charge = 0

var moving_direction = 1
var can_swap_looking_direction = true

var __is_crouched = false

var __looking_vector = Vector2.RIGHT
var __horizontal_looking_direction = 1

var __is_roof_above = false

var __can_move_on_ground = true
var __is_opening_crate = false

onready var __upper_body_shape: CollisionShape2D = $UpperBodyShape
onready var __hit_box_shape = $InHitBox/CollisionShape2D
onready var hit_box = $InHitBox
onready var state_machine = $PlayerFSM
onready var stats = $PlayerStats
onready var __sprite = $PlayerSprite

onready var __flashing_timer = $FlashingTimer
onready var __invincibility_timer = $InvincibilityTimer

onready var __ram_slide_hit_box = $RamSlideHitBox
onready var __ram_slide_hit_box_shape = $RamSlideHitBox/CollisionShape2D

onready var __crate_opening_timer = $CrateOpeningTimer

#func _physics_process(delta):
#	print(__dash_charge)
#	print(get_velocity().x)

func run(direction: int, delta: float):
	set_velocity_x(lerp(get_velocity().x, direction * max_dash_speed, dash_transition_weight))
	moving_direction = direction

func walk(direction: int, delta: float):
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
	__hit_box_shape.shape.extents.y = 3
	__hit_box_shape.position.y = 0
	
	
func stand_up():
	if !__is_crouched: return
	__upper_body_shape.disabled = false
	__is_crouched = false
	__hit_box_shape.shape.extents.y = 7
	__hit_box_shape.position.y = -4

func is_crouched():
	return __is_crouched

func jump():
	set_velocity_y(-jump_speed)
	
func increase_dash_charge(delta):
	__dash_charge = clamp(__dash_charge + __dash_charge_fill_per_second * delta, 0, MAX_DASH_CHARGE)

func reduce_dash_charge(delta):
	__dash_charge = clamp(__dash_charge - __dash_charge_loss_per_second * delta, 0, MAX_DASH_CHARGE)
	
func maximize_dash_charge():
	__dash_charge = MAX_DASH_CHARGE
	
func has_max_dash_charge():
	return __dash_charge == MAX_DASH_CHARGE
	
func clear_dash_charge():
	__dash_charge = 0
	
func is_effectively_standing_still():
	return int(round(get_velocity().x)) == 0

func is_on_ground():
	return is_on_floor()
	
func recoil_boost(speed):
	set_velocity_y(-speed)
	
func look_horizontally(dir):
	if __looking_vector.y == 0:
		__looking_vector.x = dir
	__horizontal_looking_direction = dir
	__ram_slide_hit_box.scale.x = dir
	
func get_horizontal_looking_dir():
	return __horizontal_looking_direction
	
func set_aim_up(value):
	if value and !is_crouched():
		if __looking_vector.y == 0:
			__looking_vector.x = 0
			__looking_vector.y = -1
	else:
		__looking_vector.y = 0
		__looking_vector.x = __horizontal_looking_direction
	
func is_aiming_up():
	return __looking_vector.y < 0
	
func is_aiming_down():
	return __looking_vector.y > 0
	
func toggle_aim_down():
	if __looking_vector.y == 0:
		__looking_vector.y = 1
		__looking_vector.x = 0
	else:
		__looking_vector.y = 0
		__looking_vector.x = __horizontal_looking_direction
		
func stop_aiming_down():
	__looking_vector.x = __horizontal_looking_direction
	__looking_vector.y = 0
	
func get_looking_vector():
	return __looking_vector
	
func get_horizontal_looking_direction():
	return __horizontal_looking_direction
	
func set_sprite_frames(frames):
	var frame = __sprite.frame
	__sprite.frames = frames
	__sprite.frame = frame

func set_hit_box_enabled(value):
	__hit_box_shape.set_deferred("disabled", !value)
	
func start_invinvibility_sequence():
	set_hit_box_enabled(false)
	__invincibility_timer.start()
	__flashing_timer.start()
	visible = false
	
func is_roof_above():
	return __is_roof_above
	
func is_opening_crate():
	return __is_opening_crate

func set_opening_crate(value):
	__is_opening_crate = value
	__can_move_on_ground = !value
	if value:
		__crate_opening_timer.start()

func set_ram_slide_hit_box_enabled(value):
	__ram_slide_hit_box_shape.set_deferred("disabled", !value)

func _on_FlashingTimer_timeout():
	visible = !visible
	
func set_can_move_on_ground(value):
	__can_move_on_ground = value

func can_move_on_ground():
	return __can_move_on_ground

func _on_InvincibilityTimer_timeout():
	set_hit_box_enabled(true)
	__flashing_timer.stop()
	visible = true

func _on_RoofDetector_body_entered(body):
	__is_roof_above = true

func _on_RoofDetector_body_exited(body):
	__is_roof_above = false

func _on_RamSlideHitBox_hit_dealt(hitbox):
	hitbox.take_hit(__ram_slide_hit_box, RAM_SLIDE_DAMAGE, {
		"ram_slide": true
	})


func _on_CrateOpeningTimer_timeout():
	set_opening_crate(false)
