using Godot;
using System;

public class FlareFiringDevice : ProjectileShotGunFiringDevice
{
	private void _OnDamageDealt(uint damage, VulnerableHitbox target)
	{
		if (target.GetParent() is KinematicEntity entity)
		{
			entity.ApplyStatusEffect(KinematicEntity.StatusEffectType.Burn);
		}
	}

	public override void OnDashFire()
	{
		FireProjectileInShotgunPattern(ProjectileScene, 0, SpreadAngle * 2, PelletCount * 3, GetWeapon().RecoilDashDamagePerShot);
	}
}
