using Godot;
using System;

public class Hazard : StaticEntity
{
	[Export] public uint Damage = 50;

	protected virtual void _OnEntityTouch(LivingEntity entity)
	{
		entity.TakeDamage(Damage, Vector2.Zero);
	}
}
