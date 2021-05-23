using Godot;
using System;

public class HornetRogue : Enemy
{
	[Export] public uint DamagePerHit = 45;
	[Export] public int Direction = -1;
	private AnimatedSprite _sprite;
	public bool Fly = false;

	public override void _Ready()
	{
		base._Ready();
		_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
		_sprite.FlipH = Direction < 0;
	}

	protected override void _OnMovement(float delta)
	{
		base._OnMovement(delta);
		if (IsOnFloor())
			Velocity.x = Direction * 110;
		else if (!Fly)
			Velocity.x = 0;
	}

	public override void _OnCollision(KinematicCollision2D collider)
	{
		if (IsOnWall() || IsOnCeiling())
			QueueFree();
	}

	public override void TakeDamage(uint damage, Vector2 direction)
	{
		if (direction.y != 0)
		{
			Velocity.y = 0;
			IsGravityEnabled = false;
		}
		base.TakeDamage(damage, direction);
	}

	private void _OnPlayerEnterExplosiveArea(Player player)
	{
		player.TakeDamage(DamagePerHit, new Vector2(Direction, 0));
		QueueFree();
	}
}
