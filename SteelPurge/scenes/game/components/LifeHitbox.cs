using Godot;
using System;

/// <summary>
/// Hitbox component that can be added to StaticEntity nodes.
/// It has its own unique health pool. If the health
/// of this hitbox is depleted, the parent is deleted
/// and a signal is emitted.
///
/// This node is mostly used if a StaticEntity node really
/// needs to take damage in some way when it does not inherit
/// DestructibleObstacle. 
/// </summary>
public class LifeHitbox : VulnerableHitbox
{
	[Signal]
	public delegate void Death();

	[Export] public uint Health = 100;

	private DamageIndicator _damageIndicator;
	private DamageNumberGenerator _damageNumberGenerator;
	private StaticEntity _parent;

	public override void _Ready()
	{
		base._Ready();
		_parent = GetParent<StaticEntity>();
		_damageIndicator = GetNode<DamageIndicator>("DamageIndicator");
		_damageNumberGenerator = GetNode<DamageNumberGenerator>("DamageNumberGenerator");
	}

	private void _OnHit(uint damage, Vector2 knockBackDirection, DamageType damageType)
	{
		_damageIndicator.Indicate(new Color(255, 255, 255), _parent);
		if (damage >= Health)
		{
			EmitSignal(nameof(Death));
			GetParent().QueueFree();
			_damageNumberGenerator.ShowDamageNumber(Health, _parent.Position + new Vector2(0, -16), _parent.ParentWorld,
				Colors.Red);
			return;
		}

		_damageNumberGenerator.ShowDamageNumber(damage, _parent.Position + new Vector2(0, -16), _parent.ParentWorld);
		Health -= damage;
	}
}
