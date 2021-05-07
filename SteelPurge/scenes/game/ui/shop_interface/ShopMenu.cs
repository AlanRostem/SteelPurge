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
	private Label _totalLabel;
	
	private Fabricator _parent;
	
	public override void _Ready()
	{
		_parent = GetParent().GetParent<Fabricator>();
		_fuels = GetNode<ShopItemList>("Tabs/TabContainer/Fuels");
		_weapons = GetNode<ShopItemList>("Tabs/TabContainer/Weapons");
		_cartContainer = GetNode<CartContainer>("CartContainer");
		_totalLabel = GetNode<Label>("TotalLabelCount");
	}

	public void AddFuelItemUi(ShopItem item)
	{
		var widget = (ShopItemWidget)ItemWidgetScene.Instance();
		widget.Init(item, this);
		_fuels.AddItem(widget);
	}
	
	public void AddWeaponItemUi(ShopItem item)
	{
		var widget = (ShopItemWidget)ItemWidgetScene.Instance();
		widget.Init(item, this);
		_weapons.AddItem(widget);
	}

	public void AddPurchaseUi(Purchase purchase)
	{
		var widget = (PurchaseWidget) PurchaseWidgetScene.Instance();
		widget.Init(purchase, this);
		_cartContainer.AddPurchase(widget);
	}

	public void AddItemToCartParent(ShopItem item)
	{
		_parent.AddItemToCart(item, out var purchase);
		if (purchase is null) return;
		AddPurchaseUi(purchase);
		_totalLabel.Text = _parent.TotalPurchasePrice.ToString();
	}

	public void RemoveItemFromCartParent(Purchase purchase)
	{
		_parent.RemoveItemFromCart(purchase);
		_totalLabel.Text = _parent.TotalPurchasePrice.ToString();
	}

	private void _OnCompletePurchases()
	{
		_parent.BuyAllItems();
		// TODO: Close
	}
}
