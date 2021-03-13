using Godot;
using System;

public class FallingCollectible : KinematicBody2D
{
	private static readonly RandomNumberGenerator Rng = new RandomNumberGenerator();
	
	[Export] public float LungeSpeed = 50;

	public Vector2 GravityVector = Vector2.Down;

	private Vector2 _vel;

	
	public override void _Ready()
	{
		_vel = new Vector2(
			Rng.RandfRange(-1, 1) * LungeSpeed,
			Rng.Randf() * -LungeSpeed
		);
	}

	public virtual void OnCollected(Player player)
	{
	}

	public override void _PhysicsProcess(float delta)
	{
		_vel += GravityVector * Entity.Gravity * delta;
		_vel = MoveAndSlide(_vel, Vector2.Up);
		if (IsOnFloor())
			_vel.x = 0;

		for (var i = 0; i < GetSlideCount(); i++)
		{
			var collision = GetSlideCollision(i);
			if (collision.Normal.y != -1 && IsOnFloor())
				_vel = new Vector2();
		}
	}

	private void _OnPlayerEnter(object body)
	{
		OnCollected((Player) body);
		QueueFree();
	}
}
