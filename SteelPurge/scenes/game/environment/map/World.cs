using Godot;
using System;

public class World : Node2D
{
	public Player PlayerNode {get; private set;}
	public Vector2 SpawnPoint;
	public override void _Ready()
	{
		PlayerNode = GetNode<Player>("Player");
	}
}
