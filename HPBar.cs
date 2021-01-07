using Godot;
using System;

public class HPBar : ProgressBar
{
	Player _playerRef;
	public override void _Ready()
	{
		_playerRef = GetTree().Root.GetNode<Player>("Map/Player");
	}

	public override void _Process(float delta)
	{
		Value = _playerRef.GetHP();
	}
}
