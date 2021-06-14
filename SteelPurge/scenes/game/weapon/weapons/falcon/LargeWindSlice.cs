using Godot;
using System;

public class LargeWindSlice : Projectile
{
	[Export] public float KnockBackSpeed = 300;
	public override void _OnHit(VulnerableHitbox subject)
	{
		if (subject.GetParent() is Enemy enemy)
		{
			enemy.KnockBack(Velocity.Normalized(), KnockBackSpeed);
		}
	}
}
