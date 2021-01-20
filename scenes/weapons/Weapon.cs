using Godot;
using System;

public class Weapon : Node2D
{
	[Export] public uint ClipSize;
	private uint _currentClipAmmo = 0;

	[Export] public uint ReserveAmmoSize;
	private uint _currentReserveAmmo = 0;

	[Export] public uint DamagePerShot;
	[Export] public uint RateOfFire;

	private bool _isFiring = false;
	private bool _isReloading = false;

	public override void _Ready()
	{
		_currentClipAmmo = ClipSize;
		_currentReserveAmmo = ReserveAmmoSize;
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
	}

	private void Fire()
	{
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
			}
		}

		if (Input.IsActionPressed("fire"))
		{
			if (!_isReloading && _currentClipAmmo > 0)
			{
				_isFiring = true;
				EmitSignal(nameof(TriggerFire));
			}
		}
	}
}
