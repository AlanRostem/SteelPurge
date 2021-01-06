using Godot;
using System;

public class HUD : Node2D
{
	private Label _scoreLabel;
	
	public override void _Ready()
	{
		_scoreLabel = GetNode<Label>("ScoreLabel");
	}
}
