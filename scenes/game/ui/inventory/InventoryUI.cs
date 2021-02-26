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
			if (CurrentTab == 0)
				CurrentTab = GetTabCount()-1;
			else
				CurrentTab--;
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
