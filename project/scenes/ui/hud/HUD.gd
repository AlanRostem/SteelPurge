extends Control

onready var __rush_energy_bar = $RushEnergyBar
onready var __weapon_info = $WeaponInfo

func connect_to_player(player):
	player.stats.connect("rush_energy_changed", __rush_energy_bar, "set_rush_energy")
	
	player.stats.connect("weapon_changed", __weapon_info, "set_weapon_info")
	player.stats.connect("weapon_ammo_changed", __weapon_info, "set_ammo")
