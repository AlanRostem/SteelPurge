using Godot;
using System;

public class ProjectileShotGunFiringDevice : FiringDevice
{
	[Export] public uint PelletCount = 12;
	[Export] public float SpreadAngle = 16;
	[Export] public PackedScene ProjectileScene;

	public virtual void OnProjectileShot(Projectile projectile)
	{
		
	}

	// Must be used with the shotgun firing mechanism
	public void FireProjectileInShotgunPattern(PackedScene projectileScene, float startAngle, float spreadAngle,
		float pelletCount)
	{
		FireProjectileInShotgunPattern(projectileScene, startAngle, spreadAngle, pelletCount, GetWeapon().DamagePerShot);
	}
	
	public void FireProjectileInShotgunPattern(PackedScene projectileScene, float startAngle, float spreadAngle, float pelletCount, uint damage)
	{
		var spread = Mathf.Deg2Rad(spreadAngle);
		var offsetAngle = startAngle - spread / 2;
		for (int i = 0; i < pelletCount; i++)
		{
			var projectile = FireProjectile(projectileScene, offsetAngle);
			projectile.Damage = damage;
			OnProjectileShot(projectile);
			offsetAngle += spread / pelletCount;
		}
	}

	// Override this to your liking. Will default to the FireProjectileInShotgunPattern() function
	public override void OnFireOutput()
	{
		FireProjectileInShotgunPattern(ProjectileScene, 0, SpreadAngle, PelletCount);
	}
}
