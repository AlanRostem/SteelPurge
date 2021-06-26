using Godot;
using System;
using Godot.Collections;

public class DragonsBreathAbility : ResourceAbility
{
	private readonly Dictionary<ulong, VulnerableHitbox> _hitBoxes = new Dictionary<ulong, VulnerableHitbox>();

	[Export] public uint DamagePerTick = 10;

	[Signal]
	public delegate void TurnOn();

	[Signal]
	public delegate void TurnOff();

	private FireArea _fireArea;

	public override void _Ready()
	{
		base._Ready();
		_fireArea = GetNode<FireArea>("FireArea");
	}

	public override void OnActivate()
	{
		EmitSignal(nameof(TurnOn));
		GetWeapon().IsFiring = true;
		GetWeapon().EmitSignal(nameof(Weapon.CancelFire));
	}

	public override void OnDeActivate()
	{
		EmitSignal(nameof(TurnOff));
		_hitBoxes.Clear();
		GetWeapon().IsFiring = false;
	}

	public override void _Process(float delta)
	{
		base._Process(delta);

		if (GetWeapon().OwnerPlayer.IsAimingDown)
		{
			_fireArea.RotationDegrees = 90;
			_fireArea.Scale = new Vector2(1, 1);
			return;
		}
		
		if (GetWeapon().OwnerPlayer.IsAimingUp)
		{
			_fireArea.Scale = new Vector2(1, 1);
			_fireArea.RotationDegrees = -90;
			return;
		}
		
		_fireArea.Rotation = 0;

		if (GetWeapon().OwnerPlayer.HorizontalLookingDirection < 0)
		{
			_fireArea.Scale = new Vector2(-1, 1);
		}
		else
		{
			_fireArea.Scale = new Vector2(1, 1);
		}
	}

	public override void OnTick()
	{
		var player = GetWeapon().OwnerPlayer;
		if (player.VelocityY > 0 && player.IsAimingDown)
			player.VelocityY *= 0.2f;
		foreach (var pair in _hitBoxes)
		{
			var hitBox = pair.Value;
			hitBox.TakeHit(DamagePerTick, Vector2.Zero);
			GetWeapon().EmitSignal(nameof(Weapon.DamageDealt), DamagePerTick, hitBox);
		}
	}

	private void _HitBoxEnteredFire(object area)
	{
		var hitBox = (VulnerableHitbox) area;
		_hitBoxes[hitBox.GetInstanceId()] = hitBox;
	}

	private void _HitBoxLeftFire(object area)
	{
		var hitBox = (VulnerableHitbox) area;
		_hitBoxes.Remove(hitBox.GetInstanceId());
	}
		
	private void _OnHostileProjectileHitFireArea(HostileProjectile body)
	{
		body._OnDisappear();
	}
}
