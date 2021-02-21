using Godot;
using System;

public class GameGUI : CanvasLayer
{
	public HUD Hud { get; private set; }

	public override void _Ready()
	{
		Hud = GetNode<HUD>("HUD");
		GetParent<Game>().Gui = this;
	}
}
