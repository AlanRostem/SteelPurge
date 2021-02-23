using Godot;
using System;

public class HPBar : ProgressBar
{
	private void _OnPlayerUpdateHealth(uint hp)
	{
		Value = hp;
	}
}
