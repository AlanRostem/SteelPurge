using Godot;
using System;

public class ShotGunFiringDevice : HitScanFiringDevice
{
	[Export] public uint PelletCount = 12;

	[Export] public float SpreadAngle = 30;

	public override void OnFire()
	{
		for (var offsetAngle = Mathf.Deg2Rad(SpreadAngle);
			offsetAngle > -SpreadAngle;
			offsetAngle -= SpreadAngle / PelletCount)
		{
			ScanHit(offsetAngle);
		}
	}
}
