using Godot;
using System;

public class ShopItemWidget : TextureButton
{
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
	
	public void Init(ShopItem item)
	{
		SetItemName(item.Name);
		SetItemPrice(item.Price);
		SetItemIcon(item.IconTexture);
	}
}
