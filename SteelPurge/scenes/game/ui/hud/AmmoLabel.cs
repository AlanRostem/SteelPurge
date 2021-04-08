using Godot;
using System;

public class AmmoLabel : Label
{
	private void _OnPlayerWeaponClipChanged(uint clip)
	{
		Text = clip.ToString();
	}
}
