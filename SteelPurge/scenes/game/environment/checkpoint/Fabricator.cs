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
	private World _world;
	public uint Order { get; private set; }
	private bool _isPlayerNearShop = false;
	private Player _player;

	private ShopItem[] _availableItems = //TODO: Some items can only be bought once. Consider it in the future
	{
		new ShopItem(20, "res://scenes/game/entities/collectible/fuel/FuelCollectible.tscn")
	};

	private uint _totalPurchasePrice = 0;
	
	private List<Purchase> _cart = new List<Purchase>();

	public override void _Ready()
	{
		_world = GetParent().GetParent<World>();
		Order = _world.LastCheckPointUuid++;
		
		AddItemToCart(_availableItems[0]); // TODO: Temporary solution to have something to buy
	}

	public override void _Process(float delta)
	{
		if (!_isPlayerNearShop) return;
		if (Input.IsActionJustPressed("interact")) // TODO: This is a temporary solution aside from the UI
		{
			if (_player.PlayerInventory.ScrapCount >= _totalPurchasePrice)
			{
				BuyAllItems();
			}
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
		// TODO: Consider more factors after Prototype 1
		if (_world.CurrentCheckPointEarned >= Order)
			return;
		_world.CurrentCheckPointEarned = Order;
		_world.CurrentCheckPoint = this;
	}

	private void _OnPlayerLeave(object body)
	{
		_isPlayerNearShop = false;
	}
}
