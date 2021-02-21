using Godot;
using System;

public class Game : Node2D
{
	public Map CurrentLevel { get; private set; }

	public override void _Ready()
	{
		var level1Scene = GD.Load<PackedScene>("res://scenes/game/environment/levels/Level1.tscn");
		ChangeLevel((Map)level1Scene.Instance());
	}

	public void ChangeLevel(Map level)
	{
		CurrentLevel?.QueueFree();
		CurrentLevel = level;
	}
}
