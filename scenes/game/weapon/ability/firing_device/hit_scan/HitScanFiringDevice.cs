using Godot;
using System;

public class HitScanFiringDevice : FiringDevice
{
	[Export] public float Range = 800;

	[Signal]
	public delegate void Scanned(uint damage, float range);
	
	public override void OnFire()
	{
		var damage = GetWeapon().DamagePerShot;
		EmitSignal(nameof(Scanned), damage, Range);
	}
}
