extends "res://scenes/game/world/entity/components/state_machine/State.gd"

const MAX_PATROL_SPEED = 40
const MAX_RUSH_SPEED = 110

onready var rush_rogue = get_parent().get_parent()
