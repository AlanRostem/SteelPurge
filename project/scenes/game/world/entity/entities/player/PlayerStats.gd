extends Node2D

const MAX_HEALTH = 3
const MAX_HEALING_SCRAP = 50
const MAX_RUSH_ENERGY = 6

signal scrap_changed(value)
signal health_changed(value)
signal healing_scrap_changed(value)
signal rush_energy_changed(value)
signal weapon_changed(weapon)
signal weapon_ammo_changed(value)

export(PackedScene) var default_weapon_scene

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
	if __equipped_weapon == null:
		call_deferred("equip_default_weapon")
	call_deferred("set_rush_energy", MAX_RUSH_ENERGY)
	
func _physics_process(delta):
	if __equipped_weapon == null: return
	if Input.is_action_pressed("fire"):
		__equipped_weapon.pull_trigger()
		if !__equipped_weapon.is_firing():
			__equipped_weapon.fire()
	else:
		__equipped_weapon.release_trigger()
		
	if Input.is_action_just_pressed("aim_down"):
		if __player.state_machine.get_current_state() == "PlayerAirBorneState":
			__player.toggle_aim_down()
	
	if __player.is_on_ground():
		__player.stop_aiming_down()
		if __rush_energy_count < MAX_RUSH_ENERGY and !__is_recharging_rush_energy:
			__rush_energy_recharge_starting_delay_timer.start()
			__is_recharging_rush_energy = true
	
func instance_and_equip_weapon(scene):
	var weapon = scene.instance()
	equip_weapon(weapon)

func equip_weapon(weapon):
	__equipped_weapon = weapon
	__equipped_weapon.owner_player = __player
	__equipped_weapon.connect("fired", self, "_on_equipped_weapon_fired")
	__equipped_weapon.connect("ammo_changed", self, "_on_equipped_weapon_ammo_changed")
	add_child(__equipped_weapon)
	emit_signal("weapon_changed", __equipped_weapon)

func equip_default_weapon():
	instance_and_equip_weapon(default_weapon_scene)
	
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
		add_scrap(count)
		return
	__healing_scrap += count
	if __healing_scrap >= MAX_HEALING_SCRAP:
		var diff = __healing_scrap - MAX_HEALING_SCRAP
		add_one_health()
		add_healing_scrap(diff)
	emit_signal("healing_scrap_changed", __healing_scrap)
	
func add_one_health():
	__health += 1
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

func _on_equipped_weapon_fired():
	if __player.get_velocity().y > -__equipped_weapon.recoil_boost_horizontal_speed and __player.get_looking_vector().y > 0 and get_rush_energy() >= __equipped_weapon.recoil_boost_rush_energy_usage:
		use_rush_energy(__equipped_weapon.recoil_boost_rush_energy_usage)
		__player.recoil_boost(__equipped_weapon.recoil_boost_horizontal_speed)

func _on_equipped_weapon_ammo_changed(ammo):
	emit_signal("weapon_ammo_changed", ammo)

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
