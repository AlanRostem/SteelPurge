using Godot;
using System;

public class AmmoLabel : Label
{
	Player _playerRef;
	public override void _Ready()
	{
		_playerRef = GetTree().Root.GetNode<Player>("Map/Player");
	}

	public override void _Process(float delta)
	{
		var gun = _playerRef.EquippedGun;
		Text = gun.GetAmmo() + " / " + gun.ReserveAmmo;
	}
}
