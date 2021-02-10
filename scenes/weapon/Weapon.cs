using Godot;
using System;

public class Weapon : Node2D
{
	[Export] public string ScreenDisplayName = "Weapon";
	[Export] public string BuyDisplayName = "Weapon";

	[Export] public uint ClipSize;
	private uint _currentClipAmmo = 0;

	[Export] public uint ReserveAmmoSize;
	private uint _currentReserveAmmo = 0;

	[Export] public uint DamagePerShot;
	[Export] public uint RateOfFire;
	[Export] public float ReloadSpeed;
	[Export] public float PassiveReloadSpeed;
	[Export] public float SlowDownMultiplier = .4f;

	private bool _isFiring = false;
	private bool _isReloading = false;
	private bool _isPassivelyReloading = false;
	public Player OwnerPlayer;
	private bool _isHoldingTrigger = false;
	public bool IsFiring => _isFiring;

	public override void _Ready()
	{
		_currentClipAmmo = ClipSize;
		_currentReserveAmmo = ReserveAmmoSize;
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
		if (Scale.x != OwnerPlayer.Direction)
			Scale = new Vector2(OwnerPlayer.Direction, 1);
		if (Rotation != OwnerPlayer.AimAngle)
			Rotation = OwnerPlayer.AimAngle;
	}

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

	private void OnReload()
	{
		if (ClipSize > _currentClipAmmo)
		{
			if (_currentReserveAmmo > (ClipSize - _currentClipAmmo))
			{
				var ammoDiff = ClipSize - _currentClipAmmo;
				_currentClipAmmo += ammoDiff;
				_currentReserveAmmo -= ammoDiff;
			}
			else
			{
				_currentClipAmmo += _currentReserveAmmo;
				_currentReserveAmmo = 0;
			}

			_isReloading = false;
			// Sounds.PlaySound(Sounds.ReloadEndSound);
		}
	}

	private void Fire()
	{
		if (_currentClipAmmo == 0)
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
			if (!_isPassivelyReloading)
			{
				_isPassivelyReloading = true;
				EmitSignal(nameof(TriggerPassiveReload));
			}
			return;
		}

		_currentClipAmmo--;
		OnFire();
	}

	public virtual void OnFire()
	{
	}

	public override void _Process(float delta)
	{
		if (Scale.x != OwnerPlayer.Direction)
			Scale = new Vector2(OwnerPlayer.Direction, 1);
		if (Rotation != OwnerPlayer.AimAngle)
			Rotation = OwnerPlayer.AimAngle;

		if (OwnerPlayer.DidReload && _currentClipAmmo < ClipSize)
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
			if (!_isReloading && !_isFiring && _currentClipAmmo > 0)
			{
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
		}
		else
		{
			_isHoldingTrigger = false;
		}
	}

	public uint GetClipAmmo()
	{
		return _currentClipAmmo;
	}

	public uint GetReserveAmmo()
	{
		return _currentReserveAmmo;
	}

	public void RefillAmmo()
	{
		_currentClipAmmo = ClipSize;
		_currentReserveAmmo = ReserveAmmoSize;

		_isFiring = false;
		_isReloading = false;
		_isPassivelyReloading = false;
		EmitSignal(nameof(CancelFire));
		EmitSignal(nameof(CancelReload));
		EmitSignal(nameof(CancelPassiveReload));
	}

	public bool IsFull()
	{
		return _currentClipAmmo == ClipSize && _currentReserveAmmo == ReserveAmmoSize;
	}
	
	
	private void _OnPassiveReload()
	{
		OnReload();
		_isPassivelyReloading = false;
	}
}






