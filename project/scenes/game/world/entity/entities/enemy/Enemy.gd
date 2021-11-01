extends MovingEntity
class_name Enemy

const STANDARD_ENEMY_HEALTH = 10

export(int) var health = STANDARD_ENEMY_HEALTH

func take_damage(damage, status_effect):
	pass

func die():
	pass
