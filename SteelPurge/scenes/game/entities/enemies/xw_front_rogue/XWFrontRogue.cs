using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Object = Godot.Object;

public class XWFrontRogue : Enemy
{
	[Export] public float WalkSpeed = 32;
	[Export] public float RushSpeed = 110;
	[Export] public uint DamagePerHit = 90;
	[Export] public float PlayerVisualLossRange = 3 * CustomTileMap.Size;
	public int Direction = 1;
	private bool _canSwapDir = true;
	private bool _isRushing = false;
	private bool _canRush = false;

	private Timer _rushDelayTimer;
	private Timer _trackCooldownTimer;

	[Signal]
	public delegate void TriggerDirSwapCooldown();
	
	[Signal]
	public delegate void OnRush();

	public override void OnEnableAi()
	{
		_trackCooldownTimer.Start();
	}

	public override void OnDisableAi()
	{
		_isRushing = false;
		_rushDelayTimer.Stop();
		_canRush = false;
		_trackCooldownTimer.Stop();
		_canSwapDir = false;
	}

	private void _OnCanSwap()
	{
		_canSwapDir = true;
	}

	public override void _Ready()
	{
		base._Ready();
		_rushDelayTimer = GetNode<Timer>("XWFrontRogueRushDelayTimer");
		_trackCooldownTimer = GetNode<Timer>("XWFrontRogueTrackCooldownTimer");
	}

	protected override void _ProcessWhenPlayerDetected(Player player)
	{
		if (!_isRushing)
		{
			_isRushing = true;
			_rushDelayTimer.Start();
			MoveX(0);
			Direction = Mathf.Sign(player.Position.x - Position.x);
		} 
		else if (_canRush)
		{
			MoveX(Direction * RushSpeed);
			
			var newDirection = Mathf.Sign(player.Position.x - Position.x);
			if (newDirection != Direction)
			{
				_canRush = false;
				_rushDelayTimer.Start();
				StopMovingX();
				Direction = newDirection;
			}
		}
	}

	protected override void _ProcessWhenPlayerNotSeen()
	{
		if (_isRushing)
		{
			_isRushing = false;
			_canRush = false;
			_rushDelayTimer.Stop();
			return;
		} // TODO: Might cause bugs
		
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
		AttackPlayer(DamagePerHit, ((Player)body), new Vector2(Direction, 0));
		_isRushing = false;
		QueueFree();
	}
	
	private void _OnCanRush()
	{
		_canRush = true;
	}
}
