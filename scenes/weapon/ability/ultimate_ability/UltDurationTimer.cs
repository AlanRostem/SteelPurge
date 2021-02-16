using Godot;
using System;

public class UltDurationTimer : Timer
{
	private UltimateAbility _parent;
	public override void _Ready()
	{
		_parent = GetParent<UltimateAbility>();
		WaitTime = _parent.Duration;
	}

	public override void _Process(float delta)
	{
		_parent.CurrentDuration = TimeLeft;
	}
	
	private void _OnTrigger()
	{
		Start();
	}
}



