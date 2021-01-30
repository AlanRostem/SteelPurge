using Godot;
using System;

public class XWFrontRogue : Enemy
{
	[Export] public float WalkSpeed = 32;
	[Export] public uint DamagePerHit = 40;
	public int Direction = 1;
	private bool _canSwapDir = true;
	
	[Signal]
	public delegate void TriggerDirSwapCooldown();
	
	private void _OnCanSwap()
	{
		_canSwapDir = true;
	}
	
	protected override void _OnMovement(float delta)
	{
		base._OnMovement(delta);
		var dir = Mathf.Sign(ParentMap.PlayerRef.Position.x - Position.x);
		if (dir != Direction && _canSwapDir)
		{
			EmitSignal(nameof(TriggerDirSwapCooldown));
			_canSwapDir = false;
			Direction = dir;
		}
		Velocity.x = WalkSpeed * Direction;
	}
	
	private void _OnAttackPlayer()
	{
		ParentMap.PlayerRef.TakeDamage(DamagePerHit);
	}
}
