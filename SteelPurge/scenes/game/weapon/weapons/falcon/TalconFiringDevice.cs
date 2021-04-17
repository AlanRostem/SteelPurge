using Godot;
using System;

public class TalconFiringDevice : FiringDevice
{
	private readonly PackedScene
		TalonScene = GD.Load<PackedScene>("res://scenes/game/weapon/weapons/falcon/Talon.tscn");
	public static readonly uint MaxAmmo = 4;
	public uint Ammo = 4;
	
	public override void OnFire()
	{
		if (Ammo > 0)
		{
			Ammo--;
			FireProjectile((Talon) TalonScene.Instance());
		}

		if (Ammo == 0)
		{
			GetWeapon().CanFire = false;
		}
	}
}
