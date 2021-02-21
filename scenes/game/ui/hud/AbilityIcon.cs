using Godot;
using System;

public class AbilityIcon : TextureRect
{
	private Player _player;

	/*
	public override void _Process(float delta)
	{
		var ability = _player.WeaponInventory.EquippedWeapon.TacticalAbilityRef;
		if (ability != null)
		{
			if (Visible) return;
			Visible = true;
			Texture = ability.Icon;
		}
		else
		{
			if (!Visible) return;
			Texture = null;
			Visible = false;
		}
	}
	*/
}
