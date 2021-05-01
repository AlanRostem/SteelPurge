using Godot;
using System;

public class FlareFiringDevice : ProjectileShotGunFiringDevice
{
	private static readonly PackedScene FlareScene = GD.Load<PackedScene>("res://scenes/game/weapon/weapons/firewall/Flare.tscn");
	
	public override void OnProjectileShot(float angle)
	{
		FireProjectile(FlareScene, angle);
	}
	
	private void _OnDamageDealt(uint damage, VulnerableHitbox target)
	{
		if (target.GetParent() is KinematicEntity entity)
		{
			entity.ApplyStatusEffect(KinematicEntity.StatusEffectType.Burn);
		}
	}
}
