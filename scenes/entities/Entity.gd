extends KinematicBody2D

const UP = Vector2(0, -1)
const GRAVITY = 600

var velocity = Vector2()

func _ready():
	pass 

func _on_move(delta):
	pass

func _physics_process(delta):
	velocity.y += GRAVITY * delta;
	velocity = move_and_slide(velocity, UP, true)
	_on_move(delta)	
