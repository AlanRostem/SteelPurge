using Godot;
using System;

public class LargeWindSlice : Projectile
{
	[Export] public float KnockBackSpeed = 300;
	public override void _OnHit(VulnerableHitbox subject)
	{
		if (subject.GetParent() is Enemy enemy)
		{
			enemy.ApplyStatusEffect(LivingEntity.StatusEffectType.KnockBack, effect =>
			{
				var knockBackEffect = (KnockBackEffect) effect;
				knockBackEffect.KnockBackForce = Velocity.Normalized() * KnockBackSpeed;
				knockBackEffect.DisableEntityMovement = true;
			});
		}
	}
}
