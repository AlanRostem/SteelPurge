using Godot;
using System;

public class HPBar : ProgressBar
{
	private void _OnPlayerHealthChanged(uint health)
	{
		Value = health;
	}
}
