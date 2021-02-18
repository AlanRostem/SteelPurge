using Godot;
using System;

public class GunNameLabel : Label
{
	private Player _player;
	public override void _Ready()
	{
		_player = GetParent<HUD>().GetParent<Player>();
	}

	public override void _Process(float delta)
	{
		var gun = _player.WeaponInventory.EquippedWeapon;
		Text = gun.ScreenDisplayName;
	}
}
