using Godot;
using System;

public class ChronoVector : Node2D
{
	public override void _Ready()
	{
		Update();
	}

	public override void _Draw()
	{
		DrawCircle(Position, 4, Colors.Aqua);
	}
	
	private void _OnLifeTimeout()
	{
		QueueFree();
	}
}
