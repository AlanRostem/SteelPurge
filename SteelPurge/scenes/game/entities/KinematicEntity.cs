using System;
using Godot;
using Godot.Collections;

public class KinematicEntity : KinematicBody2D
{
	public enum CollisionMode
	{
		Move,
		Slide,
		Snap,
	}

	public float Gravity = 600;

	[Export] public CollisionMode CurrentCollisionMode = CollisionMode.Snap;
	[Export] public bool CanReceiveStatusEffect = true;
	[Export] public uint MaxHealth = 100;

	private bool _canAccelerate = true;
	public bool IsOnSlope { get; private set; }
	
	public World ParentWorld { get; protected set; }

	[Export] public bool CanMove = true;

	public static readonly Vector2 DefaultPerspectiveDownVector = Vector2.Down;

	public Vector2 PerspectiveDownVector
	{
		get => _perspectiveDownVector;
		set
		{
			_perspectiveDownVector = value;
			_perspectiveAngle = new Vector2(DefaultPerspectiveDownVector).AngleTo(_perspectiveDownVector);
			_snapVector = new Vector2(_perspectiveDownVector);
			//if (value.Length() != 1)
			//	throw new Exception();
		}
	}

	private Vector2 _perspectiveDownVector = Vector2.Down;
	private Vector2 _snapVector = Vector2.Down;
	private float _perspectiveAngle = 0;

	[Export] public bool IsGravityEnabled = true;

	private Vector2 _velocity;

	// TODO: Edit these to modify values based on gravity vector rotation
	public Vector2 Velocity
	{
		get => _velocity;
		protected set
		{
			_velocity = value.Rotated(_perspectiveAngle);
			if (value.y < 0)
				_snapVector = Vector2.Zero;
		}
	}

	public float VelocityX
	{
		get => _velocity.Rotated(_perspectiveAngle).x;
		set => _velocity = new Vector2(value, VelocityY).Rotated(_perspectiveAngle);
	}

	public float VelocityY
	{
		get => _velocity.Rotated(_perspectiveAngle).y;
		set
		{
			_velocity = new Vector2(VelocityX, value).Rotated(_perspectiveAngle);
			if (value < 0f)
				_snapVector = Vector2.Zero;
		}
	}

	[Signal]
	public delegate void HealthChanged(uint health);

	public override void _Ready()
	{
		ParentWorld = GetParent().GetParent().GetParent<World>();
		// TODO: Figure out why this gets set to (0, 0) in scene instancing
		PerspectiveDownVector = Vector2.Down;
	}

	
	public override void _PhysicsProcess(float delta)
	{
		if (CanMove)
			_OnMovement(delta);

		if (IsGravityEnabled)
			_velocity += PerspectiveDownVector * Gravity * delta;

		switch (CurrentCollisionMode)
		{
			case CollisionMode.Move:
				MoveAndCollide(_velocity * delta);
				break;
			case CollisionMode.Slide:
				_velocity = MoveAndSlide(_velocity, -_perspectiveDownVector);
				break;
			case CollisionMode.Snap:
				_velocity.y = MoveAndSlideWithSnap(_velocity, _snapVector * CustomTileMap.Size, -_perspectiveDownVector,
					true).y;
				break;
		}

		IsOnSlope = false;
		for (var i = 0; i < GetSlideCount(); i++)
		{
			var collision = GetSlideCollision(i);
			if (IsOnFloor() && collision.Normal.Rotated(_perspectiveAngle).y != -PerspectiveDownVector.y)
				IsOnSlope = true;
			_OnCollision(collision);
		}

		if (IsOnFloor())
			_snapVector = new Vector2(_perspectiveDownVector);
	}


	/// <summary>
	/// When CanMove == true the entity's x-velocity is set to zero
	/// </summary>
	public void StopMovingX()
	{
		if (CanMove)
			VelocityX = 0;
	}

	public void AccelerateX(float x, float maxSpeed, float delta)
	{
		if (!CanMove) return;
		var movement = x * delta;
		var result = Mathf.Abs(VelocityX + movement);
		var differingDir = Mathf.Sign(movement) != Mathf.Sign(VelocityX);

		if (!_canAccelerate)
		{
			if (result < maxSpeed || differingDir)
			{
				_canAccelerate = true;
			}
			else return;
		}

		if (result > maxSpeed && !differingDir)
		{
			movement = (result - maxSpeed) * delta * Mathf.Sign(x);
			result = Mathf.Abs(VelocityX + movement);
			if (result > maxSpeed)
				movement = 0;
			_canAccelerate = false;
		}

		VelocityX += movement;
	}

	public void MoveX(float x)
	{
		if (CanMove)
			VelocityX = x;
	}

	public void MoveY(float y)
	{
		if (CanMove)
			VelocityY = y;
	}

	public virtual void _OnCollision(KinematicCollision2D collider)
	{
	}

	protected virtual void _OnMovement(float delta)
	{
	}
	
	public void ApplyForce(Vector2 knockBackForce)
	{
		if (!CanMove) return;
		Velocity += knockBackForce;
	}
}
