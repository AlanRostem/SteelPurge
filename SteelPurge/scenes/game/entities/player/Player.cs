using Godot;

public class Player : KinematicEntity
{
	public static uint ScrapDepletionPerDeath = 50;
	public static readonly uint HealthRegenCount = 15;

	private static readonly float KnockBackSpeed = 100;
	private static readonly float MaxMovementSpeed = 250;

	private static readonly float MaxWalkSpeed = 100;
	private static readonly float WalkSpeedGround = 360;
	private static readonly float MaxWalkSpeedFiring = 35;

	private static readonly float MaxJumpHeight = CustomTileMap.Size * 6;
	private static readonly float MinJumpHeight = CustomTileMap.Size;
	private static readonly float JumpDuration = .5f;

	private float _currentJumpSpeed;
	private float _minJumpSpeed;

	private static readonly float SlideFriction = 0.1f;
	private static readonly float SlideFrictionJump = 0.85f;
	private static readonly float WalkFriction = 0.95f;

	private static readonly float MaxCrouchSpeed = 20;
	private static readonly float MaxSlideMagnitude = 460;
	private static readonly float SlideDecreasePerSlide = 120;
	private static readonly float SlideIncreasePerSecond = 280;

	public float CurrentSlideMagnitude = MaxSlideMagnitude;

	public float CurrentMaxSpeed = MaxWalkSpeed;
	private bool _hasJumpedOnSlide = false;

	private bool _left = false;
	private bool _right = false;
	private bool _jump = false;
	private bool _aim = false;
	public bool CanTakeDamage = true;
	public bool CanAimDown = true;
	public bool CanSwapDirection = true;

	public bool IsInvulnerable = false;
	public bool IsAimingUp = false;
	public bool IsAimingDown = false;
	public bool IsRamSliding = false;
	private bool _isStunned = false;
	public float HorizontalLookingDirection = 1;
	public float MovingDirection = 1;
	public float AimAngle = 0;
	public bool IsWalking = false;
	public bool IsJumping = false;
	public bool IsSliding = false;
	private bool _isRoofAbove = false;

	private CollisionShape2D _bodyShape;
	private CollisionShape2D _roofDetectorShape;
	private Timer _respawnTimer;
	private Timer _slideDurationTimer;
	public Inventory PlayerInventory;


	public override void _Ready()
	{
		ParentWorld = GetParent<World>();
		Health = 100;
		PlayerInventory = GetNode<Inventory>("Inventory");
		_bodyShape = GetNode<CollisionShape2D>("BodyShape");
		_roofDetectorShape = GetNode<CollisionShape2D>("RoofDetector/UpperBodyShape");
		_respawnTimer = GetNode<Timer>("RespawnTimer");
		_slideDurationTimer = GetNode<Timer>("SlideDurationTimer");

		Gravity = 2 * MaxJumpHeight / Mathf.Pow(JumpDuration, 2);
		_currentJumpSpeed = Mathf.Sqrt(2 * Gravity * MaxJumpHeight);
		_minJumpSpeed = Mathf.Sqrt(2 * Gravity * MinJumpHeight);
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
		// TODO: Implement additional functionality after Prototype 1

		var oldPos = new Vector2(Position);
		Position = ParentWorld.CurrentSegment.ReSpawnPoint;
		ResetAllStates();
		InitiateRespawnSequence();

		var scrap = ParentWorld.Entities.SpawnEntityDeferred<Scrap>(EntityPool.ScrapScene, oldPos);
		scrap.Count = ScrapDepletionPerDeath;
		PlayerInventory.LoseScrap(ScrapDepletionPerDeath);
	}

	public void ResetAllStates()
	{
		Health = 100;
		Velocity = new Vector2();
		ClearStatusEffects();

		// TODO: Reset invulnerability 
	}


	/// <summary>
	/// Disable all movement and collision for a brief moment
	/// </summary>
	public void InitiateRespawnSequence()
	{
		_bodyShape.SetDeferred("disabled", true);
		IsGravityEnabled = false;
		CanMove = false;
		_respawnTimer.Start();
	}

	public override void TakeDamage(uint damage, Vector2 direction)
	{
		if (!IsInvulnerable)
		{
			if (direction.x != 0 || direction.y != 0)
			{
				Velocity = (new Vector2(KnockBackSpeed * direction.x, -_currentJumpSpeed / 2));
				if (!CanTakeDamage) return;

				if (PlayerInventory.EquippedWeapon.TacticalEnhancement.IsActive)
					PlayerInventory.EquippedWeapon.TacticalEnhancement.DeActivate();

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
	private bool _canCancelSlide = true;

	private void _ProcessInput()
	{
		if (_isStunned)
		{
			_left = false;
			_right = false;
			_jump = false;
			_slide = false;
			IsAimingDown = false;
			IsAimingUp = false;
			return;
		}

		_left = Input.IsActionPressed("left");
		_right = Input.IsActionPressed("right");
		_jump = Input.IsActionPressed("jump");
		IsAimingUp = Input.IsActionPressed("aim_up");

		if (_canCancelSlide)
		{
			_slide = Input.IsActionPressed("slide");
		}
		
		if (Input.IsActionJustPressed("aim_down") && CanAimDown)
		{
			//IsAimingUp = IsActionPressed("aim_up");
			IsAimingDown = !IsAimingDown;
			if (IsAimingDown)
				IsAimingUp = false;
		}

		if (Input.IsActionJustPressed("aim"))
		{
			EmitSignal(nameof(TriggerAimSwap));
			_aim = !_aim;
			HorizontalLookingDirection = -HorizontalLookingDirection;
			if (!IsJumping && IsAimingDown)
				IsAimingDown = false;
		}
	}

	private void Walk(int direction, bool canSwapDirOnMove, float delta)
	{
		AccelerateX(direction * WalkSpeedGround, CurrentMaxSpeed, delta);
		MovingDirection = direction;
		if (canSwapDirOnMove)
		{
			if (!IsSliding && IsOnFloor() && Mathf.Sign(Velocity.x) != direction)
			{
				Velocity.x *= -1;
			}

			HorizontalLookingDirection = direction;
		}

		IsWalking = true;
	}

	void Crouch()
	{
		_roofDetectorShape.SetDeferred("disabled", false);
		var shape = (CapsuleShape2D)_bodyShape.Shape;
		shape.Height = 1;
		_bodyShape.Position = new Vector2(0, 4.5f);
	}

	void Stand()
	{
		if (_isRoofAbove || !_canCancelSlide)
			return;
		IsSliding = false;
		_roofDetectorShape.SetDeferred("disabled", true);
		var shape = (CapsuleShape2D)_bodyShape.Shape;
		shape.Height = 10;
		_bodyShape.Position = new Vector2(0, 0);
	}

	protected override void _OnMovement(float delta)
	{
		bool isOnFloor = IsOnFloor();
		_ProcessInput();

		var canSwapDirOnMove = !PlayerInventory.EquippedWeapon.IsFiring && !_aim || IsAimingUp || IsAimingDown;
		canSwapDirOnMove = canSwapDirOnMove && CanSwapDirection;
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
						_canCancelSlide = false;
						_slideDurationTimer.Start();
					}
					else
					{
						CurrentSlideMagnitude = MaxCrouchSpeed;
					}
				}
				else
					CurrentMaxSpeed = MaxCrouchSpeed;

				IsSliding = true;
				Crouch();
			}
		}
		else
		{
			if (IsSliding && IsOnFloor())
				Velocity.x = Mathf.Lerp(Velocity.x, MovingDirection * MaxWalkSpeed, 0.99f);
			if (IsSliding)
				Stand();
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

		// Affects speed when player is attacking
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
			if (PlayerInventory.EquippedWeapon.IsFiring && isOnFloor && !IsAimingUp && !IsAimingDown)
				CurrentMaxSpeed = MaxWalkSpeedFiring;
			else if (PlayerInventory.EquippedWeapon.IsMeleeAttacking && isOnFloor)
				Velocity.x = 0;
			else
				CurrentMaxSpeed = MaxWalkSpeed;
			if (velX > CurrentMaxSpeed && IsOnFloor())
				Velocity.x = Mathf.Lerp(Velocity.x, MovingDirection * CurrentMaxSpeed, SlideFriction);
		}

		if (_left && !_right)
		{
			Walk(-1, canSwapDirOnMove, delta);
		}
		else if (_right && !_left)
		{
			Walk(1, canSwapDirOnMove, delta);
		}
		else
		{
			if (isOnFloor)
			{
				if (!IsSliding)
				{
					if (velX <= MaxWalkSpeed + 0.1f)
						Velocity.x = Mathf.Lerp(Velocity.x, 0, WalkFriction);
					else
						Velocity.x = Mathf.Lerp(Velocity.x, 0, SlideFriction);
				}
			}

			IsWalking = false;
		}

		IsJumping = !isOnFloor;

		Velocity.x = Mathf.Clamp(-MaxMovementSpeed, Velocity.x, MaxMovementSpeed);
		Velocity.y = Mathf.Clamp(-MaxMovementSpeed, Velocity.y, MaxMovementSpeed);

		if (Velocity.y < -_minJumpSpeed && Input.IsActionJustReleased("jump"))
			MoveY(-_minJumpSpeed);

		// Slide melee if the player has enough momentum
		if (IsSliding)
		{
			if (!PlayerInventory.EquippedWeapon.MeleeHitBoxEnabled && IsMovingFast() && isOnFloor && !IsRamSliding)
			{
				PlayerInventory.EquippedWeapon.MeleeHitBoxEnabled = true;
				IsRamSliding = true;
			}
		}

		if (IsRamSliding && !IsMovingFast())
		{
			if (PlayerInventory.EquippedWeapon.MeleeHitBoxEnabled && !PlayerInventory.EquippedWeapon.IsMeleeAttacking)
			{
				IsRamSliding = false;
				PlayerInventory.EquippedWeapon.MeleeHitBoxEnabled = false;
			}
		}

		if (!isOnFloor)
		{
			GravityVector = DefaultGravity;
			return;
		}

		if (IsWalking)
			IsAimingDown = false;

		if (_jump)
		{
			if (velX > MaxWalkSpeed)
			{
				if (!IsSliding)
					Velocity.x = Mathf.Lerp(Velocity.x, MovingDirection * CurrentMaxSpeed, SlideFrictionJump);
				else
				{
					if (!_hasJumpedOnSlide)
					{
						_hasJumpedOnSlide = true;
					}
					else
					{
						Velocity.x = Mathf.Lerp(Velocity.x, MovingDirection * CurrentMaxSpeed, SlideFrictionJump);
						_hasJumpedOnSlide = false;
					}
				}
			}

			if (IsSliding)
			{
				_canCancelSlide = true;
				Stand();
				_slideDurationTimer.Stop();
			}
			MoveY(-_currentJumpSpeed);
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


	private void _OnHitBoxHit(uint damage, Vector2 direction)
	{
		TakeDamage(damage, direction);
	}

	private void _OnRoofDetectorBodyEntered(object body)
	{
		_isRoofAbove = true;
	}

	private void _OnRoofDetectorBodyExited(object body)
	{
		_isRoofAbove = false;
	}

	private void _EndRespawnSequence()
	{
		_bodyShape.SetDeferred("disabled", false);
		IsGravityEnabled = true;
		CanMove = true;
	}

	public bool IsMovingFast()
	{
		return Mathf.Abs(Velocity.x) >= MaxWalkSpeed - 0.1f;
	}


	private void _OnCanCancelSlide()
	{
		_canCancelSlide = true;
	}
}
