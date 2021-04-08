using Godot;
using System;

public class MainMenu : Control
{
	public override void _Ready()
	{
		
	}

	private void PlayLevel(string scenePath)
	{
		// TODO: Improve this function
		GetTree().ChangeScene(scenePath);
	}
	
	private void _OnPlayButtonPressed()
	{
		PlayLevel("res://scenes/game/environment/levels/Level1.tscn");
	}

	private void _OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}
