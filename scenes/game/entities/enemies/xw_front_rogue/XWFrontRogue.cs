using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Object = Godot.Object;

public class XWFrontRogue : Enemy
{
	[Export] public float WalkSpeed = 32;
	[Export] public float RushSpeed = 110;
	[Export] public uint DamagePerHit = 40;
	[Export] public float PlayerVisualLossRange = 3 * CustomTileMap.Size;
	public int Direction = 1;
	private bool _canSwapDir = true;
	private bool _isRushing = false;

	private Timer _rushDelayTimer;

	[Signal]
	public delegate void TriggerDirSwapCooldown();
	
	[Signal]
	public delegate void OnRush();

	private void _OnCanSwap()
	{
		_canSwapDir = true;
	}

	public override void _Ready()
	{
		base._Ready();
		_rushDelayTimer = GetNode<Timer>("XWFrontRogueRushDelayTimer");
	}

	protected override void _WhenPlayerDetected(Player player)
	{
		if (!_isRushing)
		{
			_isRushing = true;
			_rushDelayTimer.Start();
			Velocity.x = 0;
			Direction = Mathf.Sign(player.Position.x - Position.x);
		}
	}

	protected override void _WhenPlayerNotSeen()
	{
		if (_isRushing) return; // TODO: Might cause bugs
		
		if (_canSwapDir)
		{
			EmitSignal(nameof(TriggerDirSwapCooldown));
			_canSwapDir = false;
			Direction = -Direction;
		}
		
		MoveX(WalkSpeed * Direction);
	}

	private void _OnPlayerEnterMeleeArea(object body)
	{
		((Player)body).TakeDamage(DamagePerHit, Direction);
		_isRushing = false;
	}
	
	private void _OnCanRush()
	{
		Velocity.x = Direction * RushSpeed;
	}
}
