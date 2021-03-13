using Godot;
using System;
using static Godot.Mathf;

public class FlareSprite : AnimatedSprite
{
	private Projectile _parent;
	
	public override void _Ready()
	{
		_parent = GetParent<Projectile>();
		Animation = "angle_zero";
	}
}
