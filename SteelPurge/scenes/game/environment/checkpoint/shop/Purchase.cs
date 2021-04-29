using Godot;
using System;

public class Purchase : Godot.Object
{
	public ShopItem Item;
	public uint Quantity;
	public Purchase(ShopItem item, uint quantity)
	{
		Item = item;
		Quantity = quantity;
	}
}
