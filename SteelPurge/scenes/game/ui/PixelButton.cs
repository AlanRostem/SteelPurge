using Godot;
using System;

public class PixelButton : Button
{
	[Export] public bool IsFocusedOnReady = false;
	public override void _Ready()
	{
		if (IsFocusedOnReady)
		{
			GrabFocus();
		}
	}
	
	private void _OnVisibilityChanged()
	{
		if (IsFocusedOnReady && Visible)
		{
			GrabFocus();
		}
	}
}
