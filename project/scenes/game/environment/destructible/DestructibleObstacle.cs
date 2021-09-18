using Godot;
using System;

public class DestructibleObstacle : StaticEntity
{
	[Export] public uint Health = 200;

	[Signal]
	public delegate void Destroyed();

	private DamageIndicator _damageIndicator;
	private DamageNumberGenerator _damageNumberGenerator;
	public bool HasDisappeared { get; private set; }

	public override void _Init()
	{
		base._Init();
		_damageIndicator = GetNode<DamageIndicator>("DamageIndicator");
		_damageNumberGenerator = GetNode<DamageNumberGenerator>("DamageNumberGenerator");
	}

	protected virtual void OnTakeDamage(uint damage, VulnerableHitbox.DamageType damageType)
	{
		
	}

	public void Disappear()
	{
		if (HasDisappeared) return;
		HasDisappeared = true;
		EmitSignal(nameof(Destroyed));
		ParentWorld.CurrentSegment.Entities.RemoveEntity(this);
	}
	
	private void OnHit(uint damage, Vector2 knockBack, VulnerableHitbox.DamageType damageType)
	{
		_damageIndicator.Indicate(new Color(255, 255, 255));
		if (damage >= Health)
		{
			_damageNumberGenerator.ShowDamageNumber(Health, Position + new Vector2(0, -16), ParentWorld, Colors.Red);
			Disappear();
			OnTakeDamage(Health, damageType);
			return;
		}

		_damageNumberGenerator.ShowDamageNumber(damage, Position + new Vector2(0, -16), ParentWorld);
		Health -= damage;
		OnTakeDamage(damage, damageType);
	}
}
