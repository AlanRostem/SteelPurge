using Godot;
using System;

public class FallingCollectible : KinematicBody2D
{
	private static readonly RandomNumberGenerator Rng = new RandomNumberGenerator();
	private static readonly float Gravity = 600;

	[Export] public float LungeSpeed = 50;
	[Export] public bool InteractToPickUp = false;
	
	public Vector2 GravityVector = Vector2.Down;
	public bool IsOnSlope = false;

	private Vector2 _vel;
	private Player _player;

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
		_vel += GravityVector * Gravity * delta;
		if (IsOnSlope)
			_vel = new Vector2();
		_vel = MoveAndSlide(_vel, Vector2.Up);
		if (IsOnFloor())
			_vel.x = 0;

		for (var i = 0; i < GetSlideCount(); i++)
		{
			var collision = GetSlideCollision(i);
			if (collision.Normal.y != -1 && IsOnFloor())
			{
				IsOnSlope = true;
			}
		}

		if (!InteractToPickUp) return;

		if (Input.IsActionJustPressed("interact"))
		{
			OnCollected(_player);
			QueueFree();
		}
	}

	private void _OnPlayerEnter(object body)
	{
		if (InteractToPickUp)
		{
			_player = (Player) body;
			return;
		}
		OnCollected((Player) body);
		QueueFree();
	}
}
