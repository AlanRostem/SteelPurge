using Godot;
using System;

public class Game : Node2D
{
	public Map CurrentLevel { get; private set; }
	public GameGUI Gui { get; set; }

	public override void _Ready()
	{
		var level1Scene = GD.Load<PackedScene>("res://scenes/game/environment/levels/Level1.tscn");
		ChangeLevel((Map)level1Scene.Instance());
		//Gui = GetNode<GameGUI>("GameGUI");

	}

	public void ChangeLevel(Map level)
	{
		CurrentLevel?.QueueFree();
		CurrentLevel = level;
	}
}
