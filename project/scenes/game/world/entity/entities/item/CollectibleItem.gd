extends MovingEntity

func _player_collected(player):
	pass

func _on_PlayerDetectionArea_body_entered(body):
	_player_collected(body)
	queue_free()
