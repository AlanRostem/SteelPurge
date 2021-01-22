using Godot;
using System;

public class HUD : CanvasLayer
{
	public Player PlayerRef;
	public override void _Ready()
	{
		var main = (Main) GetParent();
		PlayerRef = main.PlayerRef;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
