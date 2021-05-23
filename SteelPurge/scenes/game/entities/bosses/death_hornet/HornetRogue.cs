using Godot;
using System;

public class HornetRogue : Enemy
{
	[Export] public uint DamagePerHit = 45;
	[Export] public int Direction = -1;
	private AnimatedSprite _sprite;
	public override void _Ready()
	{
		base._Ready();
		_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
		Velocity.x = Direction * 110;
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		_sprite.FlipH = Velocity.x < 0;
	}

	public override void _OnCollision(KinematicCollision2D collider)
	{
		if (IsOnWall())
			QueueFree();
	}

	public override void TakeDamage(uint damage, Vector2 direction)
	{
		base.TakeDamage(damage, direction);
		if (direction.y != 0)
		{
			IsGravityEnabled = false;
		}
	}
	
	private void _OnPlayerEnterExplosiveArea(Player player)
	{
		player.TakeDamage(DamagePerHit, new Vector2(Direction, 0));
		QueueFree();
	}
}
