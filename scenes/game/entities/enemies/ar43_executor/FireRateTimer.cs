using Godot;
using System;

public class FireRateTimer : Timer
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
