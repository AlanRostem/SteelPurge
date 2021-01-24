using Godot;
using System;

public class RoundLabel : Label
{
	private Player _player;
	public override void _Ready()
	{
		_player = (Player)GetParent().GetParent();
	}

	public override void _Process(float delta)
	{
		Text = _player.Stats.Round.ToString();
	}
}
