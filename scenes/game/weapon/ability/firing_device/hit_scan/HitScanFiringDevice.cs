using Godot;
using System;

public class HitScanFiringDevice : FiringDevice
{
	[Export] public float Range = 800;

	[Signal]
	private delegate void Scanned(uint damage, float range, float angle);
	
	public void ScanHit(float angle)
	{
		EmitSignal(nameof(Scanned), GetWeapon().DamagePerShot, Range, angle);
	}
	
	public override void OnFire()
	{
		ScanHit(0);
	}
}
