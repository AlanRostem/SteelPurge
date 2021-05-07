using Godot;
using System;

public class ShopMenu : Control
{
	private static readonly PackedScene ItemWidgetScene =
		GD.Load<PackedScene>("res://scenes/game/ui/shop_interface/ShopItemWidget.tscn");

	private ShopItemList _fuels;
	private ShopItemList _weapons;
	
	public override void _Ready()
	{
		_fuels = GetNode<ShopItemList>("Tabs/TabContainer/Fuels");
		_weapons = GetNode<ShopItemList>("Tabs/TabContainer/Weapons");
	}

	public void AddFuelItem(ShopItem item)
	{
		var widget = (ShopItemWidget)ItemWidgetScene.Instance();
		widget.Init(item);
		_fuels.AddItem(widget);
	}
	
	public void AddWeaponItem(ShopItem item)
	{
		var widget = (ShopItemWidget)ItemWidgetScene.Instance();
		widget.Init(item);
		_weapons.AddItem(widget);
	}
}
