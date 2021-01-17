extends "res://scenes/entities/Entity.gd"

var WALK_SPEED = 60;
var JUMP_SPEED = 220;

func _ready():
	pass # Replace with function body.

func _on_move(delta):
	if Input.is_action_pressed("left"):
		velocity.x = -WALK_SPEED
	elif Input.is_action_pressed("right"):
		velocity.x = WALK_SPEED
	else:
		velocity.x = 0
	
	if Input.is_action_pressed("jump") and is_on_floor():
		velocity.y = -JUMP_SPEED
