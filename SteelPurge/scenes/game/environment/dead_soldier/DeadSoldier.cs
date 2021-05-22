using Godot;
using System;

public class DeadSoldier : Node2D
{
	[Export] public Inventory.InventoryWeapon Weapon = Inventory.InventoryWeapon.Falcon;
	
	private void _OnInteract(Player player)
	{
		if (player.PlayerInventory.HasWeapon(Weapon)) return;
		player.PlayerInventory.AddWeapon(Weapon);
		player.PlayerInventory.SwitchWeapon(Weapon);
	}
}
