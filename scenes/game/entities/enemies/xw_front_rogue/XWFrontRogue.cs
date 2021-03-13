using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Object = Godot.Object;

public class XWFrontRogue : Enemy
{

	[Export] public float WalkSpeed = 32;
	[Export] public uint DamagePerHit = 40;
	public int Direction = 1;
	private bool _canSwapDir = true;
	private Player _detectedPlayer;
	private bool _isPlayerFound = false;

	[Signal]
	public delegate void TriggerDirSwapCooldown();
	

	private void _OnCanSwap()
	{
		_canSwapDir = true;
	}

	public override void _OnCollision(KinematicCollision2D collider)
	{
		if (collider.Collider is TileMap && IsOnWall())
		{
			MoveY(-WalkSpeed * 1.25f);
		}
	}


	protected override void _OnMovement(float delta)
	{
		base._OnMovement(delta);
		
		
		if (!_isPlayerFound && _canSwapDir)
		{
			EmitSignal(nameof(TriggerDirSwapCooldown));
			_canSwapDir = false;
			Direction = -Direction;
		}

		if (_isPlayerFound)
		{
			Direction = Mathf.Sign(_detectedPlayer.Position.x - Position.x);
		}
		
		MoveX(WalkSpeed * Direction);
	}

	private void _OnAttackPlayer()
	{
		_detectedPlayer.TakeDamage(DamagePerHit, Direction);
	}
	
	private void _OnPlayerDetected(object body)
	{
		_detectedPlayer = (Player)body;
		_isPlayerFound = true;
	}
	
	private void _OnPlayerLeave(object body)
	{
		_isPlayerFound = false;
	}
}
