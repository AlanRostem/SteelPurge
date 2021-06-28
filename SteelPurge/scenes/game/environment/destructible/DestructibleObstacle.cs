using Godot;
using System;

public class DestructibleObstacle : StaticBody2D
{
	[Export] public uint Health = 200;

	[Signal]
	public delegate void Destroyed();
	
	private void OnHit(uint damage, int direction)
	{
		if (damage >= Health)
		{
			EmitSignal(nameof(Destroyed));
			QueueFree();
		}

		Health -= damage;
	}
}
