using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class TalconFiringDevice : FiringDevice
{
	private readonly PackedScene
		WindSliceScene = GD.Load<PackedScene>("res://scenes/game/weapon/weapons/falcon/WindSlice.tscn");
	
	public override void OnFire()
	{
		if (GetWeapon().OwnerPlayer.IsOnFloor())
		{
			var slice = FireProjectile(WindSliceScene);
			slice.Position = GetWeapon().OwnerPlayer.Position;
			slice.Scale = new Vector2(GetWeapon().OwnerPlayer.HorizontalLookingDirection, 1);
		}
		else
		{
			
		}
	}
}
