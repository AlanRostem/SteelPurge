using Godot;
using System;

public class DestructibleObstacle : StaticBody2D
{
	[Export] public uint Health = 200;

	[Signal]
	public delegate void Destroyed();
	
	private void OnHit(uint damage, Vector2 knockBack, VulnerableHitbox.DamageType damageType)
	{
		if (damage >= Health)
		{
			EmitSignal(nameof(Destroyed));
			QueueFree();
		}

		Health -= damage;
	}
}
