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

		MoveX(WalkSpeed * Direction);
	}

	protected override void _WhenPlayerDetected(Player player)
	{
		Direction = Mathf.Sign(DetectedPlayer.Position.x - Position.x);
	}

	protected override void _WhenPlayerNotSeen()
	{
		if (_canSwapDir)
		{
			EmitSignal(nameof(TriggerDirSwapCooldown));
			_canSwapDir = false;
			Direction = -Direction;
		}
	}

	private void _OnAttackPlayer()
	{
		DetectedPlayer.TakeDamage(DamagePerHit, Direction);
	}
}
