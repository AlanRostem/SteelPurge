using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class TalconFiringDevice : FiringDevice
{
	private readonly PackedScene
		TalonScene = GD.Load<PackedScene>("res://scenes/game/weapon/weapons/falcon/Talon.tscn");
	public static readonly uint MaxAmmo = 4;
	public uint Ammo = 4;
	public Array<Talon> Talons = new Array<Talon>();

	private void _OnSwap()
	{
		foreach (var talon in Talons)
		{
			talon.QueueFree();
		}
		
		Talons.Clear();
	}
	
	public override void OnFire()
	{
		if (Ammo > 0)
		{
			Ammo--;
			var talon = (Talon) TalonScene.Instance();
			FireProjectile(talon);
			Talons.Add(talon);
		}

		if (Ammo == 0)
		{
			GetWeapon().CanFire = false;
		}
	}
}
