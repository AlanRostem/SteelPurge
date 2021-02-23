using Godot;
using System;

public class GunNameLabel : Label
{
	private void _OnPlayerWeaponEquipped(Weapon weapon)
	{
		Text = weapon.DisplayName;
	}
}
