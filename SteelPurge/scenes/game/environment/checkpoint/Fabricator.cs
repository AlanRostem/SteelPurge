using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Serves as a checkpoint and crafting station. The order in
/// which these are stacked in the Node Tree determines their
/// checkpoint order. 
/// </summary>
public class Fabricator : Node2D
{
	private void _OnInteract(Player player)
	{
		var diffHealth = player.MaxHealth - player.Health;
		var diffFuel = player.PlayerInventory.MaxOrdinanceFuel - player.PlayerInventory.OrdinanceFuel;

		if (diffHealth < player.PlayerInventory.ScrapCount)
		{
			player.Health += diffHealth;
			player.PlayerInventory.LoseScrap(diffHealth);
		}
		else
		{
			player.Health += player.PlayerInventory.ScrapCount;
			player.PlayerInventory.LoseScrap(player.PlayerInventory.ScrapCount);
			return;
		}
		
		if (diffFuel < player.PlayerInventory.ScrapCount)
		{
			player.PlayerInventory.AddOrdinanceFuel(diffFuel);
			player.PlayerInventory.LoseScrap(diffFuel);
		}
	}
}
