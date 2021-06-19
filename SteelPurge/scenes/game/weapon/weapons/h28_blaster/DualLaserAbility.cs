using Godot;
using System;

public class DualLaserAbility : TacticalAbility
{
	private static readonly PackedScene LaserShotScene =
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/h28_blaster/LaserShot.tscn");

	private Timer _shotDelayTimer;
	private uint _shotsFired = 0;

	public override void _Ready()
	{
		base._Ready();
		_shotDelayTimer = GetNode<Timer>("ShotDelayTimer");
	}

	public override void OnActivate()
	{
		GetWeapon().IsFiring = true;
		_shotDelayTimer.Start();
		if (GetWeapon().OwnerPlayer.IsMovingTooFast())
			GetWeapon().OwnerPlayer.VelocityX = Mathf.Sign(GetWeapon().OwnerPlayer.VelocityX) * Player.WalkSpeed;
	}

	public override void OnUpdate()
	{
		if (GetWeapon().OwnerPlayer.VelocityY > 0)
		{
			GetWeapon().OwnerPlayer.VelocityY = GetWeapon().OwnerPlayer.Gravity / 10;
		}
	}

	public override void OnEnd()
	{
		GetWeapon().IsFiring = false;
		_shotsFired = 0;
		GetWeapon().OwnerPlayer.CanMove = true;
	}

	private void _OnShoot()
	{
		var laser = (LaserShot) LaserShotScene.Instance();
		if (_shotsFired > 0 && !GetWeapon().OwnerPlayer.IsAimingDown && !GetWeapon().OwnerPlayer.IsAimingUp)
			laser.Position = GetWeapon().OwnerPlayer.Position + new Vector2(0, 7);
		else 
			laser.Position = GetWeapon().OwnerPlayer.Position;
		if (GetWeapon().OwnerPlayer.IsAimingDown)
			laser.RotationDegrees = 90;
		else if (GetWeapon().OwnerPlayer.IsAimingUp)
			laser.RotationDegrees = -90;
		else 
			laser.Scale = new Vector2(GetWeapon().OwnerPlayer.HorizontalLookingDirection, 1);
		GetWeapon().OwnerPlayer.ParentWorld.AddChild(laser);
		_shotsFired++;
		GetWeapon().OwnerPlayer.IsGravityEnabled = true;
		GetWeapon().OwnerPlayer.VelocityY = -GetWeapon().HoverRecoilSpeed;
		if (_shotsFired >= 2)
		{
			_shotDelayTimer.Stop();
		}
	}
}
