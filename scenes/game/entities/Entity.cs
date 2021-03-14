using Godot;
using System;
using System.Collections.Generic;
using Object = Godot.Object;

public class Entity : KinematicBody2D
{
	public const float Gravity = 600;
	public World ParentWorld { get; private set; }
	public bool CanMove = true;
	public static readonly Vector2 DefaultGravity = new Vector2(0, Gravity);
	public Vector2 GravityVector = new Vector2(0, Gravity);
    public bool IsGravityEnabled = true;


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
        if (IsGravityEnabled)
		    Velocity += GravityVector * delta;
		Velocity = MoveAndSlide(Velocity, Vector2.Up, false);
		for (var i = 0; i < GetSlideCount(); i++)
		{
			var collision = GetSlideCollision(i);
            _OnCollision(collision);
		}

		_OnMovement(delta);
	}
	

    private bool _canAccelerate = true;

    public void AccelerateX(float x, float maxSpeed, float delta)
    {
        if (!CanMove) return;
        var movement = x * delta;
        var result = Mathf.Abs(Velocity.x + movement);
        
        if (!_canAccelerate)
        {
            if (result < maxSpeed)
            {
                _canAccelerate = true;
            }
            else return;
        }
        
        if (result > maxSpeed)
        {
            movement = (result - maxSpeed) * delta * Mathf.Sign(x);
            result = Mathf.Abs(Velocity.x + movement);
            if (result > maxSpeed)
                movement = 0;
            _canAccelerate = false;
        }

        Velocity.x += movement;
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
