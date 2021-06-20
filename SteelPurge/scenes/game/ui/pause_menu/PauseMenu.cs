using Godot;
using System;

public class PauseMenu : Control
{
	[Signal]
	public delegate void Resume();
	
	[Signal]
	public delegate void Return();

	public override void _Ready()
	{
		Visible = false;
	}

	private void _OnResumeButtonPressed()
	{
		EmitSignal(nameof(Resume));
		Visible = false;
	}

	private void _OnReturnButtonPressed()
	{
		EmitSignal(nameof(Return));
		Visible = false;
	}
}
