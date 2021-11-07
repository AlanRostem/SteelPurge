extends Control

onready var __rush_energy_bar = $RushEnergyBar
onready var __weapon_info = $WeaponInfo
onready var __scrap_info = $ScrapInfo
onready var __health_info = $HealthInfo

func connect_to_player(player):
	player.stats.connect("rush_energy_changed", __rush_energy_bar, "set_rush_energy")
	
	player.stats.connect("weapon_changed", __weapon_info, "set_weapon_info")
	player.stats.connect("weapon_ammo_changed", __weapon_info, "set_ammo")
	
	player.stats.connect("scrap_changed", __scrap_info, "set_scrap_count")
	
	player.stats.connect("health_changed", __health_info, "set_health")
	player.stats.connect("healing_scrap_changed", __health_info, "set_healing_scrap")
