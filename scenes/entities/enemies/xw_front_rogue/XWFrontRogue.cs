using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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
			var fx = Mathf.Clamp(dx / MaxDepth, -1.1f, 1.1f);
			var vx = 1f - fx;
			Velocity.x = -vx * WalkSpeed;
		}
		else
		{
			Velocity.x = WalkSpeed * Direction;
		}
	}

	private void _OnAttackPlayer()
	{
		ParentMap.PlayerRef.TakeDamage(DamagePerHit, Direction);
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
