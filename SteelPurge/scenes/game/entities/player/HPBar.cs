using Godot;
using System;

public class HPBar : TextureProgress
{
	private void _OnPlayerHealthChanged(uint health)
	{
		Value = health;
	}
}
