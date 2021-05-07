using Godot;
using System;

public class ShopItemList : ScrollContainer
{
	private VBoxContainer _container;

	public override void _Ready()
	{
		_container = GetNode<VBoxContainer>("VBoxContainer");
	}

	public void AddItem(ShopItemWidget widget)
	{
		_container.AddChild(widget);
	}
}
