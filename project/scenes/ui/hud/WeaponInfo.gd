extends Control

onready var __ammo_label = $AmmoLabel
onready var __name_label = $NameLabel

func set_weapon_info(weapon):
	set_ammo(weapon.get_ammo())
	set_weapon_name(weapon.display_name)

func set_ammo(count):
	__ammo_label.text = str(count)
	
func set_weapon_name(weapon_name):
	__name_label.text = weapon_name
