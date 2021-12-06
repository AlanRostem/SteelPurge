extends "res://scenes/game/world/entity/components/state_machine/State.gd"

const FLIGHT_SPEED = 40
const ANGLE_ROTATION_SPEED = PI

onready var black_star = get_parent().get_parent()
