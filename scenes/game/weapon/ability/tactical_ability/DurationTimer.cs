using Godot;
using System;

public class DurationTimer : Timer
{
	private TacticalAbility _parent;

	public override void _Ready()
	{
		_parent = GetParent<TacticalAbility>();
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
