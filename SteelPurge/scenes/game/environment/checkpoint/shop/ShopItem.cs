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
	public Texture IconTexture;
	public ItemType Type = ItemType.Fuel;

	public delegate bool PurchaseValidator(Player player, Fabricator fabricator);
	
	public ShopItem()
	{
		
	}

	public virtual void OnPurchase(Fabricator fabricator, World world, Player player)
	{
		
	}

	public virtual bool Validate(Player player, Fabricator fabricator)
	{
		return true;
	}
	
	public ShopItem(string name, uint price, ItemType type, string iconTexturePath)
	{
		IconTexture = GD.Load<Texture>(iconTexturePath);
		Price = price;
		Name = name;
		Type = type;
	}
}
