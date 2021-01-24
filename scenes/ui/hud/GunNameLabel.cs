using Godot;
using System;

public class GunNameLabel : Label
{
	private Player _player;
	public override void _Ready()
	{
		_player = (Player) GetParent().GetParent();
	}

	public override void _Process(float delta)
	{
		var gun = _player.WeaponHolder.EquippedWeapon;
		Text = gun.ScreenDisplayName;
	}
}
