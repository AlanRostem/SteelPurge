using Godot;
using System;

public class MainMenu : Control
{
	public override void _Ready()
	{
		
	}
	
	private void _OnPlayButtonPressed()
	{
		GD.Print("Play!");
	}

	private void _OnQuitButtonPressed()
	{
		GD.Print("Quit...");
	}
}
