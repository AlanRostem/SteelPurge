using Godot;
using System;

public class OrbFiringDevice : FiringDevice
{
	private static readonly PackedScene OrbScene =
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/ke_6_swarm/KineticOrb.tscn");

	public override void OnFire()
	{
		FireProjectile(OrbScene);
	}
}
