using System;
using Godot;

public class Player : KinematicEntity
{
	public enum MovementState
	{
		Walk,
		Slide,
		Airborne,
		Dash
	}

	public static uint ScrapDepletionPerDeath = 50;
	public static readonly uint HealthRegenCount = 15;

	private static readonly float KnockBackSpeed = 100;
	// private static readonly float MaxMovementSpeed = 250;

	private static readonly float WalkSpeed = 100;

	private static readonly float WalkSpeedGround = 360;

	// private static readonly float WalkSpeedAir = 60;
	// private static readonly float MaxWalkSpeedFiring = 35;
	private static float DashSpeed = 300;

	private static readonly float JumpSpeed = 200;

	// private static readonly float SlideFriction = 0.1f;
	// private static readonly float SlideFrictionJump = 0.85f;
	private static readonly float WalkFriction = 0.95f;

	// private static readonly float MaxCrouchSpeed = 20;
	private static readonly float MaxSlideMagnitude = 460;
	// private static readonly float SlideDecreasePerSlide = 120;
	// private static readonly float SlideIncreasePerSecond = 280;

	public float CurrentSlideMagnitude = MaxSlideMagnitude;

	private bool _left = false;
	private bool _right = false;
	private bool _jump = false;
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
	private Camera2D _camera;

	public override void _Ready()
	{
		ParentWorld = GetParent<World>();
		Health = 100;
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
		_jump = Input.IsActionJustPressed("jump");
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
		AccelerateX(direction * WalkSpeedGround, WalkSpeed, delta);
		MovingDirection = direction;
		if (!PlayerInventory.EquippedWeapon.IsFiring && CanSwapDirection || IsAimingDown || IsAimingUp)
		{
			HorizontalLookingDirection = direction;
			if (IsOnFloor())
				IsAimingDown = false;
		}

		if (!IsSliding && IsOnFloor() && Mathf.Sign(Velocity.x) != direction && !IsMovingFast() && CurrentMovementState == MovementState.Walk)
		{
			_StopWalking();
		}

		IsWalking = true;
	}

	void Crouch()
	{
		_roofDetectorShape.SetDeferred("disabled", false);
		var shape = (CapsuleShape2D) _bodyShape.Shape;
		shape.Height = 1;
		_bodyShape.Position = new Vector2(0, 4.5f);
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
	}

	private void _AirborneMode(float delta)
	{
		if (_left && !_right)
		{
			if (IsMovingFast())
				Walk(-1, delta);
			else
				MoveX(-WalkSpeed);
		}
		else if (!_left && _right)
		{
			if (IsMovingFast())
				Walk(1, delta);
			else
				MoveX(WalkSpeed);
		}
	}

	private void _DashMode(float delta)
	{
		if (PlayerInventory.EquippedWeapon.CanDash)
		{
			_Dash();
			PlayerInventory.EquippedWeapon.PowerDash();
		}
	}

	protected override void _OnMovement(float delta)
	{
		_ProcessInput();

		if (IsOnFloor())
		{
			CurrentMovementState = MovementState.Walk;

			IsJumping = false;
			if (_jump)
				_Jump();
		}
		else
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
			case MovementState.Dash:
				_DashMode(delta);
				break;
		}
	}

	private void _StopWalking()
	{
		IsWalking = false;
		Velocity.x = Mathf.Lerp(Velocity.x, 0, WalkFriction);
	}

	private void _Dash()
	{
		if (!IsAimingDown && !IsAimingUp)
		{
			ApplyForce(new Vector2(-HorizontalLookingDirection * DashSpeed, 0));
			Velocity.y = -PlayerInventory.EquippedWeapon.HoverRecoilSpeed;
		}
		else if (IsAimingDown)
		{
			ApplyForce(new Vector2(0, -DashSpeed));
		}
		else if (IsAimingUp)
		{
			ApplyForce(new Vector2(0, DashSpeed));
		}
	}

	private void _Jump()
	{
		Velocity.y = -JumpSpeed;
		IsJumping = true;
		CurrentMovementState = MovementState.Airborne;
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
		return Mathf.Abs(Velocity.x) >= WalkSpeed - 0.1f;
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
		GD.Print(bounds);
	}
}
