using Godot;
using System;

public class ShotGunFiringDevice : HitScanFiringDevice
{
	[Export] public uint PelletCount = 12;
    [Export] public float SpreadAngle = 16;

	public override void OnFire()
	{
		var spread = Mathf.Deg2Rad(SpreadAngle);
		var offsetAngle = -spread / 2;
		for (int i = 0; i < PelletCount; i++)
		{
			ScanHit(offsetAngle);
			offsetAngle += spread / PelletCount;
		}
	}

    /*
	public override void _Draw()
	{
		float spread = Mathf.Deg2Rad(SpreadAngle);
		var offsetAngle = -spread / 2;
		for (int i = 0; i < PelletCount; i++)
		{
			var to = new Vector2(DamageRange * Mathf.Cos(offsetAngle), DamageRange * Mathf.Sin(offsetAngle));
			DrawLine(new Vector2(), to, new Color(255, 255, 255));
			offsetAngle += spread / PelletCount;
		}
	}
	*/
}
