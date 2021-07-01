using Godot;
using System;

public class DestructibleObstacle : StaticEntity
{
	[Export] public uint Health = 200;

	[Signal]
	public delegate void Destroyed();

	private DamageIndicator _damageIndicator;
	private DamageNumberGenerator _damageNumberGenerator;

	public override void _Ready()
	{
		base._Ready();
		_damageIndicator = GetNode<DamageIndicator>("DamageIndicator");
		_damageNumberGenerator = GetNode<DamageNumberGenerator>("DamageNumberGenerator");
	}

	private void OnHit(uint damage, Vector2 knockBack, VulnerableHitbox.DamageType damageType)
	{
		_damageIndicator.Indicate(new Color(255, 255, 255));
		if (damage >= Health)
		{
			_damageNumberGenerator.ShowDamageNumber(Health, Position + new Vector2(0, -16), ParentWorld, Colors.Red);
			EmitSignal(nameof(Destroyed));
			QueueFree();
			return;
		}

		_damageNumberGenerator.ShowDamageNumber(damage, Position + new Vector2(0, -16), ParentWorld);
		Health -= damage;
	}
}
