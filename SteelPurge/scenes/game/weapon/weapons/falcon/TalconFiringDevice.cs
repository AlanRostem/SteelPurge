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

	private BurstFireTimer _burstFireTimer;
	
	public override void _Ready()
	{
		base._Ready();
		_burstFireTimer = GetNode<BurstFireTimer>("BurstFireTimer");
	}

	public override void OnFireInit()
	{
		_burstFireTimer.Start();
	}

	public override void OnFireOutput()
	{
		FireProjectile(AerialSliceScene);
	}

	public override void OnDashFire()
	{
		FireProjectile(LargeWindSliceScene, GetWeapon().RecoilDashDamagePerShot);
	}
}
