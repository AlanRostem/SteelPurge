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
		var dir = Mathf.Sign(ParentWorld.PlayerNode.Position.x - Position.x);
		if (dir != Direction && _canSwapDir)
		{
			EmitSignal(nameof(TriggerDirSwapCooldown));
			_canSwapDir = false;
			Direction = dir;
		}

		
		MoveX(WalkSpeed * Direction);
	}

	private void _OnAttackPlayer()
	{
		ParentWorld.PlayerNode.TakeDamage(DamagePerHit, Direction);
	}
}
