using Godot;
using System;

public class StatusEffectTimer : Timer
{
	public override void _Ready()
	{
		WaitTime = GetParent<StatusEffect>().Duration;
	}
}
