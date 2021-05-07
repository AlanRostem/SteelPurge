using Godot;
using System;

public class ShopMenu : Control
{
	private static readonly PackedScene ItemWidgetScene =
		GD.Load<PackedScene>("res://scenes/game/ui/shop_interface/ShopItemWidget.tscn");
	private static readonly PackedScene PurchaseWidgetScene =
		GD.Load<PackedScene>("res://scenes/game/ui/shop_interface/PurchaseWidget.tscn");

	private ShopItemList _fuels;
	private ShopItemList _weapons;
	private CartContainer _cartContainer;
	private Fabricator _parent;
	
	public override void _Ready()
	{
		_parent = GetParent<Fabricator>();
		_fuels = GetNode<ShopItemList>("Tabs/TabContainer/Fuels");
		_weapons = GetNode<ShopItemList>("Tabs/TabContainer/Weapons");
		_cartContainer = GetNode<CartContainer>("CartContainer");
	}

	public void AddFuelItem(ShopItem item)
	{
		var widget = (ShopItemWidget)ItemWidgetScene.Instance();
		widget.Init(item, this);
		_fuels.AddItem(widget);
	}
	
	public void AddWeaponItem(ShopItem item)
	{
		var widget = (ShopItemWidget)ItemWidgetScene.Instance();
		widget.Init(item, this);
		_weapons.AddItem(widget);
	}

	public void AddPurchase(Purchase purchase)
	{
		var widget = (PurchaseWidget) PurchaseWidgetScene.Instance();
		widget.Init(purchase, this);
		_cartContainer.AddPurchase(widget);
	}
	
	
	private void _OnCompletePurchases()
	{
		
	}
	
}
