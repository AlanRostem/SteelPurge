using Godot;

public class Weapon : Node2D
{
	[Export] public string DisplayName = "Weapon";

	[Export] public uint ClipSize;
	public uint CurrentAmmo = 0;

	[Export] public uint DamagePerShot;
	[Export] public uint RateOfFire;
	[Export] public float ReloadSpeed;
	[Export] public float PassiveReloadSpeed;
	[Export] public float HoverRecoilMultiplier = .1f;

	public TacticalAbility TacticalEnhancement { get; set; }
	public FiringDevice FiringDevice { get; set; }

	private bool _isFiring = false;
	private bool _isReloading = false;
	private bool _isPassivelyReloading = false;
	public Player OwnerPlayer;
	private bool _isHoldingTrigger = false;

	public bool IsFiring
	{
		get => _isFiring;
		set => _isFiring = value;
	}

	public override void _Ready()
	{
		CurrentAmmo = ClipSize;
		GetParent<Player>().KnowWeaponClipAmmo(CurrentAmmo);
		
	}

	public void OnSwap()
	{
		_isFiring = false;
		_isReloading = false;
		EmitSignal(nameof(CancelFire));
		EmitSignal(nameof(CancelReload));
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
	public delegate void TriggerReload();

	[Signal]
	public delegate void CancelReload();

	[Signal]
	public delegate void TriggerPassiveReload();

	[Signal]
	public delegate void CancelPassiveReload();

	[Signal]
	public delegate void DamageDealt(uint damage, VulnerableHitbox target);

	private void OnReload()
	{
		ReloadPerformed();
		OwnerPlayer.KnowWeaponClipAmmo(CurrentAmmo);
		_isReloading = false;
		// Sounds.PlaySound(Sounds.ReloadEndSound);
	}
	
	protected virtual void ReloadPerformed()
	{
		CurrentAmmo = ClipSize;
	}

	private void Fire()
	{
		if (CurrentAmmo == 0)
		{
			_isFiring = false;
			EmitSignal(nameof(CancelFire));
			EmitSignal(nameof(TriggerReload));
			return;
		}

		if (!_isHoldingTrigger)
		{
			_isFiring = false;
			EmitSignal(nameof(CancelFire));
			if (_isPassivelyReloading) return;
			_isPassivelyReloading = true;
			EmitSignal(nameof(TriggerPassiveReload));

			return;
		}

		CurrentAmmo--;
		OwnerPlayer.KnowWeaponClipAmmo(CurrentAmmo);
		EmitSignal(nameof(Fired));
		if (!(OwnerPlayer.Velocity.y > 0) || !OwnerPlayer.IsAimingDown) return;
		var velocity = new Vector2(OwnerPlayer.Velocity);
		velocity.y *= HoverRecoilMultiplier;
		OwnerPlayer.Velocity = velocity;
	}


	public override void _Process(float delta)
	{
		if (Scale.x != OwnerPlayer.HorizontalLookingDirection)
		{
			Scale = new Vector2(OwnerPlayer.HorizontalLookingDirection, 1);
		}


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

		if (OwnerPlayer.DidReload && CurrentAmmo < ClipSize)
		{
			if (!_isReloading)
			{
				if (_isPassivelyReloading)
				{
					_isPassivelyReloading = false;
					EmitSignal(nameof(CancelPassiveReload));
				}

				_isReloading = true;
				EmitSignal(nameof(TriggerReload));
				if (_isFiring)
				{
					_isFiring = false;
					EmitSignal(nameof(CancelFire));
				}

				// Sounds.PlaySound(Sounds.ReloadStartSound);
			}
		}

		if (OwnerPlayer.IsHoldingTrigger)
		{
			if (_isReloading || _isFiring || CurrentAmmo <= 0) return;
			_isFiring = true;
			_isHoldingTrigger = true;
			Fire();
			EmitSignal(nameof(TriggerFire));
			if (_isPassivelyReloading)
			{
				_isPassivelyReloading = false;
				EmitSignal(nameof(CancelPassiveReload));
			}
		}
		else
		{
			_isHoldingTrigger = false;
		}
	}

	public uint GetClipAmmo()
	{
		return CurrentAmmo;
	}

	private void _OnPassiveReload()
	{
		OnReload();
		_isPassivelyReloading = false;
	}
}
