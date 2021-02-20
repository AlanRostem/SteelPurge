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
	public Vector2 Velocity {
		get => _velocity;
		set => _velocity = value;
	}

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



	public void MoveX(float x)
	{
		if (CanMove)
			_velocity.x = x;
	}

	public void MoveY(float y)
	{
		if (CanMove)
			_velocity.y = y;
	}

	public virtual void _OnCollision(Object collider)
	{
	}

	protected virtual void _OnMovement(float delta)
	{
	}
}
