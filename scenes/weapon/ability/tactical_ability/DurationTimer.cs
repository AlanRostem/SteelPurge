using Godot;
using System;

public class DurationTimer : Timer
{
    public override void _Ready()
    {
        WaitTime = GetParent<TacticalAbility>().Duration;
    }

    private void _OnTrigger()
	{
		Start();
	}
}
