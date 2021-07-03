using Godot;
using System;

public class SeismicBlast : StaticEntity
{
	// TODO: Add sprite that shows the blast
	public static uint BlastDamage = 80;

	private void _OnExistenceTimerTimeout()
	{
		QueueFree();
	}
	
	private void _OnVulnerableHitBoxEntered(VulnerableHitbox area)
	{
		area.TakeHit(BlastDamage, Vector2.Zero, VulnerableHitbox.DamageType.Explosive);
		if (area.GetParent() is Enemy enemy)
			enemy.ApplyStatusEffect(LivingEntity.StatusEffectType.Stun);
	}
}
