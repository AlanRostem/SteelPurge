using Godot;

public class Explosion : StaticEntity
{
	[Export] public uint Damage = 1;
	[Export] public float KnockBackForce = 300;
	[Export] public float Radius = 24;

	
	public override void _Init()
	{
		base._Init();
		var circle = (CircleShape2D) GetNode<CollisionShape2D>("ExplosiveAreaHitBox/CollisionShape2D").Shape;
		circle.SetDeferred("radius", Radius);
	}

	private void _OnVulnerableHitBoxHit(object area)
	{
		var hitBox = (VulnerableHitbox)area;
		hitBox.TakeHit(Damage, Vector2.Zero, VulnerableHitbox.DamageType.Explosive);
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
			player.TakeDamage(Vector2.Zero);
			var angle = Position.AngleToPoint(player.Position);
			player.ApplyStatusEffect(LivingEntity.StatusEffectType.KnockBack, effect =>
			{
				var knockBackEffect = (KnockBackEffect) effect;
				knockBackEffect.KnockBackForce = new Vector2(-Mathf.Cos(angle), -Mathf.Sin(angle)) * KnockBackForce;
				knockBackEffect.StackForce = true;
			});
		}
	}

	private void OnDisappear()
	{
		ParentWorld.CurrentSegment.Entities.RemoveEntity(this);
	}
}
