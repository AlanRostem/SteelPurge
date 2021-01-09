using Godot;
using System;

public class Gun : RayCast2D
{
	[Export] public float DamageRange = 50;
	[Export] public uint ClipSize = 30;
	[Export] public uint StartingAmmo = 120;
	[Export] public uint RoundsPerMinute = 600;
	[Export] public uint DamagePerShot = 15;
	[Export] public float SpriteOffset;

	private uint _ammoCount;
	private bool _isFireButtonPressed = false;
	private bool _isFiring = false;

	private float _direction = 1;
	private bool _isEquipped = true;
	private Timer _fireCycleTimer;
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
		_ammoCount = ClipSize;
		_fireCycleTimer = GetNode<Timer>("FireCycleTimer");
		_fireCycleTimer.WaitTime = 60f / RoundsPerMinute;
		_firingSoundPlayer = GetNode<AudioStreamPlayer>("FiringSoundPlayer");
		_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}

	public void ScanHit(float angle = 0)
	{
		Enabled = true;
		ForceRaycastUpdate();
		ConfigureScanLine(angle);
		var collider = GetCollider();
		if (collider != null)
		{
			var pos = GetCollisionPoint() - GlobalPosition;
			pos.y = 0;
			CastTo = pos;
			if (collider is Enemy enemy)
			{
				enemy.TakeDamage(DamagePerShot);
			}
		}

		Enabled = false;
	}

	private void ResetStats()
	{
		_isFiring = false;
		_fireCycleTimer.Stop();
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
		if (!_isFiring)
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

		if (Input.IsActionPressed("fire"))
		{
			_isFireButtonPressed = true;
			if (!_isFiring)
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

	private void OnFireCycle()
	{
		if (_ammoCount == 0 || !_isFireButtonPressed)
		{
			_isFiring = false;
			_fireCycleTimer.Stop();
			return;
		}

		OnFire();
		//_ammoCount--;
	}
}
