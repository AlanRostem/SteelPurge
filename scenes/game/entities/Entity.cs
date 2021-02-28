using Godot;
using System;
using System.Collections.Generic;
using Object = Godot.Object;

public class Entity : KinematicBody2D
{
	public const float Gravity = 600;
	public World ParentWorld { get; private set; }
	public bool CanMove = true;
	public Vector2 FloorNormal = Vector2.Up;

	public uint Health
	{
		get => _health;

		set
		{
			_health = value;
			EmitSignal(nameof(HealthChanged), _health);
		}
	}

	private uint _health = 100;

    public Vector2 Velocity;

	[Signal]
	public delegate void HealthChanged(uint health);
	
	public override void _Ready()
	{
		ParentWorld = GetParent<World>();
	}

	public void ApplyStatusEffect(StatusEffect effect)
	{
		AddChild(effect);
	}

	public override void _PhysicsProcess(float delta)
	{
		Velocity.y += Gravity * delta;
		Velocity = MoveAndSlide(Velocity, FloorNormal, false);
		for (var i = 0; i < GetSlideCount(); i++)
		{
			var collision = GetSlideCollision(i);
            _OnCollision(collision);
		}

		_OnMovement(delta);
	}
	


    public void AccelerateX(float x, float maxSpeed, float delta)
    {
        if (CanMove)
            Velocity.x = Mathf.Clamp(Velocity.x + x * delta, -maxSpeed, maxSpeed);
    }

	public void MoveX(float x)
	{
		if (CanMove)
			Velocity.x = x;
	}

	public void MoveY(float y)
	{
		if (CanMove)
			Velocity.y = y;
	}

	public virtual void _OnCollision(KinematicCollision2D collider)
	{
	}

	protected virtual void _OnMovement(float delta)
	{
	}
}
