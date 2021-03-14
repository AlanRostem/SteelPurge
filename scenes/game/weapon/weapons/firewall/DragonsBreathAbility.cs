using Godot;
using System;
using Godot.Collections;

public class DragonsBreathAbility : ResourceAbility
{
	private readonly Dictionary<ulong, VulnerableHitbox> _hitBoxes = new Dictionary<ulong, VulnerableHitbox>();

	[Export]
	public uint DamagePerTick = 10;
	
	[Signal]
	public delegate void TurnOn();

	[Signal]
	public delegate void TurnOff();

	public override void OnActivate()
	{
		EmitSignal(nameof(TurnOn));
	}

	public override void OnDeActivate()
	{
		EmitSignal(nameof(TurnOff));
		GetWeapon().OwnerPlayer.IsGravityEnabled = true;
		_hitBoxes.Clear();
	}

	public override void OnUpdate()
	{
		var player = GetWeapon().OwnerPlayer;
		player.IsGravityEnabled = !player.IsAimingDown;
		if (!player.IsGravityEnabled)
			player.Velocity.y = 0;
	}

	public override void OnTick()
	{
		foreach (var pair in _hitBoxes)
		{
			var hitBox = pair.Value;
			hitBox.EmitSignal(nameof(VulnerableHitbox.Hit), DamagePerTick);
		}
	}

	private void _HitBoxEnteredFire(object area)
	{
		var hitBox = (VulnerableHitbox) area;
		_hitBoxes[hitBox.GetInstanceId()] = hitBox;
	}
	
	private void _HitBoxLeftFire(object area)
	{
		var hitBox = (VulnerableHitbox)area;
		_hitBoxes.Remove(hitBox.GetInstanceId());
	}
}
