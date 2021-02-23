using Godot;
using System;

public class AbilityIcon : TextureRect
{
	public override void _Ready()
	{
		Visible = false; // Default 
	}

	/*
	public override void _Process(float delta)
	{
		var ability = _player.WeaponInventory.EquippedWeapon.TacticalEnhancement;
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
