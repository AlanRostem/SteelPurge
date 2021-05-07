using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Serves as a checkpoint and crafting station. The order in
/// which these are stacked in the Node Tree determines their
/// checkpoint order. 
/// </summary>
public class Fabricator : Area2D
{
	private bool _isPlayerNearShop = false;
	private Player _player;

	private ShopItem[] _availableItems = //TODO: Some items can only be bought once. Consider it in the future
	{
		new ShopItem("Gasoline", 20, 
			"res://scenes/game/entities/collectible/fuel/FuelCollectible.tscn", 
			ShopItem.ItemType.Fuel, 
			"res://assets/texture/ui/icon/gas.png")
	};

	private uint _totalPurchasePrice = 0;

	public uint TotalPurchasePrice => _totalPurchasePrice;

	private List<Purchase> _cart = new List<Purchase>();

	private ShopMenu _shopMenu;
	
	public override void _Ready()
	{
		_shopMenu = GetNode<ShopMenu>("CanvasLayer/ShopMenu");
		_shopMenu.Visible = false;
		foreach (var item in _availableItems)
		{
			switch (item.Type)
			{
				case ShopItem.ItemType.Weapon:
					_shopMenu.AddWeaponItemUi(item);
					break;
				case ShopItem.ItemType.Fuel:
					_shopMenu.AddFuelItemUi(item);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}

	public override void _Process(float delta)
	{
		if (!_isPlayerNearShop) return;
		if (Input.IsActionJustPressed("interact")) // TODO: This is a temporary solution aside from the UI
		{
			_shopMenu.Visible = !_shopMenu.Visible;
			_player.ParentWorld.SetPaused(_shopMenu.Visible);
			/*
			if (_player.PlayerInventory.ScrapCount >= _totalPurchasePrice)
			{
				BuyAllItems();
			}
			*/
		}
	}

	public void AddItemToCart(ShopItem item, out Purchase purchase, uint quantity = 1)
	{
		if (quantity > item.MaxCount)
		{
			purchase = null;
			return;
		}
		_cart.Add(purchase = new Purchase(item, quantity));
		_totalPurchasePrice += item.Price;
	}

	public void RemoveItemFromCart(Purchase purchase)
	{
		// TODO: May need to consider quantity soon
		_cart.Remove(purchase);
	}

	public void BuyAllItems()
	{
		_player.PlayerInventory.LoseScrap(_totalPurchasePrice);
		_totalPurchasePrice = 0;
		foreach (var purchase in _cart)
		{
			_player.ParentWorld.Entities.SpawnEntityDeferred<FallingCollectible>(
				purchase.Item.CollectibleScene,
				Position - new Vector2(0, 24));
		}
		_cart.Clear();
	}

	private void _OnPlayerEnter(object body)
	{
		_isPlayerNearShop = true;
		_player = (Player) body;
	}

	private void _OnPlayerLeave(object body)
	{
		_isPlayerNearShop = false;
	}
}
