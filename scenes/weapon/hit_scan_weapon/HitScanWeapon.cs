using Godot;
using System;

public class HitScanWeapon : Weapon
{
	[Export] public float DamageRange;
	
	[Signal]
	public delegate void Scan(float range, float direction, uint damage);
	
	public override void OnFire()
	{
		var dir = 1f;
		EmitSignal(nameof(Scan), DamageRange, dir, DamagePerShot);    
	}
}
