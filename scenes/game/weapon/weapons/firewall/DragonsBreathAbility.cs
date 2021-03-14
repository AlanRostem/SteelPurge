using Godot;
using System;

public class DragonsBreathAbility : ResourceAbility
{
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
	}

	public override void OnUpdate()
	{
		var player = GetWeapon().OwnerPlayer;
		player.IsGravityEnabled = !player.IsAimingDown;
		if (!player.IsGravityEnabled)
			player.Velocity.y = 0;
	}
}
