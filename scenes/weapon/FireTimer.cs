using Godot;
using System;

public class FireTimer : Timer
{
	public override void _Ready()
	{
		var weapon = (Weapon) GetParent();
		WaitTime = 60f / weapon.RateOfFire;
	}

	private void _OnWeaponTriggerFire()
	{
		Start();
	}
	
	private void _OnWeaponCancelFire()
	{
		Stop();
	}
}
