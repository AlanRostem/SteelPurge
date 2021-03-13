using Godot;
using System;

public class FlareFiringDevice : ProjectileShotGunFiringDevice
{
	public override void OnProjectileShot(float angle)
	{
		var world = GetWeapon().OwnerPlayer.ParentWorld;
		base.OnProjectileShot(angle);
	}
}
