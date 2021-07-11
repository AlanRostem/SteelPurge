using Godot;

public class Player : LivingEntity
{
	[Signal]
	public delegate void WeaponEquipped(Weapon weapon);

	[Signal]
	public delegate void Died();
	
	[Signal]
	public delegate void ChronoDriftTriggered();
	
	public enum MovementState
	{
		Walk,
		Slide,
		Crouch,
		Airborne,
	}

	private static readonly PackedScene ChronoVectorScene = GD.Load<PackedScene>("res://scenes/game/entities/player/ChronoVector.tscn");

	public static readonly float KnockBackSpeed = 100;
	// private static readonly float MaxMovementSpeed = 250;

	public static readonly float WalkSpeed = 140;

	public static readonly float WalkAcceleration = 360;

	// private static readonly float WalkSpeedAir = 60;
	// private static readonly float MaxWalkSpeedFiring = 35;
	public static float DashSpeed = 250;

	public static readonly float JumpSpeed = 255;
	public static readonly float MinJumpSpeed = 100;

	public static readonly float SlideFriction = 0.02f;
	public static readonly float AirFriction = 0.006f;

	// private static readonly float SlideFrictionJump = 0.85f;
	public static readonly float WalkFriction = 0.95f;

	public static readonly float CrouchSpeed = 20;

	public static readonly float SlideSpeed = 260; // 460;
	// private static readonly float SlideDecreasePerSlide = 120;
	// private static readonly float SlideIncreasePerSecond = 280;


	private bool _left = false;
	private bool _right = false;
	private bool _jump = false;
	private bool _notJump = false;
	private bool _dash = false;
	private bool _chronoDrift = false;

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
	public bool IsWalking = false;
	public bool IsJumping = false;
	public bool IsSliding = false;
	public bool IsCrouching = false;
	private bool _isRoofAbove = false;

	public int MovingDirection { get; private set; }
	public bool IsRespawning { get; private set; }

	private CollisionShape2D _bodyShape;
	private CollisionShape2D _roofDetectorShape;
	private Timer _respawnTimer;
	private Timer _damageReceptionCooldownTimer;
	private Timer _invincibilityCooldownTimer;
	public Inventory PlayerInventory;
	private Camera2D _camera;
	private Label _speedometer;
	private int _speedKmH = 0;

	private ChronoVector _currentChronoVector;

	public override void _Init()
	{
		ParentWorld = GetParent<World>();
		Health = MaxHealth;
		PlayerInventory = GetNode<Inventory>("Inventory");
		_bodyShape = GetNode<CollisionShape2D>("BodyShape");
		_camera = GetNode<Camera2D>("PlayerCamera");
		_roofDetectorShape = GetNode<CollisionShape2D>("RoofDetector/UpperBodyShape");
		_respawnTimer = GetNode<Timer>("RespawnTimer");
		_damageReceptionCooldownTimer = GetNode<Timer>("DamageReceptionCooldownTimer");
		_invincibilityCooldownTimer = GetNode<Timer>("InvincibilityCooldownTimer");
		_speedometer = GetNode<Label>("CanvasLayer/Speedometer");
	}


	public override void Die()
	{
		Position = new Vector2(ParentWorld.CurrentReSpawnPoint);
		ResetAllStates();
		InitiateRespawnSequence();
		_currentChronoVector?.QueueFree();
		_currentChronoVector = null;
		EmitSignal(nameof(Died));
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
		IsGravityEnabled = false;
		CanMove = false;
		PlayerInventory.EquippedWeapon.CanFire = false;
		IsInvulnerable = false;
		_respawnTimer.Start();
		IsRespawning = true;
		
		_damageReceptionCooldownTimer.Stop();
		_invincibilityCooldownTimer.Stop();
	}

	public void TakeDamage(Vector2 direction, VulnerableHitbox.DamageType damageType = VulnerableHitbox.DamageType.Standard,
		bool isCritical = false)
	{
		TakeDamage(1, direction, damageType, isCritical);
	}

	public void BecomeInvincible()
	{
		IsInvulnerable = true;
		_damageReceptionCooldownTimer.Start();
		_invincibilityCooldownTimer.Start();
	}
	
	public override void TakeDamage(uint damage, Vector2 direction, VulnerableHitbox.DamageType damageType,
		bool isCritical = false)
	{
		EmitSignal(nameof(OnTakeDamage), damage, direction, damageType, isCritical);

		if (!IsInvulnerable)
		{
			if (direction.x != 0 || direction.y != 0)
			{
				Velocity = new Vector2(KnockBackSpeed * direction.x, -JumpSpeed / 2);
				if (!CanTakeDamage) return;

				var ability = PlayerInventory.EquippedWeapon.TacticalEnhancement;
				if (ability != null && ability.IsActive)
					ability.DeActivate();

				IsInvulnerable = true;
				_isStunned = true;
			}

			_damageReceptionCooldownTimer.Start();
			_invincibilityCooldownTimer.Start();
		}
		else
		{
			return;
		}

		if (!CanTakeDamage) return;

		if (damage >= Health)
		{
			Health = 0;
			Die();
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
		_chronoDrift = Input.IsActionJustPressed("chrono_drift");
		IsAimingUp = Input.IsActionPressed("aim_up") && CanAimUp;

		if (_canCancelSlide)
		{
			_slide = Input.IsActionPressed("slide");
		}

		if (Input.IsActionJustPressed("aim_down") && CanAimDown)
		{
			//IsAimingUp = IsActionPressed("aim_up");
			if (!IsOnFloor())
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
		if (CanSwapDirection || IsAimingDown || IsAimingUp)
		{
			HorizontalLookingDirection = direction;
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
		
		if (IsOnFloor())
		{
			CanAimUp = true;
			IsAimingDown = false;
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
			
			CanAimUp = true;
			IsAimingDown = false;
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
		
		if (IsOnFloor())
		{
			CanAimUp = true;
			IsAimingDown = false;
		}
	}

	protected override void _OnMovement(float delta)
	{
		_ProcessInput();

		if (_chronoDrift)
			ChronoDrift();
		
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
			else if (!IsMovingTooFast() && (IsSliding || IsCrouching) ||
					 _isRoofAbove && !IsMovingFasterThanCrouch())
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

		var speedKmH = Mathf.RoundToInt(Mathf.Abs(VelocityX) / CustomTileMap.Size * 3.6f);
		if (_speedKmH != speedKmH)
		{
			_speedKmH = speedKmH;
			_speedometer.Text = _speedKmH + " km/h";
		}
	}
	
	public void ChronoDrift()
	{
		if (_currentChronoVector != null)
		{
			Position = new Vector2(_currentChronoVector.Position);
			_currentChronoVector.QueueFree();
			_currentChronoVector = null;
			EmitSignal(nameof(ChronoDriftTriggered));
			ParentWorld.CurrentSegment.Entities.ReturnToPreviouslyCapturedEntityStates();
			return;
		}
		
		_currentChronoVector = (ChronoVector) ChronoVectorScene.Instance();
		_currentChronoVector.Position = new Vector2(Position);
		ParentWorld.CurrentSegment.Environment.AddChild(_currentChronoVector);
		_currentChronoVector.Connect(nameof(ChronoVector.Disappear), this, nameof(_OnChronoVectorDisappear));
		ParentWorld.CurrentSegment.Entities.CaptureCurrentEntityStates();
	}

	public void ClearChronoVector()
	{
		_currentChronoVector = null;
	}
	
	private void _OnChronoVectorDisappear()
	{
		_currentChronoVector = null;
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
		IsGravityEnabled = true;
		CanMove = true;
		PlayerInventory.EquippedWeapon.CanFire = true;
		IsRespawning = false;
		IsInvulnerable = false;
	}

	public bool IsMovingTooFast()
	{
		const float margin = 0.1f;
		return Mathf.Abs(VelocityX) >= WalkSpeed + margin;
	}

	public bool IsMovingFasterThanCrouch()
	{
		const float margin = 0.5f;
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
