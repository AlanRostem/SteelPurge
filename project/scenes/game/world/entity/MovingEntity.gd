# Base class for moving entities. Its collision mode can be modified between 
# MOVE, SLIDE, and SNAP. Its velocity can be changed at any time but is only 
# registered in the next physics process call.

extends KinematicBody2D
class_name MovingEntity

enum CollisionModes {
	MOVE,
	SLIDE,
	SNAP
}

export(CollisionModes) var collision_mode

export var is_gravity_enabled = true

export var gravity = 400

export var stop_on_slope = true

var parent_world = null

var __velocity = Vector2()

var __down_vector = Vector2.DOWN
var __snap_vector = Vector2.DOWN

var __can_accelerate = true

func _ready():
	if parent_world == null:
		parent_world = get_parent().get_parent()

# Returns true if the absolute value of the entity's x-velocity is greater
# than the max_viable_x_speed export variable
func is_moving_too_fast(max_viable_x_speed):
	var margin = 0.1
	return abs(get_velocity().x) >= max_viable_x_speed + margin
	
func is_moving_approximately_at_speed(max_viable_x_speed):
	var margin = 1
	return max_viable_x_speed - abs(get_velocity().x) <=  + margin

# Change where "down" points to relative to the entity. This affects how the 
# changing of velocity is done.
func set_down_vector(vector):
	__down_vector = vector
	__snap_vector = vector 

func get_perspective_angle():
	return __down_vector.angle() - PI / 2

# Retrieve the velocity vector as perceived by the down vector
func get_velocity():
	return __velocity.rotated(get_perspective_angle())

# Change the velocity vector as perceived by the down vector
func set_velocity(velocity):
	if velocity.y < 0:
		__snap_vector = Vector2.ZERO
	__velocity = velocity.rotated(get_perspective_angle())

# Change the velocity on the x-axis vector as perceived by the down vector
func set_velocity_x(x):
	__velocity = Vector2(x, get_velocity().y).rotated(get_perspective_angle())

# Change the velocity on the y-axis vector as perceived by the down vector. If
# the y-velocity is negative when the collision mode is SNAP, then the snap 
# vector is set to the zero-vector until the entity is on the floor again.
func set_velocity_y(y):
	if y < 0:
		__snap_vector = Vector2.ZERO
	__velocity = Vector2(get_velocity().x, y).rotated(get_perspective_angle())
	
func accelerate_x(x, max_speed, delta):
	var vx = get_velocity().x
	var movement = x * delta;
	var result = abs(vx + movement);
	var differing_dir = sign(movement) != sign(vx);

	if !__can_accelerate:
		if result < max_speed or differing_dir:
			__can_accelerate = true;
		else: return

	if result > max_speed and !differing_dir:
		movement = (result - max_speed) * delta * sign(x);
		result = abs(vx + movement);
		if result > max_speed:
			movement = 0;
		__can_accelerate = false;

	set_velocity_x(vx + movement)

func _physics_process(delta):
	if is_gravity_enabled:
		__velocity += __down_vector * gravity * delta
	
	match collision_mode:
		CollisionModes.MOVE:
			move_and_collide(__velocity * delta)
		CollisionModes.SLIDE:
			__velocity = move_and_slide(__velocity, -__down_vector)
		CollisionModes.SNAP:
			# TODO: Determine if this needs rotation when changing 
			# perspective vector
			__velocity.y = move_and_slide_with_snap(
				__velocity, __snap_vector * parent_world.get_tile_size(),
				-__down_vector, stop_on_slope).y
				
			if is_on_floor():
				__snap_vector = Vector2(__down_vector)
				
	# TODO: Check if the entity is on a slope and store that in a variable
