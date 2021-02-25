using Godot;
using System;

public class Map : Node2D
{
	public Player PlayerNode;
	private World _world;

	public override void _Ready()
	{
		base._Ready();
		_world = GetNode<World>("World");
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("pause"))
		{
			GetTree().Paused = !GetTree().Paused;
		}
	}
}
