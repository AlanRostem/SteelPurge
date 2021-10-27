extends Node2D

const MAX_HEALTH = 3
const MAX_HEALING_SCRAP = 50
const MAX_RUSH_ENERGY = 6

export(PackedScene) var default_weapon_scene

var __scrap_count = 0
var __rush_energy_count = MAX_RUSH_ENERGY
var __health = MAX_HEALTH
var __healing_scrap = 0

var __equipped_weapon

onready var __player = get_parent()

func _ready():
	if __equipped_weapon == null:
		equip_default_weapon()
		
func _physics_process(delta):
	if Input.is_action_pressed("fire"):
		__equipped_weapon.pull_trigger()
		if !__equipped_weapon.is_firing():
			__equipped_weapon.fire()
	else:
		__equipped_weapon.release_trigger()
			
func instance_and_equip_weapon(scene):
	var weapon = scene.instance()
	equip_weapon(weapon)

func equip_weapon(weapon):
	__equipped_weapon = weapon
	__equipped_weapon.owner_player = __player
	add_child(__equipped_weapon)

func equip_default_weapon():
	instance_and_equip_weapon(default_weapon_scene)
	
func get_equipped_weapon():
	return __equipped_weapon

func add_scrap(count):
	__scrap_count += count
	
func lose_scrap(count):
	__scrap_count = clamp(__scrap_count - count, 0, INF)
	
func get_scrap_count():
	return __scrap_count

func add_healing_scrap(count):
	__healing_scrap += count
	if __healing_scrap >= MAX_HEALING_SCRAP:
		var diff = __healing_scrap - MAX_HEALING_SCRAP
		__health += 1
		add_healing_scrap(diff)

func get_rush_energy():
	return __rush_energy_count

func use_rush_energy_half():
	__rush_energy_count = clamp(__rush_energy_count - 1, 0, INF)

func use_rush_energy_full():
	__rush_energy_count = clamp(__rush_energy_count - 2, 0, INF)
