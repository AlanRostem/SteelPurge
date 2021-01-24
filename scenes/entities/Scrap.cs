using Godot;
using System;

public class Scrap : Entity
{
	[Export]
	public uint Value = 50;
	private void _OnPickUp(object body)
	{
	 	var player = (Player)body;
		player.Stats.Money += Value;  
		QueueFree();
	}
}
