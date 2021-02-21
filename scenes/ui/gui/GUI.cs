using Godot;
using System;

public class GUI : CanvasLayer
{
	public HUD Hud { get; private set; }

	public override void _Ready()
	{
		Hud = GetNode<HUD>("HUD");
	}
}
