using Godot;
using System;

public class Scrap : FallingCollectible
{
	[Export] public uint Count = 10;
	public override void OnCollected(Player player)
	{
		player.PlayerInventory.PickUpScrap(Count);
	}
}
