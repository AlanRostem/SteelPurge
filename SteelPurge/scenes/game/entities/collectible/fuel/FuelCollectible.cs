using Godot;
using System;

public class FuelCollectible : FallingCollectible
{

	[Export] public uint Count = 20;
	
	public override void OnCollected(Player player)
	{
		player.PlayerInventory.AddOrdinanceFuel(Count);
	}
}
