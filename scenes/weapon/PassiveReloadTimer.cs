using Godot;
using System;

public class PassiveReloadTimer : Timer
{
	public override void _Ready()
	{
		var weapon = (Weapon)GetParent();
		WaitTime = weapon.PassiveReloadSpeed;
	}
	
	private void _OnTrigger()
	{
		Start();
	}

	private void _OnCancel()
	{
		Stop();
	}
}
