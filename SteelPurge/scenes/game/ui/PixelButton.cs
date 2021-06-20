using Godot;
using System;

public class PixelButton : Button
{
	[Export] public bool IsFocusedOnReady = false;
	public override void _Ready()
	{
		if (IsFocusedOnReady)
			GrabFocus();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
