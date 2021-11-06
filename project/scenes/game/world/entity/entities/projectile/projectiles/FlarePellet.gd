extends "res://scenes/game/world/entity/entities/projectile/Projectile.gd"

func _on_LifeTimer_timeout():
	destroy()
