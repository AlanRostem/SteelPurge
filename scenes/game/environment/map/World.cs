using Godot;
using System;

public class World : Node2D
{
	public Player PlayerNode {get; private set;}
	public override void _Ready()
	{
		PlayerNode = GetNode<Player>("Player");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
