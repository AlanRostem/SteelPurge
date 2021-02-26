using Godot;
using System;

public class InventoryUI : TabContainer
{
	public override void _Ready()
	{
		Visible = false;
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("ui_inventory_switch_tab_left"))
		{
			CurrentTab = (CurrentTab - 1) % GetTabCount();
		}

		if (Input.IsActionJustPressed("ui_inventory_switch_tab_right"))
		{
			CurrentTab = (CurrentTab + 1) % GetTabCount();
		}
	}

	private void _OnOpen()
	{
		Visible = !Visible;
	}
}
