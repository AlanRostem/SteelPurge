using Godot;
using System;

public class KineticSlamAbility : TacticalAbility
{
	public float JumpSpeed = 300;
	public float SlamSpeed = 300;

	private bool _isJumping = false;
	private bool _isSlamming = false;
	private Timer _jumpTimer;

	public override void _Ready()
	{
		base._Ready();
		_jumpTimer = GetNode<Timer>("JumpTimer");
	}

	public override void OnActivate()
	{
		var player = GetWeapon().OwnerPlayer;
		player.CanMove = false;

		if (!player.IsOnFloor())
		{
			Slam();
		}
		else
		{
			_isJumping = true;
			player.VelocityY = -JumpSpeed;
		}
	}

	private void Slam()
	{
		var player = GetWeapon().OwnerPlayer;
		player.VelocityY = SlamSpeed;
		_isSlamming = true;
		_isJumping = false;
	}

	public override void OnUpdate()
	{
		var player = GetWeapon().OwnerPlayer;
		if (!_isJumping && _isSlamming && player.IsOnFloor())
		{
			DeActivate();
		}
	}

	public override void OnEnd()
	{
		var player = GetWeapon().OwnerPlayer;
		player.CanMove = true;
		// TODO: Perform AOE damage and stun
	}

	private void _OnJumpEnd()
	{
		Slam();
	}
	
}
