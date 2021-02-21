using Godot;
using System;
using System.Collections.Generic;
using Object = Godot.Object;

public class Entity : KinematicBody2D
{
	public const float Gravity = 600;
	public Map ParentMap { get; private set; }
	public bool CanMove = true;
    public uint Health = 100;
	public Vector2 Velocity {
		get => _velocity;
		set => _velocity = value;
	}

	private Vector2 _velocity;
	
	public override void _Ready()
	{
		ParentMap = GetParent<Map>();
	}

	public void ApplyStatusEffect(StatusEffect effect)
	{
		AddChild(effect);
	}

	public override void _PhysicsProcess(float delta)
	{
		_velocity.y += Gravity * delta;
		_velocity = MoveAndSlide(_velocity, Vector2.Up, true);
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
