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
	private StaticEntity _staticParent;
	private KinematicEntity _kinematicParent;

	public override void _Ready()
	{
		base._Ready();
		
		if (GetParent() is StaticEntity sEntity)
			_staticParent = sEntity;
		
		else if (GetParent() is KinematicEntity kEntity)
			_kinematicParent = kEntity;

		_damageIndicator = GetNode<DamageIndicator>("DamageIndicator");
		_damageNumberGenerator = GetNode<DamageNumberGenerator>("DamageNumberGenerator");
	}

	private void _OnHit(uint damage, Vector2 knockBackDirection, DamageType damageType)
	{
		World parentWorld = _staticParent != null ? _staticParent.ParentWorld : _kinematicParent.ParentWorld;
		
		var parent = GetParent<Node2D>();
		_damageIndicator.Indicate(new Color(255, 255, 255), parent);
		if (damage >= Health)
		{
			EmitSignal(nameof(Death));
			GetParent().QueueFree();
			_damageNumberGenerator.ShowDamageNumber(Health, parent.Position + new Vector2(0, -16), parentWorld,
				Colors.Red);
			return;
		}

		_damageNumberGenerator.ShowDamageNumber(damage, parent.Position + new Vector2(0, -16), parentWorld);
		Health -= damage;
	}
}
