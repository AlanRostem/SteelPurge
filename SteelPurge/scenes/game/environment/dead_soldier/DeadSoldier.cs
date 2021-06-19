using Godot;
using System;

public class DeadSoldier : Node2D
{
	private static readonly Texture[] _weaponHeldTextures = new[]
	{
		null, // H28
		GD.Load<Texture>("res://assets/texture/dead_soldier_with_firewall.png"), // Firewall
		GD.Load<Texture>("res://assets/texture/dead_soldier_with_joule.png"), // Joule
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
		if (player.PlayerInventory.HasWeapon(Weapon)) return;
		player.PlayerInventory.AddWeapon(Weapon);
		player.PlayerInventory.SwitchWeapon(Weapon);
		_sprite.Texture = _weaponHeldTextures[(int) Inventory.InventoryWeapon.Count];
	}
}
