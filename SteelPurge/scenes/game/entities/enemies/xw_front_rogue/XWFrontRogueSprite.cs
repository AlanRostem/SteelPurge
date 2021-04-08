using Godot;
using System;

public class XWFrontRogueSprite : AnimatedSprite
{
	private XWFrontRogue _parent;
	public override void _Ready()
	{
		_parent = GetParent<XWFrontRogue>();
	}

	public override void _Process(float delta)
	{
		FlipH = _parent.Direction < 0;
	}
}
