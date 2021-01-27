using Godot;
using System;

public class XWFrontRogueMeleeArea : Area2D
{
	private XWFrontRogue _parent;
	public override void _Ready()
	{
		_parent = GetParent<XWFrontRogue>();
	}

	public override void _Process(float delta)
	{
		Scale = new Vector2(_parent.Direction, 1);
	}
}
