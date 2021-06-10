using Godot;
using Godot.Collections;

public class KinematicEntity : KinematicBody2D
{
	public float Gravity = 600;

	[Export] public bool StopOnSlope = true;
	[Export] public bool CanReceiveStatusEffect = true;

	private bool _canAccelerate = true;
	public bool IsOnSlope { get; private set; }

	public enum StatusEffectType
	{
		Burn,
		None,
	}

	private static readonly PackedScene[] StatusEffectScenes =
	{
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/firewall/BurnEffect.tscn")
	};

	private readonly Dictionary<StatusEffectType, StatusEffect> _effects =
		new Dictionary<StatusEffectType, StatusEffect>();

	public World ParentWorld { get; protected set; }

	[Export] public bool CanMove = true;

	public static readonly Vector2 DefaultPerspectiveDownVector = Vector2.Down;

	[Export]
	public Vector2 PerspectiveDownVector
	{
		get => _perspectiveDownVector;
		set
		{
			_perspectiveDownVector = value;
			_perspectiveAngle = new Vector2(DefaultPerspectiveDownVector).AngleTo(_perspectiveDownVector);
		}
	}
	
	private Vector2 _perspectiveDownVector;
	private float _perspectiveAngle = 0;

	[Export] public bool IsGravityEnabled = true;

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

	private Vector2 _velocity;
	// TODO: Edit these to modify values based on gravity vector rotation
	public Vector2 Velocity { get => _velocity; protected set => _velocity = value.Rotated(_perspectiveAngle); }
	public float VelocityX 
	{ 
		get => _velocity.Rotated(_perspectiveAngle).x; 
		set => _velocity = new Vector2(value, VelocityY).Rotated(_perspectiveAngle); 
	}

	public float VelocityY
	{
		get => _velocity.Rotated(_perspectiveAngle).y; 
		set => _velocity = new Vector2(VelocityX, value).Rotated(_perspectiveAngle); 
	}

	[Signal]
	public delegate void HealthChanged(uint health);

	public override void _Ready()
	{
		ParentWorld = GetParent().GetParent().GetParent<World>();
	}

	public void ApplyStatusEffect(StatusEffectType type)
	{
		if (type == StatusEffectType.None || !CanReceiveStatusEffect)
			return;

		if (_effects.ContainsKey(type))
		{
			var effect = _effects[type];
			effect.ResetTime();
			return;
		}


		var newEffect = (StatusEffect) StatusEffectScenes[(int) type].Instance();
		_effects[type] = newEffect;
		AddChild(newEffect);
	}

	public void ClearStatusEffects()
	{
		foreach (var effect in _effects)
		{
			effect.Value.EmitSignal(nameof(StatusEffect.End));
		}
	}

	public virtual void TakeDamage(uint damage, Vector2 direction)
	{
		Health -= damage;
	}

	public override void _PhysicsProcess(float delta)
	{
		if (IsGravityEnabled)
			_velocity += PerspectiveDownVector * Gravity * delta;
		_velocity.y = MoveAndSlide(_velocity, -PerspectiveDownVector, StopOnSlope).y;
		IsOnSlope = false;
		for (var i = 0; i < GetSlideCount(); i++)
		{
			var collision = GetSlideCollision(i);
			if (IsOnFloor() && collision.Normal.y != -1)
				IsOnSlope = true;
			_OnCollision(collision);
		}

		_OnMovement(delta);
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

	public void RemoveStatusEffect(StatusEffectType type)
	{
		_effects.Remove(type);
	}

	public void ApplyForce(Vector2 knockBackForce)
	{
		if (!CanMove) return;
		Velocity += knockBackForce;
	}
}
