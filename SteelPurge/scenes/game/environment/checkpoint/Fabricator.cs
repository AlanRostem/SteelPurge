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
		new ShopItem(20, "res://scenes/game/entities/collectible/fuel/FuelCollectible.tscn")
	};

	private uint _totalPurchasePrice = 0;
	
	private List<Purchase> _cart = new List<Purchase>();

	private Control _shopMenu;
	
	public override void _Ready()
	{
		AddItemToCart(_availableItems[0]); // TODO: Temporary solution to have something to buy
		_shopMenu = GetNode<Control>("CanvasLayer/ShopMenu");
		_shopMenu.Visible = false;
	}

	public override void _Process(float delta)
	{
		if (!_isPlayerNearShop) return;
		if (Input.IsActionJustPressed("interact")) // TODO: This is a temporary solution aside from the UI
		{
			_shopMenu.Visible = !_shopMenu.Visible;
			/*
			if (_player.PlayerInventory.ScrapCount >= _totalPurchasePrice)
			{
				BuyAllItems();
			}
			*/
		}
	}

	public void AddItemToCart(ShopItem item, uint quantity = 1)
	{
		if (quantity > item.MaxCount) return;
		_cart.Add(new Purchase(item, quantity));
		_totalPurchasePrice += item.Price;
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
		
		AddItemToCart(_availableItems[0]); // TODO: Temporary solution to have something to buy, but again
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
