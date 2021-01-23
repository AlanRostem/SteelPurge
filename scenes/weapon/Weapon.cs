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

	private bool _isFiring = false;
	private bool _isReloading = false;
	public Player OwnerPlayer;
	private bool _isHoldingTrigger = false;

	public override void _Ready()
	{
		_currentClipAmmo = ClipSize;
		_currentReserveAmmo = ReserveAmmoSize;
	}
	
	public void OnSwap()
	{
		EmitSignal(nameof(CancelFire));
		EmitSignal(nameof(CancelReload));
		Visible = false;
		SetProcess(false);
	}

	public void OnEquip()
	{
		Visible = true;
		SetProcess(true);
	}

	[Signal]
	public delegate void TriggerFire();

	[Signal]
	public delegate void CancelFire();

	[Signal]
	public delegate void TriggerReload();

	[Signal]
	public delegate void CancelReload();

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
			// Sounds.PlaySound(Sounds.ReloadEndSound);
		}
	}

	private void Fire()
	{
		if (!_isHoldingTrigger)
		{
			_isFiring = false;
			EmitSignal(nameof(CancelFire));
			return;
		}
		_currentClipAmmo--;
		OnFire();
		if (_currentClipAmmo == 0)
		{
			EmitSignal(nameof(CancelFire));
		}
	}
	
	public virtual void OnFire()
	{
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("reload"))
		{
			if (!_isReloading && !_isFiring)
			{
				EmitSignal(nameof(TriggerReload));
				// Sounds.PlaySound(Sounds.ReloadStartSound);
			}
		}

		if (Input.IsActionPressed("fire"))
		{
			if (!_isReloading && !_isFiring && _currentClipAmmo > 0)
			{
				_isFiring = true;
				_isHoldingTrigger = true;
				Fire();
				EmitSignal(nameof(TriggerFire));
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
}
