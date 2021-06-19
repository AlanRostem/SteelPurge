using Godot;

public class Player : KinematicEntity
{
	public enum MovementState
	{
		Walk,
		Slide,
		Crouch,
		Airborne,
	}
	
	private static readonly float KnockBackSpeed = 100;
	// private static readonly float MaxMovementSpeed = 250;

	public static readonly float WalkSpeed = 120;

	private static readonly float WalkAcceleration = 360;

	// private static readonly float WalkSpeedAir = 60;
	// private static readonly float MaxWalkSpeedFiring = 35;
	private static float DashSpeed = 250;

	private static readonly float JumpSpeed = 255;
	private static readonly float MinJumpSpeed = 100;

	private static readonly float SlideFriction = 0.02f;
	private static readonly float AirFriction = 0.01f;

	// private static readonly float SlideFrictionJump = 0.85f;
	private static readonly float WalkFriction = 0.95f;

	private static readonly float CrouchSpeed = 20;

	private static readonly float SlideSpeed = 230; // 460;
	// private static readonly float SlideDecreasePerSlide = 120;
	// private static readonly float SlideIncreasePerSecond = 280;


	private bool _left = false;
	private bool _right = false;
	private bool _jump = false;
	private bool _notJump = false;
	private bool _dash = false;

	public bool CanTakeDamage = true;
	public bool CanAimDown = true;
	public bool CanAimUp = true;
	public bool CanSwapDirection = true;
	public MovementState CurrentMovementState { get; private set; }

	public bool IsInvulnerable = false;
	public bool IsAimingUp = false;
	public bool IsAimingDown = false;
	public bool IsRamSliding = false;
	private bool _isStunned = false;
	public float HorizontalLookingDirection = 1;
	public float AimAngle = 0;
	public bool IsWalking = false;
	public bool IsJumping = false;
	public bool IsSliding = false;
	public bool IsCrouching = false;
	private bool _isRoofAbove = false;

	public int MovingDirection { get; private set; }

	private CollisionShape2D _bodyShape;
	private CollisionShape2D _roofDetectorShape;
	private Timer _respawnTimer;
	private Timer _slideDurationTimer;
	public Inventory PlayerInventory;
	private Camera2D _camera;

	public override void _Ready()
	{
		ParentWorld = GetParent<World>();
		Health = MaxHealth;
		PlayerInventory = GetNode<Inventory>("Inventory");
		_bodyShape = GetNode<CollisionShape2D>("BodyShape");
		_camera = GetNode<Camera2D>("PlayerCamera");
		_roofDetectorShape = GetNode<CollisionShape2D>("RoofDetector/UpperBodyShape");
		_respawnTimer = GetNode<Timer>("RespawnTimer");
		_slideDurationTimer = GetNode<Timer>("SlideDurationTimer");
	}

	[Signal]
	public delegate void WeaponEquipped(Weapon weapon);
	
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

		Position = ParentWorld.CurrentSegment.ReSpawnPoint;
		ResetAllStates();
		InitiateRespawnSequence();
	}

	public void ResetAllStates()
	{
		Health = MaxHealth;
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

	public override void TakeDamage(uint damage, Vector2 direction, bool isCritical = false)
	{
		if (!IsInvulnerable)
		{
			if (direction.x != 0 || direction.y != 0)
			{
				Velocity = (new Vector2(KnockBackSpeed * direction.x, -JumpSpeed / 2));
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
		_jump = Input.IsActionJustPressed("jump");
		_notJump = Input.IsActionJustReleased("jump");
		_dash = Input.IsActionJustPressed("dash");
		IsAimingUp = Input.IsActionPressed("aim_up") && CanAimUp;

		if (_canCancelSlide)
		{
			_slide = Input.IsActionPressed("slide");
		}

		if (Input.IsActionJustPressed("aim_down") && CanAimDown)
		{
			//IsAimingUp = IsActionPressed("aim_up");
			IsAimingDown = !IsAimingDown;
			if (IsAimingDown)
			{
				CanAimUp = false;
				IsAimingUp = false;
			}
			else
			{
				CanAimUp = true;
			}
		}
	}

	private void Walk(int direction, float delta)
	{
		AccelerateX(direction * WalkAcceleration, WalkSpeed, delta);
		MovingDirection = direction;
		if (!PlayerInventory.EquippedWeapon.IsFiring && CanSwapDirection || IsAimingDown || IsAimingUp)
		{
			HorizontalLookingDirection = direction;
			if (IsOnFloor())
			{
				CanAimUp = true;
				IsAimingDown = false;
			}
		}

		if (!IsSliding && IsOnFloor() && Mathf.Sign(VelocityX) != direction && !IsMovingTooFast() &&
			CurrentMovementState == MovementState.Walk)
		{
			_StopWalking();
		}

		IsWalking = true;
	}

	private void NegateSlide(int direction, float delta)
	{
		AccelerateX(direction * WalkAcceleration, SlideSpeed, delta);
	}

	private void Sneak(int direction, float delta)
	{
		MoveX(direction * CrouchSpeed);
		MovingDirection = direction;
		if (!PlayerInventory.EquippedWeapon.IsFiring && CanSwapDirection || IsAimingDown || IsAimingUp)
		{
			HorizontalLookingDirection = direction;
			if (IsOnFloor())
			{
				CanAimUp = true;
				IsAimingDown = false;
			}
		}

		if (IsOnFloor() && Mathf.Sign(VelocityX) != direction && !IsMovingTooFast() &&
			CurrentMovementState == MovementState.Slide)
		{
			_StopWalking();
		}

		IsCrouching = true;
	}

	void Crouch()
	{
		_roofDetectorShape.SetDeferred("disabled", false);
		var shape = (CapsuleShape2D) _bodyShape.Shape;
		shape.Height = 1;
		_bodyShape.Position = new Vector2(0, 4.5f);
		PlayerInventory.EquippedWeapon.Position = new Vector2(0, 4);
	}

	void Stand()
	{
		if (_isRoofAbove || !_canCancelSlide)
			return;
		IsSliding = false;
		_roofDetectorShape.SetDeferred("disabled", true);
		var shape = (CapsuleShape2D) _bodyShape.Shape;
		shape.Height = 10;
		_bodyShape.Position = new Vector2(0, 0);
		PlayerInventory.EquippedWeapon.Position = new Vector2(0, 0);
	}

	private void _WalkMode(float delta)
	{
		if (_left && !_right)
		{
			Walk(-1, delta);
		}
		else if (!_left && _right)
		{
			Walk(1, delta);
		}
		else if (IsOnFloor())
		{
			_StopWalking();
		}
	}

	private void _SlideMode(float delta)
	{
		if (!IsMovingTooFast() && IsRamSliding)
		{
			IsRamSliding = false;
			PlayerInventory.EquippedWeapon.MeleeHitBoxEnabled = false;
		}

		if (IsOnFloor())
		{
			VelocityX = Mathf.Lerp(VelocityX, 0, SlideFriction);
			if (IsMovingTooFast())
			{
				IsRamSliding = true;
				PlayerInventory.EquippedWeapon.MeleeHitBoxEnabled = true;
			}
		}
		else
		{
			_AirborneMode(delta);
			CanAimDown = true;
			IsRamSliding = false;
			PlayerInventory.EquippedWeapon.MeleeHitBoxEnabled = false;
			return;
		}

		if (_left && !_right)
		{
			if (IsMovingFasterThanCrouch() && VelocityX > 0)
				NegateSlide(-1, delta);
		}
		else if (!_left && _right)
		{
			if (IsMovingFasterThanCrouch() && VelocityX < 0)
				NegateSlide(1, delta);
		}
	}

	private void _AirborneMode(float delta)
	{
		if (_notJump && IsJumping && VelocityY < -MinJumpSpeed)
		{
			VelocityY = -MinJumpSpeed;
			IsJumping = false;
		}
		
		if (!IsMovingTooFast())
			VelocityX = Mathf.Lerp(VelocityX, 0, AirFriction);
		if (_left && !_right)
		{
			if (IsMovingTooFast())
				Walk(-1, delta);
			else
			{
				if (!PlayerInventory.EquippedWeapon.IsFiring || IsAimingDown)
					HorizontalLookingDirection = -1;
				if (CanMove) MovingDirection = -1;
				Walk(-1, delta);
			}
		}
		else if (!_left && _right)
		{
			if (IsMovingTooFast())
				Walk(1, delta);
			else
			{
				if (!PlayerInventory.EquippedWeapon.IsFiring || IsAimingDown)
					HorizontalLookingDirection = 1;
				if (CanMove) MovingDirection = 1;
				Walk(1, delta);
			}
		}
	}

	private void _CrouchMode(float delta)
	{
		if (_left && !_right)
		{
			Sneak(-1, delta);
		}
		else if (!_left && _right)
		{
			Sneak(1, delta);
		}
		else
		{
			_StopWalking();
		}
	}

	protected override void _OnMovement(float delta)
	{
		_ProcessInput();

		if (_dash)
			_Dash();

		if (IsMovingTooFast() && CurrentMovementState != MovementState.Slide)
			_Slide();

		if (IsOnFloor())
		{
			if (CurrentMovementState == MovementState.Airborne)
				CurrentMovementState = MovementState.Walk;

			IsJumping = false;
			if (_jump)
				_Jump();

			if (_slide)
			{
				if (IsOnSlope || IsMovingFasterThanCrouch())
					_Slide();
				else
					_ActivateCrouchMode();
			}
			else if (!IsMovingTooFast() && (IsSliding || IsCrouching))
			{
				_StopSliding();
			}
		}
		else if (CurrentMovementState != MovementState.Slide)
		{
			CurrentMovementState = MovementState.Airborne;
		}

		switch (CurrentMovementState)
		{
			case MovementState.Walk:
				_WalkMode(delta);
				break;
			case MovementState.Slide:
				_SlideMode(delta);
				break;
			case MovementState.Airborne:
				_AirborneMode(delta);
				break;
			case MovementState.Crouch:
				_CrouchMode(delta);
				break;
		}
	}

	private void _StopWalking()
	{
		IsWalking = false;
		VelocityX = Mathf.Lerp(VelocityX, 0, WalkFriction);
	}

	private void _Dash()
	{
		if (!PlayerInventory.EquippedWeapon.CanDash) return;

		PlayerInventory.EquippedWeapon.PowerDash();

		if (!IsAimingDown && !IsAimingUp)
		{
			Velocity = new Vector2(-HorizontalLookingDirection * DashSpeed,
				-PlayerInventory.EquippedWeapon.HoverRecoilSpeed);
		}
		else if (IsAimingDown)
		{
			VelocityY = -DashSpeed;
		}
		else if (IsAimingUp)
		{
			VelocityY = DashSpeed;
		}
	}

	private void _Jump()
	{
		VelocityY = -JumpSpeed;
		IsJumping = true;
		if (CurrentMovementState != MovementState.Slide)
			CurrentMovementState = MovementState.Airborne;
	}

	private void _Slide()
	{
		if (!IsSliding && !IsOnSlope && !IsMovingTooFast())
		{
			IsRamSliding = true;
			PlayerInventory.EquippedWeapon.MeleeHitBoxEnabled = true;
			VelocityX = SlideSpeed * MovingDirection;
		}

		IsSliding = true;
		IsCrouching = false;
		IsAimingDown = false;
		CanAimUp = true;
		CanAimDown = false;
		Crouch();
		CurrentMovementState = MovementState.Slide;
		CurrentCollisionMode = CollisionMode.Slide;
	}

	private void _ActivateCrouchMode()
	{
		if (IsCrouching) return;
		IsCrouching = true;
		IsSliding = false;
		CanAimDown = true;
		Crouch();
		CurrentMovementState = MovementState.Crouch;
		CurrentCollisionMode = CollisionMode.Snap;
	}

	private void _StopSliding()
	{
		if (IsMovingTooFast()) return;
		CanAimDown = true;
		if (_isRoofAbove)
		{
			CurrentMovementState = MovementState.Crouch;
			CurrentCollisionMode = CollisionMode.Snap;
			return;
		}

		IsSliding = false;
		IsCrouching = false;
		Stand();
		CurrentMovementState = MovementState.Walk;
		CurrentCollisionMode = CollisionMode.Snap;
		if (IsRamSliding)
		{
			IsRamSliding = false;
			PlayerInventory.EquippedWeapon.MeleeHitBoxEnabled = false;
		}
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

	public bool IsMovingTooFast()
	{
		const float margin = 0.1f;
		return Mathf.Abs(VelocityX) >= WalkSpeed + margin;
	}

	public bool IsMovingFasterThanCrouch()
	{
		const float margin = 0.1f;
		return Mathf.Abs(VelocityX) >= CrouchSpeed + margin;
	}

	private void _OnCanCancelSlide()
	{
		_canCancelSlide = true;
	}

	public void SetCameraBounds(Rect2 bounds)
	{
		_camera.LimitLeft = (int) bounds.Position.x;
		_camera.LimitTop = (int) bounds.Position.y;
		_camera.LimitRight = (int) bounds.Size.x;
		_camera.LimitBottom = (int) bounds.Size.y;
	}
}
