extends Area2D
class_name HitBox

enum HitBoxActionType {
	HIT_DEALER,
	HIT_RECEIVER
}

const HIT_BOX_IN_COLLISION_BIT = 1
const UN_SET_TEAM = "unset"
const PLAYER_TEAM = "player_team"
const ENEMY_TEAM = "enemy_team"

signal hit_received(hitbox, damage, damage_type)
signal hit_dealt(hitbox)
signal received_additional_message(message)

# Describes what the hit box should do in the game world. By changing this enum,
# the hitbox will be used to either receive a hit or give one out to another hit
# box that can receive hits.
export(HitBoxActionType) var hit_box_action_type

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
	
# Manually call this function to deal damage to a hitbox exclusively. Should be
# used after connecting to "hit_dealt"
func take_hit(hitbox, damage, message: Dictionary = {}, damage_type = HealthComponent.DAMAGE_TYPE_STANDARD):
	emit_signal("hit_received", hitbox, damage, damage_type)
	if message.size() > 0:
		emit_signal("received_additional_message", message)

# This connected signal will only be called if the hit box action type is set to
# deal hits. 
func _on_HitBox_area_entered(area):
	assert(__team != UN_SET_TEAM)
	assert(area.get_team() != UN_SET_TEAM)
	if __team == area.get_team(): return
	emit_signal("hit_dealt", area)
