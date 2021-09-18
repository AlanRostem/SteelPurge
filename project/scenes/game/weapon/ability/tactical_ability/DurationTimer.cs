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
}
