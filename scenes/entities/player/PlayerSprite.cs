using Godot;
using System;

public class PlayerSprite : AnimatedSprite
{
	private Player _player;
	public override void _Ready()
	{
		_player = (Player)GetParent();
	}

	public override void _Process(float delta)
	{
		FlipH = _player.Direction < 0;
		Animation = _player.IsWalking ? "walk" : "idle";
		if (_player.IsJumping)
		{
			Animation = "jump";
		}
	}
}
