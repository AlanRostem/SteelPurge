using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Serves as a checkpoint and crafting station
/// </summary>
public class Fabricator : Node2D
{
	private void _OnInteract(Player player)
	{
		var diffHealth = player.MaxHealth - player.Health;
		var diffFuel = player.PlayerInventory.MaxOrdinanceFuel -
					   player.PlayerInventory.GetOrdinanceFuel(player.PlayerInventory.EquippedWeaponEnum);

		if (diffHealth != 0)
		{
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
		}

		if (diffFuel < player.PlayerInventory.ScrapCount)
		{
			player.PlayerInventory.IncreaseOrdinanceFuel(player.PlayerInventory.EquippedWeaponEnum, diffFuel);
			player.PlayerInventory.LoseScrap(diffFuel);
		}
		else
		{
			player.PlayerInventory.IncreaseOrdinanceFuel(player.PlayerInventory.EquippedWeaponEnum,
				player.PlayerInventory.ScrapCount);
			player.PlayerInventory.LoseScrap(player.PlayerInventory.ScrapCount);
		}
	}
}
