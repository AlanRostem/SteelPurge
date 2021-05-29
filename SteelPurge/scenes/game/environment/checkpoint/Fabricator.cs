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
	public Player PlayerCustomer { get; private set; }
	


	private ShopItem[] _availableItems = //TODO: Some items can only be bought once. Consider it in the future
	{
		new FuelShopItem("Gasoline", 2,
			"res://assets/texture/ui/icon/gas.png",
			"res://scenes/game/entities/collectible/fuel/GasCanCollectible.tscn"),
		new FuelShopItem("EM-Cells", 2,
			"res://assets/texture/ui/icon/xe_slug.png",
			"res://scenes/game/entities/collectible/fuel/EmCellCollectible.tscn"),
	};

	private uint _totalPurchasePrice = 0;

	public uint TotalPurchasePrice => _totalPurchasePrice;

	private readonly List<Purchase> _cart = new List<Purchase>();


	public bool CanBuy => _totalPurchasePrice <= PlayerCustomer.PlayerInventory.ScrapCount && PlayerCustomer.PlayerInventory.ScrapCount > 0;

	public void AddItemToCart(ShopItem item, uint quantity = 1)
	{
		if (quantity > item.MaxCount)
		{
			// return;
		}

		_cart.Add(new Purchase(item, quantity));
		_totalPurchasePrice += item.Price;
	}

	public void RemoveItemFromCart(Purchase purchase)
	{
		_cart.Remove(purchase);
		_totalPurchasePrice -= purchase.Item.Price;
	}

	public void BuyAllItems()
	{
		PlayerCustomer.PlayerInventory.LoseScrap(_totalPurchasePrice);
		_totalPurchasePrice = 0;
		foreach (var purchase in _cart)
		{
			purchase.Item.OnPurchase(this, PlayerCustomer.ParentWorld, PlayerCustomer);
		}

		_cart.Clear();
	}

	public void CancelShopping()
	{
		_totalPurchasePrice = 0;
		_cart.Clear();
	}
	
	private void _OnInteract(Player player)
	{
		PlayerCustomer = player;
		foreach (var item in _availableItems)
			AddItemToCart(item);
		if (_totalPurchasePrice > player.PlayerInventory.ScrapCount)
		{
			CancelShopping();
			return;
		}
		BuyAllItems();
	}
}
