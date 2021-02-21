using Godot;
using System;

public class WoodenWall : DestructibleObstacle
{
	public override void _Draw()
	{
		base._Draw();
		var shape = GetNode<CollisionShape2D>("CollisionShape2D");
		var rect = (RectangleShape2D) shape.Shape;
		DrawRect(new Rect2(-rect.Extents, rect.Extents.x * 2, rect.Extents.y * 2), new Color(255));
	}
}
