using Godot;
using System;

public class InventoryUI : Control
{
	public override void _Ready()
	{
		Visible = false;
	}

	private void _OnOpen()
	{
		Visible = !Visible;
	}
}
