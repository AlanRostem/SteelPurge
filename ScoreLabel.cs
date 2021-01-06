using Godot;
using System;

public class ScoreLabel : Label
{
	Player _playerRef;
	public override void _Ready()
	{
		_playerRef = GetTree().Root.GetNode<Player>("Player");
	}

	public override void _Process(float delta)
	{
	  	Text = _playerRef.Score.ToString();
	}
}
