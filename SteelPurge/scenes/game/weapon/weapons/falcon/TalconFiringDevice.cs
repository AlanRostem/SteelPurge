using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class TalconFiringDevice : FiringDevice
{
	private readonly PackedScene
		TalonScene = GD.Load<PackedScene>("res://scenes/game/weapon/weapons/falcon/Talon.tscn");
	public Array<Talon> Talons = new Array<Talon>();

	private void _OnSwap()
	{
		foreach (var talon in Talons)
		{
			talon.QueueFree();
		}

		GetWeapon().CanFire = true;
		GetWeapon().CurrentRecoilHoverAmmo = GetWeapon().MaxRecoilHoverShots;
		Talons.Clear();
	}
	
	public override void OnFire()
	{
		var talon = (Talon)FireProjectile(TalonScene);
		Talons.Add(talon);

		if (GetWeapon().CurrentRecoilHoverAmmo == 0)
			GetWeapon().CanFire = false;
	}
}
