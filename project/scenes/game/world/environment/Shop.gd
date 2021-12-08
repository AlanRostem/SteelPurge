extends Node2D

export var __price = 25

var __player

onready var __price_tag = $PriceTag

func _ready():
	__price_tag.text = str(__price)
	__price_tag.rect_size.x = 0
	__price_tag.rect_position.y = -24
	__price_tag.rect_position.x = -__price_tag.rect_size.x / 2

func _on_Area2D_body_entered(body):
	__player = body


func _on_Area2D_body_exited(body):
	__player = null
	
func _physics_process(delta):
	if __player == null: return
	if __player.stats.get_scrap_count() < __price or !_purchase_condition(__player): 
		return
	if Input.is_action_just_pressed("fire"):
		__player.stats.lose_scrap(__price)
		_purchase_response(__player)
		
func _purchase_condition(player):
	return false
		
func _purchase_response(player):
	pass
