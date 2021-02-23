using Godot;
using System;

public class HPBar : ProgressBar
{
 	private Player _player;
	private void _OnPlayerUpdateHealth(uint hp)
	{
		Value = hp;
	}
}
