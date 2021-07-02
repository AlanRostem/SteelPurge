using Godot;
using System;

public class Hazard : StaticEntity
{
	[Export] public uint Damage = 50;
	[Export] public bool InstaKill = true;
	[Export] public bool TargetEnemies = true;

	protected virtual void _OnEntityTouch(LivingEntity entity)
	{
		if (!TargetEnemies && entity is Enemy)
			return;
		
		if (InstaKill)
		{
			entity.TakeDamage(entity.MaxHealth, Vector2.Zero);
			if (entity is Player player)
			{
				player.Die();
			}
			return;
		}
		
		entity.TakeDamage(Damage, new Vector2(-Mathf.Sign(entity.VelocityX), 0));
	}
}
