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
	public string Name;
	public PackedScene CollectibleScene;
	public Texture IconTexture;
	public ItemType Type = ItemType.Fuel;
	
	public ShopItem()
	{
		
	}
	
	public ShopItem(string name, uint price, string collectibleScenePath, ItemType type, string iconTexturePath)
	{
		CollectibleScene = GD.Load<PackedScene>(collectibleScenePath);
		IconTexture = GD.Load<Texture>(iconTexturePath);
		Price = price;
		Name = name;
	}
}
