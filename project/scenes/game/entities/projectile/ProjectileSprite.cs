using Godot;

public class ProjectileSprite : Sprite
{
	private Projectile _parent;
	public override void _Ready()
	{
		_parent = GetParent<Projectile>();
	}

	public override void _Process(float delta)
	{
		var angle = Mathf.Deg2Rad(_parent.VisualAngle);
		Rotation = angle;
		var sign = Mathf.Sign(_parent.VelocityX);
		if (Rotation == 0 && Scale.x != sign)
		{
			Scale = new Vector2(_parent.DirectionSign, Scale.y);
		}
		else
		{
			Scale = new Vector2(Scale.x, _parent.DirectionSign);
		}
	}
}
