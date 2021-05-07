using Godot;
using System;

public class CartContainer : ScrollContainer
{
	private VBoxContainer _container;
	
	public override void _Ready()
	{
		_container = GetNode<VBoxContainer>("VBoxContainer");
	}
	
	public void AddPurchase(PurchaseWidget widget)
	{
		_container.AddChild(widget);
	}
}
