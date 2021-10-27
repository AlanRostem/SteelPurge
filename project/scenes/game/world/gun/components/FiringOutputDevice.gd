extends Node

onready var parent_weapon = get_parent()

func _on_Gun_fired():
	print("bang! ammo: " + str(parent_weapon.get_ammo()))
