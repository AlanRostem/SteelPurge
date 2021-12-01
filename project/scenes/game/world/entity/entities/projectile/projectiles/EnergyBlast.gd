extends Projectile


func _on_HitBox_hit_dealt_to_counter(hitbox):
	var parent = hitbox.get_parent()
	if parent is Projectile:
		parent.destroy()
