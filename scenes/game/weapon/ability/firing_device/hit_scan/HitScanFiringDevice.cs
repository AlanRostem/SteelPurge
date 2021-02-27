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
	
	public override void OnFire()
	{
		ScanHit(0);
	}
}
