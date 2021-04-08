using Godot;
using System;

public class RegenCycleTimer : Timer
{
	private void _OnStartRegen()
	{
		Start();
	}
	
	
	private void _OnCancelRegen()
	{
		Stop();
	}
}
