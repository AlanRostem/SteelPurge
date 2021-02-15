using Godot;
using System;

public class MoneyLabel : Label
{
	private Player _player;
	public override void _Ready()
	{
		_player = GetParent<HUD>().GetParent<Player>();
	}

	public override void _Process(float delta)
	{
		Text = "$" + _player.Stats.Money;
	}
}
