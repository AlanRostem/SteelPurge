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

	public override void _Draw()
	{
		var rect = new Rect2(new Vector2(), RectSize);
		var color = new Color(255, 255, 0);
		DrawRect(rect, color);
	}
}
