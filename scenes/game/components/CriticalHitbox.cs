using Godot;
using System;

public class CriticalHitbox : VulnerableHitbox
{
	[Export] public float CriticalHitMultiplier = 1.5f;
	public override void TakeHit(uint damage)
	{
		base.TakeHit((uint)(damage * CriticalHitMultiplier));
	}
}
