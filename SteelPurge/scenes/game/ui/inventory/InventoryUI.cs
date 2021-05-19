using Godot;
using System;

public class InventoryUI : Control
{
	public override void _Ready()
	{
		Visible = false;
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("inventory") && !GetTree().Paused)
			Visible = !Visible; // TODO: Remove temporary solution
	}

	private void _OnOpen()
	{
		Visible = !Visible;
	}
}
