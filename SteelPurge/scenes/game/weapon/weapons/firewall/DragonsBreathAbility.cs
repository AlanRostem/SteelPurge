using Godot;
using System;
using Godot.Collections;

public class DragonsBreathAbility : ResourceAbility
{
	private readonly Dictionary<ulong, VulnerableHitbox> _hitBoxes = new Dictionary<ulong, VulnerableHitbox>();

	[Export] public uint DamagePerTick = 1;

	[Signal]
	public delegate void TurnOn();

	[Signal]
	public delegate void TurnOff();

	public override void OnActivate()
	{
		EmitSignal(nameof(TurnOn));
		GetWeapon().IsFiring = true;
		GetWeapon().CanFire = false;
	}

	public override void OnDeActivate()
	{
		EmitSignal(nameof(TurnOff));
		_hitBoxes.Clear();
		GetWeapon().IsFiring = false;
		GetWeapon().CanFire = true;
		GD.PrintStack();
	}

	public override void _Process(float delta)
	{
		base._Process(delta);

		if (GetWeapon().OwnerPlayer.IsAimingDown)
		{
			RotationDegrees = 90;
			Scale = new Vector2(1, 1);
			return;
		}
		
		if (GetWeapon().OwnerPlayer.IsAimingUp)
		{
			Scale = new Vector2(1, 1);
			RotationDegrees = -90;
			return;
		}
		
		Rotation = 0;

		if (GetWeapon().OwnerPlayer.HorizontalLookingDirection < 0)
		{
			Scale = new Vector2(-1, 1);
		}
		else
		{
			Scale = new Vector2(1, 1);
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
			hitBox.TakeHit(DamagePerTick, Vector2.Zero, VulnerableHitbox.DamageType.Heat);
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
