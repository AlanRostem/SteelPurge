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
	public int Direction = 1;
	private bool _canSwapDir = true;
	private bool _isRushing = true;

	[Signal]
	public delegate void TriggerDirSwapCooldown();


	private void _OnCanSwap()
	{
		_canSwapDir = true;
	}

	protected override void _OnMovement(float delta)
	{
		base._OnMovement(delta);

		MoveX(WalkSpeed * Direction);
	}

	protected override void _WhenPlayerDetected(Player player)
	{
		
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

	private void _OnPlayerEnterMeleeArea(object body)
	{
		((Player)body).TakeDamage(DamagePerHit, Direction);
		_isRushing = false;
	}
}


