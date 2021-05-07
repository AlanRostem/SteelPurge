using Godot;
using System;

public class PurchaseWidget : TextureButton
{
	private ShopMenu _contextMenu;
	private Purchase _purchase;
	
	public void SetItemPrice(uint price)
	{
		GetNode<Label>("PriceLabel").Text = "x" + price;
	}

	public void SetItemIcon(Texture texture)
	{
		GetNode<TextureRect>("ItemIcon").Texture = texture;
	}
	
	public void Init(Purchase purchase, ShopMenu menu)
	{
		_contextMenu = menu;
		_purchase = purchase;
		SetItemPrice(purchase.Item.Price);
		SetItemIcon(purchase.Item.IconTexture);
	}
	
	private void _OnPressed()
	{
		_contextMenu.RemoveItemFromCartParent(_purchase);
		QueueFree();
	}
}
