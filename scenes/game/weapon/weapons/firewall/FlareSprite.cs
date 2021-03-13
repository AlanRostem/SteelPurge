using Godot;
using System;
using static Godot.Mathf;

public class FlareSprite : AnimatedSprite
{
	private Projectile _parent;
	
	private static readonly float MaxSin = Sin(Deg2Rad(45));
	
	public override void _Ready()
	{
		_parent = GetParent<Projectile>();
		Animation = "angle_zero";

		var dir = Mathf.Sign(_parent.Velocity.x);
		if (dir != Scale.x)
			Scale = new Vector2(dir, 1);

		var sinDir = Abs(Sin(_parent.DirectionAngle));
		if (sinDir > MaxSin && sinDir > -MaxSin)
			Animation = "angle_up";
		
		if (sinDir > MaxSin && sinDir > -MaxSin)
			Animation = "angle_down";
	}
}
