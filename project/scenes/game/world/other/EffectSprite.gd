extends AnimatedSprite

func _on_EffectSprite_animation_finished():
	queue_free()
