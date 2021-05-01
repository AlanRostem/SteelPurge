using Godot;
using System;

public class WorldSegment : Node2D
{
	public Position2D SpawnPoint { get; private set; }
		
	public override void _Ready()
	{
		SpawnPoint = GetNode<Position2D>("SpawnPoint");
	}
}
