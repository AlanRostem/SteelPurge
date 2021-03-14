using Godot;
using System;

public class LingerTimer : Timer
{
	public override void _Ready()
	{
		WaitTime = GetParent<ResourceAbility>().LingerDuration;
	}
	
	private void _OnTrigger()
	{
		Start();
	}
}
