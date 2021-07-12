using System;
using Godot;

public class Weapon : Node2D
{
	public const uint MeleeDamage = 6;

	private static PackedScene WeaponCollectibleScene
		= GD.Load<PackedScene>("res://scenes/game/entities/collectible/weapon/WeaponCollectible.tscn");

	[Export] public string DisplayName = "Weapon";

	[Export] public Texture CollectibleSprite;
	[Export] public Inventory.InventoryWeapon WeaponType;


	[Export] public uint DamagePerShot;
	[Export] public uint RecoilDashDamagePerShot;
	[Export] public uint RateOfFire;
	[Export] public uint MaxRecoilHoverShots = 3;
	[Export] public bool ReloadOnFloor = true;
	[Export] public bool LoseAmmoOnHover = true;
	[Export] public float HoverRecoilSpeed = 100;
	[Export] public SpriteFrames PlayerSpriteFrames;
	[Export] public uint MaxAmmo = 100;
	[Export] public bool InfiniteAmmo = false;
	
	private uint _ammo = 0;

	public uint Ammo => _ammo;
	
	[Signal]
	public delegate void Swapped();

	public bool Equipped { get; private set; }

	public WeaponAbility TacticalEnhancement { get; set; }
	public FiringDevice FiringDevice { get; set; }

	private bool _canFire = true;
	private bool _canDash = true;
	public bool CanDash => _canDash && _currentRecoilHoverAmmo > 0;

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
	private bool _removeOnFire = false;
	public Player OwnerPlayer;
	private bool _isHoldingTrigger = false;


	public uint CurrentRecoilHoverAmmo
	{
		get => _currentRecoilHoverAmmo;
		set
		{
			_currentRecoilHoverAmmo = value;
			_recoilHoverBar.CurrentShots = value;
		}
	}

	private uint _currentRecoilHoverAmmo;

	public bool MeleeHitBoxEnabled
	{
		get => !_meleeShape.Disabled;
		set => _meleeShape?.SetDeferred("disabled", !value);
	}

	private Timer _meleeCooldownTimer;
	private Timer _meleeDurationTimer;
	private Timer _firingDashTimer;
	private CollisionShape2D _meleeShape;
	private RecoilHoverBar _recoilHoverBar;
	private Label _weaponNameLabel;
	private Label _ammoLabel;

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

	public void OnSwitchTo()
	{
		OnEquip();
	}

	[Signal]
	public delegate void DashFire();

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
		_firingDashTimer = GetNode<Timer>("FiringDashTimer");
		_meleeShape = GetNode<CollisionShape2D>("MeleeArea/CollisionShape2D");
		_recoilHoverBar = GetNode<RecoilHoverBar>("RecoilHoverBar");
		_weaponNameLabel = GetNode<Label>("CanvasLayer/WeaponNameLabel");
		_ammoLabel = GetNode<Label>("CanvasLayer/AmmoLabel");
		_weaponNameLabel.Text = DisplayName;
		CurrentRecoilHoverAmmo = MaxRecoilHoverShots;
		RefillAmmo();
		if (InfiniteAmmo)
			_ammoLabel.Text = "∞";
	}

	private void Fire()
	{
		if (_removeOnFire)
			OwnerPlayer.PlayerInventory.SwitchWeapon(Inventory.InventoryWeapon.P336);
		
		if (!_isHoldingTrigger || (_ammo == 0 && !InfiniteAmmo))
		{
			_isFiring = false;
			EmitSignal(nameof(CancelFire));
			return;
		}

		EmitSignal(nameof(Fired));
		RemoveAmmo();
		ProduceRecoilToHoverAndLoseBoosts();
	}

	public void RefillAmmo()
	{
		if (InfiniteAmmo)
			return;
		_ammo = MaxAmmo;
		_ammoLabel.Text = "x" + _ammo;
	}

	public void AddAmmo(uint amount)
	{
		if (InfiniteAmmo)
			return;
		_ammo += amount;
		_ammoLabel.Text = "x" + _ammo;
	}
	
	public void RemoveAmmo(uint amount = 1)
	{
		if (InfiniteAmmo)
			return;
		_ammo -= amount;
		_ammoLabel.Text = "x" + _ammo;
		if (_ammo == 0)
			OwnerPlayer.PlayerInventory.SwitchWeapon(Inventory.InventoryWeapon.P336);
	}

	public void RemoveAmmoButDontDisappear(uint amount = 1)
	{
		if (InfiniteAmmo)
			return;
		_ammo -= amount;
		_ammoLabel.Text = "x" + _ammo;
		if (_ammo == 0)
			_removeOnFire = true;
	}
	
	public void ProduceRecoilToHoverOnly()
	{
		if (OwnerPlayer.VelocityY <= -HoverRecoilSpeed || !OwnerPlayer.IsAimingDown) return;
		OwnerPlayer.VelocityY = -HoverRecoilSpeed;
	}
	
	public void ProduceRecoilToHoverAndLoseBoosts()
	{
		if (OwnerPlayer.VelocityY <= -HoverRecoilSpeed || !OwnerPlayer.IsAimingDown) return;
		if (CurrentRecoilHoverAmmo == 0) return;
		if (LoseAmmoOnHover) CurrentRecoilHoverAmmo--;
		OwnerPlayer.VelocityY = -HoverRecoilSpeed;
	}

	public void PowerDash()
	{
		if (_canDash && _currentRecoilHoverAmmo > 0)
		{
			_canDash = false;
			if (LoseAmmoOnHover)
				CurrentRecoilHoverAmmo = 0;
			_firingDashTimer.Start();
			EmitSignal(nameof(DashFire));
		}
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


		if (OwnerPlayer.IsOnFloor() && CurrentRecoilHoverAmmo < MaxRecoilHoverShots && ReloadOnFloor)
			CurrentRecoilHoverAmmo = MaxRecoilHoverShots;

		if (_isHoldingTrigger && CanFire)
		{
			if (_isFiring) return;
			_isFiring = true;
			Fire();
			EmitSignal(nameof(TriggerFire));
		}
	}

	private void _OnMeleeAreaHitBoxEntered(object area)
	{
		if (area is CriticalHitbox)
			return;

		var hitBox = (VulnerableHitbox) area;
		var direction = new Vector2(OwnerPlayer.HorizontalLookingDirection, 0);
		if (OwnerPlayer.IsRamSliding)
		{
			direction = Vector2.Up;
			hitBox.TakeHit(MeleeDamage, direction, VulnerableHitbox.DamageType.RamSlide);
		}
		else
		{
			hitBox.TakeHit(MeleeDamage, direction, VulnerableHitbox.DamageType.Melee);
		}

		EmitSignal(nameof(OnMeleeHit), hitBox);
	}

	public void Drop(World world, Vector2 position)
	{
		var item = world.CurrentSegment.Entities.SpawnEntityDeferred<WeaponCollectible>(WeaponCollectibleScene, position);
		OwnerPlayer?.RemoveChild(this);
		item.Weapon = this;
	}

	private void _OnCanDash()
	{
		_canDash = true;
	}
}
