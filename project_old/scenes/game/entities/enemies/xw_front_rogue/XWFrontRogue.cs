using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Object = Godot.Object;

public class XWFrontRogue : Enemy
{
	[Export] public float WalkSpeed = 32;
	[Export] public float RushSpeed = 110;
	public int Direction = 1;
	private bool _canSwapDir = true;
	private bool _isRushing = false;
	private bool _canRush = false;

	private CustomTimer _rushDelayTimer;
	private CustomTimer _trackCooldownTimer;

	public override void FeedEntityData(Godot.Collections.Dictionary<string, object> data)
	{
		base.FeedEntityData(data);
		var eData = new EntityData(data);
		eData.ConfigureTimer(nameof(_rushDelayTimer), _rushDelayTimer);
		eData.ConfigureTimer(nameof(_trackCooldownTimer), _trackCooldownTimer);
		_isRushing = eData.GetAny<bool>(nameof(_isRushing));
		_canSwapDir = eData.GetAny<bool>(nameof(_canSwapDir));
		_canRush = eData.GetAny<bool>(nameof(_canRush));
		Direction = eData.GetAny<int>(nameof(Direction));
	}

	public override Godot.Collections.Dictionary<string, object> ExportEntityData()
	{
		var data = new EntityData(base.ExportEntityData());
		data.SetTimer(nameof(_rushDelayTimer), _rushDelayTimer);
		data.SetTimer(nameof(_trackCooldownTimer), _trackCooldownTimer);
		data.SetAny(nameof(_isRushing), _isRushing);
		data.SetAny(nameof(_canSwapDir), _canSwapDir);
		data.SetAny(nameof(_canRush), _canRush);
		data.SetAny(nameof(Direction), Direction);
		return data.GetJson();
	}

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

	public override void _Init()
	{
		base._Init();
		_rushDelayTimer = GetNode<CustomTimer>("RushDelayTimer");
		_trackCooldownTimer = GetNode<CustomTimer>("TrackCooldownTimer");
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
			_trackCooldownTimer.Start();
			_canSwapDir = false;
			Direction = -Direction;
		}
		
		MoveX(WalkSpeed * Direction);
	}

	private void _OnPlayerEnterMeleeArea(Player player)
	{
		if (player.IsInvulnerable) return;
		AttackPlayer(player, new Vector2(Direction, 0));
		_isRushing = false;
		if (IsCurrentlyLethal) ParentWorld.CurrentSegment.Entities.RemoveEntity(this);
	}
	
	private void _OnCanRush()
	{
		_canRush = true;
	}
}
