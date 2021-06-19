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
		if (!GetWeapon().OwnerPlayer.IsOnFloor())
			GetWeapon().OwnerPlayer.IsGravityEnabled = false;
	}

	public override void OnEnd()
	{
		GetWeapon().IsFiring = false;
	}

	private void _OnShoot()
	{
		var laser = (LaserShot) LaserShotScene.Instance();
		laser.Position = GetWeapon().OwnerPlayer.Position;
		laser.Scale = new Vector2(GetWeapon().OwnerPlayer.HorizontalLookingDirection, 1);
		GetWeapon().OwnerPlayer.ParentWorld.AddChild(laser);
		_shotsFired++;
		GetWeapon().OwnerPlayer.IsGravityEnabled = true;
		GetWeapon().OwnerPlayer.VelocityY = -GetWeapon().HoverRecoilSpeed;
		if (_shotsFired >= 2)
		{
			_shotDelayTimer.Stop();
			DeActivate();
		}
	}
}
