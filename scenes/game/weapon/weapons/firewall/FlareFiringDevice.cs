using Godot;
using System;

public class FlareFiringDevice : ProjectileShotGunFiringDevice
{
	private static readonly PackedScene FlareScene = GD.Load<PackedScene>("res://scenes/game/weapon/weapons/firewall/Flare.tscn");
	
	public override void OnProjectileShot(float angle)
	{
		var player = GetWeapon().OwnerPlayer;
		var world = player.ParentWorld;
		var flare = (Projectile)FlareScene.Instance();
		
		flare.DirectionAngle = Mathf.Rad2Deg(angle);
		flare.Scale = GetWeapon().Scale;
		if (player.IsAimingDown)
		{
			flare.DirectionAngle += 90;
			flare.Rotation += Mathf.Deg2Rad(90);
		}
		else if (player.HorizontalLookingDirection < 0)
		{
			flare.DirectionAngle = 180 - flare.DirectionAngle;
		}


		flare.InitVelocity();
		flare.Position = player.Position;
		world.AddChild(flare);
	}
}
