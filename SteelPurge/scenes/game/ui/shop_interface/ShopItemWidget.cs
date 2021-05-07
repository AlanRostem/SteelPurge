using Godot;
using System;

public class ShopItemWidget : TextureButton
{
	private ShopMenu _contextMenu;
	private ShopItem _item;
	
	public void SetItemName(string name)
	{
		GetNode<Label>("NameLabel").Text = name;
	}

	public void SetItemPrice(uint price)
	{
		GetNode<Label>("PriceLabel").Text = "x" + price;
	}

	public void SetItemIcon(Texture texture)
	{
		GetNode<TextureRect>("ItemIcon").Texture = texture;
	}
	
	public void Init(ShopItem item, ShopMenu menu)
	{
		_item = item;
		_contextMenu = menu;
		SetItemName(item.Name);
		SetItemPrice(item.Price);
		SetItemIcon(item.IconTexture);
	}
	
	private void _OnPressed()
	{
		_contextMenu.AddPurchase(new Purchase(_item, 1));
	}
}
