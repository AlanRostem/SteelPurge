using Godot;
using System;

public class TestLevel : Game
{

	public override void _Ready()
	{
		base._Ready();
		GameWorld.PlayerNode.PlayerInventory.AddWeapon(Inventory.InventoryWeapon.Falcon);
		GameWorld.PlayerNode.PlayerInventory.AddWeapon(Inventory.InventoryWeapon.Firewall);
		GameWorld.PlayerNode.PlayerInventory.AddWeapon(Inventory.InventoryWeapon.H28);
		GameWorld.PlayerNode.PlayerInventory.AddWeapon(Inventory.InventoryWeapon.Joule);
	}
	
}
