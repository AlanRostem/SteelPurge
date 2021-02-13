using Godot;
using System;

public class CoolDownTimer : Timer
{
	public override void _Ready()
	{
		WaitTime = GetParent<TacticalAbility>().CoolDown;
	}
	
	private void _OnTrigger()
	{
		Start();
	}
}
