using Godot;
using System;

public class HPBar : ProgressBar
{
 	private Player _player;
	public override void _Ready()
	{
		_player = GetParent<HUD>().GetParent<Player>();
	}

	public override void _Process(float delta)
	{
		Value = _player.Stats.Health;
	}
}
