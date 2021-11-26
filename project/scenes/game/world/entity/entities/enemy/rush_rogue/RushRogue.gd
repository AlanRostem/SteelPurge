extends Enemy

var __is_charging_up = false

func set_charging_up(value):
	__is_charging_up = value
	
func is_charging_up():
	return __is_charging_up
