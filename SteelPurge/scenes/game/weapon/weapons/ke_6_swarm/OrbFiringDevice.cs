using Godot;
using System;

public class OrbFiringDevice : FiringDevice
{
	private static readonly PackedScene OrbScene =
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/ke_6_swarm/KineticOrb.tscn");
	
	private static readonly PackedScene LargeOrbScene =
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/ke_6_swarm/LargeKineticOrb.tscn");

	public override void OnFireOutput()
	{
		FireProjectile(OrbScene);
	}

	public override void OnDashFire()
	{
		FireProjectile(LargeOrbScene);
	}
}
