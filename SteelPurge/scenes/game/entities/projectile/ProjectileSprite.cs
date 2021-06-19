using Godot;

public class ProjectileSprite : Sprite
{
	private Projectile _parent;
	public override void _Ready()
	{
		_parent = GetParent<Projectile>();
		var angle = Mathf.Deg2Rad(_parent.VisualAngle);
		Rotation = angle;
		var sign = Mathf.Sign(_parent.VelocityX);
		if (Scale.x != sign)
		{
			Scale = new Vector2(_parent.DirectionSign, Scale.y);
		}
	}
}
