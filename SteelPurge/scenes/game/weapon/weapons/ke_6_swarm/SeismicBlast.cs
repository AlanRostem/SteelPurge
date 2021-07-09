using Godot;
using System;

public class SeismicBlast : StaticEntity
{
	// TODO: Add sprite that shows the blast
	public static uint BlastDamage = 5;

	private void _OnExistenceTimerTimeout()
	{
		ParentWorld.CurrentSegment.Entities.RemoveEntity(this);
	}
	
	private void _OnVulnerableHitBoxEntered(VulnerableHitbox area)
	{
		area.TakeHit(BlastDamage, Vector2.Zero, VulnerableHitbox.DamageType.Explosive);
		if (area.GetParent() is Enemy enemy)
			enemy.ApplyStatusEffect(LivingEntity.StatusEffectType.Stun);
	}
}
