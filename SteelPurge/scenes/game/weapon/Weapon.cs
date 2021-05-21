using System;
using Godot;

public class Weapon : Node2D
{
	private static PackedScene WeaponCollectibleScene
		= GD.Load<PackedScene>("res://scenes/game/entities/collectible/weapon/WeaponCollectible.tscn");

	[Export] public string DisplayName = "Weapon";

	[Export] public Texture CollectibleSprite;
	[Export] public Inventory.InventoryWeapon WeaponType;


	[Export] public uint DamagePerShot;
	[Export] public uint MeleeDamage = 80;
	[Export] public uint RateOfFire;
	[Export] public uint MaxAmmoCount = 6;
	[Export] public uint ReloadCount = 1;
	[Export] public bool AutoReloadEnabled = true;
	[Export] public float HoverRecoilSpeed = 100;
	[Export] public float MinFallSpeedForRecoilHovering = -20;
	[Export] public SpriteFrames PlayerSpriteFrames;

	[Signal]
	public delegate void Swapped();

	public bool Equipped { get; private set; }

	public WeaponAbility TacticalEnhancement { get; set; }
	public FiringDevice FiringDevice { get; set; }

	private bool _canFire = true;

	public bool CanFire
	{
		get => _canFire;
		set
		{
			_canFire = value;
			if (value) return;
			_isFiring = false;
			EmitSignal(nameof(CancelFire));
		}
	}

	private bool _isFiring = false;
	public Player OwnerPlayer;
	private bool _isHoldingTrigger = false;
	private bool _isReloading = false;
	private bool _isRecharging = false;
	public bool IsMeleeAttacking = false;

	public bool CanMelee = true;

	public uint CurrentAmmo
	{
		get => _currentAmmo;
		set
		{
			_ammoLabel.Text = value + " / " + MaxAmmoCount;
			_currentAmmo = value;
		}
	}

	private uint _currentAmmo;

	public bool MeleeHitBoxEnabled
	{
		get => !_meleeShape.Disabled;
		set => _meleeShape.SetDeferred("disabled", !value);
	}

	private Timer _meleeCooldownTimer;
	private Timer _meleeDurationTimer;
	private Label _ammoLabel;
	private CollisionShape2D _meleeShape;
	private Timer _reloadDelayTimer;
	private Timer _individualReloadTimer;


	public bool IsFiring
	{
		get => _isFiring;
		set => _isFiring = value;
	}

	public void OnSwap()
	{
		// Visible = false;
		// SetProcess(false);
		// Equipped = false;

		_isFiring = false;
		EmitSignal(nameof(CancelFire));
		EmitSignal(nameof(Swapped));
		if (TacticalEnhancement != null && TacticalEnhancement.IsActive)
		{
			TacticalEnhancement.DeActivate();
			TacticalEnhancement.OnWeaponSwapped();
		}
		FiringDevice?.OnSwap();
	}

	public void OnEquip()
	{
		Visible = true;
		SetProcess(true);
		Equipped = true;
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
	public delegate void CriticalDamageDealt(uint damage, VulnerableHitbox target);

	[Signal]
	public delegate void OnMeleeHit(VulnerableHitbox hitBox);


	public override void _Ready()
	{
		_meleeCooldownTimer = GetNode<Timer>("MeleeCooldownTimer");
		_meleeDurationTimer = GetNode<Timer>("MeleeDurationTimer");
		_reloadDelayTimer = GetNode<Timer>("ReloadDelayTimer");
		_individualReloadTimer = GetNode<Timer>("IndividualReloadTimer");
		_meleeShape = GetNode<CollisionShape2D>("MeleeArea/CollisionShape2D");
		_ammoLabel = GetNode<Label>("CanvasLayer/AmmoLabel");
		CurrentAmmo = MaxAmmoCount;
	}

	private void Fire()
	{
		if (!_isHoldingTrigger || CurrentAmmo == 0)
		{
			_isFiring = false;
			EmitSignal(nameof(CancelFire));
			return;
		}

		CurrentAmmo--;
		EmitSignal(nameof(Fired));
		if (CurrentAmmo == 0)
		{
			_isFiring = false;
			if (AutoReloadEnabled)
				_isRecharging = true;
			EmitSignal(nameof(CancelFire));
		}

		if (_isReloading)
		{
			_isReloading = false;
			_individualReloadTimer.Stop();
		}

		if (AutoReloadEnabled)
			_reloadDelayTimer.Start();
		if (!(OwnerPlayer.Velocity.y > MinFallSpeedForRecoilHovering) || !OwnerPlayer.IsAimingDown) return;
		OwnerPlayer.Velocity.y = -HoverRecoilSpeed;
	}


	public override void _Process(float delta)
	{
		if (OwnerPlayer is null) return;

		_isHoldingTrigger = Input.IsActionPressed("fire");

		if (Mathf.Sign(_meleeShape.Position.x) != OwnerPlayer.HorizontalLookingDirection)
		{
			_meleeShape.Position =
				new Vector2(OwnerPlayer.HorizontalLookingDirection * Mathf.Abs(_meleeShape.Position.x),
					_meleeShape.Position.y);
		}

		if (IsMeleeAttacking)
		{
			return;
		}

		if (Input.IsActionJustPressed("melee") && CanMelee && !OwnerPlayer.IsRamSliding)
		{
			IsMeleeAttacking = true;
			_meleeShape.Disabled = false;
			_meleeCooldownTimer.Start();
			_meleeDurationTimer.Start();

			// TODO: Remove this after adding animations
			Position = new Vector2(8 * Scale.x, 0);
		}

		if (_isHoldingTrigger && CanFire && CurrentAmmo > 0 && !_isRecharging)
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
		var direction = new Vector2(OwnerPlayer.HorizontalLookingDirection, 0);
		if (OwnerPlayer.IsSliding && OwnerPlayer.IsMovingFast())
		{
			direction = Vector2.Up;
		}

		hitBox.TakeHit(MeleeDamage, direction);
		EmitSignal(nameof(OnMeleeHit), hitBox);
	}

	public void Drop(World world, Vector2 position)
	{
		var item = world.Entities.SpawnEntityDeferred<WeaponCollectible>(WeaponCollectibleScene, position);
		OwnerPlayer?.RemoveChild(this);
		item.Weapon = this;
	}

	private void _OnReloadShot()
	{
		CurrentAmmo += ReloadCount;
		if (CurrentAmmo >= MaxAmmoCount)
		{
			CurrentAmmo = MaxAmmoCount;
			_isReloading = false;
			if (_isRecharging)
				_isRecharging = false;
			_individualReloadTimer.Stop();
		}
	}

	private void _OnReloadStart()
	{
		_individualReloadTimer.Start();
		_isReloading = true;
		_OnReloadShot();
	}
}
