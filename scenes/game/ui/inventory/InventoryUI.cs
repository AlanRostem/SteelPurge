using Godot;
using System;

public class InventoryUI : TabContainer
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
