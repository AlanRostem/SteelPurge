using Godot;
using System;

public class ChronoVector : Node2D
{
	[Signal]
	public delegate void Disappear();
	
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
		EmitSignal(nameof(Disappear));
	}
}
