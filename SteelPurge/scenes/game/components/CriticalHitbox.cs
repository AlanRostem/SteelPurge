using System;
using Godot;

public class CriticalHitbox : VulnerableHitbox
{
	[Export] public Vector2 CriticalHitDirection = Vector2.Down;
	[Export] public float CriticalHitAngularMargin = 15;
	[Export] public float CriticalHitMultiplier = 1.5f;

	public override void TakeHit(uint damage, Vector2 knockBackDirection, DamageType damageType)
	{
		base.TakeHit((uint)(damage * CriticalHitMultiplier), knockBackDirection, damageType);
	}
}
