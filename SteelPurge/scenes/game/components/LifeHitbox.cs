using Godot;
using System;

/// <summary>
/// Hitbox component that can be added to any node.
/// It has its own unique health pool. If the health
/// of this hitbox is depleted, the parent is deleted
/// and a signal is emitted. 
/// </summary>
public class LifeHitbox : VulnerableHitbox
{
	[Signal]
	public delegate void Death();
	
	[Export] public uint MaxHealth;
	
	public uint Health;

	public override void TakeHit(uint damage, Vector2 knockBackDirection, DamageType damageType)
	{
		base.TakeHit(damage, knockBackDirection, damageType);
		if (damage >= Health)
		{
			EmitSignal(nameof(Death));
			GetParent().QueueFree();
			return;
		}

		Health -= damage;
	}
}
