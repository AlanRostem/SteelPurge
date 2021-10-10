using Godot;
using System;

public class BlasterDevice : FiringDevice
{
	private static readonly PackedScene BlastScene = GD.Load<PackedScene>("res://scenes/game/weapon/weapons/h28_blaster/Blast.tscn");
	private static readonly PackedScene LargeBlastScene = GD.Load<PackedScene>("res://scenes/game/weapon/weapons/h28_blaster/LargeBlast.tscn");
	public override void OnFireOutput()
	{
		FireProjectile(BlastScene);
	}
}
