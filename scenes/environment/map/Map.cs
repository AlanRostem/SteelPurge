using Godot;
using System;

public class Map : Node2D
{
	public Player PlayerRef;

	[Signal]
	public delegate void TriggerRoundChangeTimer();
	public override void _Ready()
	{
		var main = (Main) GetParent();
		main.CurrentMap = this;
		// TODO: Remove this temporary solution 
	}
}
