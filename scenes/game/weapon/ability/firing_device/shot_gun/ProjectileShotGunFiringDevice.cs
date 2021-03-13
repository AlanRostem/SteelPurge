using Godot;
using System;

public class ProjectileShotGunFiringDevice : FiringDevice
{
	[Export] public uint PelletCount = 12;
	[Export] public float SpreadAngle = 16;

	public virtual void OnProjectileShot(float angle)
	{
		
	}
	
	public override void OnFire()
	{
		var spread = Mathf.Deg2Rad(SpreadAngle);
		var offsetAngle = -spread / 2;
		for (int i = 0; i < PelletCount; i++)
		{
			OnProjectileShot(offsetAngle);
			offsetAngle += spread / PelletCount;
		}
	}
}
