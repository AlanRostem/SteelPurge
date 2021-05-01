using Godot;
using System;

public class WorldSegment : Node2D
{
	public Vector2 SpawnPoint { get; private set; }
		
	public override void _Ready()
	{
		SpawnPoint = GetNode<Fabricator>("Fabricator").Position;
	}
}
