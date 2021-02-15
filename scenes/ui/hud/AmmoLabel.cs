using Godot;
using System;

public class AmmoLabel : Label
{
	private Player _player;
	public override void _Ready()
	{
		_player = GetParent<HUD>().GetParent<Player>();
	}

	public override void _Process(float delta)
	{
		var gun = _player.WeaponHolder.EquippedWeapon;
		Text = gun.GetClipAmmo() +  "/" + gun.GetReserveAmmo();
	}
}
