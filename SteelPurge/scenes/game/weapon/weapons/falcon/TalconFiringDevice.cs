using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class TalconFiringDevice : FiringDevice
{
	private static readonly PackedScene
		AerialSliceScene = GD.Load<PackedScene>("res://scenes/game/weapon/weapons/falcon/AerialSlice.tscn");
	
	public override void OnFire()
	{
		var slice = FireProjectile(AerialSliceScene);
		slice.Scale = new Vector2(GetWeapon().OwnerPlayer.HorizontalLookingDirection, 1);
	}
}
