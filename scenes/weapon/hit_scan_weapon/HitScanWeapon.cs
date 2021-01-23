using Godot;
using System;

public class HitScanWeapon : Weapon
{
	[Export] public float DamageRange;
	
	[Signal]
	public delegate void Scan(float range, uint damage);
	
	public override void OnFire()
	{
		EmitSignal(nameof(Scan), DamageRange, DamagePerShot);    
	}
}
