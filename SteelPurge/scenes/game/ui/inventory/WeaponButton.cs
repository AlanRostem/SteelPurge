using Godot;
using System;

public class WeaponButton : TextureButton
{
	private static Texture[] _weaponIcons =
	{
		GD.Load<Texture>("res://assets/texture/ui/icon/falcon_weapon_inventory.png"),
		GD.Load<Texture>("res://assets/texture/ui/icon/firewall_weapon_inventory.png"),
		GD.Load<Texture>("res://assets/texture/ui/icon/joule_weapon_inventory.png"),
	};
	
	private TextureRect _iconTextureRect;
	public Inventory.InventoryWeapon Weapon = Inventory.InventoryWeapon.Count;
	// private Inventory _inventory;
	
	public override void _Ready()
	{
		// _inventory = GetParent().GetParent().GetParent().GetParent<Inventory>();
		_iconTextureRect = GetNode<TextureRect>("WeaponIcon");
		_iconTextureRect.Texture = _weaponIcons[(int) Weapon];
	}
	
	private void _OnPressed()
	{
		// TODO: Close menu and switch weapon
		if (!Pressed)
		{
			Pressed = true;
		}
	}	
	
	private void _OnGetFocus()
	{
		Pressed = true;
	}
	
	private void _OnLoseFocus()
	{
		Pressed = false;
	}
}
