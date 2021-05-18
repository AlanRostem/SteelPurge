using Godot;
using System;

public class FuelCollectible : FallingCollectible
{

	[Export] public uint Count = 20;
	[Export] public Inventory.OrdinanceFuelType FuelType = Inventory.OrdinanceFuelType.Gasoline;
	
	public override void OnCollected(Player player)
	{
		player.PlayerInventory.AddOrdinanceFuel(Count, FuelType);
	}
}
