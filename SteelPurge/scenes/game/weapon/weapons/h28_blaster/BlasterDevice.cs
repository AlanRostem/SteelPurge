using Godot;
using System;

public class BlasterDevice : FiringDevice
{
	private static readonly PackedScene BlastScene = GD.Load<PackedScene>("res://scenes/game/weapon/weapons/h28_blaster/Blast.tscn");
	public override void OnFire()
	{
		FireProjectile(BlastScene);
	}
}
