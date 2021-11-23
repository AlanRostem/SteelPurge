extends Node2D
class_name PlayerStats

const MAX_HEALTH = 3
const MAX_HEALING_SCRAP = 5
const MAX_RUSH_ENERGY = 6

signal scrap_changed(value)
signal health_changed(value)
signal healing_scrap_changed(value)
signal died()
signal rush_energy_changed(value)
signal weapon_changed(weapon)
signal weapon_ammo_changed(value)

export(PackedScene) var test_weapon_scene

var __default_player_sprite_frames = preload("res://assets/resources/sprite_frames/char/PlayerSpriteFrames.tres")

var __scrap_count = 0
var __rush_energy_count = MAX_RUSH_ENERGY
var __health = MAX_HEALTH
var __healing_scrap = 0

var __equipped_weapon

var __is_recharging_rush_energy = false

onready var __player = get_parent()
onready var __rush_energy_recharge_timer = $RushEnergyRechargeTimer
onready var __rush_energy_recharge_starting_delay_timer = $RushEnergyRechargeStartingDelayTimer

func _ready():
	if test_weapon_scene != null:
		call_deferred("equip_test_weapon")
	call_deferred("set_rush_energy", MAX_RUSH_ENERGY)
	call_deferred("set_health", MAX_HEALTH)
	
func _physics_process(delta):
	if __player.is_on_ground():
		__player.stop_aiming_down()
		if __rush_energy_count < MAX_RUSH_ENERGY and !__is_recharging_rush_energy:
			__rush_energy_recharge_starting_delay_timer.start()
			__is_recharging_rush_energy = true
	
	if __equipped_weapon == null: return
	if Input.is_action_just_pressed("fire"):
		__equipped_weapon.attack()
		
	if Input.is_action_just_pressed("aim_down"):
		if __player.state_machine.get_current_state() == "PlayerAirBorneState":
			__player.toggle_aim_down()
	
func instance_and_equip_weapon(scene):
	var weapon = scene.instance()
	equip_weapon(weapon)

func equip_weapon(weapon):
	__equipped_weapon = weapon
	add_child(__equipped_weapon)
	__equipped_weapon.equip()
	emit_signal("weapon_changed", __equipped_weapon)

func equip_test_weapon():
	instance_and_equip_weapon(test_weapon_scene)
	
func get_equipped_weapon():
	return __equipped_weapon

func add_scrap(count):
	__scrap_count += count
	emit_signal("scrap_changed", __scrap_count)
	
func lose_scrap(count):
	__scrap_count = clamp(__scrap_count - count, 0, INF)
	emit_signal("scrap_changed", __scrap_count)
	
func get_scrap_count():
	return __scrap_count

func add_healing_scrap(count):
	if __health == MAX_HEALTH:
		add_scrap(count + __healing_scrap)
		__healing_scrap = 0
		return
	__healing_scrap += count
	if __healing_scrap >= MAX_HEALING_SCRAP:
		var diff = __healing_scrap - MAX_HEALING_SCRAP
		add_one_health()
		add_healing_scrap(diff)
		
	emit_signal("healing_scrap_changed", __healing_scrap)

func take_one_damage():
	set_health(__health - 1)
	if __equipped_weapon != null:
		__equipped_weapon.drop()
		__equipped_weapon.queue_free()
		__equipped_weapon = null
		__player.set_sprite_frames(__default_player_sprite_frames)
	if __health == 0:
		emit_signal("died")
	
func add_one_health():
	set_health(__health + 1)
	
func set_health(value):
	__health = value
	emit_signal("health_changed", __health)

func get_rush_energy():
	return __rush_energy_count
	
func set_rush_energy(count):
	__rush_energy_count = clamp(count, 0, MAX_RUSH_ENERGY)
	emit_signal("rush_energy_changed", __rush_energy_count)

func use_rush_energy(count):
	set_rush_energy(__rush_energy_count - count)
	__rush_energy_recharge_starting_delay_timer.stop()
	__rush_energy_recharge_timer.stop()
	__is_recharging_rush_energy = false

func recharge_rush_energy():
	set_rush_energy(__rush_energy_count + 2)
	if __rush_energy_count == MAX_RUSH_ENERGY:
		__rush_energy_recharge_timer.stop()
		__is_recharging_rush_energy = false

func _on_RushEnergyRechargeTimer_timeout():
	recharge_rush_energy()

func _on_RushEnergyRechargeStartingDelayTimer_timeout():
	__rush_energy_recharge_timer.start()
	recharge_rush_energy()
