using Godot;
using System;

public class Gun : RayCast2D
{
	[Export] public String Name = "Gun";
	[Export] public float DamageRange = 50;
	[Export] public uint ClipSize = 30;
	[Export] public uint ReserveAmmo = 120;
	[Export] public uint ReloadSpeed = 2;
	[Export] public uint RoundsPerMinute = 600;
	[Export] public uint DamagePerShot = 15;
	[Export] public float SpriteOffset;

	public uint AmmoCount;
	private bool _isFireButtonPressed = false;
	private bool _isFiring = false;
	private bool _isReloading = false;

	private float _direction = 1;
	private bool _isEquipped = true;
	private Timer _fireCycleTimer;
	private Timer _reloadTimer;
	private AudioStreamPlayer _firingSoundPlayer;

	private AnimatedSprite _sprite;

	public bool IsEquipped
	{
		get => _isEquipped;
		set
		{
			Visible = value;
			_isEquipped = value;
			if (!value)
				ResetStats();
		}
	}

	public override void _Ready()
	{
		ConfigureScanLine(0);
		AmmoCount = ClipSize;
		_fireCycleTimer = GetNode<Timer>("FireCycleTimer");
		_fireCycleTimer.WaitTime = 60f / RoundsPerMinute;

		_reloadTimer = GetNode<Timer>("ReloadTimer");
		_reloadTimer.WaitTime = ReloadSpeed;

		_firingSoundPlayer = GetNode<AudioStreamPlayer>("FiringSoundPlayer");
		_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}

	public uint GetAmmo()
	{
		return AmmoCount;
	}
	
	public void ScanHit(float angle = 0)
	{
		Enabled = true;
		ForceRaycastUpdate();
		ConfigureScanLine(angle);
		var collider = GetCollider();
		if (collider is Enemy enemy)
		{
			enemy.TakeDamage(DamagePerShot);
		}

		Enabled = false;
	}

	private void ResetStats()
	{
		_isFiring = false;
		_fireCycleTimer.Stop();
		_isReloading = false;
		_reloadTimer.Stop();
	}

	private void ConfigureScanLine(float angle)
	{
		CastTo = new Vector2(
			Mathf.Cos(angle) * DamageRange * _direction,
			Mathf.Sin(angle) * DamageRange
		);
	}

	public void SetDirection(float dir)
	{
		if (!_isFireButtonPressed)
			_direction = dir;
	}

	public override void _Process(float delta)
	{
		if (!IsEquipped)
			return;

		if (_direction < 0)
		{
			_sprite.FlipH = true;
			_sprite.Position = new Vector2(-SpriteOffset, 0);
		}
		else
		{
			_sprite.FlipH = false;
			_sprite.Position = new Vector2(SpriteOffset, 0);
		}

		if (Input.IsActionJustPressed("reload") && !_isReloading)
		{
			if (AmmoCount < ClipSize && ReserveAmmo > 0)
				StartReloadCycle();
		}
		
		if (Input.IsActionPressed("fire"))
		{
			_isFireButtonPressed = true;
			if (!_isFiring && !_isReloading)
			{
				OnFireCycle();
				_fireCycleTimer.Start();
				_isFiring = true;
			}
		}
		else
		{
			_isFireButtonPressed = false;
		}
	}

	public virtual void OnFire()
	{
		ScanHit();
		_firingSoundPlayer.Play();
	}

	public void StartReloadCycle()
	{
		_isReloading = true;
		_reloadTimer.Start();
	}

	private void OnFireCycle()
	{
		if (AmmoCount == 0 || !_isFireButtonPressed)
		{
			_isFiring = false;
			_fireCycleTimer.Stop();
			if (AmmoCount == 0 && ReserveAmmo > 0)
				StartReloadCycle();
			return;
		}

		OnFire();
		AmmoCount--;
	}

	private void OnReloaded()
	{
		if (ClipSize > AmmoCount)
		{
			if (ReserveAmmo > (ClipSize - AmmoCount))
			{
				var ammoDiff = ClipSize - AmmoCount;
				AmmoCount += ammoDiff;
				ReserveAmmo -= ammoDiff;
			}
			else
			{
				AmmoCount += ReserveAmmo;
				ReserveAmmo = 0;
			}
		}

		_isReloading = false;
	}
}
