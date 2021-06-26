using Godot;
using System;

public class FuelCollectible : FallingCollectible
{

	[Export] public uint Count = 20;

	public override bool CollectionCondition(Player player)
	{
		var fuel = player.PlayerInventory.GetOrdinanceFuel(player.PlayerInventory.EquippedWeaponEnum);
		return fuel < player.PlayerInventory.MaxOrdinanceFuel;
	}

	public override void OnCollected(Player player)
	{
		player.PlayerInventory.IncreaseOrdinanceFuel(player.PlayerInventory.EquippedWeaponEnum, Count);
	}
}
