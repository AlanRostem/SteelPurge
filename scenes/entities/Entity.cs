using Godot;
using System;
using Object = Godot.Object;

public class Entity : KinematicBody2D
{
	private const float Gravity = 600;
	private static readonly Vector2 Up = new Vector2(0, -1);
	private Vector2 _velocity = new Vector2();
	public Map ParentMap;
	public bool CanMove = true;
	public Vector2 Velocity => _velocity;

	public override void _Ready()
	{
		ParentMap = GetParent<Map>();
	}

	public override void _PhysicsProcess(float delta)
	{
		_velocity.y += Gravity * delta;
		_velocity = MoveAndSlide(_velocity, Up, true);
		for (var i = 0; i < GetSlideCount(); i++)
		{
			var collision = GetSlideCollision(i);
			_OnCollision(collision.Collider);
		}

		_OnMovement(delta);
	}

	public void Move(Vector2 velocity)
	{
		if (CanMove)
			_velocity = velocity;
	}

	public void Move(float x, float y)
	{
		if (x == 0f)
			x = Velocity.x;
		if (y == 0f)
			y = Velocity.y;
		if (CanMove)
			Move(new Vector2(x, y));
	}

	public void SetVelocity(Vector2 velocity)
	{
		_velocity = velocity;
	}

	public void SetVelocity(float x, float y)
	{
		SetVelocity(new Vector2(x, y));
	}

	public virtual void _OnCollision(Object collider)
	{
	}

	protected virtual void _OnMovement(float delta)
	{
	}
}
