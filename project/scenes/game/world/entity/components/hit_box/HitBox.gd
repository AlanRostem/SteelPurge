extends Area2D

enum HitBoxActionType {
	HIT_DEALER,
	HIT_RECEIVER
}

const HIT_BOX_IN_COLLISION_BIT = 1
const UN_SET_TEAM = "unset"

signal hit_received(hitbox)
signal hit_dealt(hitbox)

# Describes what the hit box should do in the game world. By changing this enum,
# the hitbox will be used to either receive a hit or give one out to another hit
# box that can receive hits.
export(HitBoxActionType) var hit_box_action_type = HitBoxActionType.HIT_DEALER

export(String) var __team = UN_SET_TEAM

onready var parent_entity = get_parent()

func _ready():
	match hit_box_action_type:
		HitBoxActionType.HIT_DEALER:
			set_collision_mask_bit(HIT_BOX_IN_COLLISION_BIT, true)
		HitBoxActionType.HIT_RECEIVER:
			set_collision_layer_bit(HIT_BOX_IN_COLLISION_BIT, true)

func change_team(team):
	__team = team

func get_team():
	return __team

# This connected signal will only be called if the hit box action type is set to
# deal hits. 
func _on_HitBox_area_entered(area):
	assert(__team != UN_SET_TEAM)
	assert(area.get_team() != UN_SET_TEAM)
	if __team == area.get_team(): return
	
	area.emit_signal("hit_received", self)
	emit_signal("hit_dealt", area)
