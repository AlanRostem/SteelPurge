using Godot;
using System;

public class Scrap : FallingCollectible
{
	[Export] public uint Count = 10;
	public override void OnCollected(Player player)
	{
		player.Health += Count;
	}
	
	private void _OnScrapEntered(object body)
	{
		if (body is Scrap scrap && body != this && IsOnFloor())
		{
			Count += scrap.Count;
			scrap.QueueFree();
		}
	}
}
