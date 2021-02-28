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

	private bool _pushingOut = false;
	private readonly HashSet<XWFrontRogue> _otherRogues = new HashSet<XWFrontRogue>();

	public float MaxDepth;

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

		if (_pushingOut)
		{
			XWFrontRogue otherRogue = _otherRogues.First();
			var lowestDistance = Position.DistanceSquaredTo(otherRogue.Position);
			foreach (var rogue in _otherRogues)
			{
				var current = Position.DistanceSquaredTo(rogue.Position);
				if (lowestDistance > current)
				{
					otherRogue = rogue;
					lowestDistance = current;
				}
			}

			var dx = Position.x - otherRogue.Position.x;
			if (dx == 0f)
				dx = Mathf.Epsilon;
			var fx = MaxDepth / dx;
			MoveX(fx * WalkSpeed);
		}
		else
		{
			MoveX(WalkSpeed * Direction);

		}
	}

	private void _OnAttackPlayer()
	{
		ParentWorld.PlayerNode.TakeDamage(DamagePerHit, Direction);
	}

	private void _OnRogueCollided(object body)
	{
		if (body is XWFrontRogue rogue && rogue != this)
		{
			_otherRogues.Add(rogue);
			_pushingOut = true;
		}
	}


	private void _OnRogueExited(object body)
	{
		if (body is XWFrontRogue rogue && rogue != this)
		{
			_otherRogues.Remove(rogue);
			if (_otherRogues.Count == 0)
				_pushingOut = false;
		}
	}
}
