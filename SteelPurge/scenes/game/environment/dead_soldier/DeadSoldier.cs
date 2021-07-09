using Godot;
using System;

public class DeadSoldier : StaticEntity
{
	private static readonly Texture[] _weaponHeldTextures =
	{
		null, // H28
		GD.Load<Texture>("res://assets/texture/dead_soldier_with_firewall.png"), // Firewall
		GD.Load<Texture>("res://assets/texture/dead_soldier_with_joule.png"), // Joule
		null, // Falcon
		GD.Load<Texture>("res://assets/texture/dead_soldier.png"), // Count (Basically none)
	};
	
	[Export] public Inventory.InventoryWeapon Weapon = Inventory.InventoryWeapon.H28;

	private Sprite _sprite;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite>("Sprite");
		_sprite.Texture = _weaponHeldTextures[(int) Weapon];
	}

	private void _OnInteract(Player player)
	{
		player.PlayerInventory.SwitchWeapon(Weapon);
		_sprite.Texture = _weaponHeldTextures[(int) Inventory.InventoryWeapon.Count];
	}
}
