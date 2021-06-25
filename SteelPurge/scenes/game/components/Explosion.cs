using Godot;

public class Explosion : StaticEntity
{
	[Export] public uint Damage = 30;
	[Export] public float KnockBackForce = 300;
	
	private void _OnVulnerableHitBoxHit(object area)
	{
		var hitBox = (VulnerableHitbox)area;
		hitBox.TakeHit(Damage, Vector2.Zero);
		if (hitBox.GetParent() is Enemy entity)
		{
			var angle = Position.AngleToPoint(entity.Position);
			entity.ApplyStatusEffect(LivingEntity.StatusEffectType.KnockBack, effect =>
			{
				var knockBackEffect = (KnockBackEffect) effect;
				knockBackEffect.KnockBackForce = new Vector2(-Mathf.Cos(angle), -Mathf.Sin(angle)) * KnockBackForce;
				knockBackEffect.DisableEntityMovement = true;
			});
		}
	}
	
	// TODO: Move this code to a KineticOrbExplosion scene
	private void _OnPlayerHit(object body)
	{
		if (body is Player player)
		{
			var angle = Position.AngleToPoint(player.Position);
			var force = new Vector2(-Mathf.Cos(angle) * 0.2f, -Mathf.Sin(angle)) * KnockBackForce;
			player.ApplyStatusEffect(LivingEntity.StatusEffectType.KnockBack, effect =>
			{
				var knockBackEffect = (KnockBackEffect)effect;
				knockBackEffect.KnockBackForce = force;
			});
		}
	}

	private void OnDisappear()
	{
		QueueFree();
	}
}
