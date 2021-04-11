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
		FlipH = _player.HorizontalLookingDirection < 0;
		Animation = _player.IsWalking ? "walk" : "idle";
		if (_player.IsJumping)
		{
			Animation = "jump";
		}

		if (_player.IsSliding && _player.IsOnFloor())
		{
			Animation = "slide";
		}
		
		_player.Modulate = new Color(Modulate)
		{
			a = !_player.IsInvulnerable ? 1f : .5f
		};
	}
	
	private void _OnPlayerWeaponEquipped(Weapon weapon)
	{
		Frames = weapon.PlayerSpriteFrames;
	}
}
