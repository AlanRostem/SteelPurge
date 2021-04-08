using Godot;
using System;

public class ApproachIntervalTimer : Timer
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
