using System;
using Godot;

public class Player : Entity
{
	public static readonly uint HealthRegenCount = 15;
	
	private static readonly float MaxMovementSpeed = 250;
	
	private static readonly float MaxWalkSpeed = 80;
	private static readonly float WalkSpeedGround = 330;
	private static readonly float MaxWalkSpeedFiring = 35;
	
	private static readonly float MaxJumpSpeed = 850;
	private static readonly float MinJumpSpeed = 100;

	private static readonly float MaxSlideMagnitude = 320;
	private static readonly float MaxCrouchSpeed = 20;
	private static readonly float SlideDecreasePerSlide = 120;
	private static readonly float SlideIncreasePerSecond = 280;
	private static readonly float SlideFriction = 0.1f;
	private static readonly float SlideFrictionJump = 0.85f;

	public float CurrentMaxSpeed = MaxWalkSpeed;
	
	public float CurrentSlideMagnitude = MaxSlideMagnitude;

	private bool _left = false;
	private bool _right = false;
	private bool _jump = false;
	private bool _aim = false;
	public bool CanTakeDamage = true;
	public bool IsInvulnerable = false;
	public bool IsAimingUp = false;
	public bool IsAimingDown = false;
	private bool _isStunned = false;
	public float HorizontalLookingDirection = 1;
	public float MovingDirection = 1;
	public float AimAngle = 0;
	public bool IsWalking = false;
	public bool IsJumping = false;
	public bool IsHoldingTrigger = false;
	public bool DidReload = false;
	public bool IsSliding = false;

	private Weapon _weapon;
	public Inventory PlayerInventory;

	public Weapon EquippedWeapon
	{
		get => _weapon;
		set
		{
			_weapon?.QueueFree();
			_weapon = value;
			_weapon.OwnerPlayer = this;
			// TODO: Call AddChild normally after tests
			CallDeferred("add_child", _weapon);
			EmitSignal(nameof(WeaponEquipped), value);
		}
	}


	public override void _Ready()
	{
		base._Ready();
		Health = 100;
		PlayerInventory = GetNode<Inventory>("Inventory");

		// TODO: Put this in static declarations 
		Gravity = 800; 
	}

	[Signal]
	public delegate void WeaponEquipped(Weapon weapon);
	
	[Signal]
	public delegate void WeaponAddedToInventory(Weapon weapon);

	[Signal]
	public delegate void ScrapCountChanged(uint count);

	[Signal]
	public delegate void OrdinanceFuelCountChanged(uint count, Inventory.OrdinanceFuelType type);

	[Signal]
	private delegate void TriggerAimSwap();

	[Signal]
	private delegate void TriggerRegenCooldown();

	[Signal]
	private delegate void CancelRegen();

	[Signal]
	private delegate void TriggerDamageReceptionCooldown();

	[Signal]
	private delegate void TriggerInvincibility();

	[Signal]
	private delegate void Died();

	private void _Die()
	{
		EmitSignal(nameof(Died));
	}
	
	public void KnowInventoryScrapCount(uint count)
	{
		EmitSignal(nameof(ScrapCountChanged), count);
	}

	public void KnowInventoryOrdinanceFuelCount(uint count, Inventory.OrdinanceFuelType type)
	{
		EmitSignal(nameof(OrdinanceFuelCountChanged), count, type);
	}

	public override void TakeDamage(uint damage, float direction = 0)
	{
		if (!IsInvulnerable)
		{
			if (direction != 0)
			{
				Velocity = (new Vector2(MaxWalkSpeed * direction, -MinJumpSpeed));
				if (!CanTakeDamage) return;

				IsInvulnerable = true;
				_isStunned = true;
			}

			EmitSignal(nameof(TriggerDamageReceptionCooldown));
			EmitSignal(nameof(TriggerInvincibility));
		}
		else
		{
			return;
		}
		
		if (!CanTakeDamage) return;

		EmitSignal(nameof(CancelRegen));
		EmitSignal(nameof(TriggerRegenCooldown));

		if (damage >= Health)
		{
			Health = 0;
			_Die();
		}
		else
		{
			Health -= damage;
		}
	}

	private bool _slide = false;

	private void _ProcessInput()
	{
		if (_isStunned) return;
		_left = Input.IsActionPressed("left");
		_right = Input.IsActionPressed("right");
		_jump = Input.IsActionPressed("jump");
		_slide = Input.IsActionPressed("slide");

		if (!EquippedWeapon.IsFiring && Input.IsActionJustPressed("aim_down"))
		{
			//IsAimingUp = IsActionPressed("aim_up");
			IsAimingDown = !IsAimingDown;
		}

		if (Input.IsActionJustPressed("aim"))
		{
			EmitSignal(nameof(TriggerAimSwap));
			_aim = !_aim;
			HorizontalLookingDirection = -HorizontalLookingDirection;
			if (!IsJumping && IsAimingDown)
				IsAimingDown = false;
		}

		DidReload = Input.IsActionJustPressed("reload");
		IsHoldingTrigger = Input.IsActionPressed("fire");
	}


	protected override void _OnMovement(float delta)
	{
		bool isOnFloor = IsOnFloor();
		_ProcessInput();

		var canSwapDirOnMove = !EquippedWeapon.IsFiring && !_aim || IsAimingUp || IsAimingDown;
		var velX = Mathf.Abs(Velocity.x);

		if (_slide)
		{
			if (!IsSliding && isOnFloor)
			{
				if (velX > 0)
				{
					CurrentMaxSpeed = CurrentSlideMagnitude;
					if (CurrentSlideMagnitude > MaxCrouchSpeed)
					{
						CurrentSlideMagnitude -= SlideDecreasePerSlide;
					}
					else
					{
						CurrentSlideMagnitude = MaxCrouchSpeed;
					}
				}
				else
					CurrentMaxSpeed = MaxCrouchSpeed;

				IsSliding = true;
			}
		}
		else
		{
			if (IsSliding && IsOnFloor())
				Velocity.x = Mathf.Lerp(Velocity.x, MovingDirection * MaxWalkSpeed, 0.99f);
			IsSliding = false;
		}

		if (isOnFloor && !IsSliding)
		{
			if (CurrentSlideMagnitude < MaxSlideMagnitude)
			{
				CurrentSlideMagnitude += SlideIncreasePerSecond * delta;
			}
			else
			{
				CurrentSlideMagnitude = MaxSlideMagnitude;
			}
		}

		StopOnSlope = !IsSliding;

		if (IsSliding && isOnFloor)
		{
			CurrentMaxSpeed = Mathf.Lerp(CurrentMaxSpeed, MaxCrouchSpeed, SlideFriction);
			if (velX < MaxCrouchSpeed + 0.1)
				Velocity.x = Mathf.Lerp(Velocity.x, 0, SlideFriction);
			else
				Velocity.x = Mathf.Lerp(Velocity.x, MovingDirection * CurrentMaxSpeed, SlideFriction);
		}
		else
		{
			if (EquippedWeapon.IsFiring && isOnFloor && !IsAimingUp && !IsAimingDown)
				CurrentMaxSpeed = MaxWalkSpeedFiring;
			else
				CurrentMaxSpeed = MaxWalkSpeed;
			if (velX > CurrentMaxSpeed && IsOnFloor())
				Velocity.x = Mathf.Lerp(Velocity.x, MovingDirection * CurrentMaxSpeed, SlideFriction);
		}


		if (_left && !_right)
		{
			AccelerateX(-WalkSpeedGround, CurrentMaxSpeed, delta);
			MovingDirection = -1;
			if (canSwapDirOnMove)
				HorizontalLookingDirection = -1;
			IsWalking = true;
		}

		else if (_right && !_left)
		{
			AccelerateX(WalkSpeedGround, CurrentMaxSpeed, delta);
			MovingDirection = 1;
			if (canSwapDirOnMove)
				HorizontalLookingDirection = 1;
			IsWalking = true;
		}
		else
		{
			if (isOnFloor)
			{
				if (!IsSliding)
				{
					if (velX <= MaxWalkSpeed + 0.1f)
						Velocity.x = Mathf.Lerp(Velocity.x, 0, .95f);
					else
						Velocity.x = Mathf.Lerp(Velocity.x, 0, .1f);
				}
			}

			IsWalking = false;
		}

		IsJumping = !isOnFloor;

		Velocity.x = Mathf.Clamp(-MaxMovementSpeed, Velocity.x, MaxMovementSpeed);
		Velocity.y = Mathf.Clamp(-MaxMovementSpeed, Velocity.y, MaxMovementSpeed);

		if (Velocity.y < -MinJumpSpeed && Input.IsActionJustReleased("jump"))
			MoveY(-MinJumpSpeed);
		
		if (!isOnFloor)
		{
			GravityVector = DefaultGravity;
			return;
		}

		if (IsWalking)
			IsAimingDown = false;

		if (_jump)
		{
			MoveY(-MaxJumpSpeed);
			if (velX > MaxWalkSpeed)
				Velocity.x *= SlideFrictionJump;
		}
	}

	public override void _OnCollision(KinematicCollision2D collider)
	{
		if (collider.Normal.y != -1 && IsOnFloor() && !IsSliding)
		{
			GravityVector = -collider.Normal;
		}
		else
		{
			GravityVector = DefaultGravity;
		}
	}
	
	private void _OnRegen()
	{
		if (HealthRegenCount + Health < 100)
		{
			Health += HealthRegenCount;
		}
		else
		{
			Health = 100;
			EmitSignal(nameof(CancelRegen));
		}
	}

	private void _OnSwapTimeOver()
	{
		_aim = false;
	}

	private void _OnCanTakeDamage()
	{
		_isStunned = false;
	}

	private void _OnInvincibilityEnd()
	{
		IsInvulnerable = false;
	}
	
	
	private void _OnHitBoxHit(uint damage, float direction)
	{
		TakeDamage(damage, direction);
	}
}

