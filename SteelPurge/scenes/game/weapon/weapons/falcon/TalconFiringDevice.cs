using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class TalconFiringDevice : FiringDevice
{
	private static readonly PackedScene
		WindSliceScene = GD.Load<PackedScene>("res://scenes/game/weapon/weapons/falcon/WindSlice.tscn");
	private static readonly PackedScene
		AerialSliceScene = GD.Load<PackedScene>("res://scenes/game/weapon/weapons/falcon/AerialSlice.tscn");
	
	public override void OnFire()
	{
		var player = GetWeapon().OwnerPlayer;
		if (player.IsOnFloor() && !player.IsAimingDown && !player.IsAimingUp)
		{
			var slice = FireProjectile(WindSliceScene);
			slice.Position = GetWeapon().OwnerPlayer.Position;
			slice.Scale = new Vector2(GetWeapon().OwnerPlayer.HorizontalLookingDirection, 1);
		}
		else
		{
			var slice = FireProjectile(AerialSliceScene);
			slice.Scale = new Vector2(GetWeapon().OwnerPlayer.HorizontalLookingDirection, 1);
		}
	}
}
