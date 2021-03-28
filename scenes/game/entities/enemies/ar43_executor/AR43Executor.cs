using Godot;
using System;

public class AR43Executor : Enemy
{
	public int Direction = -1;
	[Export] public float WalkSpeed = 40;

	private bool _startWalking = false;

	[Signal]
	public delegate void TriggerApproach();

	[Signal]
	public delegate void TriggerStand();

	protected override void _OnMovement(float delta)
	{
		Velocity.x = Direction * WalkSpeed;
	}

	protected override void _WhenPlayerDetected(Player player)
	{
		if (_startWalking)
			Direction = Mathf.Sign(player.Position.x - Position.x);
		else
			Velocity.x = 0;
	}
	
	private void _OnApproach()
	{
		_startWalking = false;
		EmitSignal(nameof(TriggerStand));
	}
	
	private void _OnStand()
	{
		_startWalking = true;
		EmitSignal(nameof(TriggerApproach));
	}
}
