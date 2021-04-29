using Godot;
using System;

public class ShopItem : Godot.Object
{
	public uint Price = 20;
	public uint MaxCount = 100;
	public PackedScene CollectibleScene;
	
	public ShopItem(uint price, string collectibleScenePath)
	{
		CollectibleScene = GD.Load<PackedScene>(collectibleScenePath);
		Price = price;
	}
}
