using Godot;
using System;

public class AR43Executor : Enemy
{
	public int Direction = -1;
	[Export] public float WalkSpeed = 40;
	private RayCast2D _groundScanner;

	private bool _startWalking = false;

	[Signal]
	public delegate void TriggerApproach();

	[Signal]
	public delegate void TriggerStand();

	public override void _Ready()
	{
		base._Ready();
		_groundScanner = GetNode<RayCast2D>("GroundScanner");
	}
	
	protected override void _WhenPlayerDetected(Player player)
	{
		Direction = Mathf.Sign(player.Position.x - Position.x);

		if (_startWalking && _groundScanner.IsColliding())
			Velocity.x = Direction * WalkSpeed;
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
	
	private void _OnFire()
	{
		
	}
}
