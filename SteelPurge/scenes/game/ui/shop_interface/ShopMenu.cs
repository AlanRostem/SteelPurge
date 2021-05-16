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
		_fuels = GetNode<ShopItemList>("Tabs/CustomTabContainer/Fuels");
		_weapons = GetNode<ShopItemList>("Tabs/CustomTabContainer/Weapons");
		_cartContainer = GetNode<CartContainer>("CartContainer");
		_totalLabel = GetNode<Label>("TotalLabelCount");
	}

	public void Open()
	{
		Visible = true;
		GetTree().Paused = true;
	}

	public void Close()
	{
		Visible = false;
		GetTree().Paused = false;		
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
		if (!item.Validate(_parent.PlayerCustomer, _parent) ||
			_parent.PlayerCustomer.PlayerInventory.ScrapCount < _parent.TotalPurchasePrice + item.Price) return;
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
		 if (!_parent.CanBuy) return;
		 _parent.BuyAllItems();
		_cartContainer.RemoveAllItems();
		_totalLabel.Text = _parent.TotalPurchasePrice.ToString();
		Close();
	}
	
	private void _OnCancel()
	{
		_parent.CancelShopping();
		_cartContainer.RemoveAllItems();
		_totalLabel.Text = _parent.TotalPurchasePrice.ToString();
		Close();
	}
}



