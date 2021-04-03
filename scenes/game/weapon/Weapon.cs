using Godot;

public class Weapon : Node2D
{
	[Export] public string DisplayName = "Weapon";

	[Export] public uint ClipSize;

	[Export] public uint DamagePerShot;
	[Export] public uint RateOfFire;
	[Export] public float HoverRecoilSpeed = 100;
	[Export] public float MinFallSpeedForRecoilHovering = -20;

	public TacticalAbility TacticalEnhancement { get; set; }
	public FiringDevice FiringDevice { get; set; }

	private bool _isFiring = false;
	public Player OwnerPlayer;
	private bool _isHoldingTrigger = false;
	private bool _isMeleeAttacking = false;
	private bool _isWaitingForFire = false;

	private Timer _meleeToFireCheckTimer;
	private Timer _meleeCooldownTimer;

	public bool IsFiring
	{
		get => _isFiring;
		set => _isFiring = value;
	}

	public void OnSwap()
	{
		_isFiring = false;
		EmitSignal(nameof(CancelFire));
		Visible = false;
		SetProcess(false);
	}

	public void OnEquip()
	{
		Visible = true;
		SetProcess(true);
		if (Scale.x != OwnerPlayer.HorizontalLookingDirection)
			Scale = new Vector2(OwnerPlayer.HorizontalLookingDirection, 1);
		if (Rotation != OwnerPlayer.AimAngle)
			Rotation = OwnerPlayer.AimAngle;
	}

	[Signal]
	public delegate void Fired();

	[Signal]
	public delegate void TriggerFire();


	[Signal]
	public delegate void CancelFire();

	[Signal]
	public delegate void DamageDealt(uint damage, VulnerableHitbox target);


	public override void _Ready()
	{
		_meleeToFireCheckTimer = GetNode<Timer>("MeleeToFireCheckTimer");
		_meleeCooldownTimer = GetNode<Timer>("MeleeCooldownTimer");
	}

	private void Fire()
	{
		if (!_isHoldingTrigger)
		{
			_isFiring = false;
			EmitSignal(nameof(CancelFire));
			return;
		}

		EmitSignal(nameof(Fired));
		if (!(OwnerPlayer.Velocity.y > MinFallSpeedForRecoilHovering) || !OwnerPlayer.IsAimingDown) return;
		OwnerPlayer.Velocity.y = -HoverRecoilSpeed;
	}


	public override void _Process(float delta)
	{
		if (Scale.x != OwnerPlayer.HorizontalLookingDirection)
		{
			Scale = new Vector2(OwnerPlayer.HorizontalLookingDirection, 1);
		}

		_isHoldingTrigger = Input.IsActionPressed("fire");

		var triggerHeld = Input.IsActionJustPressed("fire");
		if (triggerHeld)
		{
			if (!_isWaitingForFire)
			{
				_isWaitingForFire = true;
				_meleeToFireCheckTimer.Start();
			}
		}

		if (Input.IsActionJustReleased("fire") && !_isFiring)
		{
			if (!_isMeleeAttacking)
			{
				_meleeCooldownTimer.Start();
				_meleeToFireCheckTimer.Stop();
				_isMeleeAttacking = true;
				_isWaitingForFire = false;
					
				// TODO: Replace with melee functionality
				GD.Print("Melee Attack!");
			}
		}

		if (_isWaitingForFire)
			return;
		
		if (OwnerPlayer.IsAimingDown)
		{
			Rotation = OwnerPlayer.HorizontalLookingDirection * Mathf.Pi / 2f;
		}
		else if (OwnerPlayer.IsAimingUp)
		{
			Rotation = -OwnerPlayer.HorizontalLookingDirection * Mathf.Pi / 2f;
		}
		else
		{
			Rotation = 0;
		}

		if (_isHoldingTrigger)
		{
			if (_isFiring) return;
			_isFiring = true;
			Fire();
			EmitSignal(nameof(TriggerFire));
		}
	}

	private void _OnMeleeCooldownTimerTimeout()
	{
		_isMeleeAttacking = false;
	}

	private void _OnMeleeToFireCheckTimerTimeout()
	{
		_isWaitingForFire = false;
	}
}
