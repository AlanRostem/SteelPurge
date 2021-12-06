extends Enemy

var __is_recovering_from_slam = false

func set_recovering_from_slam(value):
	__is_recovering_from_slam = value
	
func is_recovering_from_slam():
	return __is_recovering_from_slam
