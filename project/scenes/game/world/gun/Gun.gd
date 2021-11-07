extends Node2D

export(String) var display_name

export(int) var damage_per_shot
export(int) var shots_per_minute

export(int) var recoil_boost_rush_energy_usage = 1
export(bool) var use_rush_energy_on_recoil_boost = true
export(float) var recoil_boost_horizontal_speed = 100

export(int) var max_ammo = 60
export(int) var ammo_usage_per_shot = 1
export(bool) var infinite_ammo = false

export(SpriteFrames) var player_sprite_frames

signal fired()
signal ammo_changed(value)
signal start_firing_cycle()
signal end_firing_cycle()

signal dropped()

onready var __ammo = max_ammo

var __is_firing = false

var __is_holding_trigger = false

var owner_player

func equip():
	owner_player.set_sprite_frames(player_sprite_frames)

func fire():
	if __ammo == 0: 
		return
	
	if __is_firing and !__is_holding_trigger:
		stop_firing()
		__is_holding_trigger = false
		return
		
	if !__is_firing:
		__is_firing = true
		__is_holding_trigger = true
		emit_signal("start_firing_cycle")
	emit_signal("fired")
	if !infinite_ammo and __ammo > 0:
		use_ammo()
		if __ammo == 0:
			stop_firing()
			__is_holding_trigger = false

func use_ammo(amount = 1):
	__ammo -= amount
	emit_signal("ammo_changed", __ammo)
	
func set_ammo(amount):
	__ammo = amount
	emit_signal("ammo_changed", __ammo)
	
func get_ammo():
	return __ammo

func stop_firing():
	emit_signal("end_firing_cycle")
	__is_firing = false
	
func pull_trigger():
	__is_holding_trigger = true
	
func release_trigger():
	__is_holding_trigger = false

func is_firing():
	return __is_firing
