using Godot;
using System;

public class StandIntervalTimer : Timer
{
	private void _OnTrigger()
	{
		Start();
	}
	
	private void _OnCancel()
	{
		Stop();
	}
}
