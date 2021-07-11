using Godot;
using System;

public class HitScanFiringDevice : FiringDevice
{
	[Export] public float DamageRange = 800;

	[Signal]
	private delegate void Scanned(uint damage, float range, float angle);
	
	public void ScanHit(float angle)
	{
		EmitSignal(nameof(Scanned), GetWeapon().DamagePerShot, DamageRange, angle);
	}
	
	public override void OnFireOutput()
	{
		var player = GetWeapon().OwnerPlayer;
		var angle = 0;
		if (player.IsAimingDown)
		{
			angle = 90;
		}
		else if (player.IsAimingUp)
		{
			angle = -90;
		}
		if (player.HorizontalLookingDirection < 0)
		{
			angle = 180 - angle;
		}

		ScanHit(Mathf.Deg2Rad(angle));
	}
}
