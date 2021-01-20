using Godot;
using System;

public class FireTimer : Timer
{
	
	public override void _Ready()
	{
		
	}
	
	private void _OnWeaponTriggerFire()
	{
		Start();
	}
}



