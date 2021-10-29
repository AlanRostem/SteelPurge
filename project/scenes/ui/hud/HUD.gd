extends Control

onready var __rush_energy_bar = $RushEnergyBar

func connect_to_player(player):
	player.stats.connect("rush_energy_changed", self, "set_rush_energy_bar")

func set_rush_energy_bar(count):
	__rush_energy_bar.set_rush_energy(count)
