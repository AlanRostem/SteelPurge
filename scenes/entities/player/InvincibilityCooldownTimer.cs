using Godot;
using System;

public class InvincibilityCooldownTimer : Timer
{
	private void _OnTrigger()
	{
		Start();
	}
}
