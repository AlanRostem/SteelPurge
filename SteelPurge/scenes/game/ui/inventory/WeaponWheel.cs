using Godot;
using System;

public class WeaponWheel : Control
{
	private static readonly PackedScene WeaponButtonScene =
		GD.Load<PackedScene>("res://scenes/game/ui/inventory/WeaponButton.tscn");

	private int _weaponCount = (int)Inventory.InventoryWeapon.Count;
	private GridContainer _gridContainer;
	
	public override void _Ready()
	{
		_gridContainer = GetNode<GridContainer>("GridContainer");
		_gridContainer.Columns = _weaponCount;
		for (var i = 0; i < _weaponCount; i++)
		{
			AddWeaponButton((Inventory.InventoryWeapon)i);
		}
	}

	private void AddWeaponButton(Inventory.InventoryWeapon weapon)
	{
		var button = (WeaponButton) WeaponButtonScene.Instance();
		button.Weapon = weapon;
		_gridContainer.AddChild(button);
	}
}
