using Godot;
using System;

public class PlayButton : Button
{
	public override void _Ready()
	{
		GrabFocus();
	}
}
