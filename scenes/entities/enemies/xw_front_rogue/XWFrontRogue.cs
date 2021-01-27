using Godot;
using System;

public class XWFrontRogue : Enemy
{
	[Export] public float WalkSpeed = 40;
	[Export] public uint DamagePerHit = 40;
	public int Direction = 1;
	protected override void _OnMovement(float delta)
	{
		base._OnMovement(delta);
		Direction = Mathf.Sign(ParentMap.PlayerRef.GlobalPosition.x - GlobalPosition.x);
		Velocity.x = WalkSpeed * Direction;
	}
	
	private void _OnAttackPlayer()
	{
		ParentMap.PlayerRef.TakeDamage(DamagePerHit);
	}
}
