using Godot;
using System;

public class HornetRogue : Enemy
{
	private AnimatedSprite _sprite;
	public override void _Ready()
	{
		base._Ready();
		_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		_sprite.FlipH = Velocity.x < 0;
	}

	public override void TakeDamage(uint damage, Vector2 direction)
	{
		base.TakeDamage(damage, direction);
		if (direction.x != 0)
		{
			IsGravityEnabled = false;
		}
	}
}
