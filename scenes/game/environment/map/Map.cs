using Godot;
using System;

public class Map : Node2D
{
	private World _world;

	[Signal]
	private delegate void Paused();

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
			EmitSignal(nameof(Paused));
		}
	}
}
