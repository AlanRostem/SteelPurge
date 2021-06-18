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
		var total = diffFuel + diffHealth;

		player.Health += diffHealth;
		player.PlayerInventory.LoseScrap(diffHealth);
		player.PlayerInventory.AddOrdinanceFuel(diffFuel);
		player.PlayerInventory.LoseScrap(diffFuel);
	}
}
