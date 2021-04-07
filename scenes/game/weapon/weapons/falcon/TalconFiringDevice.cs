using Godot;
using System;

public class TalconFiringDevice : FiringDevice
{
	private readonly PackedScene
		TalonScene = GD.Load<PackedScene>("res://scenes/game/weapon/weapons/falcon/Talon.tscn");
	private static readonly uint MaxAmmo = 4;
	private uint _ammo = 4;
	
	public override void OnFire()
	{
		if (_ammo > 0)
		{
			// _ammo--;
			FireProjectile((Talon) TalonScene.Instance());
		}
	}
}
