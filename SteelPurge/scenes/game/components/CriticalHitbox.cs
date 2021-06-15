using System;
using Godot;

public class CriticalHitbox : VulnerableHitbox
{
	[Export] public Vector2 CriticalHitDirection = Vector2.Down;
	[Export] public float CriticalHitAngularMargin = 15;
	[Export] public float CriticalHitMultiplier = 1.5f;

	public void TakeHit(uint damage)
	{
		TakeHit(damage, Vector2.Zero);
	}

	public override void TakeHit(uint damage, Vector2 knockBackDirection)
	{
		base.TakeHit((uint)(damage * CriticalHitMultiplier), knockBackDirection);
	}
}
