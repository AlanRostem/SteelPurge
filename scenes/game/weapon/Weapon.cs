using Godot;

public class Weapon : Node2D
{
	[Export] public string DisplayName = "Weapon";

	[Export] public uint ClipSize;

	[Export] public uint DamagePerShot;
	[Export] public uint MeleeDamage = 80;
	[Export] public uint RateOfFire;
	[Export] public float HoverRecoilSpeed = 100;
	[Export] public float MinFallSpeedForRecoilHovering = -20;

	public WeaponAbility TacticalEnhancement { get; set; }
	public FiringDevice FiringDevice { get; set; }

	private bool _isFiring = false;
	public Player OwnerPlayer;
	private bool _isHoldingTrigger = false;
	public bool IsMeleeAttacking = false;

	public bool MeleeHitBoxEnabled
	{
		get => !_meleeShape.Disabled;
		set => _meleeShape.Disabled = !value;
	}

	private Timer _meleeCooldownTimer;
	private Timer _meleeDurationTimer;
	private CollisionShape2D _meleeShape;

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

	[Signal]
	public delegate void OnMeleeHit(VulnerableHitbox hitBox);


	public override void _Ready()
	{
		_meleeCooldownTimer = GetNode<Timer>("MeleeCooldownTimer");
		_meleeDurationTimer = GetNode<Timer>("MeleeDurationTimer");
		_meleeShape = GetNode<CollisionShape2D>("MeleeArea/CollisionShape2D");
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

		if (IsMeleeAttacking)
		{
			return;
		}
	
		if (Input.IsActionJustPressed("melee") && OwnerPlayer.IsOnFloor())
		{
			IsMeleeAttacking = true;
			_meleeShape.Disabled = false;
			_meleeCooldownTimer.Start();
			_meleeDurationTimer.Start();
			
			// TODO: Remove this after adding animations
			Position = new Vector2(8 * Scale.x, 0);
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
		IsMeleeAttacking = false;
	}

	private void _OnMeleeDurationTimerTimeout()
	{
		_meleeShape.Disabled = true;
		// TODO: Remove this after adding animations
		Position = new Vector2(0, 0);
	}

	private void _OnMeleeAreaHitBoxEntered(object area)
	{
		if (area is CriticalHitbox)
			return;

		var hitBox = (VulnerableHitbox) area;
		hitBox.TakeHit(MeleeDamage);
		EmitSignal(nameof(OnMeleeHit), hitBox);
	}
}
