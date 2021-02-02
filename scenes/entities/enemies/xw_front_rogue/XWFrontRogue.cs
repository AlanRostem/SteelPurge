using Godot;
using System;

public class XWFrontRogue : Enemy
{
	[Export] public float WalkSpeed = 32;
	[Export] public uint DamagePerHit = 40;
	public int Direction = 1;
	private bool _canSwapDir = true;
	private XWFrontRogue _otherRogue = null;
	private bool _pushingOut = false;

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
			Velocity.x = Position.x - _otherRogue.Position.x;
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
            _otherRogue = rogue;
			_pushingOut = true;
		}
	}


	private void _OnRogueExited(object body)
	{
		if (body is XWFrontRogue rogue && rogue != this)
		{
			_pushingOut = false;
		}
	}
}
