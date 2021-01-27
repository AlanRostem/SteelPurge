using Godot;
using System;

public class XWFrontRogueAttackCycleTimer : Timer
{
	private void _OnPlayerEnterMeleeRange(object body)
	{
		Start();
	}
	
	private void _OnPlayerExitMeleeRange(object body)
	{
		Stop();
	}
}
