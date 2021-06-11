using Godot;
using System;

public class CoolDownTimer : Timer
{
	private TacticalAbility _parent;

	public override void _Ready()
	{
		_parent = GetParent<TacticalAbility>();
		WaitTime = _parent.CoolDown;
	}

	private void _OnTrigger()
	{
		Start();
	}
}
