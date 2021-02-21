using Godot;
using System;

public class Map : Node2D
{
	public Player PlayerNode;
	public Main MainNode;

	public override void _Ready()
	{
		MainNode = GetParent<Main>();
	}
}
