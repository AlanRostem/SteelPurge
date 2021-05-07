using Godot;
using System;

public class ShopItem : Godot.Object
{
	public enum ItemType
	{
		Weapon,
		Fuel,
	}
	
	public uint Price = 20;
	public uint MaxCount = 100;
	public PackedScene CollectibleScene;
	public ItemType Type = ItemType.Fuel;
	
	public ShopItem()
	{
		
	}
	
	public ShopItem(uint price, string collectibleScenePath, ItemType type)
	{
		CollectibleScene = GD.Load<PackedScene>(collectibleScenePath);
		Price = price;
	}
}
