using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class TalconFiringDevice : FiringDevice
{
	private static readonly PackedScene
		AerialSliceScene = GD.Load<PackedScene>("res://scenes/game/weapon/weapons/falcon/AerialSlice.tscn");
	private static readonly PackedScene
		LargeWindSliceScene = GD.Load<PackedScene>("res://scenes/game/weapon/weapons/falcon/LargeWindSlice.tscn");
	
	public override void OnFireOutput()
	{
		FireProjectile(AerialSliceScene);
	}

	public override void OnDashFire()
	{
		FireProjectile(LargeWindSliceScene, GetWeapon().RecoilDashDamagePerShot);
	}
}
