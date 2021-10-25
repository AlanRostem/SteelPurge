extends Node2D

export(int) var damage_per_shot
export(int) var rate_of_fire

export(int) var recoil_boost_rush_energy_usage = 1
export(bool) var use_rush_energy_on_recoil_boost = true
export(float) var recoil_boost_horizontal_speed = 100

export(int) var max_ammo = 60;
export(int) var ammo_usage_per_shot = 1;
export(bool) var infinite_ammo = false

# export(SpriteFrames) var player_sprite_frames;

signal fired()
signal start_firing_cycle()
signal end_firing_cycle()

signal dropped()

onready var __ammo = max_ammo

var firing_mechanism
var firing_output_device

var __is_firing = false

func fire():
	if !__is_firing:
		__is_firing = true
		emit_signal("start_firing_cycle")
	emit_signal("fired")
	if !infinite_ammo and __ammo > 0:
		__ammo -= 1
		if __ammo == 0:
			emit_signal("end_firing_cycle")

func is_firing():
	return __is_firing
