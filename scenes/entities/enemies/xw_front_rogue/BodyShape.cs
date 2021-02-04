using Godot;
using System;

public class BodyShape : CollisionShape2D
{
	public override void _Ready()
	{
	 	GetParent<XWFrontRogue>().MaxDepth = ((RectangleShape2D)Shape).Extents.x;   
	}
}
