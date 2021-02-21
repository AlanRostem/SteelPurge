using Godot;
using System;

public class Map : Node2D
{
	public Player PlayerNode;
	public Game GameNode;

	public override void _Ready()
	{
		GameNode = GetParent<Game>();
	}
}
