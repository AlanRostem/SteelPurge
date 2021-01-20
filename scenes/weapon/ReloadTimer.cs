using Godot;
using System;

public class ReloadTimer : Timer
{
	public override void _Ready()
	{
		var weapon = (Weapon)GetParent();
		WaitTime = weapon.ReloadSpeed;
	}
	
	private void _OnWeaponTriggerReload()
	{
		Start();
	}
	
	private void _OnWeaponCancelReload()
	{
		Stop();
	}
}
