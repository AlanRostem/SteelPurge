using Godot;
using System;

public class PlayerStats : Godot.Object
{
	public class InventoryStats
	{
		public uint[] OrdinanceFuels =
		{
			40,
			40
		};
	}

	public Weapon CreateWeapon()
	{
		return (Weapon) _currentWeaponScene.Instance();
	}

	public InventoryStats Inventory;
	
	public uint ScrapCount = 0;

	private PackedScene _currentWeaponScene =
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/falcon/Falcon.tscn");
	
	public PlayerStats()
	{
		
	}
}
